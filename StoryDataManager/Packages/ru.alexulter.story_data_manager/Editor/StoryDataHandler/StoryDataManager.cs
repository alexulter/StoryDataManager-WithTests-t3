using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StoryDataManager : MonoBehaviour
{
    private StoryDataContainer m_StoryData;

    //private StoryNodeData m_CurrentStoryNode;

    public void InitializeStoryData()
    {
        m_StoryData = StoryGraphDataSaveLoader.LoadStoryGraphData();
        if (m_StoryData == null) { throw new System.Exception("Failed to load story data"); }
    }

    public string GetFirstStepGuid()
    {
        return m_StoryData.StoryNodes.Where(x => x.IsFirst).First().NodeGuid;
    }

    public string GetDefaultStepGuid()
    {
        return "49be1494-f29f-4061-ba16-0003391c3161";
    }

    public StoryStep InstantiateStoryStep(string guid)
    {
        var storyNode = m_StoryData.StoryNodes.Where(x => x.NodeGuid == guid).First();
        ///Story step object is null if the node is "Story End Node"
        if (storyNode.NodeType == StoryGraph.NodeType.End) { return null; }
        var step = new StoryStep()
        {
            storyText = storyNode.TextContent
        };
        step.storyActions = new StoryActionBase[storyNode.OutputPorts.Count];
        int k = 0;
        foreach(var port in storyNode.OutputPorts)
        {
            StoryActionBase newAction;
            if (port.IsImmediateAction)
            {
                newAction = new StoryActionImmediate();
            }
            else
            {
                newAction = new StoryAction()
                {
                    actionInProgressName = port.ActionProgressText
                };
            }
            newAction.actionName = port.ChoiceText;
            newAction.ResultingStepNodeGuid = port.InputNodeGuid;
            newAction.OnActionEventName = port.OnChoiceEventName;
            newAction.AvailabilityCondition = port.AvailabilityCondition;

            step.storyActions[k] = newAction;
            k++;
        }
        return step;
    }
}
