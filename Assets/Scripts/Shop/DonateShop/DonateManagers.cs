using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DonateManagers : MonoBehaviour
{
    public static DonateManagers Instance;
    public List<GameObject> Managers = new List<GameObject>();
    void Awake()
    {
        Instance = this;
    }
    public DonateManagerItem FindManagerByName(string name){
        foreach(var manager in Managers){
            var tempManager = manager.GetComponentInChildren<DonateManagerItem>();
            if(tempManager.BonusName == name){
                return tempManager;
            }
        }
        return null;
    }
}
