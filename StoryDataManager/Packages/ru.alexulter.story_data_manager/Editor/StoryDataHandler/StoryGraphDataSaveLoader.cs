using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StoryGraph;
using UnityEditor.Experimental.GraphView;
using System.Linq;

public class StoryGraphDataSaveLoader
{
    private readonly static string testFilename = "TestData";
    public static void SaveDataFromEditor(StoryGraphView storyGraphView)
    {
        var storyData = ScriptableObject.CreateInstance<StoryDataContainer>();//new StoryDataContainer();
        List<Edge> edges = storyGraphView.edges.ToList();
        List<StoryNode> nodes = storyGraphView.nodes.ToList().Cast<StoryNode>().ToList();
        foreach (var node in nodes)
        {
            var nodeData = new StoryNodeData
            {
                NodeGuid = node.GUID,
                NodeTitle = node.title,
                TextContent = node.Contents,
                NodePosition = node.GetPosition().position,
                NodeSize = node.GetPosition().size,
                OutputPorts = new List<NodeOutputData>(),
                IsFirst = (!(node.inputContainer.Children().First() as Port).connected),
                NodeType = node.NodeType
            };
            var outputPorts = node.outputContainer.Children();
            foreach (Port port in outputPorts)
            {
                var outputData = new NodeOutputData
                {
                    InputNodeGuid = (port.connected) ? ((StoryNode)port.connections.FirstOrDefault().input.node).GUID : null,
                    ChoiceText = (port.userData as StoryChoice).ChoiceText,
                    ActionProgressText = (port.userData as StoryChoice).ActionProgressText,
                    IsImmediateAction = (port.userData as StoryChoice).IsImmediateAction,
                    OnChoiceEventName = (port.userData as StoryChoice).OnChoiceEventName,
                    AvailabilityCondition = (port.userData as StoryChoice).AvailabilityCondition
                };
                nodeData.OutputPorts.Add(outputData);
            }
            storyData.StoryNodes.Add(nodeData);
        }
        
        if (!UnityEditor.AssetDatabase.IsValidFolder(path: "Assets/Resources"))
        {
            UnityEditor.AssetDatabase.CreateFolder(parentFolder: "Assets", newFolderName: "Resources");
        }
        UnityEditor.AssetDatabase.CreateAsset(storyData, path: $"Assets/Resources/{testFilename}.asset");
        UnityEditor.AssetDatabase.SaveAssets();
    }

    public static void LoadDataForEditor(StoryGraphView storyGraphView)
    {
        storyGraphView.viewTransform.position = new Vector2();
        var Nodes = storyGraphView.nodes.ToList().Cast<StoryNode>().ToList();
        var Edges = storyGraphView.edges.ToList();
        //Nodes.Find(x => x.EntyPoint).GUID = _dialogueContainer.NodeLinks[0].BaseNodeGUID;
        foreach (var perNode in Nodes)
        {
            //if (perNode.EntyPoint) continue;
            Edges.Where(x => x.input.node == perNode).ToList()
                .ForEach(edge => storyGraphView.RemoveElement(edge));
            storyGraphView.RemoveElement(perNode);
        }

        var readStoryData = Resources.Load<StoryDataContainer>(testFilename);
        if (readStoryData == null)
        {
            Debug.LogError("failed to read story data: data is null");
            return;
        }
        foreach(var nodeData in readStoryData.StoryNodes)
        {
            var newNode = storyGraphView.CreateNewNode(nodeData.NodeTitle,nodeData.TextContent);
            newNode.Contents = nodeData.TextContent;
            newNode.GUID = nodeData.NodeGuid;
            newNode.SetPosition(new Rect(nodeData.NodePosition, nodeData.NodeSize));
            newNode.NodeType = nodeData.NodeType;
        }
        foreach (var nodeData in readStoryData.StoryNodes)
        {
            foreach (var portData in nodeData.OutputPorts)
            {
                var storyChoice = new StoryChoice()
                {
                    ChoiceText = portData.ChoiceText,
                    ActionProgressText = portData.ActionProgressText,
                    IsImmediateAction = portData.IsImmediateAction,
                    OnChoiceEventName = portData.OnChoiceEventName,
                    AvailabilityCondition = portData.AvailabilityCondition 
                };
                var newPort = storyGraphView.AddOutputPort(storyGraphView.nodes.ToList().Cast<StoryNode>().First(x => x.GUID == nodeData.NodeGuid),
                    storyChoice);
                if (string.IsNullOrEmpty(portData.InputNodeGuid)) continue;
                var newEdge = new Edge
                {
                    output = newPort,
                    input = (Port)storyGraphView.nodes.ToList().Cast<StoryNode>().First(x => x.GUID == portData.InputNodeGuid).inputContainer.Children().FirstOrDefault()
                };
                newEdge.input.Connect(newEdge);
                newEdge.output.Connect(newEdge);
                storyGraphView.Add(newEdge);
            }
        }
    }

    public static StoryDataContainer LoadStoryGraphData(string filename = "")
    {
        if (string.IsNullOrEmpty(filename))
        {
            filename = testFilename;
        }
        var readStoryData = Resources.Load<StoryDataContainer>(filename);
        if (readStoryData == null)
        {
            Debug.LogError("failed to read story data: data is null");
            return null;
        }

        return readStoryData;
    }
}
