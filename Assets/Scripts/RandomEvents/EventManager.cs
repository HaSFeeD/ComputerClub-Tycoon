using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] private GameObject _eventPanel;
    [SerializeField] private List<GameObject> events;
    [SerializeField] private GameObject mainEventCanvas;

    [SerializeField] private GameObject busEventPanel;
    [SerializeField] private GameObject diamondsEventPanel;
    [SerializeField] private GameObject investmentsEventPanel;
    [SerializeField] private GameObject doubleIncomeEventPanel;

    private GameObject gameEvent;
    private float time = 0f;
    private float eventCoolDown = 10f;
    private float eventActiveTime = 5f;

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= eventActiveTime)
        {
            EndEvent();
        }
        if (time > eventCoolDown)
        {
            StartRandomEvent();
        }
    }

    private void StartRandomEvent()
    {
        var prefab = GetRandomEvent();
        gameEvent = Instantiate(prefab, _eventPanel.transform);
        
        var eventButton = gameEvent.GetComponent<EventButtonScript>();
        if (eventButton != null)
        {
            if (prefab.name.Contains("Bus"))
            {
                eventButton.Setup(mainEventCanvas, busEventPanel);
            }
            else if (prefab.name.Contains("Diamonds"))
            {
                eventButton.Setup(mainEventCanvas, diamondsEventPanel);
            }
            else if (prefab.name.Contains("Investment"))
            {
                eventButton.Setup(mainEventCanvas, investmentsEventPanel);
            }
            else if (prefab.name.Contains("DoubleIncome"))
            {
                eventButton.Setup(mainEventCanvas, doubleIncomeEventPanel);
            }
        }

        time = 0f;
    }

    private GameObject GetRandomEvent()
    {
        int random = Random.Range(0, events.Count);
        return events[random];
    }

    private void EndEvent()
    {
        if (gameEvent != null)
        {
            Destroy(gameEvent);
        }
    }
}
