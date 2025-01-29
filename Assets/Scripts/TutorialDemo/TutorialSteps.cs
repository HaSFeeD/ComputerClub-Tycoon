using System;
using System.Collections.Generic;

public class TutorialSteps 
{
    private List<string> tutorialStepKeys = new List<string>()
    {
        "Tutorial.Step.First",
        "Tutorial.Step.Second",
        "Tutorial.Step.Third",
        "Tutorial.Step.Fourth",
        "Tutorial.Step.Fifth",
        "Tutorial.Step.Sixth",
        "Tutorial.Step.Seventh",
        "Tutorial.Step.Eighth",
        "Tutorial.Step.Ninth",
        "Tutorial.Step.Tenth",
    };
    public List<String> GetTutorialSteps(){
        return tutorialStepKeys;
    }
    
}
