using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private int LevelNumber = 1;
    public GameData gameData;
    public LevelData levelData;
    public bool IsBotRegistrationOccupied = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        gameData = SaveLoadManager.LoadData();
        if (gameData == null)
        {
            gameData = new GameData();
            Debug.Log("No save file. Created new GameData with default values.");
        }

        levelData = gameData.Levels.Find(obj => obj.LevelNumber == LevelNumber);
        if (levelData == null)
        {
            levelData = new LevelData();
            levelData.LevelNumber = LevelNumber;
            levelData.IsLevelOpened = true;
            levelData.Cash = 0;
            levelData.rooms = new List<RoomData>();
            gameData.Levels.Add(levelData);
            Debug.Log("No LevelData save file");
        }

        RoomData mainRoom = levelData.rooms.Find(r => r.RoomID == 0);
        if (mainRoom == null)
        {
            mainRoom = new RoomData();
            mainRoom.RoomID = 0;
            mainRoom.purhcasedObjects = 1;
            mainRoom.upgrades = new List<UpgradeSaveData>();
            levelData.rooms.Add(mainRoom);
            Debug.Log("RoomData for id0 was missing; created new with 1 computer.");
        }
        else
        {
            if (mainRoom.purhcasedObjects < 1)
            {
                mainRoom.purhcasedObjects = 1;
                Debug.Log("RoomData for id0 updated to have at least 1 computer.");
            }
        }
        ApplyGameData();
        LoadRoom();
        LoadRoomUpgrades();
    }

    public void ApplyGameData()
    {
        Debug.Log("Loaded Cash: " + levelData.Cash);
        Debug.Log("Loaded Diamonds: " + gameData.Diamonds);
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.SetCash(levelData.Cash);
            EconomyManager.Instance.SetDiamonds(gameData.Diamonds);
            RatingManager.Instance.SetRating(levelData.Rating);
            IncomeManager.Instance.SetIncome(levelData.Income);
        }
        else
            Debug.LogError("EconomyManager instance not found!");
        if (RoomManager.Instance == null)
            Debug.LogError("RoomManager instance not found!");
    }

    public void SaveData()
    {
        if (EconomyManager.Instance != null && RoomManager.Instance != null)
        {
            levelData.Cash = EconomyManager.Instance.Cash;
            gameData.Diamonds = EconomyManager.Instance.Diamonds;
            levelData.Income = IncomeManager.Instance.GetIncome();
            levelData.Rating = RatingManager.Instance.GetCurrentRating();
            SaveLoadManager.SaveData(gameData);
            Debug.Log("Data saved!");
        }
        else
            Debug.LogError("EconomyManager or RoomManager instance not found!");
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void SaveRoom(int roomID, int purchasedObjects, float roomIncome)
    {
        var room = levelData.rooms.Find(obj => obj.RoomID == roomID);
        if (room == null)
        {
            RoomData roomData = new RoomData();
            roomData.RoomID = roomID;
            roomData.purhcasedObjects = purchasedObjects;
            roomData.RoomIncome = roomIncome;
            roomData.upgrades = new List<UpgradeSaveData>();
            levelData.rooms.Add(roomData);
        }
        else
        {
            room.purhcasedObjects = purchasedObjects;
            room.RoomIncome = roomIncome;
        }
        SaveData();
    }

    public void LoadRoom()
    {
        foreach (var roomData in levelData.rooms)
        {
            Room room = RoomManager.Instance.FindRoomByID(roomData.RoomID);
            if (room != null)
            {
                room.ObjectsCount = roomData.purhcasedObjects;
                room._roomIncome = roomData.RoomIncome;
                if (room._roomIncome == 0)
                {
                    room._roomIncome = 1;
                }
                room._roomAvailable = true;
                room.BlockRoomGameobject.SetActive(false);
                if (!RoomManager.Instance.PurchasedRooms.Contains(room))
                    RoomManager.Instance.PurchasedRooms.Add(room);

                ComputerManager.Instance.LoadPC(roomData.purhcasedObjects, roomData.RoomID);

                LoadRoomUtilities(roomData, room);

                RoomItemData roomItemData = RoomManager.Instance.roomItemDatas.Find(r => r.roomID == room.id);
                if (roomItemData != null)
                {
                    roomItemData.isRoomPurchased = true;
                    PurchaseRoomItemUI ui = roomItemData.GetComponent<PurchaseRoomItemUI>();
                    if (ui != null)
                    {
                        ui.Setup(roomItemData);
                    }
                }
            }
            else
            {
                Debug.LogWarning("Room with ID " + roomData.RoomID + " not found in scene during LoadRoom.");
            }
        }
    }

    public void SaveRoomUpgrade(UpgradeData upgradeData, int roomID)
    {
        var roomData = levelData.rooms.Find(obj => obj.RoomID == roomID);
        if (roomData != null)
        {
            var upgrade = roomData.upgrades.Find(obj => obj.UpgradeName == upgradeData.UpgradeName);
            if (upgrade == null)
            {
                UpgradeSaveData newUpgrade = new UpgradeSaveData();
                newUpgrade.UpgradeLevel = upgradeData.UpgradeLevel;
                newUpgrade.UpgradeName = upgradeData.UpgradeName;
                if (roomData.upgrades == null)
                    roomData.upgrades = new List<UpgradeSaveData>();
                roomData.upgrades.Add(newUpgrade);
            }
            else
            {
                upgrade.UpgradeLevel = upgradeData.UpgradeLevel;
            }
            roomData.RoomIncome = GetRoomByID(roomID)._roomIncome;
        }
        else
        {
            Debug.LogError("Room not found in LevelData for roomID: " + roomID);
        }
        SaveData();
    }

    public void LoadRoomUpgrades()
    {
        foreach (var roomData in levelData.rooms)
        {
            Room room = RoomManager.Instance.FindRoomByID(roomData.RoomID);
            if (room != null && room.roomUpgradesManager != null)
            {
                room.roomUpgradesManager.LoadUpgrades(roomData.upgrades);
            }
        }
    }

    public void SaveRoomUtilities(int roomID)
    {
        Room room = RoomManager.Instance.FindRoomByID(roomID);
        if (room == null)
        {
            Debug.LogError("Room with ID " + roomID + " not found!");
            return;
        }
        
        RoomData roomData = levelData.rooms.Find(r => r.RoomID == roomID);
        if (roomData == null)
        {
            roomData = new RoomData();
            roomData.RoomID = roomID;
            roomData.purhcasedObjects = room.ObjectsCount;
            roomData.RoomIncome = room._roomIncome;
            roomData.upgrades = new List<UpgradeSaveData>();
            levelData.rooms.Add(roomData);
        }
        
        roomData.vendingMachines = new List<VendingMachineSaveData>();
        foreach(var vending in room.VendingMachines)
        {
            VendingMachineSaveData vmData = new VendingMachineSaveData();
            vmData.id = vending.ID;
            vmData.income = vending.Income;
            vmData.isPurchased = vending.isPurchased;
            vmData.upgradeLevel = vending.UpgradeLevel;
            Debug.Log($"Saving VendingMachine ID:{vending.ID} - isPurchased: {vending.isPurchased}");
            roomData.vendingMachines.Add(vmData);
        }
        
        roomData.toilets = new List<ToiletSaveData>();
        foreach (var toilet in room.Toilets)
        {
            ToiletSaveData tData = new ToiletSaveData();
            tData.id = toilet.ID;
            tData.income = toilet.Income;
            tData.isPurchased = toilet.isPurchased;
            UtilityUpgradeData upgrade = toilet.GetComponent<UtilityUpgradeData>();
            if (upgrade != null)
            {
                tData.upgradeLevel = upgrade.UpgradeLevel;
            }
            roomData.toilets.Add(tData);
        }
        
        SaveData();
    }

    public void LoadRoomUtilities(RoomData roomData, Room room)
    {
        if (roomData.vendingMachines != null)
        {
            foreach (var vmData in roomData.vendingMachines)
            {
                VendingMachine vm = Resources.FindObjectsOfTypeAll<VendingMachine>()
                                       .FirstOrDefault(x => x.ID == vmData.id);
                UtilityUpgradeData utilityUpgradeData = UpgradeManager.Instance.AllUtilityUpgrades.Find(obj => obj.UtilityID == vm.ID);
                if (vm != null)
                {
                    vm.UpdateIncome(vmData.income);
                    vm.UpgradeLevel = vmData.upgradeLevel;
                    utilityUpgradeData.SetLevel(vmData.upgradeLevel);
                    if (vmData.isPurchased)
                    {
                        vm.gameObject.SetActive(true);
                        vm.SetPurchased(true);
                    }
                }
            }
        }
        
        if (roomData.toilets != null)
        {
            foreach (var tData in roomData.toilets)
            {
                Toilet t = Resources.FindObjectsOfTypeAll<Toilet>()
                            .FirstOrDefault(x => x.ID == tData.id);
                if (t != null)
                {
                    t.SetIncome(tData.income);
                    if (tData.isPurchased)
                    {
                        t.gameObject.SetActive(true);
                        t.SetPurchased(true);
                    }
                    UtilityUpgradeData upgrade = t.GetComponent<UtilityUpgradeData>();
                    if (upgrade != null)
                    {
                        upgrade.SetLevel(tData.upgradeLevel);
                        upgrade.IsPurchased = tData.isPurchased;
                    }
                }
            }
        }
    }

    private Room GetRoomByID(int roomID)
    {
        return RoomManager.Instance.FindRoomByID(roomID);
    }
}
