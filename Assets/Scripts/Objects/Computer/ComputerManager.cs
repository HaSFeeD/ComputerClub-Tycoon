using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComputerManager : MonoBehaviour
{
    public static ComputerManager Instance;
    private List<GameObject> _activePC = new List<GameObject>();                      
    [SerializeField]
    private GameObject _PCPrefab;                     
    private bool isLoading = false;                   

    private void Awake()
    {
        Instance = this;
    }


    public void BuyPC(int roomID, bool simulatePurchase = true, int index = 0)
    {
        Room room = RoomManager.Instance.FindRoomByID(roomID);
        if (room == null)
        {
            Debug.LogError("Room with ID " + roomID + " not found.");
            return;
        }

        List<GameObject> roomPCPlaces = room.GetComponentsInChildren<Transform>()
            .Where(t => t.CompareTag("PC_Place") && t.gameObject.activeSelf)
            .Select(t => t.gameObject)
            .ToList();
        if (roomPCPlaces.Count == 0)
        {
            Debug.LogError("No available PC places found in room " + roomID);
            return;
        }
        
        GameObject placePoint = roomPCPlaces[Mathf.Min(index, roomPCPlaces.Count - 1)];
        Vector3 spawnPosition = placePoint.transform.position;

        GameObject newPC = Instantiate(_PCPrefab, spawnPosition, Quaternion.identity);
        newPC.transform.rotation = placePoint.transform.rotation * Quaternion.Euler(0, 0, 180);
        newPC.transform.localScale = new Vector3(30, 30, 30);
        newPC.transform.position = spawnPosition;

        placePoint.SetActive(false);

        Computer computer = newPC.GetComponent<Computer>();
        if (computer != null)
        {
            computer.RoomID = roomID;
        }
        _activePC.Add(newPC);

        if (simulatePurchase)
        {
            room.ObjectsCount += 1;
            RoomManager.Instance.RefreshComputers();
            SavePC(room.id, room.ObjectsCount);
        }
        else
        {
            RoomManager.Instance.RefreshComputers();
        }
    }

    private void SavePC(int roomID, int objectsCount)
    {
        Room room = RoomManager.Instance.FindRoomByID(roomID);
        if (GameManager.Instance != null && room != null)
        {
            GameManager.Instance.SaveRoom(roomID, objectsCount, room._roomIncome);
        }
        else
        {
            Debug.LogError("GameManager instance is null or room not found in SavePC.");
        }
    }

    public void LoadPC(int purchasedComputers, int roomID)
    {
        isLoading = true;
        for (int i = 0; i < purchasedComputers; i++)
        {
            BuyPC(roomID, simulatePurchase: false, index: i);
        }
        isLoading = false;
    }

}
