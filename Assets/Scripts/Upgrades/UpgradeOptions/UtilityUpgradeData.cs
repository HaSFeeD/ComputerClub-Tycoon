using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public enum UtilityUpgradeType {
    VendingMachine,
    Toilet
}

public class UtilityUpgradeData : UpgradeData
{
    private GameObject utility;
    private Room room;

    [SerializeField] public int UtilityID;
    [SerializeField] public UtilityUpgradeType upgradeType;
    [SerializeField] private float purchaseCost = 200f;
    public float PurchaseCost => purchaseCost;

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
            Debug.Log("Недостатньо коштів для покупки " + UpgradeName);
            return;
        }
        
        EconomyManager.Instance.SubtractCash(purchaseCost);
        IsPurchased = true;
        Debug.Log("Куплено об'єкт: " + UpgradeName + ", IsPurchased = " + IsPurchased);
        
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
                    room.Toilets.Add(t);
                    t.SetIncome(CurrentIncome);
                }
            }
        }
        
        UpdateShopDataUI.Instance.UpdateUpgradePanelUI(this);
        if(room != null)
            GameManager.Instance.SaveRoomUtilities(room.id);
    }
    
    private void UpgradeUtilityObject()
    {
        if (IsMaxUpgradeLevel())
        {
            Debug.Log("Досягнуто максимальний рівень для " + UpgradeName);
            return;
        }
        
        if (EconomyManager.Instance.Cash < CurrentCost)
        {
            Debug.Log("Недостатньо коштів для апгрейду " + UpgradeName);
            return;
        }
        
        EconomyManager.Instance.SubtractCash(CurrentCost);
        LevelUp();
        Debug.Log("Покращено " + UpgradeName + " до рівня " + UpgradeLevel);
        
        if (utility != null)
        {
            if (upgradeType == UtilityUpgradeType.VendingMachine)
            {
                VendingMachine vm = utility.GetComponent<VendingMachine>();
                if (vm != null)
                {
                    vm.AddIncome(CurrentIncome);
                    vm.UpgradeLevel = UpgradeLevel;
                }
            }
            else if (upgradeType == UtilityUpgradeType.Toilet)
            {
                Toilet t = utility.GetComponent<Toilet>();
                if (t != null)
                {
                    t.AddIncome(CurrentIncome);
                }
            }
        }
        
        IncomeManager.Instance.AddIncome(CurrentIncome);
        RatingManager.Instance.AddRating(CurrentUpgradeRating);
        UpdateShopDataUI.Instance.UpdateUpgradePanelUI(this);
        if(room != null)
            GameManager.Instance.SaveRoomUtilities(room.id);
    }
    
    public T ReturnObject<T>() where T : Component
    {
        if (upgradeType == UtilityUpgradeType.VendingMachine)
        {
            return utility.GetComponent<VendingMachine>() as T;
        }
        else
        {
            return utility.GetComponent<Toilet>() as T;
        }
    }
}
