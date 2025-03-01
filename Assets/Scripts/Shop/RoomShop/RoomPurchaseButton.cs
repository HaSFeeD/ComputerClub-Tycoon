using UnityEngine;
using UnityEngine.UI;

public class RoomPurchaseButton : MonoBehaviour
{
    [SerializeField] private GameObject _purchaseButtonPrefab;
    private Button _purchaseRoomButton;
    private RoomItemData _roomItemData;

    private void Awake()
    {
        _roomItemData = GetComponentInParent<RoomItemData>();
        _purchaseRoomButton = _purchaseButtonPrefab.GetComponentInChildren<Button>();
        _purchaseRoomButton.onClick.AddListener(OnPurchaseClicked);   
    }

    private void OnPurchaseClicked()
    {
        int roomID = _roomItemData.roomID;
        int roomCost = _roomItemData.roomCost;
        RoomPurchaseManager.Instance.OnRoomPurchase(_roomItemData, _purchaseButtonPrefab);
    }
}
