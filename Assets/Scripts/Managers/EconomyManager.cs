using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }
    public event Action<float> OnCashChanged;
    public event Action<float> OnDiamondChanged;
    public event Action<float> OnEnergyChanged;
    public float Cash {get; private set;}
    public int Diamonds {get; private set;}
    public float Energy {get; private set;}
    public float DailyExpenses;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void AddCash(float amount)
    {
        Cash += amount;
        OnCashChanged?.Invoke(Cash);
    }
    public void SubtractCash(float amount)
    {
        Cash -= amount;
        OnCashChanged?.Invoke(Cash);
    }

    public void AddDiamonds(int amount){
        Diamonds += amount;
        OnDiamondChanged?.Invoke(Diamonds);
        GameManager.Instance.SaveData();
    }
    public void SubtractDiamonds(int amount){
        Diamonds -= amount;
        OnDiamondChanged?.Invoke(Diamonds);
    }
    public void SetCash(float amount){
        Cash = amount;
        OnCashChanged?.Invoke(Cash);
    }
    public void SetDiamonds (int amount){
        Diamonds = amount;
        OnDiamondChanged?.Invoke(Diamonds);
    }
    public void SetEnergy(float amount){
        Energy = amount;
        OnEnergyChanged?.Invoke(Energy);
    }
    public void AddEnergy(float amount){
        Energy += amount;
        OnEnergyChanged?.Invoke(Energy);
    }
    public void SubtractEnergy(float amount){
        Energy -= amount;
        OnEnergyChanged?.Invoke(Energy);
    }
    public float GetEnergy(){
        return Energy;
    }
    
}
