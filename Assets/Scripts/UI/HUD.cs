using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD Instance;
    [Header("Cash")]
    [SerializeField] private TextMeshProUGUI _balanceText;
    private float _currentBalance;
    [Header("Diamonds")]
    [SerializeField] private TextMeshProUGUI _diamondsText;
    [SerializeField] private TextMeshProUGUI _donateShopDiamondsText;
    private float _currentDiamonds;
    [Header("DayTime")]
    [SerializeField] public TextMeshProUGUI _hoursTextMesh;
    [Header("Rating")]
    [SerializeField] private Image _ratingImage;
    private float _currentRating;
    //MANAGERS
    //Energy
    [SerializeField]
    private TextMeshProUGUI _currentEnergyText;
    private float _currentEnergy;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (EconomyManager.Instance != null)
        {
            _currentBalance = EconomyManager.Instance.Cash;
            UpdateBalanceText(_currentBalance);
            _currentDiamonds = EconomyManager.Instance.Diamonds;
            UpdateDiamondText(_currentDiamonds);
            _currentEnergy = EconomyManager.Instance.Energy;
            UpdateEnergyText(_currentEnergy);
        }
        if(RatingManager.Instance != null){
            _currentRating = RatingManager.Instance.GetCurrentRating();
            UpdateRatingText(_currentRating / 10);
        }

    }
    private void UpdateRatingText(float amount){
        _currentRating = amount;
        _ratingImage.fillAmount = _currentRating;
    }
    private void UpdateBalanceText(float newBalance)
    {
        _currentBalance = newBalance;
        if (_currentBalance >= 1_000_000)
        {
            _balanceText.text = (_currentBalance / 1_000_000).ToString("F2") + "M";
        }
        else if (_currentBalance >= 1_000)
        {
            _balanceText.text = (_currentBalance / 1_000).ToString("F2") + "K";
        }
        else
        {
            _balanceText.text = _currentBalance.ToString("F2");
        }
    }
    private void UpdateDiamondText(float amount)
    {
        _currentDiamonds = amount;
        if (_currentDiamonds >= 1_000_000)
        {
            _diamondsText.text = (_currentDiamonds / 1_000_000).ToString("F2") + "M";
            _donateShopDiamondsText.text = (_currentDiamonds / 1_000_000).ToString("F2") + "M";
        }
        else if (_currentDiamonds >= 1_000)
        {
            _diamondsText.text = (_currentDiamonds / 1_000).ToString("F2") + "K";
            _donateShopDiamondsText.text = (_currentDiamonds / 1_000).ToString("F2") + "K";
        }
        else
        {
            _diamondsText.text = _currentDiamonds.ToString("F2");
            _donateShopDiamondsText.text = _currentDiamonds.ToString("F2");

        }
    }
    private void UpdateEnergyText(float amount){
        _currentEnergy = amount;
        _currentEnergyText.text = _currentEnergy.ToString("F2");
    }

    private void OnEnable()
    {
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.OnCashChanged += UpdateBalanceText;
            EconomyManager.Instance.OnDiamondChanged += UpdateDiamondText;
            EconomyManager.Instance.OnEnergyChanged += UpdateEnergyText;
        }
        if(RatingManager.Instance != null){
            RatingManager.Instance.OnRatingChanged += UpdateRatingText;
        }
    }

    private void OnDisable()
    {
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.OnCashChanged -= UpdateBalanceText;
            EconomyManager.Instance.OnDiamondChanged -= UpdateDiamondText;
            EconomyManager.Instance.OnEnergyChanged -= UpdateEnergyText;
        }
        if(RatingManager.Instance != null){
            RatingManager.Instance.OnRatingChanged -= UpdateRatingText;
        }
    }
}
