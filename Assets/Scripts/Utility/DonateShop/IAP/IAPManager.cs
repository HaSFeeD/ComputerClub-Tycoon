using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _purchaseButtons;
    [SerializeField]
    private DonateManagerItem _smallOfflineIncomeBonus;
    [SerializeField]
    private DonateManagerItem _mediumOfflineIncomeBonus;
    [SerializeField]
    private DonateManagerItem _largeOfflineIncomeBonus;
private void Start()
{
    RefreshPrice();
}

    public void OnPurchaseComplete(Product product)
    {
        switch(product.definition.id)
        {
            case "com.company.ComputerGame-Tycoon.500Diamonds": 
                EconomyManager.Instance.AddDiamonds(500);
                break;
            case "com.company.ComputerGame-Tycoon.1200Diamonds": 
                EconomyManager.Instance.AddDiamonds(1200);
                break;
            case "com.company.ComputerGame-Tycoon.2500Diamonds": 
                EconomyManager.Instance.AddDiamonds(2500);
                break;
            case "com.company.ComputerGame-Tycoon.6500Diamonds": 
                EconomyManager.Instance.AddDiamonds(6500);
                break;
            case "com.company.ComputerGame-Tycoon.14000Diamonds": 
                EconomyManager.Instance.AddDiamonds(14000);
                break;
            case "com.company.ComputerGame-Tycoon.NoAds": 
                GameManager.Instance._isAdBlockerPurchased = true;
                break;
            case "com.company.ComputerGame-Tycoon.OfflineIncomeSmallBonus": 
                _smallOfflineIncomeBonus.SetPurchased();
                break;
            case "com.company.ComputerGame-Tycoon.OfflineIncomeMediumBonus": 
                _mediumOfflineIncomeBonus.SetPurchased();
                break;
            case "com.company.ComputerGame-Tycoon.OfflineIncomeLargeBonus": 
                _largeOfflineIncomeBonus.SetPurchased();
                break;
            default:
                Debug.LogWarning("Невідомий ID продукту: " + product.definition.id);
                break;
        }
    }

    private void RefreshPrice()
{
    foreach (var button in _purchaseButtons)
    {
        var iapButton = button.GetComponentInChildren<IAPButton>(true) ?? 
                        (Component)button.GetComponentInChildren<CodelessIAPButton>(true);
        if (iapButton == null)
        {
            Debug.LogWarning("Компонент IAPButton або CodelessIAPButton не знайдено на кнопці " + button.name);
            continue;
        }
        
        var priceTextValue = iapButton.GetType().GetProperty("priceText")?.GetValue(iapButton, null);
        Debug.Log("Ціна для кнопки " + button.name + ": " + priceTextValue);

        var textComponent = button.GetComponentInChildren<TextMeshProUGUI>(true);
        if(textComponent != null)
        {
            textComponent.text = priceTextValue != null ? priceTextValue.ToString() : "N/A";
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI не знайдено на кнопці " + button.name);
        }
    }
}

}