using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace StoryGraph
{
    public class StoryNode : Node
    {
        public string GUID;
        public string Contents;
        public NodeType NodeType;
    }
}
