/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

#if UNITY_EDITOR
using Inworld.Framework.Graph;
using Inworld.Framework.Samples.Node;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Inworld.Framework.Editor
{
    public class InworldNodeView : UnityEditor.Experimental.GraphView.Node
    {
        public InworldGraphAsset GraphAsset { get; set; }
        public InworldNodeAsset NodeAsset { get; set; }

        public virtual void Initialize(Vector2 position)
        {
            SetPosition(new Rect(position, Vector2.zero));
            CreateInputPorts();
            CreateOutputPorts();
            CreateNodeContent();
            SetupContextMenu();
            RegisterSingleClickMethod();
            RegisterDoubleClickMethod();
        }

        public void SetColor()
        {
            if (!NodeAsset)
                return;
            if (NodeAsset.BackgroundColor == Color.clear || NodeAsset.BackgroundColor == Color.white)
            {
                NodeAsset.BackgroundColor = NodeAsset switch
                {
                    LLMNodeAsset => new Color(0.25f, 0.45f, 0.95f),
                    STTNodeAsset => new Color(0.10f, 0.75f, 0.45f),
                    TTSNodeAsset => new Color(0.88f, 0.45f, 0.10f),
                    SafetyNodeAsset => new Color(0.80f, 0.25f, 0.25f),
                    _ => Color.clear
                };
            }
            style.backgroundColor = NodeAsset.BackgroundColor;
        }

        protected virtual void CreateInputPorts()
        {
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(InworldBaseData));
            inputPort.portName = "Input";
            inputContainer.Add(inputPort);
        }

        protected virtual void CreateOutputPorts()
        {
            Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(InworldBaseData));
            outputPort.portName = "Output";
            outputContainer.Add(outputPort);
        }

        protected virtual void CreateNodeContent()
        {
            TextField textField = new TextField("Node Name:");
            textField.value = title;
            textField.RegisterValueChangedCallback(evt => title = evt.newValue);
            mainContainer.Add(textField);
        }

        void RegisterDoubleClickMethod()
        {
            RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button != 0 || evt.clickCount < 2)
                    return;
                if (!NodeAsset)
                    return;
                MonoScript script = MonoScript.FromScriptableObject(NodeAsset);
                if (script)
                {
                    AssetDatabase.OpenAsset(script);
                    evt.StopImmediatePropagation();
                }
            });
        }

        void RegisterSingleClickMethod()
        {
            RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button != 0 || evt.clickCount != 1)
                    return;
                if (!NodeAsset)
                    return;
                Selection.activeObject = NodeAsset;
                EditorGUIUtility.PingObject(NodeAsset);
            });
        }
        
        public virtual void LoadFromAsset(InworldGraphAsset graphAsset, InworldNodeAsset nodeAsset)
        {
            GraphAsset = graphAsset;
            NodeAsset = nodeAsset;
            title = nodeAsset.NodeName;
            TextField textField = mainContainer.Q<TextField>();
            if (textField != null)
            {
                textField.value = title;
            }

            Debug.Log($"Set NodeName: {title}");
            SetColor();
            SetPosition(new Rect(nodeAsset.EditorPosition, GetPosition().size));
        }

        public virtual InworldNodeAsset SaveToAsset()
        {
            if (NodeAsset == null)
            {
                NodeAsset = ScriptableObject.CreateInstance<InworldNodeAsset>();
            }
            
            NodeAsset.NodeName = title;
            NodeAsset.EditorPosition = GetPosition().position;
            
            return NodeAsset;
        }
        
        public void UpdateNodeTitle()
        {
            string baseTitle = NodeAsset?.NodeName ?? "Node";
            if (NodeAsset == GraphAsset.StartNode)
            {
                title = $"StartNode - {baseTitle}";
            }
            else if (GraphAsset.EndNodes.Contains(NodeAsset))
            {
                title = $"EndNode - {baseTitle}";
            }
            else
            {
                title = baseTitle;
            }
        }
        void SetupContextMenu()
        {
            this.AddManipulator(new ContextualMenuManipulator(evt =>
            {
                InworldGraphView graphView = GetFirstAncestorOfType<InworldGraphView>();
                if (graphView == null || graphView.GraphAsset != GraphAsset)
                    return;
                if (NodeAsset != GraphAsset.StartNode)
                    evt.menu.AppendAction("Set as Start Node", _ => graphView.SetStartNode(this));
                if (!GraphAsset.EndNodes.Contains(NodeAsset))
                    evt.menu.AppendAction("Set as End Node", _ => graphView.AddEndNode(this));
                else
                    evt.menu.AppendAction("Remove End Node", _ => graphView.RemoveEndNode(this));
            }));
        }
    }
}
#endif