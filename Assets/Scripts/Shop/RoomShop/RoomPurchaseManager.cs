using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomPurchaseManager : MonoBehaviour
{
    public static RoomPurchaseManager Instance;

    private List<Room> rooms = new List<Room>();

    private void Awake()
    {
        Instance = this;
    }
    public void OnRoomPurchase(int id, int roomCost){
        foreach(var obj in GameObject.FindGameObjectsWithTag("Building"))
        if(obj.GetComponent<Room>()){
            rooms.Add(obj.GetComponent<Room>());
        }
        foreach(var room in rooms){
            if(id == room.id){
                if(EconomyManager.Instance.Cash > roomCost){
                    EconomyManager.Instance.SubtractCash(roomCost);
                    room._roomAvailable = true;
                    room.BlockRoomGameobject.SetActive(!room._roomAvailable);
                    RoomManager.Instance.PurchasedRooms.Add(room);
                }
                else {
                    Debug.Log("No money");
                }
            }
        }
    }
}
