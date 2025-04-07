using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfflineIncomeUI : MonoBehaviour
{
    public static OfflineIncomeUI Instance;
    private TextMeshProUGUI _incomeText;
    private Image _incomeBarImage;
    [SerializeField] private TextMeshProUGUI _offlineTime;
    private void Awake(){
        Instance = this;
        _incomeText = GameObject.Find("TopBackGround.Income.Text").GetComponent<TextMeshProUGUI>();
        _incomeBarImage = GameObject.Find("OfflineEarning.IncomeBar").GetComponent<Image>();
    }


    public void SetIncomeUIText(string income, string time){
        _incomeText.text = income;
        _offlineTime.text = time + " Seconds / " + OfflineEarning.Instance.initialOfflineIncomeTimeInSeconds;
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
