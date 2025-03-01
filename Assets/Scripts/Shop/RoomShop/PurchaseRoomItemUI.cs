using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseRoomItemUI : MonoBehaviour
{
    [Header("Room Purchase Prefab Components")]
    [SerializeField]
    private GameObject _purchaseButtonPrefab;
    [SerializeField]
    private TextMeshProUGUI _roomTitle;
    [SerializeField]
    private TextMeshProUGUI _roomDescription;
    [SerializeField]
    private TextMeshProUGUI _roomCostText;
    [SerializeField]
    private TextMeshProUGUI _roomEnergyCostText;
    [SerializeField]
    private GameObject _roomPurchasedImagePrefab;
    public void Setup(RoomItemData data)
    {
        _roomTitle.text = data.roomTitle;
        _roomDescription.text = data.roomDescription;
        _roomCostText.text = data.roomCost.ToString();
        _roomEnergyCostText.text = data.roomEnergyConsumption.ToString();
        PurchaseCompleted(data);

    }
    public void PurchaseCompleted(RoomItemData data){
        if(data.isRoomPurchased){
            _purchaseButtonPrefab.SetActive(false);
            _roomPurchasedImagePrefab.SetActive(true);
        }
        else{
            _purchaseButtonPrefab.SetActive(true);
            _roomPurchasedImagePrefab.SetActive(false);
        }
    }
    
}
