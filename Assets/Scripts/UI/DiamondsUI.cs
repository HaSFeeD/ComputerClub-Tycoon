using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiamondsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _diamondsText;
    private float _currentDiamonds;

    private void Awake()
    {
        _currentDiamonds = EconomyManager.Instance.Diamonds;
    }
    private void Update(){

    }
    private void UpdateDiamondsText(float amount){
        _currentDiamonds = amount;
    }
    private void OnEnable()
    {
        EconomyManager.Instance.OnDiamondChanged += UpdateDiamondsText;
    }

    private void OnDisable()
    {
        EconomyManager.Instance.OnDiamondChanged -= UpdateDiamondsText;
    }
}
