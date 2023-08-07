using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoryDataContainer : ScriptableObject
{
    public List<StoryNodeData> StoryNodes = new List<StoryNodeData>();
}
