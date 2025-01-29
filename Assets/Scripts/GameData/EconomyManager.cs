using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }
    public event Action<float> OnCashChanged;
    public event Action<float> OnDiamondChanged;
    public float Cash {get; private set;}
    public int Diamonds {get; private set;}
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Cash = 50.0f;
        Diamonds = 0;
    }

    public void AddCash(float amount){
        Cash += amount;
        OnCashChanged?.Invoke(Cash);
        Debug.Log("Баланс оновлено: " + Cash);
    }
    public void SubtractCash(int amount){
        Cash -= amount;
        OnCashChanged?.Invoke(Cash);
    }
    public void AddDiamonds(int amount){
        Diamonds += amount;
        OnDiamondChanged?.Invoke(Diamonds);
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
    
}
