//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
namespace StoryGraph
{
    public enum NodeType { Choice, End };
}

public class StoryStep //: ScriptableObject//test
{
    public bool isInterruptable = false;
    public string storyText;
    public StoryActionBase[] storyActions;

    public static readonly StoryStep Repeat = new StoryStep { isInterruptable = true };
}

//public class StoryEvent : StoryStep
//{
//    public System.Action OnEventResolved; //use this to reenable "start progress" button in UI
//}

public class StoryAction : StoryActionBase
{
    public string actionInProgressName;
    //public string actionFinishedName;
}

public class StoryActionImmediate : StoryActionBase { }

public class StoryActionBase
{
    public string actionName; //button label
    public string ResultingStepNodeGuid;
    public string OnActionEventName = string.Empty;
    public UnityEngine.Events.UnityAction resultingActionOnProgress = null;
    public UnityEngine.Events.UnityAction resultingActionOnFinish = null;
    public string AvailabilityCondition;
}
