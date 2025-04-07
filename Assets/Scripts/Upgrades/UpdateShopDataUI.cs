using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateShopDataUI : MonoBehaviour
{
    public static UpdateShopDataUI Instance; 
    public GameObject UpgradePanel;
    public GameObject UpgradeItemsContainer;
    public GameObject ShopUpgradeElement;

    public GameObject NextPageButton;
    public GameObject PreviousPage;
    [SerializeField] private Image _upgradeIcon;
    [SerializeField] private Image _upgradeLevelImage;
    [SerializeField] private TextMeshProUGUI _currentIncome;
    [SerializeField] private TextMeshProUGUI _additionalIncome;
    [SerializeField] private Button _upgradeButton;
    

    private void Awake()
    {
        Instance = this;
    }
    
    public void UpdateUpgradePanelUI(UpgradeData upgradeData)
    {
        if (upgradeData == null)
        {
            Debug.LogError("UpgradeData is null.");
            return;
        }

        if (RoomManager.Instance == null)
        {
            Debug.LogError("RoomManager.Instance is null.");
            return;
        }
        
        var roomUpgradeManager = upgradeData.GetRoomUpgradeManager();
        if (roomUpgradeManager == null)
        {
            Debug.LogError("upgradeData.GetRoomUpgradeManager() returned null.");
            return;
        }
        
        int roomID = roomUpgradeManager.GetRoomID();
        var room = RoomManager.Instance.FindRoomByID(roomID);
        if (room == null)
        {
            Debug.LogError("Room with ID " + roomID + " not found.");
            return;
        }
        
        if (_upgradeIcon != null)
        {
            _upgradeIcon.sprite = upgradeData.UpgradeIcon;
        }
        
        if (_upgradeLevelImage != null)
        {
            TextMeshProUGUI levelText = _upgradeLevelImage.GetComponentInChildren<TextMeshProUGUI>();
            if (levelText != null)
                levelText.text = upgradeData.UpgradeLevel.ToString();
            else
                Debug.LogError("No TextMeshProUGUI component found in _upgradeLevelImage.");
        }
        
        if (upgradeData is UtilityUpgradeData utilityUpgrade)
        {
            if (utilityUpgrade.upgradeType == UtilityUpgradeType.VendingMachine)
            {
                VendingMachine vm = utilityUpgrade.ReturnObject<VendingMachine>();
                _currentIncome.text = (vm != null ? vm.Income.ToString() : room._roomIncome.ToString());
            }
            else if (utilityUpgrade.upgradeType == UtilityUpgradeType.Toilet)
            {
                Toilet t = utilityUpgrade.ReturnObject<Toilet>();
                _currentIncome.text = (t != null ? t.Income.ToString() : room._roomIncome.ToString());
            }
            else
            {
                _currentIncome.text = room._roomIncome.ToString();
            }
        }
        else
        {
            _currentIncome.text = room._roomIncome.ToString();
        }
        
        int costToDisplay = upgradeData.CurrentCost;
        if (upgradeData is UtilityUpgradeData utilityUpgrade2)
        {
            costToDisplay = (int)utilityUpgrade2.DisplayCost;
        }
        
        if (_upgradeButton != null)
        {
            TextMeshProUGUI buttonText = _upgradeButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = !upgradeData.IsMaxUpgradeLevel() ? costToDisplay.ToString() : "MAX LEVEL";
            }
        }
        
        float currentLevelIncome = upgradeData.UpgradeIncome * Mathf.Pow(upgradeData.IncomeMultiplier, upgradeData.UpgradeLevel - 1);
        float nextLevelIncome = upgradeData.UpgradeIncome * Mathf.Pow(upgradeData.IncomeMultiplier, upgradeData.UpgradeLevel);
        float addedIncome = nextLevelIncome - currentLevelIncome;
        
        if (_additionalIncome != null)
        {
            _additionalIncome.text = !upgradeData.IsMaxUpgradeLevel() ? "+ " + addedIncome.ToString() : "UPGRADE MAX LEVEL!";
        }
        
        UpdateOnClickMethod(upgradeData);
    }

    public void UpdatePurchasePanelUI(UtilityUpgradeData purchaseData)
    {
        if (!UpgradePanel.activeSelf)
            UpgradePanel.SetActive(true);

        _upgradeIcon.sprite = purchaseData.UpgradeIcon;
        _upgradeLevelImage.GetComponentInChildren<TextMeshProUGUI>().text = purchaseData.IsPurchased ? "1" : "0";
        _currentIncome.text = purchaseData.CurrentIncome.ToString();
        _upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text =
            purchaseData.IsPurchased ? "PURCHASED" : purchaseData.PurchaseCost.ToString();
        _additionalIncome.text = "";
        _upgradeButton.onClick.RemoveAllListeners();
        _upgradeButton.onClick.AddListener(purchaseData.OnUpgradeClicked);
    }
    private void UpdateOnClickMethod(UpgradeData upgradeData)
    {
        _upgradeButton.onClick.RemoveAllListeners();
        _upgradeButton.onClick.AddListener(upgradeData.OnUpgradeClicked);
    }
    private void UpdateOnClickMethod(UtilityUpgradeData upgradeData)
    {
        _upgradeButton.onClick.RemoveAllListeners();
        _upgradeButton.onClick.AddListener(upgradeData.OnUpgradeClicked);
    }
    
    
}
