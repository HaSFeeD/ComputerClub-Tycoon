using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UtilityButtonManager : MonoBehaviour
{
    [Header("Main Buttons")]
    [SerializeField] private Button[] mainButtons;       

    [Header("Additional Buttons")]
    [SerializeField] private Button[] additionalButtons;   

    [Header("Panels")]
    [SerializeField] private GameObject[] panels; 

    [Header("Icons: 'Opened'")]
    [SerializeField] private Sprite[] closeIcons;

    [Header("Icons: 'Closed'")]
    [SerializeField] private Sprite[] openIcons; 

    private int _lastIndex;

    private void Start()
    {
        _lastIndex = -1;
        for(int i = 0; i < panels.Length; i++){
            panels[i].SetActive(false);
            additionalButtons[i].image.sprite = closeIcons[i];
        }
        for(int i = 0; i < mainButtons.Length; i++){
            int index = i;
            mainButtons[i].onClick.AddListener(() => OnButtonClicked(index));
        }
        for(int i = 0; i < additionalButtons.Length; i++){
            int index = i;
            additionalButtons[i].onClick.AddListener(() => OnButtonClicked(index));
        }
    }

    private void OnButtonClicked(int index)
    {
        if (_lastIndex == index)
        {
            return;
        }
        if(_lastIndex != -1){
            panels[_lastIndex].SetActive(false);
        }
        panels[index].SetActive(true);
        _lastIndex = index;
        UpdateButtonVisuals(index);
    }
    private void UpdateButtonVisuals(int activeIndex)
    {
        for (int i = 0; i < additionalButtons.Length; i++)
        {
            additionalButtons[i].image.sprite = (i == activeIndex) ? openIcons[i] : closeIcons[i];
        }
    }
}
