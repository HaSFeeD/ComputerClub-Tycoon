using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusEvent : MonoBehaviour
{
    [SerializeField]
    private GameObject _bus;
    [SerializeField]
    private Button eventButton;
    [SerializeField]
    private Button _leaveEventPanelButton;
    void Start()
    {
        eventButton.onClick.AddListener(() => OnClickRewardAd());
    }
    private void OnClickRewardAd()
    {
        RewardedAdManager manager = FindObjectOfType<RewardedAdManager>();
        manager.ShowRewardedAd(() =>
        {
            Instantiate(_bus, PointManager.instance.busSpawnPoint.transform.position, PointManager.instance.busSpawnPoint.transform.rotation);
        });
    }
}
