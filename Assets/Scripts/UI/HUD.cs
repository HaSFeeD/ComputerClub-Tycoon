using System;
using TMPro;
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
    private float _currentDiamonds;
    [Header("DayTime")]
    [SerializeField] public TextMeshProUGUI _hoursTextMesh;
    [SerializeField] public TextMeshProUGUI _minutesTextMesh;
    [Header("Rating")]
    [SerializeField] private Image _ratingImage;
    private float _currentRating;
    //MANAGERS
    //Energy

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (EconomyManager.Instance != null)
        {
            _currentBalance = EconomyManager.Instance.Cash;
            _currentDiamonds = EconomyManager.Instance.Diamonds;
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
        }
        else if (_currentDiamonds >= 1_000)
        {
            _diamondsText.text = (_currentDiamonds / 1_000).ToString("F2") + "K";
        }
        else
        {
            _diamondsText.text = _currentDiamonds.ToString("F2");
        }
    }

    private void OnEnable()
    {
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.OnCashChanged += UpdateBalanceText;
            EconomyManager.Instance.OnDiamondChanged += UpdateDiamondText;
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
        }
        if(RatingManager.Instance != null){
            RatingManager.Instance.OnRatingChanged -= UpdateRatingText;
        }
    }
}
