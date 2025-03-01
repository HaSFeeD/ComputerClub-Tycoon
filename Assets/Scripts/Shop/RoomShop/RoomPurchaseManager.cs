using System.Collections.Generic;
using UnityEngine;

public class RoomPurchaseManager : MonoBehaviour
{
    public static RoomPurchaseManager Instance;
    private List<Room> rooms = new List<Room>();

    private void Awake()
    {
        Instance = this;
    }

    public void OnRoomPurchase(RoomItemData roomItemData, GameObject button)
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag("Building"))
        {
            if(obj.GetComponent<Room>() != null)
            {
                rooms.Add(obj.GetComponent<Room>());
            }
        }
        foreach (var room in rooms)
        {
            if (roomItemData.roomID == room.id)
            {
                if (EconomyManager.Instance.Cash > roomItemData.roomCost)
                {
                    EconomyManager.Instance.SubtractCash(roomItemData.roomCost);
                    room._roomAvailable = true;
                    room.BlockRoomGameobject.SetActive(!room._roomAvailable);
                    RoomManager.Instance.PurchasedRooms.Add(room);
                    GameManager.Instance.SaveRoom(room.id, 0, room._roomIncome);
                    roomItemData.isRoomPurchased = true;
                    button.SetActive(false);
                    roomItemData.UpdateRoomItemUI();
                }
                else 
                {
                    Debug.Log("No money");
                }
            }
        }
    }
}
