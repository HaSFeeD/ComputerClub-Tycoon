using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.Localization;


public class Room : MonoBehaviour
{
    [SerializeField]
    public int id;
    public float _roomIncome;
    [SerializeField]
    public GameObject BlockRoomGameobject;
    [SerializeField]
    public bool _roomAvailable = false;
    [SerializeField]
    private bool _hasUpgrades = false;
    public List<Computer> Computers = new List<Computer>();
    public List<Toilet> Toilets = new List<Toilet>();
    public List<VendingMachine> VendingMachines = new List<VendingMachine>();
    public int ObjectsCount;
    public GamingRoomUpgradesManager roomUpgradesManager;

    private void Awake()
    {
        roomUpgradesManager = GetComponentInChildren<GamingRoomUpgradesManager>();
        Computers = GetComponentsInChildren<Computer>().ToList();
        Toilets = GetComponentsInChildren<Toilet>(true).ToList();
        VendingMachines = GetComponentsInChildren<VendingMachine>(true).ToList();
        if(_roomIncome == 0){
            _roomIncome = 1;
        }
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
    public void AddRoomIncome(float amount){
        _roomIncome += amount;
        Debug.Log("Room:" + id + " income: " + _roomIncome);
    }
}
