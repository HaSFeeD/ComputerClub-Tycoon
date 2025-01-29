<<<<<<< HEAD
// RoomManager.cs
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomManager : MonoBehaviour
{
    private List<Room> Rooms = new List<Room>();

    void Start()
    {
        Rooms = FindObjectsOfType<Room>().ToList();
    }

    public T FindNearestAvailableObject<T>(Vector3 position) where T : MonoBehaviour, IAvailableObject
    {
        T nearestObject = null;
        float minDistance = float.MaxValue;

        foreach (var room in Rooms)
        {
            var availableObjects = room.GetAvailableObjects<T>().Where(obj => !obj.IsOccupied);
            foreach (var obj in availableObjects)
            {
                float distance = Vector3.Distance(position, obj.Position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestObject = obj;
                }
            }
        }

        return nearestObject;
    }
=======
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;
    public List<Room> PurchasedRooms = new List<Room>();
    public List<Room> AllRooms = new List<Room>();
    public List<Computer> Computers = new List<Computer>();
    public List<Toilet> Toilets = new List<Toilet>();
    public List<VendingMachine> VendingMachines = new List<VendingMachine>();

    private void Awake()
    {
        Instance = this;
        AllRooms.AddRange(FindObjectsOfType<Room>());
        Computers = FindObjectsOfType<Computer>().ToList();
        Toilets = FindObjectsOfType<Toilet>().ToList();
        VendingMachines = FindObjectsOfType<VendingMachine>().ToList();
    }
    private void Start(){
        foreach(var PurchasedRoom in AllRooms){
            if(PurchasedRoom._roomAvailable)
            {
            PurchasedRooms.Add(PurchasedRoom);
            }
        }
    }

    public Computer FindAvailableComputer()
    {
        foreach (var computer in Computers)
        {
            if (!computer.IsOccupied && computer.ReservedBy == null)
            {
                return computer;
            }
        }
        return null;
    }

    public T FindNearestAvailableObject<T>(Vector3 fromPosition) where T : MonoBehaviour, IUsableObject
    {
        List<Room> unlockedRooms = AllRooms.Where(r => r._roomAvailable).ToList();

        T nearest = null;
        float minDist = float.MaxValue;

        foreach (var room in unlockedRooms)
        {
            var objects = room.GetAvailableObjects<T>();
            if (objects == null) continue;

            foreach (var obj in objects)
            {
                if (!obj.IsOccupied)
                {
                    float dist = Vector3.Distance(fromPosition, obj.Position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearest = obj;
                    }
                }
            }
        }
        return nearest;
    }
    public Room FindRoomByID(int id){
        foreach(Room room in AllRooms){
            if(room.id == id) return room;
        }
        return null;
    }
    
>>>>>>> 27866b6 (Refactored Some Code and add new Features)
}
