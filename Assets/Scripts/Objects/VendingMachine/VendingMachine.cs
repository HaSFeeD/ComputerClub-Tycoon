using UnityEngine;
using UnityEngine.Localization.SmartFormat.GlobalVariables;

public class VendingMachine : MonoBehaviour, IUsableObject
{
    [SerializeField] public int ID;
    [SerializeField] public string QuestLink;
    [SerializeField] private int upgradeLevel = 1;
    public int UpgradeLevel { get => upgradeLevel; set => upgradeLevel = value; }
    
    [SerializeField] public float Income;
    [SerializeField] public float UsageTime;
    
    public bool isPurchased = false;
    public bool IsOccupied { get; private set; }
    public Vector3 Position => transform.position;
    public BotController ReservedBy { get; private set; }
    
    public void SetPurchased(bool value)
    {
        isPurchased = value;
    }
    
    public bool TryOccupy(BotController bot)
    {
        if (!IsOccupied)
        {
            IsOccupied = true;
            ReservedBy = bot;
            return true;
        }
        return false;
    }

    public void Vacate(BotController bot)
    {
        if (ReservedBy == bot)
        {
            IsOccupied = false;
            ReservedBy = null;
        }
    }

    public void Reserve(BotController bot) { }
    public void ReleaseReservation(BotController bot) { }

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
}
