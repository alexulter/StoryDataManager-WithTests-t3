using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Linq;


namespace StoryGraph
{
    public class StoryGraphView : GraphView
    {
        public StoryGraphView()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            AddElement(GenerateFirstNode());
        }

        private StoryNode GenerateFirstNode()
        {
            var node = new StoryNode
            {
                title = "Start",
                GUID = System.Guid.NewGuid().ToString(),
                Contents = "Some text"
            };

            var generatedPort = GetNewPortInstance(node, Direction.Output);
            generatedPort.portName = "Next";
            node.outputContainer.Add(generatedPort);

            node.RefreshExpandedState();
            node.RefreshPorts();
            return node;
        }

        public StoryNode CreateNewNode(string myTitle, string myContents = null, Vector2 position = new Vector2())
        {
            if (string.IsNullOrEmpty(myContents)) { myContents = "Enter text here"; }
            var node = new StoryNode
            {
                title = myTitle,
                GUID = System.Guid.NewGuid().ToString(),
                Contents = myContents
            };

            node.SetPosition(new Rect(position, new Vector2(10,10)));

            var inputPort = GetNewPortInstance(node, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";
            node.inputContainer.Add(inputPort);

            var button = new Button(clickEvent: () => { AddOutputPort(node); });
            button.text = "New Output";
            node.titleButtonContainer.Add(button);

            //var titleTextfield = new TextField();
            //titleTextfield.RegisterValueChangedCallback(evt =>
            //{
            //    node.title = evt.newValue;
            //});
            //titleTextfield.SetValueWithoutNotify(myTitle);
            //node.mainContainer.Add(new Label($"\nTitle:"));
            //node.mainContainer.Add(titleTextfield);
            //node.mainContainer.Add(new Label($"Contents:"));

            var contentTextfield = new TextField();
            contentTextfield.style.maxWidth = 200;
            contentTextfield.style.whiteSpace = WhiteSpace.Normal;
            contentTextfield.RegisterValueChangedCallback(evt =>
            {
                node.Contents = evt.newValue;
            });
            contentTextfield.SetValueWithoutNotify(myContents);
            node.mainContainer.Add(contentTextfield);
            contentTextfield.multiline = true;

            node.RefreshExpandedState();
            node.RefreshPorts();

            AddElement(node);
            return node;
        }
        private Port GetNewPortInstance(StoryNode node, Direction nodeDirection,
            Port.Capacity capacity = Port.Capacity.Single)
        {
            return node.InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
        }
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            var startPortView = startPort;

            ports.ForEach((port) =>
            {
                var portView = port;
                if (startPortView != portView && startPortView.node != portView.node)
                    compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }
        public Port AddOutputPort(StoryNode node, StoryChoice storyChoice = null)
        {
            var newPort = GetNewPortInstance(node, Direction.Output);
            var outputPortCount = node.outputContainer.childCount;
            if (storyChoice == null)
            {
                storyChoice = new StoryChoice()
                {
                    ChoiceText = $"Choice {outputPortCount + 1}",
                    ActionProgressText = $"Action in progress..."
                };
            }
            newPort.portName = string.Empty;
            newPort.userData = storyChoice;

            var textField = new TextField()
            {
                value = (newPort.userData as StoryChoice).ChoiceText
            };
            textField.RegisterValueChangedCallback(evt => (newPort.userData as StoryChoice).ChoiceText = evt.newValue);
            newPort.contentContainer.Add(textField);
            var deleteButton = new Button(() => RemovePort(node, newPort))
            {
                text = "X"
            };
            newPort.contentContainer.Add(deleteButton);
            


            node.outputContainer.Add(newPort);
            node.RefreshExpandedState();
            node.RefreshPorts();
            return newPort;
        }

        private void RemovePort(StoryNode node, Port port)
        {
            foreach (var edge in port.connections.ToList())
            {
                edge.input.Disconnect(edge);
                edge.output.Disconnect(edge);
                RemoveElement(edge);
            }

            node.outputContainer.Remove(port);
            node.RefreshPorts();
            node.RefreshExpandedState();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if (evt.target == this)
            {
                var pos = (evt.localMousePosition - new Vector2(viewTransform.position.x, viewTransform.position.y))/viewTransform.scale;
                evt.menu.AppendAction("New Node",
                    x => { CreateNewNode("Story Node", null, pos); });
                evt.menu.AppendSeparator();
            }
            else base.BuildContextualMenu(evt);
        }
    }
}

