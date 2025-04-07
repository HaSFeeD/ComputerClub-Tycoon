using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum RoomType{
    Computer,
    Toilet,
    VendingMachine,
    Generator
}

public class Room : MonoBehaviour
{
    public int id;
    public string RoomLinkID;
    public float _roomIncome;
    public RoomType roomType;
    public GameObject BlockRoomGameobject;
    public bool _roomAvailable = false;
    [SerializeField] private bool _hasUpgrades = false;
    public List<Computer> Computers = new List<Computer>();
    public List<Toilet> Toilets = new List<Toilet>();
    public List<VendingMachine> VendingMachines = new List<VendingMachine>();
    public List<Generator> Generators = new List<Generator>();
    public int ObjectsCount;
    public RoomUpgradesManager roomUpgradesManager;

    private void Awake()
    {
        roomUpgradesManager = GetComponentInChildren<RoomUpgradesManager>();
        Computers = GetComponentsInChildren<Computer>().ToList();
        Toilets = GetComponentsInChildren<Toilet>(true).ToList();
        VendingMachines = GetComponentsInChildren<VendingMachine>(true).ToList();
        Generators = GetComponentsInChildren<Generator>(true).ToList();
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
        if (typeof(T) == typeof(Generator))
            return Generators as List<T>;
        return new List<T>();
    }
    public void AddRoomIncome(float amount){
        _roomIncome += amount;
        Debug.Log("Room:" + id + " income: " + _roomIncome);
    }
}
