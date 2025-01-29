using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    private float duration;
    private float timer;
    private bool isRunning;

    public void SetDuration(float seconds)
    {
        duration = seconds;
    }

    public void Begin()
    {
        timer = duration;
        isRunning = true;
    }

    private void Update()
    {
        if (isRunning)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0f;
                isRunning = false;
            }
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (_fillImage != null)
        {
            _fillImage.fillAmount = timer / duration;
        }
    }
}
