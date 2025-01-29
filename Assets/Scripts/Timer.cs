using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Timer UI references")]
    [SerializeField] private Image _UIFillImage;
    public float Duration { get; private set; } 
    private float _remainingDuration;
    [SerializeField] private Vector3 _offset = new Vector3(3,5,0);
    [SerializeField] private Quaternion _quaternion = Quaternion.Euler(-180,-180, 0);

    private void Awake()
    {
        ResetTimer();
    }
    private void Update(){
        transform.position = transform.parent.position + _offset;
        transform.rotation = _quaternion;
    }

    private void ResetTimer()
    {
        _UIFillImage.fillAmount = 0f;
        Duration = _remainingDuration = 0f;
    }

    public Timer SetDuration(float seconds) 
    {
        Duration = _remainingDuration = seconds;
        return this;
    }

    public void Begin()
    {
        StopAllCoroutines();
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (_remainingDuration > 0f)
        {
            _remainingDuration -= Time.deltaTime;
            UpdateUI(_remainingDuration);
            yield return null; 
        }
        End();
        yield break;
    }

    private void UpdateUI(float seconds)
    {
        _UIFillImage.fillAmount = Mathf.InverseLerp(0f, Duration, seconds);
    }

    public void End()
    {
        ResetTimer();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    
}
