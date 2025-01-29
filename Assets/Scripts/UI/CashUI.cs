using System;
using TMPro;
using UnityEngine;

public class CashUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _balanceText;
    private float _currentCash;

   private void Start()
    {
        if (EconomyManager.Instance != null)
        {
            _currentCash = EconomyManager.Instance.Cash;
            EconomyManager.Instance.OnCashChanged += OnCashChanged;
            OnCashChanged(_currentCash);
        }
    }

    private void Update()
    {
        if (_currentCash >= 1000)
        {
            _balanceText.text = (_currentCash / 1000).ToString("F2") + "K";
        }
        else
        {
            _balanceText.text = _currentCash.ToString("F2");
        }
    }

    private void OnCashChanged(float newBalance)
    {
        _currentCash = newBalance;
    }

    private void OnEnable()
    {
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.OnCashChanged += OnCashChanged;
            OnCashChanged(EconomyManager.Instance.Cash);
        }
    }

    private void OnDisable()
    {
        EconomyManager.Instance.OnCashChanged -= OnCashChanged;
    }
}
