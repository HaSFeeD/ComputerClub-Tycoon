using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeJumpItem : MonoBehaviour
{
    public int skippedTimeInHours;
    [HideInInspector]
    public float cashPerSkippedTime;
    [SerializeField]
    private int _timeJumpCost;
    [SerializeField]
    private TextMeshProUGUI _timeJumpCostText;
    public TextMeshProUGUI cashPerSkippedTimeText;
    public Button purchaseTimeSkipButton;
    private void Start(){
        SetButtonListener();
        _timeJumpCostText.text = _timeJumpCost.ToString();
    }        
    public void SetCashPerSkippedTime(float spawnTime){
        cashPerSkippedTime = Calculate(spawnTime);
        cashPerSkippedTimeText.text = cashPerSkippedTime.ToString();
    }
    public float Calculate(float spawnTime){
        int countOfServedBots = OfflineEarning.Instance.BotsServedOffline(HoursToSeconds(skippedTimeInHours), spawnTime);
        float earnings = OfflineEarning.Instance.CalculateOfflineIncome(countOfServedBots);
        return earnings;
    }

    private int HoursToSeconds(int value){
        var time = value * 3600;
        Debug.Log(value + " + " + time);
        return time;
    }
    private void PurchaseTimeJump(){
        if(!(EconomyManager.Instance.Diamonds < _timeJumpCost)){
            EconomyManager.Instance.SubtractDiamonds(_timeJumpCost);
            EconomyManager.Instance.AddCash(cashPerSkippedTime);
        }
        
    }
    private void SetButtonListener(){
        purchaseTimeSkipButton.onClick.AddListener(() => PurchaseTimeJump());
    }
}
