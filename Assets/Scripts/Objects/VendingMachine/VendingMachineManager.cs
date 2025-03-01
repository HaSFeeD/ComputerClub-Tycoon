using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachineManager : MonoBehaviour {
    [SerializeField] private GameObject[] vendingMachinePrefabs;
    [SerializeField] private GameObject[] vendingMachinePlacePoint;
    private int nextPrefabIndex = 0;

    public void PurchaseVendingMachine(int roomId) {
        Room room = RoomManager.Instance.FindRoomByID(roomId);
        GameObject newMachine = Instantiate(vendingMachinePrefabs[nextPrefabIndex], vendingMachinePlacePoint[nextPrefabIndex].transform);
        nextPrefabIndex = (nextPrefabIndex + 1) % vendingMachinePrefabs.Length;
        newMachine.AddComponent<UtilityUpgradeData>();
        room.VendingMachines.Add(newMachine.GetComponent<VendingMachine>());
    }
}
