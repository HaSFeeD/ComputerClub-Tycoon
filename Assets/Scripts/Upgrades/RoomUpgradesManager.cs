using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum ShopPageType
{
    Upgrades,
    Purchases
}

public class RoomUpgradesManager : MonoBehaviour, IRoomClickHandler
{
    public static RoomUpgradesManager CurrentActiveRoom;
    private int _roomId;
    private List<UpgradeData> _roomUpgrades = new List<UpgradeData>();
    [SerializeField] private GameObject[] _roomUpgradesPrefabs;
    [SerializeField] private GameObject[] _roomPurchasePrefabs;
    private List<GameObject> _instantiatedUpgrades;
    private GameObject _upgradePanel;
    private GameObject _upgradeItemsContainer;
    private GameObject _upgradeElement;

    private GameObject _nextPage;
    private GameObject _previousPage;


    private Vector3 _mouseDownPos;
    private float _clickThreshold = 100f;
    private Room room;
    private ShopPageType currentPage = ShopPageType.Upgrades;
    
    
    private void Awake()
    {
        _upgradePanel = UpdateShopDataUI.Instance.UpgradePanel;
        _upgradeItemsContainer = UpdateShopDataUI.Instance.UpgradeItemsContainer;
        _upgradeElement = UpdateShopDataUI.Instance.ShopUpgradeElement;
        _nextPage = UpdateShopDataUI.Instance.NextPageButton;
        _previousPage = UpdateShopDataUI.Instance.PreviousPage;

        InitializeUpgrades();
        _instantiatedUpgrades = new List<GameObject>();
        room = GetComponent<Room>();
        if (room != null)
            _roomId = room.id;
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

    public void HandleRoomClick(Vector2 clickPosition)
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Vector2 currentMousePos = Pointer.current != null
            ? Pointer.current.position.ReadValue()
            : Vector2.zero;


        if (room != null && room._roomAvailable)
        {
            OpenShop();
        }
    }
    public void NextPage()
    {
        if (RoomUpgradesManager.CurrentActiveRoom != null)
        {
            RoomUpgradesManager.CurrentActiveRoom.OnNextPageClicked();
        }
    }

    public void PreviousPage()
    {
        if (RoomUpgradesManager.CurrentActiveRoom != null)
        {
            RoomUpgradesManager.CurrentActiveRoom.OnPreviousPageClicked();
        }
    }
    
    private void OpenShop()
{
    CurrentActiveRoom = this;
    currentPage = ShopPageType.Upgrades;

    LoadShopPage();

    if (_upgradePanel != null && !_upgradePanel.activeSelf)
    {
        _upgradePanel.SetActive(true);
    }

    StartCoroutine(UpdatePanelAfterFrame());
}
    
    private IEnumerator UpdatePanelAfterFrame()
    {
        yield return new WaitForEndOfFrame();

        if (_upgradeItemsContainer != null && _upgradeItemsContainer.transform.childCount > 0)
        {
            UpgradeData firstUpgrade = _upgradeItemsContainer.transform.GetChild(0).GetComponent<UpgradeData>();
            if (firstUpgrade != null)
            {
                UpdateShopDataUI.Instance.UpdateUpgradePanelUI(firstUpgrade);
            }
        }
        if (room.roomType != RoomType.Computer)
        {
            _nextPage.SetActive(false);
            _previousPage.SetActive(false);
        }
        else
        {
            if (currentPage == ShopPageType.Upgrades)
            {
                _previousPage.SetActive(false);
                _nextPage.SetActive(true);
            }
            else if (currentPage == ShopPageType.Purchases)
            {
                _nextPage.SetActive(false);
                _previousPage.SetActive(true);
            }
        }

        Button nextPageBtn = _nextPage != null ? _nextPage.GetComponent<Button>() : null;
        if (nextPageBtn != null)
        {
            nextPageBtn.onClick.RemoveAllListeners();
            nextPageBtn.onClick.AddListener(OnNextPageClicked);
        }

        Button previousPageBtn = _previousPage != null ? _previousPage.GetComponent<Button>() : null;
        if (previousPageBtn != null)
        {
            previousPageBtn.onClick.RemoveAllListeners();
            previousPageBtn.onClick.AddListener(OnPreviousPageClicked);
        }
    }
    public void OnNextPageClicked()
    {
        currentPage = ShopPageType.Purchases;
        LoadShopPage();
        StartCoroutine(UpdatePanelAfterFrame());
    }

    public void OnPreviousPageClicked()
    {
        currentPage = ShopPageType.Upgrades;
        LoadShopPage();
        StartCoroutine(UpdatePanelAfterFrame());
    }

    private void LoadShopPage()
    {
        ClearShopPanel();

        switch (currentPage)
        {
            case ShopPageType.Upgrades:
                Debug.Log($"Завантажуємо апгрейди у кімнаті {gameObject.name}, префабів: {_roomUpgradesPrefabs.Length}");
                foreach (var prefab in _roomUpgradesPrefabs)
                {
                    GameObject temp = Instantiate(prefab, _upgradeItemsContainer.transform);
                    UpgradeData upgradeData = temp.GetComponent<UpgradeData>();
                    upgradeData.SetRoomUpgradeManager(this);
                    temp.GetComponent<Button>().onClick.RemoveAllListeners();
                    temp.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        UpdateShopDataUI.Instance.UpdateUpgradePanelUI(upgradeData);
                    });
                }
                break;

            case ShopPageType.Purchases:
                Debug.Log($"Завантажуємо покупки у кімнаті {gameObject.name}, префабів: {_roomPurchasePrefabs.Length}");
                foreach (var prefab in _roomPurchasePrefabs)
                {
                    GameObject temp = Instantiate(prefab, _upgradeItemsContainer.transform);
                    UpgradeData purchaseData = temp.GetComponent<UpgradeData>();
                    purchaseData.SetRoomUpgradeManager(this);
                    temp.GetComponent<Button>().onClick.RemoveAllListeners();
                    temp.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        UpdateShopDataUI.Instance.UpdateUpgradePanelUI(purchaseData);
                    });
                }
                break;
        }
    }
    private void ClearShopPanel()
    {
        foreach (Transform child in _upgradeItemsContainer.transform)
            Destroy(child.gameObject);
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
