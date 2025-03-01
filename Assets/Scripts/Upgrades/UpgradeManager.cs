using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }
    public List<UpgradeData> AllUpgrades = new List<UpgradeData>();
    public List<UtilityUpgradeData> AllUtilityUpgrades = new List<UtilityUpgradeData>();

    private void Awake()
    {
        Instance = this;
        FindAllUpgrades();
    }

    private void FindAllUpgrades()
    {
        AllUpgrades = Resources.FindObjectsOfTypeAll<UpgradeData>().ToList();
        AllUtilityUpgrades = Resources.FindObjectsOfTypeAll<UtilityUpgradeData>().ToList();
    }
}
