using System;
using UnityEngine;

public class IncomeManager : MonoBehaviour
{
    public static IncomeManager Instance;
    private float _basicIncome = 1;
    private float _offlineIncome = 0;
    private float _upgradeIncome = 0;

    private void OnEnable(){
        
    }
    private void Awake(){
        Instance = this;
    }
    void Start()
    {
        RecalculateUpgradeIncome();
        _offlineIncome = OfflineEarning.Instance.CalculateOfflineIncome();
        OfflineIncome.Instance.SetIncomeUIText(Convert.ToString(_offlineIncome));
        OfflineIncome.Instance.SetIncomePercentOfMaxIncome(Convert.ToString(GlobalTimeManager.Instance.GetOfflineTime()), Convert.ToString(OfflineEarning.Instance.maxOfflineIncomeTimeInSeconds));
        Debug.Log(_offlineIncome);
    }
    public float GetIncome(){
        return _basicIncome;
    }
    public void SetIncome(float amount){
        _basicIncome = amount;
    }
    public void AddIncome(float amount){
        _basicIncome += amount;
    }
    public void AddOfflineIncome(){
        EconomyManager.Instance.AddCash(_offlineIncome);
    }
    public void AddX2OfflineIncome(){
        EconomyManager.Instance.AddCash(_offlineIncome * 2);
    }
    public void AddX3OfflineIncome(){
        EconomyManager.Instance.AddCash(_offlineIncome * 3);
    }
    public void RecalculateUpgradeIncome()
    {
        float total = 0;
        foreach(var upgrade in UpgradeManager.Instance.AllUpgrades){
            total += upgrade.CurrentIncome;
        }
        _upgradeIncome = total;
        Debug.Log("Total upgrade income recalculated: " + _upgradeIncome);
    }

}
