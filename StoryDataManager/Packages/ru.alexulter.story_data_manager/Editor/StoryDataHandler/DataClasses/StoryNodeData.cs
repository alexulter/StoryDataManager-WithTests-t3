using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoryNodeData
{
    public string NodeGuid;
    public string NodeTitle;
    public string TextContent;
    public Vector2 NodePosition;
    public Vector2 NodeSize;
    public List<NodeOutputData> OutputPorts;
    public bool IsFirst;
    public StoryGraph.NodeType NodeType;
}
