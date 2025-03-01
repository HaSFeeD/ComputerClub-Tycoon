using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;
    public List<Room> PurchasedRooms = new List<Room>();
    public List<RoomItemData> roomItemDatas = new List<RoomItemData>();
    public List<Room> AllRooms = new List<Room>();
    public List<Computer> Computers = new List<Computer>();
    public List<Toilet> Toilets = new List<Toilet>();
    public List<VendingMachine> VendingMachines = new List<VendingMachine>();

    private void Awake()
    {
        Instance = this;
        AllRooms.AddRange(FindObjectsOfType<Room>());
        Computers = FindObjectsOfType<Computer>().ToList();
        Toilets = Resources.FindObjectsOfTypeAll<Toilet>().ToList();
        VendingMachines = Resources.FindObjectsOfTypeAll<VendingMachine>().ToList();
        roomItemDatas = Resources.FindObjectsOfTypeAll<RoomItemData>().ToList();
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
    public void RefreshComputers()
    {
        Computers = FindObjectsOfType<Computer>().ToList();
    }
}
