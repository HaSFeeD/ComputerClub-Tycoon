using System;
using TMPro;
using UnityEngine;

public class BalanceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _balanceText;
    private float _currentBalance;

    private void Awake()
    {
        _currentBalance = GameManager.instance.GetBalance();
        UpdateBalanceText(_currentBalance);
    }

    private void Update()
    {
        if (_currentBalance >= 1000)
        {
            _balanceText.text = (_currentBalance / 1000).ToString("F2") + "K";
        }
        else
        {
            _balanceText.text = _currentBalance.ToString("F2");
        }
    }

    private void UpdateBalanceText(float newBalance)
    {
        _currentBalance = newBalance;
    }

    private void OnEnable()
    {
        GameManager.instance.OnBalanceChanged += UpdateBalanceText;
    }

    private void OnDisable()
    {
        GameManager.instance.OnBalanceChanged -= UpdateBalanceText;
    }
}
