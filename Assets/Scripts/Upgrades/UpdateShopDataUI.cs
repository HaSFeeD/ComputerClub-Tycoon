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
        var room = RoomManager.Instance.FindRoomByID(upgradeData.GetRoomUpgradeManager().GetRoomID());
        _upgradeIcon.sprite = upgradeData.UpgradeIcon;
        _upgradeLevelImage.GetComponentInChildren<TextMeshProUGUI>().text = upgradeData.UpgradeLevel.ToString();

        if (upgradeData is UtilityUpgradeData utilityUpgrade)
        {
            if (utilityUpgrade.upgradeType == UtilityUpgradeType.VendingMachine)
            {
                VendingMachine vm = utilityUpgrade.ReturnObject<VendingMachine>();
                if(vm != null)
                {
                    _currentIncome.text = vm.Income.ToString();
                }
                else
                {
                    _currentIncome.text = room._roomIncome.ToString();
                }
            }
            else if (utilityUpgrade.upgradeType == UtilityUpgradeType.Toilet)
            {
                Toilet t = utilityUpgrade.ReturnObject<Toilet>();
                if(t != null)
                {
                    _currentIncome.text = t.Income.ToString();
                }
                else
                {
                    _currentIncome.text = room._roomIncome.ToString();
                }
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

        if (!upgradeData.IsMaxUpgradeLevel())
        {
            _upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = costToDisplay.ToString();
        }
        else 
        {
            _upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = "MAX LEVEL";
        }

        float currentLevelIncome = upgradeData.UpgradeIncome 
                                * Mathf.Pow(upgradeData.IncomeMultiplier, upgradeData.UpgradeLevel - 1);
        float nextLevelIncome = upgradeData.UpgradeIncome 
                            * Mathf.Pow(upgradeData.IncomeMultiplier, upgradeData.UpgradeLevel);
        float addedIncome = nextLevelIncome - currentLevelIncome;

        if (!upgradeData.IsMaxUpgradeLevel())
        {
            _additionalIncome.text = "+ " + addedIncome.ToString();
        }
        else
        {
            _additionalIncome.text = "UPGRADE MAX LEVEL!";
        }

        UpdateOnClickMethod(upgradeData);
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
