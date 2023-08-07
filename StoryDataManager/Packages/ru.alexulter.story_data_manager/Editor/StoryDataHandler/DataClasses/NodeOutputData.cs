using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeOutputData
{
    public string InputNodeGuid;
    public string ChoiceText;
    public string ActionProgressText;
    public bool IsImmediateAction;
    public string OnChoiceEventName;
    public string AvailabilityCondition;
}
