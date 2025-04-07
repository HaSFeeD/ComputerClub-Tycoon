using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GlobalTimeManager : MonoBehaviour
{
    public static GlobalTimeManager Instance;
    private long _joinTime;
    private long _exitTime;
    private long _offlineTime;
    private void Awake(){
        Instance = this;
        _joinTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        _exitTime = Convert.ToInt64(PlayerPrefs.GetString("LastExitTime", "0"));

        if(_exitTime > 0){
            _offlineTime = _joinTime - _exitTime;
        }
        else {
            _offlineTime = 0;
        }
        Debug.Log("Offline Time: " + _offlineTime);
    }
    private void OnApplicationQuit(){
        SaveExitTime();
    }
    private void OnApplicationPause(bool pause){
        if(pause){
            SaveExitTime();
        }
    }
    private void SaveExitTime(){
        _exitTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        PlayerPrefs.SetString("LastExitTime", _exitTime.ToString());
        PlayerPrefs.Save();
    }
    public long GetExitTime(){
        return _exitTime;
    }
    public long GetJoinTime(){
        return _joinTime;
    }
    public long GetOfflineTime(){
        return _offlineTime;
    }
}
