using UnityEngine;

public enum RoomCategory { PCRoom, ToiletRoom, VendingMachineRoom }

public class RoomItemData : MonoBehaviour
{
    [Header("Main Parameters")]
    public int roomID;
    public string roomTitle;
    [TextArea] 
    public string roomDescription;
    public int roomCost;
    public int roomEnergyConsumption;
    public RoomCategory roomCategory;
    public bool isRoomPurchased;
    PurchaseRoomItemUI purchaseRoomItemUI;
    private void Start(){
        purchaseRoomItemUI = GetComponent<PurchaseRoomItemUI>();
        purchaseRoomItemUI.Setup(this);
        if(isRoomPurchased){
            Debug.Log("ROOM - TRUE");

        }
    }
    public void UpdateRoomItemUI(){
        purchaseRoomItemUI = GetComponent<PurchaseRoomItemUI>();
        purchaseRoomItemUI.Setup(this);
    }
}
