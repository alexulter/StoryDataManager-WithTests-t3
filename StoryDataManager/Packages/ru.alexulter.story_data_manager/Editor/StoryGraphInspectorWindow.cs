using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace StoryGraph {
    public class StoryGraphInspectorWindow : EditorWindow
    {
        //private SerializedObject serializedObject;
        //private Skill skill;

        bool showing = true;
        StoryGraphView m_GraphView;
        StoryGraphView GraphView { get {
                if (m_GraphView == null)
                {
                    m_GraphView = EditorWindow.GetWindow<StoryGraphEditorWindow>().GraphView;
                }
                return m_GraphView; } }

        [MenuItem("StoryGraph/OpenInspector")]
        public static void Init()
        {
            StoryGraphInspectorWindow window = GetWindow<StoryGraphInspectorWindow>();
            window.titleContent = new GUIContent(text: "Story Inspector");
            //window.Show();
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }

        void OnGUI()
        {
            if (GraphView == null) return;
            string text = "Select node";

            var currSelection = GraphView.selection;//Selection.activeObject;
            if (currSelection.Any() && currSelection.First().GetType() == typeof(StoryNode))
            {
                text = currSelection.First().GetType().ToString();
                EditorGUILayout.LabelField(text);
                EditorGUILayout.Separator();

                var selectedNode = currSelection.First() as StoryNode;
                EditorGUILayout.LabelField("Node type:");
                selectedNode.NodeType = (NodeType) EditorGUILayout.EnumPopup(selectedNode.NodeType);
                EditorGUILayout.Separator();
                if (selectedNode.outputContainer.childCount < 1) return;
                var ports = selectedNode.outputContainer.Children();
                foreach (UnityEditor.Experimental.GraphView.Port port in ports)
                {
                    if (port.userData == null) continue;
                    var choiceData = port.userData as StoryChoice;
                    EditorGUILayout.LabelField(choiceData.ChoiceText);
                    choiceData.IsImmediateAction = EditorGUILayout.Toggle("Immediate Choice", choiceData.IsImmediateAction);
                    if (!choiceData.IsImmediateAction)
                    {
                        choiceData.ActionProgressText =
                        EditorGUILayout.TextField((port.userData as StoryChoice).ActionProgressText, GUILayout.MaxWidth(200));
                    }
                    EditorGUILayout.Separator();

                    EditorGUILayout.LabelField("OnChoiceEvent name:");
                    choiceData.OnChoiceEventName = 
                        EditorGUILayout.TextField((port.userData as StoryChoice).OnChoiceEventName, GUILayout.MaxWidth(200)).Trim();
                    EditorGUILayout.Separator();
                    EditorGUILayout.LabelField("Availability condition Id:");
                    choiceData.AvailabilityCondition =
                        EditorGUILayout.TextField((port.userData as StoryChoice).AvailabilityCondition, GUILayout.MaxWidth(200)).Trim();
                    EditorGUILayout.Separator();
                }
            }
            else
            {
                EditorGUI.DropShadowLabel(
                new Rect(3, 15, position.width, 20),
                    text);
            }
            


            //if (currSelection.Any())
            //    {
            //        //showing = EditorGUI.InspectorTitlebar(new Rect(0, 0, position.width, 20), showing, currSelection, showing);
            //        //if (showing)
            //        {
            //            EditorGUI.DropShadowLabel(
            //                new Rect(3, 15, position.width, 20),
            //                "Showing info!!");
            //        }
            //    }
            //    else
            //    {
            //        EditorGUI.DropShadowLabel(
            //            new Rect(3, 15, position.width, 20),
            //            "Select an Object to inspect");
            //    }
        }
    }
}