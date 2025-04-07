using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OfflineIncomeX2Button : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClickRewardAd);
    }

    private void OnClickRewardAd()
    {
        RewardAdManager manager = FindObjectOfType<RewardAdManager>();
        manager.ShowRewardedAd(() =>
        {
            IncomeManager.Instance.AddX2OfflineIncome();
        });
    }
}