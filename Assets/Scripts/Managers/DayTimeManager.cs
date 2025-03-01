using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

public class DayTimeManager : MonoBehaviour
{
    public static DayTimeManager Instance; 

    [SerializeField] private GameObject _dayLightGameObject;
    private Light _light;
    private int _minutes;
    private int _hours;
    public float _offsetDemo = 0;
    private float _currentTime;
    public int CurrentHour {get { return _hours; }}
    private void Awake()
    {
        Instance = this;
    }
    private void Start(){
        _hours = 10; _minutes = 0;
        _light = _dayLightGameObject.GetComponent<Light>();
    }
    void Update()
    {
        _currentTime += Time.deltaTime + _offsetDemo;
        if(_currentTime >= 20){
            _minutes += 10;
            _currentTime = 0;
        }
        if(_minutes >= 60){
            _hours += 1;
            _minutes = 0;
            BotSpawnTimeManager.Instance.AdjustSpawnTime(_hours);
        }
        if(_hours >= 24){
            TimeReset();
        }
        HUD.Instance._hoursTextMesh.text = _hours.ToString();
        HUD.Instance._minutesTextMesh.text = _minutes.ToString();
        switch(_hours){
            case 6: _light.color = new Color32(142,134,114,255);
            break;
            case 10: _light.color = new Color32(255,240,196,255); 
            break;
            case 18: _light.color = new Color32(250,184,18,255); 
            break;
            case 21: _light.color = new Color32(22,21,18,255); 
            break;
        }
    }
    private void TimeReset(){
        _hours = 0;
        _minutes = 0;
        _currentTime = 0;
    }
}
