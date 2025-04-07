using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class OfflineEarning : MonoBehaviour
{
    public static OfflineEarning Instance;
    private float _offlineIncome;
    private int _botsOfflineServed;
    public int initialOfflineIncomeTimeInSeconds = 7200;
    public long OfflineTime;
    void Awake()
    {
        Instance = this;
    }

    public void CheckBonuses(){
        foreach(var donManager in DonateManagers.Instance.Managers){
            var manager = donManager.GetComponent<DonateManagerItem>();
            if(manager.GetInfo()){
                initialOfflineIncomeTimeInSeconds += manager.bonusOfflineTime;
            }
        }
    }
    
    private int BotsServedOffline(){
        OfflineTime = GlobalTimeManager.Instance.GetOfflineTime();
        if(OfflineTime > initialOfflineIncomeTimeInSeconds){
            OfflineTime = initialOfflineIncomeTimeInSeconds;
        }
        float randomBotSpawnTime = BotSpawnTimeManager.Instance.GetAdditionalSpawnTime();
        _botsOfflineServed = Convert.ToInt32(OfflineTime / randomBotSpawnTime);
        Debug.Log(_botsOfflineServed);
        return _botsOfflineServed;
    }
    public int BotsServedOffline(int offlineTime, float spawnTime){
        _botsOfflineServed = Convert.ToInt32(offlineTime / spawnTime);
        return _botsOfflineServed;
    }
    public float CalculateOfflineIncome(){
        _offlineIncome = BotsServedOffline() * (IncomeManager.Instance.GetIncome() / 1.2f);
        return _offlineIncome;
    }
    public float CalculateOfflineIncome(int botServedAmount){
        _offlineIncome = botServedAmount * (IncomeManager.Instance.GetIncome() / 2);
        return _offlineIncome;
    }
    public int SecondsToHours(int value){
        var time = value * 3600;
        return time;
    }

}
