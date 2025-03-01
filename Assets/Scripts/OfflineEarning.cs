using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineEarning : MonoBehaviour
{
    public static OfflineEarning Instance;
    private float _offlineIncome;
    private int _botsOfflineServed;
    public int maxOfflineIncomeTimeInSeconds = 7200;
    
    void Awake()
    {
        Instance = this;
    }
    
    private int BotsServedOffline(){
        long offlineTime = GlobalTimeManager.Instance.GetOfflineTime();
        if(offlineTime > maxOfflineIncomeTimeInSeconds){
            offlineTime = maxOfflineIncomeTimeInSeconds;
        }
        float randomBotSpawnTime = BotSpawnTimeManager.Instance.GetSpawnTime();
        _botsOfflineServed = Convert.ToInt32(offlineTime / randomBotSpawnTime);
        Debug.Log(_botsOfflineServed);
        return _botsOfflineServed;
    }
    public float CalculateOfflineIncome(){
        _offlineIncome = BotsServedOffline() * (IncomeManager.Instance.GetIncome() / 2);
        return _offlineIncome;
    }
    

}
