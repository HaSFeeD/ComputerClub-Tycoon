using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using System.Runtime.InteropServices;

public enum UtilityUpgradeType {
    VendingMachine,
    Toilet,
    Generator,
    Computer
}

public class UtilityUpgradeData : UpgradeData
{
    private GameObject utility;
    private Room room;

    [SerializeField] public int UtilityID;
    [SerializeField] public UtilityUpgradeType upgradeType;
    [SerializeField] private float purchaseCost = 200f;
    public float PurchaseCost => purchaseCost;
    [SerializeField] private float _energyConsumptionPerUpgrade = 5f;

    [SerializeField]
    private float _initialGeneratorEnergyProduction = 3;
    [SerializeField]
    private float _generatorEnergyProductionMultiplier = 1.5f;
    public float GeneratorEnergyProduction => _initialGeneratorEnergyProduction * Mathf.Pow(_generatorEnergyProductionMultiplier, UpgradeLevel - 1);

    public float DisplayCost {
        get {
            return !IsPurchased ? PurchaseCost : CurrentCost;
        }
    }

    [SerializeField] private bool isPurchased = false;
    public bool IsPurchased {
        get { 
            if (upgradeType == UtilityUpgradeType.VendingMachine && utility != null)
            {
                VendingMachine vm = utility.GetComponent<VendingMachine>();
                return (vm != null) ? vm.isPurchased : isPurchased;
            }
            else if (upgradeType == UtilityUpgradeType.Toilet && utility != null)
            {
                Toilet t = utility.GetComponent<Toilet>();
                return (t != null) ? t.isPurchased : isPurchased;
            }
            else if (upgradeType == UtilityUpgradeType.Generator && utility != null)
            {
                Generator g = utility.GetComponent<Generator>();
                return (g != null) ? g.isPurchased : isPurchased;
            }
            else if(upgradeType == UtilityUpgradeType.Computer && utility != null)
            {
                Computer c = utility.GetComponent<Computer>();
                return (c != null) ? c.IsPurchased : isPurchased;
            }
            return isPurchased;
        }
        set {
            isPurchased = value;
            if (upgradeType == UtilityUpgradeType.VendingMachine && utility != null)
            {
                VendingMachine vm = utility.GetComponent<VendingMachine>();
                if (vm != null) vm.SetPurchased(value);
            }
            else if (upgradeType == UtilityUpgradeType.Toilet && utility != null)
            {
                Toilet t = utility.GetComponent<Toilet>();
                if (t != null) t.SetPurchased(value);
            }
            else if (upgradeType == UtilityUpgradeType.Generator && utility != null)
            {
                Generator g = utility.GetComponent<Generator>();
                if (g != null) g.SetPurchased(value);
            }
            else if (upgradeType == UtilityUpgradeType.Computer && utility != null)
            {
                Computer c = utility.GetComponent<Computer>();
                if (c != null) c.SetPurchased(value, c);
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();

        if (upgradeType == UtilityUpgradeType.VendingMachine)
        {
            VendingMachine vm = Resources.FindObjectsOfTypeAll<VendingMachine>()
                                  .FirstOrDefault(x => x.ID == UtilityID);
            if (vm != null)
            {
                utility = vm.gameObject;
            }
            else
            {
                Debug.LogError($"VendingMachine with UtilityID {UtilityID} not found!");
            }
        }
        else if (upgradeType == UtilityUpgradeType.Toilet)
        {
            Toilet t = Resources.FindObjectsOfTypeAll<Toilet>()
                        .FirstOrDefault(x => x.ID == UtilityID);
            if (t != null)
            {
                utility = t.gameObject;
            }
            else
            {
                Debug.LogError($"Toilet with UtilityID {UtilityID} not found!");
            }
        }
        else if (upgradeType == UtilityUpgradeType.Generator)
        {
            Generator g = Resources.FindObjectsOfTypeAll<Generator>()
                        .FirstOrDefault(x => x.ID == UtilityID);
            if (g != null)
            {
                utility = g.gameObject;
            }
            else
            {
                Debug.LogError($"Toilet with UtilityID {UtilityID} not found!");
            }
        }
        else if (upgradeType == UtilityUpgradeType.Computer)
        {
            Computer c = Resources.FindObjectsOfTypeAll<Computer>()
                        .FirstOrDefault(x => x.ID == UtilityID);
            if (c != null)
            {
                utility = c.gameObject;
            }
            else
            {
                Debug.LogError($"Toilet with UtilityID {UtilityID} not found!");
            }
        }

        if (utility != null)
        {
            room = utility.GetComponentInParent<Room>();
            if (room == null)
            {
                Debug.LogError("Room not found in parents for utility with UtilityID " + UtilityID);
            }
        }
        else
        {
            Debug.LogError("Utility object not found for UtilityID " + UtilityID);
        }
        if (IsPurchased && utility != null)
        {
            utility.SetActive(true);
        }
    }

    public override void OnUpgradeClicked()
    {
        if (upgradeType == UtilityUpgradeType.Computer)
        {
            if (!IsPurchased)
            {
                PurchaseUtilityObject();
            }
            else
            {
                Debug.Log("ПК вже куплено. Апгрейд недоступний.");
            }
            return;
        }
        if (!IsPurchased)
        {
            PurchaseUtilityObject();
        }
        else
        {
            UpgradeUtilityObject();
        }
    }

    
    private void PurchaseUtilityObject()
    {
        if (EconomyManager.Instance.Cash < purchaseCost)
        {
            return;
        }

        if (UpdateShopDataUI.Instance.UpgradePanel != null && !UpdateShopDataUI.Instance.UpgradePanel.activeSelf)
        {
            UpdateShopDataUI.Instance.UpgradePanel.SetActive(true);
        }

        EconomyManager.Instance.SubtractCash(purchaseCost);
        IsPurchased = true;

        if (utility != null)
        {
            utility.SetActive(true);

            if (upgradeType == UtilityUpgradeType.VendingMachine)
            {
                VendingMachine vm = utility.GetComponent<VendingMachine>();
                if (vm != null)
                {
                    if (room != null && !room.VendingMachines.Contains(vm))
                    {
                        QuestManager.Instance.UpdateQuestProgress(QuestType.BuyObject, vm.QuestLink, 1);
                        room.VendingMachines.Add(vm);
                        vm.UpgradeLevel = UpgradeLevel;
                        vm.SetIncome(CurrentIncome);
                    }
                }
            }
            else if (upgradeType == UtilityUpgradeType.Toilet)
            {
                Toilet t = utility.GetComponent<Toilet>();
                if (t != null && room != null && !room.Toilets.Contains(t))
                {
                    QuestManager.Instance.UpdateQuestProgress(QuestType.BuyObject, t.QuestLink, 1);
                    room.Toilets.Add(t);
                    IncomeManager.Instance.AddBonusIncome(CurrentIncome);
                }
            }
            else if (upgradeType == UtilityUpgradeType.Generator)
            {
                Generator g = utility.GetComponent<Generator>();
                if (g != null && room != null && !room.Generators.Contains(g))
                {
                    QuestManager.Instance.UpdateQuestProgress(QuestType.BuyObject, g.QuestLink, 1);
                    room.Generators.Add(g);
                    EconomyManager.Instance.AddEnergy(g.EnergyProduction);
                }
            }
            else if (upgradeType == UtilityUpgradeType.Computer)
            {
                Computer c = utility.GetComponent<Computer>();
                if (c != null && room != null && !room.Computers.Contains(c))
                {
                    QuestManager.Instance.UpdateQuestProgress(QuestType.BuyObject, c.QuestLink, 1);
                    room.Computers.Add(c);
                }
            }
        }

        UpdateShopDataUI.Instance.UpdateUpgradePanelUI(this);

        if (room != null)
        {
            GameManager.Instance.SaveRoomUtilities(room.id);
        }
    }


    private void UpgradeUtilityObject()
    {
        if (IsMaxUpgradeLevel())
        {
            return;
        }
        
        if (EconomyManager.Instance.Cash < CurrentCost)
        {
            return;
        }
        
        if ((upgradeType == UtilityUpgradeType.VendingMachine || upgradeType == UtilityUpgradeType.Toilet) &&
            EconomyManager.Instance.Energy < _energyConsumptionPerUpgrade)
        {
            Debug.Log("Недостатньо енергії для апгрейду " + upgradeType);
            return;
        }
        
        EconomyManager.Instance.SubtractCash(CurrentCost);
        
        float previousGeneratorProduction = 0f;
        if (upgradeType == UtilityUpgradeType.Generator && UpgradeLevel > 1)
        {
            previousGeneratorProduction = _initialGeneratorEnergyProduction * Mathf.Pow(_generatorEnergyProductionMultiplier, UpgradeLevel - 2);
        }
        
        LevelUp();
        
        if (utility != null)
        {
            if (upgradeType == UtilityUpgradeType.VendingMachine)
            {
                EconomyManager.Instance.SubtractEnergy(_energyConsumptionPerUpgrade);
                VendingMachine vm = utility.GetComponent<VendingMachine>();
                if (vm != null)
                {
                    QuestManager.Instance.UpdateQuestProgress(QuestType.BuyUpgrade, base.UpgradeLinkID, 1);
                    vm.AddIncome(CurrentIncome);
                    vm.UpgradeLevel = UpgradeLevel;
                }
            }
            else if (upgradeType == UtilityUpgradeType.Toilet)
            {
                EconomyManager.Instance.SubtractEnergy(_energyConsumptionPerUpgrade);
                Toilet t = utility.GetComponent<Toilet>();
                if (t != null)
                {
                    QuestManager.Instance.UpdateQuestProgress(QuestType.BuyUpgrade, base.UpgradeLinkID, 1);
                    IncomeManager.Instance.AddBonusIncome(CurrentIncome);
                }
            }
            else if (upgradeType == UtilityUpgradeType.Generator)
            {
                Generator g = utility.GetComponent<Generator>();
                if (g != null)
                {
                    QuestManager.Instance.UpdateQuestProgress(QuestType.BuyUpgrade, base.UpgradeLinkID, 1);
                    float newProduction = GeneratorEnergyProduction;
                    float additionalProduction = newProduction - previousGeneratorProduction;
                    EconomyManager.Instance.AddEnergy(additionalProduction);
                }
            }
        }
        
        IncomeManager.Instance.AddIncome(CurrentIncome);
        RatingManager.Instance.AddRating(CurrentUpgradeRating);
        UpdateShopDataUI.Instance.UpdateUpgradePanelUI(this);
        if (room != null)
            GameManager.Instance.SaveRoomUtilities(room.id);
    }


    
    public T ReturnObject<T>() where T : Component
    {
        if (upgradeType == UtilityUpgradeType.VendingMachine)
        {
            return utility.GetComponent<VendingMachine>() as T;
        }
        else if(upgradeType == UtilityUpgradeType.Toilet)
        {
            return utility.GetComponent<Toilet>() as T;
        }
        else {
            return utility.GetComponent<Generator>() as T;
        }
    }
}
