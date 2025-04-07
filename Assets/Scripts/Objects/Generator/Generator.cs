using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] public int ID;
    [SerializeField] public string QuestLink;
    [SerializeField] public float Income;
    public float EnergyProduction;
    public bool isPurchased = false;
    [SerializeField] private int upgradeLevel = 0;
    public int UpgradeLevel { get => upgradeLevel; set => upgradeLevel = value; }
    
    public void AddIncome(float amount)
    {
        Income += amount;
    }
    
    public void SetIncome(float amount)
    {
        Income = amount;
    }
    
    public void UpdateIncome(float newIncome)
    {
        Income = newIncome;
    }
    
    public void SetPurchased(bool value)
    {
        isPurchased = value;
    }
}
