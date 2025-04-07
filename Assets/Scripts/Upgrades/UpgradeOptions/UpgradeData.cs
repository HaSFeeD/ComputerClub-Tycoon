using System;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeData : MonoBehaviour
{
    [SerializeField] private string _upgradeName = "Default Upgrade";
    public string UpgradeName => _upgradeName;
    [SerializeField] protected string UpgradeLinkID;

    [SerializeField] private int _upgradeLevel = 1;
    public int UpgradeLevel => _upgradeLevel;

    [SerializeField] private int _initialCost = 50;
    public int BaseCost => _initialCost;

    [SerializeField] private float _costMultiplier = 1.9f;
    public int CurrentCost => Convert.ToInt32(_initialCost * Mathf.Pow(_costMultiplier, _upgradeLevel - 1));

    [SerializeField] private float _upgradeIncome = 1f;
    public float UpgradeIncome => _upgradeIncome;

    [SerializeField] private float _incomeMultiplier = 1.5f;
    public float IncomeMultiplier => _incomeMultiplier;

    private float _usageTimeDecrease = 0.1f; 
    public float CurrentIncome => _upgradeIncome * Mathf.Pow(_incomeMultiplier, _upgradeLevel - 1);

    private float _ratingPerLevelUp;
    public float CurrentUpgradeRating => _ratingPerLevelUp;

    public int _maxUpgradeLevel{ get; private set; } = 5;
    public bool IsMaxUpgradeLevel() => _upgradeLevel >= _maxUpgradeLevel;

    public Sprite UpgradeIcon;
    protected GameObject _upgradePanel;
    protected Button _upgradeButton;
    protected TextMeshProUGUI _levelTextInShop;

    private RoomUpgradesManager _roomUpgradeManager;

    protected virtual void Awake()
    {
        _ratingPerLevelUp = RatingManager.Instance.GetRatingOfEachUpgrade();

        _upgradePanel = Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(obj => obj.name == "ShopUpgradeElement");

        _upgradeButton = GetComponentInChildren<Button>();
        _levelTextInShop = GetComponentInChildren<TextMeshProUGUI>();

        UpdateButtonListeners();
        UpdateLevelUI();
    }
    public virtual void OnUpgradeClicked()
    {
        Debug.Log("UpgradeIncome " + CurrentIncome);
        if (IsMaxUpgradeLevel())
        {
            Debug.Log("Max Level reached for " + _upgradeName);
            return;
        }

        if (_roomUpgradeManager != null){
            _roomUpgradeManager.UpgradeLevelUp(this);
            QuestManager.Instance.UpdateQuestProgress(QuestType.BuyUpgrade, UpgradeLinkID, 1);
        }
        else
            Debug.LogWarning("RoomUpgradeManager is null. Check if you called SetRoomUpgradeManager().");
    }

    public void SetRoomUpgradeManager(RoomUpgradesManager manager)
    {
        _roomUpgradeManager = manager;
    }

    public RoomUpgradesManager GetRoomUpgradeManager(){
        return _roomUpgradeManager;
    }

    public void LevelUp()
    {
        if (!IsMaxUpgradeLevel())
        {
            _upgradeLevel++;
            UpdateLevelUI();
        }
    }

    public void SetLevel(int level)
    {
        _upgradeLevel = level;
        UpdateLevelUI();
    }

    public void UpdateLevelUI()
    {
        if (_levelTextInShop != null)
            _levelTextInShop.text = _upgradeLevel.ToString();
    }

    private void UpdateButtonListeners()
    {
        if (_upgradeButton == null) return;

        _upgradeButton.onClick.RemoveAllListeners();
        _upgradeButton.onClick.AddListener(() => {
        UpdateShopDataUI.Instance.UpdateUpgradePanelUI(this);
            if (_upgradePanel != null) 
                _upgradePanel.SetActive(true);
        });
    }

    public float GetIncreasedIncome()
    {
        return _upgradeIncome * Mathf.Pow(_incomeMultiplier, _upgradeLevel - 1);
    }
}
