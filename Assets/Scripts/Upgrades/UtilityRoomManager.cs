using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum UtilityRoomType {
    Toilet,
    VendingMachine
}

public class UtilityRoomManager : MonoBehaviour
{
    public UtilityRoomType utilityRoomType;
    [SerializeField] private GameObject[] _objectPrefabs;
    [SerializeField] private GameObject[] _objectsPlacementPoint;
    private Vector3 _mouseDownPos;
    private float _clickThreshold = 10f;
    private Room room;

    public int _roomId;
    public void UpgradeLevelUp(UtilityUpgradeData vendingMachineUpgradeData){

    }
    public int GetRoomID(){
        return _roomId;
    }
}
