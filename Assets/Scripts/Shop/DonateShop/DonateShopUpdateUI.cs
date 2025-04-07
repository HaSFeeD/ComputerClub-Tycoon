using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DonateShopUpdateUI : MonoBehaviour
{
    [SerializeField]
    private Button openShopButton;
    [SerializeField]
    private GameObject shopCanvas;
    public List<TimeJumpItem> timeJumpItems;
    public float cachedSpawnTime;
    public void Awake()
    {
        timeJumpItems = Resources.FindObjectsOfTypeAll<TimeJumpItem>()
            .Where(item => !string.IsNullOrEmpty(item.gameObject.scene.name))
            .ToList();

        openShopButton.onClick.AddListener(() => OpenShop());
    }
    private void OpenShop(){
        cachedSpawnTime = BotSpawnTimeManager.Instance.GetAdditionalSpawnTime();
        UpdateShop();
        shopCanvas.SetActive(true);
    }
    private void UpdateShop(){
        foreach(var timeJumpItem in timeJumpItems){
            timeJumpItem.SetCashPerSkippedTime(cachedSpawnTime);
        }
    }
}
