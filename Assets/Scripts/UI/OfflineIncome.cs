using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfflineIncome : MonoBehaviour
{
    public static OfflineIncome Instance;
    private TextMeshProUGUI _incomeText;
    private Image _incomeBarImage;
    private void Awake(){
        Instance = this;
        _incomeText = GameObject.Find("TopBackGround.Income.Text").GetComponent<TextMeshProUGUI>();
        _incomeBarImage = GameObject.Find("OfflineEarning.IncomeBar").GetComponent<Image>();
    }

    public void SetIncomeUIText(string incomeText){
        _incomeText.text = incomeText;
    }
    public void SetIncomePercentOfMaxIncome(string offlineTimeText, string maxOfflineIncomeTime)
    {
        if (float.TryParse(offlineTimeText, out float offlineTime) &&
            float.TryParse(maxOfflineIncomeTime, out float maxTime) &&
            maxTime > 0)
        {
            _incomeBarImage.fillAmount = offlineTime / maxTime;
        }
        else {
            _incomeBarImage.fillAmount = 0;
        }
    }

}
