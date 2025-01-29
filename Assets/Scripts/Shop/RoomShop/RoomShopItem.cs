using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomShopItem : MonoBehaviour
{
    public int id;
    public GameObject RoomShopButton;
    public TextMeshProUGUI RoomCost;
    public void OnBuyRoom(){
        int roomCost = 0;
        if(!int.TryParse(RoomCost.text, out roomCost)){
            Debug.Log("Error");
        }
        RoomPurchaseManager.Instance.OnRoomPurchase(id, roomCost);
    }
}
