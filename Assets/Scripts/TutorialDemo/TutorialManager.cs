using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UIElements;

public class TutorialManager : MonoBehaviour
{
    private int _currentStepIndex = 0;
    private bool isTutorialCompleted = false;
    private GameManager gameManager;
    private GameObject gameObject;
    private LocalizeStringEvent tutorialText;
    private GameObject canvasTutorial;
    private TutorialSteps tutorialStepKeys = new TutorialSteps();
    
    void Start()
    {
        gameObject = GameObject.Find("GameManager");
        gameManager = gameObject.GetComponent<GameManager>();
        tutorialText = GameObject.Find("Tutorial.Text").GetComponent<LocalizeStringEvent>();
        if(tutorialText == null){ Debug.Log("Tutorial.Text doesnt exist"); }
        canvasTutorial = GameObject.Find("canvas.Tutorial");
    }
    void Update()
    {
        isTutorialCompleted = false;
        if(_currentStepIndex >= tutorialStepKeys.GetTutorialSteps().Count - 1) isTutorialCompleted = true;

        NextStep();
        if (Input.GetMouseButtonDown(0))
       {
           _currentStepIndex++;
           Debug.Log(_currentStepIndex);
       }
        if(EconomyManager.Instance.Cash >= 100.0f){
            isTutorialCompleted = true;
        }
        if(isTutorialCompleted){
            gameManager.GetComponent<BotSpawner>().enabled = true;
            Destroy(GetComponent<TutorialManager>());
            Destroy(canvasTutorial);
        }
    }
    void NextStep()
    {
        string stepText = tutorialStepKeys.GetTutorialSteps()[_currentStepIndex];
        tutorialText.SetEntry(stepText);
    }

}
