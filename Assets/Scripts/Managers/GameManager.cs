using UnityEngine;
<<<<<<< HEAD
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    private float _balance = 0f;
    public event Action<float> OnBalanceChanged;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddBalance(float amount)
    {
        _balance += amount;
        OnBalanceChanged?.Invoke(_balance);
        Debug.Log("Баланс оновлено: " + _balance);
    }

    public float GetBalance()
    {
        return _balance;
=======
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameData gameData; 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        gameData = SaveLoadManager.LoadData();
        if (gameData == null)
        {
            gameData = new GameData();
            Debug.Log("No save file. Created new GameData with default values.");
        }

        ApplyGameData();
    }

    public void ApplyGameData()
    {
        EconomyManager.Instance.SetCash(gameData.Cash);
        EconomyManager.Instance.SetDiamonds(gameData.Diamonds);

        RoomManager.Instance.PurchasedRooms.Clear();

        gameData.purchasedRoomIDs = gameData.purchasedRoomIDs.Distinct().ToList();

        foreach (int roomId in gameData.purchasedRoomIDs)
        {
            Room room = RoomManager.Instance.FindRoomByID(roomId);
            if (room != null)
            {
                room._roomAvailable = true;
                if (room.BlockRoomGameobject != null)
                {
                    room.BlockRoomGameobject.SetActive(false);
                }

                if (!RoomManager.Instance.PurchasedRooms.Contains(room))
                    RoomManager.Instance.PurchasedRooms.Add(room);
            }
        }
    }

    public void SaveData()
    {
        gameData.Cash = EconomyManager.Instance.Cash;
        gameData.Diamonds = EconomyManager.Instance.Diamonds;

        gameData.purchasedRoomIDs.Clear();

        foreach (var room in RoomManager.Instance.PurchasedRooms)
        {
            gameData.purchasedRoomIDs.Add(room.id);
        }
        SaveLoadManager.SaveData(gameData);
        Debug.Log("Data saved!");
    }
    public void OnDisable(){
        SaveData();
>>>>>>> 27866b6 (Refactored Some Code and add new Features)
    }
}
