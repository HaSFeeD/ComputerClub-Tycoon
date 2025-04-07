using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int LevelNumber = 1;
    public GameData gameData;
    public LevelData levelData;
    public bool IsBotRegistrationOccupied = false;
    public bool _isAdBlockerPurchased = false;

    private void Awake()
    {
        Application.targetFrameRate = 120;
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
            levelData.questSaveDatas = new List<QuestSaveData>();
            gameData.Levels.Add(levelData);
            Debug.Log("No LevelData save file");
        }
        if(gameData.DonateManagers == null)
            gameData.DonateManagers = new List<OfflineTimeManagers>();

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
        ApplyDonateBonuses();
        LoadRoom();
        LoadRoomUpgrades();
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnInitialized += OnQuestManagerInitialized;
        }
    }
    public void ApplyGameData()
    {
        Debug.Log("Loaded Cash: " + levelData.Cash);
        Debug.Log("Loaded Diamonds: " + gameData.Diamonds);
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.SetCash(levelData.Cash);
            EconomyManager.Instance.SetDiamonds(gameData.Diamonds);
            EconomyManager.Instance.SetEnergy(levelData.Energy);
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
            levelData.Energy = EconomyManager.Instance.GetEnergy();
            SaveLoadManager.SaveData(gameData);
            Debug.Log("Data saved!");
        }
        else
            Debug.LogError("EconomyManager or RoomManager instance not found!");
    }
        public void SaveDonateBonuses(string name, bool isPurchased){
        gameData.isAdBlockPurchased = _isAdBlockerPurchased;
        var manager = gameData.DonateManagers.Find(obj => obj.Name == name);
        if(manager == null){
            var donateManager = new OfflineTimeManagers();
            donateManager.Name = name;
            donateManager.IsPurchased = isPurchased;
            gameData.DonateManagers.Add(donateManager);
        } else {
            manager.IsPurchased = isPurchased;
        }
        SaveData();
    }

    private void ApplyDonateBonuses(){
        _isAdBlockerPurchased = gameData.isAdBlockPurchased;
        foreach(var managerData in gameData.DonateManagers){
            DonateManagerItem manager = DonateManagers.Instance.FindManagerByName(managerData.Name);
            if(manager != null){
                manager.BonusName = managerData.Name;
                if(managerData.IsPurchased){
                    manager.SetPurchased();
                }
            }
        }

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
        roomData.generators = new List<GeneratorSaveData>();
        foreach (var generator in room.Generators)
        {
            GeneratorSaveData gData = new GeneratorSaveData();
            gData.id = generator.ID;
            gData.income = generator.Income;
            gData.isPurchased = generator.isPurchased;
            UtilityUpgradeData upgrade = generator.GetComponent<UtilityUpgradeData>();
            if (upgrade != null)
            {
                gData.upgradeLevel = upgrade.UpgradeLevel;
            }
            roomData.generators.Add(gData);
        }
        if (roomData.computers == null)
        roomData.computers = new List<ComputerData>();

        foreach (var computer in room.Computers)
        {
            var savedComputer = roomData.computers.FirstOrDefault(c => c.id == computer.ID);
            if (savedComputer == null)
            {
                ComputerData newComputer = new ComputerData();
                newComputer.id = computer.ID;
                newComputer.isPurchased = computer.IsPurchased;
                roomData.computers.Add(newComputer);
            }
            else
            {
                savedComputer.isPurchased = computer.IsPurchased;
            }
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
        if (roomData.generators != null)
        {
            foreach (var gmData in roomData.generators)
            {
                Generator gm = Resources.FindObjectsOfTypeAll<Generator>()
                                       .FirstOrDefault(x => x.ID == gmData.id);
                UtilityUpgradeData utilityUpgradeData = UpgradeManager.Instance.AllUtilityUpgrades.Find(obj => obj.UtilityID == gm.ID);
                if (gm != null)
                {
                    gm.UpdateIncome(gmData.income);
                    gm.UpgradeLevel = gmData.upgradeLevel;
                    utilityUpgradeData.SetLevel(gmData.upgradeLevel);
                    if (gmData.isPurchased)
                    {
                        gm.gameObject.SetActive(true);
                        gm.SetPurchased(true);
                    }
                }
            }
        }
        if (roomData.computers != null)
        {
            foreach (var cData in roomData.computers)
            {
                Computer c = Resources.FindObjectsOfTypeAll<Computer>()
                                       .FirstOrDefault(x => x.ID == cData.id);
                UtilityUpgradeData utilityUpgradeData = UpgradeManager.Instance.AllUtilityUpgrades.Find(obj => obj.UtilityID == c.ID);
                if (c != null)
                {
                    if (cData.isPurchased)
                    {
                        c.gameObject.SetActive(true);
                        c.SetPurchased(true, c);
                    }
                }
            }
        }
    }

    private void OnQuestManagerInitialized()
    {
        LoadQuests();
    }
    public void SaveQuest(int questId, string questName, int currentAmount, int targetAmount, bool isCompleted, bool isRewardTaken){
        var quest = levelData.questSaveDatas.Find(q => q.QuestName == questName);
        if(quest == null){
            quest = new QuestSaveData();
            quest.Id = questId;
            quest.QuestName = questName;
            quest.CurrentAmount = currentAmount;
            quest.TargetAmount = targetAmount;
            quest.IsCompleted = isCompleted;
            quest.IsRewardTaken = isRewardTaken;
            levelData.questSaveDatas.Add(quest);
        }
        else{
            quest.CurrentAmount = currentAmount;
            quest.TargetAmount = targetAmount;
            quest.IsCompleted = isCompleted;
            quest.IsRewardTaken = isRewardTaken;
        }
        SaveData();
    }
    public void LoadQuests()
    {
        foreach (var questSaveData in levelData.questSaveDatas)
        {
            QuestData questData = QuestManager.Instance.FindQuestByName(questSaveData.QuestName);
            if (questData != null)
            {
                questData.CurrentAmount = questSaveData.CurrentAmount;
                questData.TargetAmount = questSaveData.TargetAmount;
                questData.IsCompleted = questSaveData.IsCompleted;
                questData.IsRewardTaken = questSaveData.IsRewardTaken;
            }
            else
            {
            }
        }
        QuestManager.Instance.UpdateQuestMenu();
    }
    private Room GetRoomByID(int roomID)
    {
        return RoomManager.Instance.FindRoomByID(roomID);
    }
    private void OnApplicationQuit()
    {
        SaveData();
    }
    private void OnApplicationPause()
    {
        SaveData();
    }
}
