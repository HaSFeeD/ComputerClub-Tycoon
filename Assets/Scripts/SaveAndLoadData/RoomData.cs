using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomData
{
    public int RoomID;
    public int purhcasedObjects;
    public float RoomIncome;
    public List<UpgradeSaveData> upgrades;
    public List<VendingMachineSaveData> vendingMachines;
    public List<ToiletSaveData> toilets;
    public List<GeneratorSaveData> generators;
    public List<ComputerData> computers;
}
