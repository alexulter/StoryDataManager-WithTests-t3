using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoryChoice
{
    public string ChoiceText;
    public string ActionProgressText;
    public UnityEngine.Events.UnityEvent resultingActionOnProgress;
    public UnityEngine.Events.UnityEvent resultingActionOnFinish;
    public bool IsImmediateAction = false;
    public string OnChoiceEventName = string.Empty;
    public string AvailabilityCondition = string.Empty;
}
