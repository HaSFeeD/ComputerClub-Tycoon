using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class GamingRoomUpgradesManager : MonoBehaviour
{
    private int _roomId;
    private List<UpgradeData> _roomUpgrades = new List<UpgradeData>();
    [SerializeField] private GameObject[] _roomUpgradesPrefabs;
    private List<GameObject> _instantiatedUpgrades;
    private GameObject _upgradePanel;
    private GameObject _upgradeItemsContainer;
    private GameObject _upgradeElement;
    [SerializeField]
    private Button _purchaseRoomObjectButton;

    private Vector3 _mouseDownPos;
    private float _clickThreshold = 10f;
    private Room room;
    private void Awake()
    {
        _upgradePanel = UpdateShopDataUI.Instance.UpgradePanel;
        _upgradeItemsContainer = UpdateShopDataUI.Instance.UpgradeItemsContainer;
        _upgradeElement = UpdateShopDataUI.Instance.ShopUpgradeElement;
        InitializeUpgrades();
        _instantiatedUpgrades = new List<GameObject>();
        room = GetComponent<Room>();
        if (room != null)
            _roomId = room.id;
        else
            Debug.LogError("Room component not found in RoomUpgradesManager");
    }

    private void InitializeUpgrades()
    {
        foreach (var prefab in _roomUpgradesPrefabs)
            _roomUpgrades.Add(prefab.GetComponent<UpgradeData>());
    }

    public List<UpgradeData> GetAllRoomUpgrades()
    {
        return _roomUpgrades;
    }

    private void OnMouseDown()
    {
        _mouseDownPos = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        if (Vector3.Distance(_mouseDownPos, Input.mousePosition) < _clickThreshold)
        {
            if(room._roomAvailable){
                OpenShop();
            }
        }
    }

    private void OpenShop()
    {
        _upgradeElement.SetActive(false);
        ClearUpgradePanel();
        if (_instantiatedUpgrades.Count > 0)
        {
            foreach (var obj in _instantiatedUpgrades)
                obj.transform.SetParent(_upgradeItemsContainer.transform);
        }
        else
        {
            foreach (var prefab in _roomUpgradesPrefabs)
            {
                GameObject temp = Instantiate(prefab, _upgradeItemsContainer.transform);
                UpgradeData upData = temp.GetComponent<UpgradeData>();
                if (upData != null)
                    upData.SetRoomUpgradeManager(this);
                _instantiatedUpgrades.Add(temp);
            }
        }
        _upgradePanel.SetActive(true);
        InitializePurchaseObjectButton();
    }
    private void InitializePurchaseObjectButton(){
        _purchaseRoomObjectButton.onClick.RemoveAllListeners();
        _purchaseRoomObjectButton.onClick.AddListener(() => ComputerManager.Instance.BuyPC(_roomId));
    }

    private void ClearUpgradePanel()
    {
        List<Transform> children = _upgradeItemsContainer.transform.Cast<Transform>().ToList();
        foreach (var child in children)
            child.SetParent(null);
    }

    public void UpgradeLevelUp(UpgradeData upgradeData)
    {
        if (EconomyManager.Instance.Cash < upgradeData.CurrentCost)
            return;
        EconomyManager.Instance.SubtractCash(upgradeData.CurrentCost);
        upgradeData.LevelUp();
        room.AddRoomIncome(upgradeData.CurrentIncome);
        IncomeManager.Instance.AddIncome(upgradeData.CurrentIncome);
        RatingManager.Instance.AddRating(upgradeData.CurrentUpgradeRating);
        UpdateShopDataUI.Instance.UpdateUpgradePanelUI(upgradeData);
        GameManager.Instance.SaveRoomUpgrade(upgradeData, _roomId);
    }

    public void LoadUpgrades(List<UpgradeSaveData> savedUpgrades)
    {
        if (_instantiatedUpgrades != null && _instantiatedUpgrades.Count > 0)
        {
            foreach (var obj in _instantiatedUpgrades)
                Destroy(obj);
            _instantiatedUpgrades.Clear();
        }
        foreach (var prefab in _roomUpgradesPrefabs)
        {
            int savedLevel = 0;
            UpgradeData prefabData = prefab.GetComponent<UpgradeData>();
            if (savedUpgrades != null)
            {
                var saved = savedUpgrades.Find(x => x.UpgradeName == prefabData.UpgradeName);
                if (saved != null)
                    savedLevel = saved.UpgradeLevel;
            }
            GameObject temp = Instantiate(prefab, _upgradeItemsContainer.transform);
            UpgradeData newData = temp.GetComponent<UpgradeData>();
            if (newData != null)
            {
                newData.SetRoomUpgradeManager(this);
                if (savedLevel > 0)
                    newData.SetLevel(savedLevel);
            }
            _instantiatedUpgrades.Add(temp);
        }
    }
    public int GetRoomID(){
        return _roomId;
    }
}
