using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


namespace StoryGraph
{
    public class StoryGraphEditorWindow : GraphViewEditorWindow
    {
        public StoryGraphView GraphView;

        [MenuItem("StoryGraph/OpenGraph")]
        public static void OpenStoryGraphWindow()
        {
            var window = GetWindow<StoryGraphEditorWindow>();
            window.titleContent = new GUIContent(text: "Story Graph");
        }

        private void OnEnable()
        {
            MakeGraphView();
            GenerateToolbar();
        }
        private void MakeGraphView()
        {
            GraphView = new StoryGraphView
            {
                name = "Story Graph"
            };
            GraphView.StretchToParentSize();
            rootVisualElement.Add(GraphView);
        }
        private void OnDisable()
        {
            rootVisualElement.Remove(GraphView);
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();
            //var fileNameTextField = new TextField("File Name:");
            //fileNameTextField.SetValueWithoutNotify(_fileName);
            //fileNameTextField.MarkDirtyRepaint();
            //fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
            //toolbar.Add(fileNameTextField);

            //toolbar.Add(new Button(() => RequestDataOperation(true)) { text = "Save Data" });

            //toolbar.Add(new Button(() => RequestDataOperation(false)) { text = "Load Data" });
            toolbar.Add(new Button(() => GraphView.CreateNewNode("Story Node")) { text = "New Node", });
            toolbar.Add(new Button(() => SaveData()) { text = "Save Data" });
            toolbar.Add(new Button(() => LoadData()) { text = "Load Data" });
            rootVisualElement.Add(toolbar);
        }

        private void SaveData()
        {
            StoryGraphDataSaveLoader.SaveDataFromEditor(GraphView);
        }
        private void LoadData()
        {
            StoryGraphDataSaveLoader.LoadDataForEditor(GraphView);
        }
    }
}
