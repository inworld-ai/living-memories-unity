

#if UNITY_EDITOR
using System;
using Inworld.Framework.Graph;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Inworld.Framework.Editor
{
    public class InworldEdgeView: UnityEditor.Experimental.GraphView.Edge
    {
        public InworldGraphAsset GraphAsset { get; set; }
        public InworldEdgeAsset EdgeAsset { get; set; }
        public InworldNodeAsset StartNode{ get; set; }
        public InworldNodeAsset EndNode { get; set; }
        
        public InworldEdgeView()
        {
            SetupContextMenu();
            RegisterSingleClickMethod();
        }

        void RegisterSingleClickMethod()
        {
            this.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button != 0 || evt.clickCount != 1)
                    return;
                if (!EdgeAsset)
                    return;
                Selection.activeObject = EdgeAsset;
                EditorGUIUtility.PingObject(EdgeAsset);
            });
        }

        void SetupContextMenu()
        {
            this.AddManipulator(new ContextualMenuManipulator(evt =>
            {
                evt.menu.AppendAction("Set Edge Type/Text",
                    _ => SwitchEdgeType(typeof(InworldTextEdgeAsset)));
                evt.menu.AppendAction("Set Edge Type/Audio",
                    _ => SwitchEdgeType(typeof(InworldAudioEdgeAsset)));
                evt.menu.AppendAction("Set Edge Type/Safety",
                    _ => SwitchEdgeType(typeof(InworldSafetyEdgeAsset)));
                evt.menu.AppendAction("Set Edge Type/Json",
                    _ => SwitchEdgeType(typeof(InworldJsonEdgeAsset)));
                evt.menu.AppendAction("Set Edge Type/LLM",
                    _ => SwitchEdgeType(typeof(InworldLLMEdgeAsset)));
            }));
        }

        void SwitchEdgeType(Type targetType)
        {
            if (targetType == null || !typeof(InworldEdgeAsset).IsAssignableFrom(targetType))
                return;

            if (EdgeAsset && EdgeAsset.GetType() == targetType)
                return;

            InworldEdgeAsset newAsset = (InworldEdgeAsset)ScriptableObject.CreateInstance(targetType);

            InworldNodeAsset start = StartNode;
            InworldNodeAsset end = EndNode;

            if (output?.node is InworldNodeView startView)
                start = startView.NodeAsset;
            if (input?.node is InworldNodeView endView)
                end = endView.NodeAsset;

            newAsset.StartNode = start;
            newAsset.EndNode = end;

            if (EdgeAsset)
                newAsset.EdgeColor = EdgeAsset.EdgeColor;

            if (GraphAsset)
            {
                int idx = GraphAsset.Edges.IndexOf(EdgeAsset);
                if (idx >= 0)
                    GraphAsset.Edges[idx] = newAsset;
                else if (!GraphAsset.Edges.Contains(newAsset))
                    GraphAsset.Edges.Add(newAsset);
            }

            EdgeAsset = newAsset;
            StartNode = newAsset.StartNode;
            EndNode = newAsset.EndNode;
        }
        public virtual void LoadFromAsset(InworldGraphAsset graphAsset, InworldEdgeAsset edgeAsset)
        {
            GraphAsset = graphAsset;
            EdgeAsset = edgeAsset;
            StartNode = edgeAsset.StartNode;
            EndNode = edgeAsset.EndNode;
        }

        public virtual InworldEdgeAsset SaveToAsset()
        {
            if (EdgeAsset == null)
            {
                EdgeAsset = ScriptableObject.CreateInstance<InworldEdgeAsset>();
            }
            // Update edge connections from the visual edge
            if (output?.node is InworldNodeView startNodeView && input?.node is InworldNodeView endNodeView)
            {
                EdgeAsset.StartNode = startNodeView.NodeAsset;
                EdgeAsset.EndNode = endNodeView.NodeAsset;
                StartNode = startNodeView.NodeAsset;
                EndNode = endNodeView.NodeAsset;
            }
            return EdgeAsset;
        }
    }
}
#endif