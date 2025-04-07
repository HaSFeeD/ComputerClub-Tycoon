using System;
using UnityEngine;

public class IncomeManager : MonoBehaviour
{
    public static IncomeManager Instance;
    private float _basicIncome = 1;
    private float _offlineIncome = 0;
    private float _upgradeIncome = 0;
    private float _bonusIncome = 0;

    private void OnEnable(){
        
    }
    private void Awake(){
        Instance = this;
    }
    void Start()
    {
        RecalculateUpgradeIncome();
        OfflineEarning.Instance.CheckBonuses();
        _offlineIncome = OfflineEarning.Instance.CalculateOfflineIncome();
        OfflineIncomeUI.Instance.SetIncomeUIText(_offlineIncome.ToString("F2"), OfflineEarning.Instance.OfflineTime.ToString());
        OfflineIncomeUI.Instance.SetIncomePercentOfMaxIncome(Convert.ToString(GlobalTimeManager.Instance.GetOfflineTime()), Convert.ToString(OfflineEarning.Instance.initialOfflineIncomeTimeInSeconds));
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
    public float GetBonusIncome(){
        return _bonusIncome;
    }
    public void SetBonusIncome(float amount){
        _bonusIncome = amount;
    }
    public void AddBonusIncome(float amount){
        _bonusIncome += amount;
    }
    public void AddOfflineIncome(){
        EconomyManager.Instance.AddCash(_offlineIncome);
    }
    public void AddX2OfflineIncome(){
        EconomyManager.Instance.AddCash(_offlineIncome * 2);
    }
    public void AddX3OfflineIncome(){
        EconomyManager.Instance.AddCash(_offlineIncome * 3);
        EconomyManager.Instance.SubtractDiamonds(50);
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
