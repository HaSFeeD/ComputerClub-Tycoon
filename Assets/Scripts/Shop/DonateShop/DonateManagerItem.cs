using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DonateManagerItem : MonoBehaviour
{
    public string BonusName;
    public int bonusOfflineTime;
    private bool isBonusPurchased;
    [SerializeField] private Button purchaseButton;
    private void Start()
    {

    }
    public void SetPurchased(){
        isBonusPurchased = true;
        if(isBonusPurchased){
            purchaseButton.interactable = false;
        }
        GameManager.Instance.SaveDonateBonuses(BonusName, GetInfo());
    }
    public bool GetInfo(){
        return isBonusPurchased;
    }

}
