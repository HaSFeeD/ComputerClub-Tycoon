using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.Localization;

public class Room : MonoBehaviour
{
    [SerializeField]
    public int id;
    [SerializeField]
    public GameObject BlockRoomGameobject;
    [SerializeField]
    public bool _roomAvailable = true;
    public List<Computer> Computers = new List<Computer>();
    public List<Toilet> Toilets = new List<Toilet>();
    public List<VendingMachine> VendingMachines = new List<VendingMachine>();

    private void Awake()
    {
        Computers = GetComponentsInChildren<Computer>().ToList();
        Toilets = GetComponentsInChildren<Toilet>().ToList();
        VendingMachines = GetComponentsInChildren<VendingMachine>().ToList();
    }

    public List<T> GetAvailableObjects<T>() where T : MonoBehaviour, IUsableObject
    {
        if(!_roomAvailable){return null;}
        if (typeof(T) == typeof(Computer))
            return Computers as List<T>;
        if (typeof(T) == typeof(Toilet))
            return Toilets as List<T>;
        if (typeof(T) == typeof(VendingMachine))
            return VendingMachines as List<T>;
        return new List<T>();
    }
    private async void BlockedRooms(){

    }
}
