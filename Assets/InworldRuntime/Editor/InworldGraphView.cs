/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Inworld.Framework.Graph;
using Inworld.Framework.Samples.Node;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using EdgeView = UnityEditor.Experimental.GraphView.Edge;
using NodeView = UnityEditor.Experimental.GraphView.Node;

namespace Inworld.Framework.Editor
{
    public class InworldGraphView : GraphView
    {
        const string k_StylePath = "Assets/InworldRuntime/Editor/Styles/InworldGraphEditor.uss";
        InworldGraphEditorWindow m_EditorWindow;
        InworldNodeSearchWindow m_SearchWindow;
        InworldGraphAsset m_GraphAsset;
        InworldEdgeConnectorListener m_EdgeListener;
        
        public InworldGraphAsset GraphAsset => m_GraphAsset;
        public InworldGraphView(InworldGraphEditorWindow editorWindow)
        {
            m_EditorWindow = editorWindow;
            
            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(k_StylePath));
            
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            GridBackground grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();
            graphViewChanged = OnGraphViewChanged;
            CreateSearchWindow();
        }
        GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                foreach (GraphElement el in graphViewChange.elementsToRemove)
                {
                    if (el is InworldEdgeView ev)
                    {
                        if (m_GraphAsset != null && ev.EdgeAsset != null)
                            m_GraphAsset.Edges.Remove(ev.EdgeAsset);
                    }
                    else if (el is InworldNodeView nv)
                    {
                        List<EdgeView> connected = edges.ToList()
                            .Where(e => e?.input?.node == nv || e?.output?.node == nv)
                            .ToList();

                        foreach (EdgeView e in connected)
                        {
                            if (e is InworldEdgeView cev && m_GraphAsset != null && cev.EdgeAsset != null)
                                m_GraphAsset.Edges.Remove(cev.EdgeAsset);
                            RemoveElement(e);
                        }
                        if (m_GraphAsset != null)
                        {
                            m_GraphAsset.Nodes.Remove(nv.NodeAsset);
                            m_GraphAsset.EndNodes.Remove(nv.NodeAsset);
                            if (m_GraphAsset.StartNode == nv.NodeAsset)
                                m_GraphAsset.StartNode = null;
                        }
                    }
                }
            }
            if (graphViewChange.edgesToCreate == null) 
                return graphViewChange;
            foreach (EdgeView edge in graphViewChange.edgesToCreate)
            {
                RemoveElement(edge);
                InworldEdgeView inworldEdge = new InworldEdgeView
                {
                    output = edge.output,
                    input = edge.input
                };

                inworldEdge.EdgeAsset = ScriptableObject.CreateInstance<InworldEdgeAsset>();
                AddElement(inworldEdge);
            }
            graphViewChange.edgesToCreate = null;

            return graphViewChange;
        }
        void CreateSearchWindow()
        {
            m_SearchWindow = ScriptableObject.CreateInstance<InworldNodeSearchWindow>();
            m_SearchWindow.Init(this, m_EditorWindow);
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), m_SearchWindow);
            m_EdgeListener = new InworldEdgeConnectorListener(this, m_SearchWindow);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
                endPort.direction != startPort.direction &&
                endPort.node != startPort.node).ToList();
        }

        public InworldNodeView CreateNode(Vector2 position)
        {
            InworldNodeView node = new InworldNodeView();
            node.Initialize(position);
            AddElement(node);
            AttachEdgeConnectors(node);
            return node;
        }
        
        InworldNodeView InstantiateNodeViewForAssetType(Type assetType)
        {
            if (assetType == null)
                return new InworldNodeView();
            // Hard mapping of NodeAsset -> NodeView subclasses
            if (typeof(AddSpeechEventNodeAsset).IsAssignableFrom(assetType))
                return new AddSpeechEventNodeView();
            if (typeof(ConversationEndpointNodeAsset).IsAssignableFrom(assetType))
                return new ConversationEndpointNodeView();
            if (typeof(TTSNodeAsset).IsAssignableFrom(assetType))
                return new TTSNodeView();
            return new InworldNodeView();
        }
        
        public InworldNodeView CreateNodeForAsset(InworldNodeAsset asset, Vector2 position)
        {
            InworldNodeView node = InstantiateNodeViewForAssetType(asset?.GetType());
            node.Initialize(position);
            AddElement(node);
            AttachEdgeConnectors(node);
            if (asset)
                node.LoadFromAsset(m_GraphAsset, asset);
            return node;
        }

        public void SetStartNode(InworldNodeView newStartNode)
        {
            if (m_GraphAsset == null) 
                return;
            InworldNodeView oldStartNodeView = FindNodeViewByAsset(m_GraphAsset.StartNode);
            m_GraphAsset.StartNode = newStartNode.NodeAsset;
        }

        public void AddEndNode(InworldNodeView endNode)
        {
            if (m_GraphAsset == null) return;

            if (!m_GraphAsset.EndNodes.Contains(endNode.NodeAsset))
            {
                m_GraphAsset.EndNodes.Add(endNode.NodeAsset);
            }
        }

        public void RemoveEndNode(InworldNodeView endNode)
        {
            if (m_GraphAsset == null) return;
            m_GraphAsset.EndNodes.Remove(endNode.NodeAsset);
        }

        InworldNodeView FindNodeViewByAsset(InworldNodeAsset nodeAsset)
        {
            if (nodeAsset == null) 
                return null;
        
            return nodes.Cast<InworldNodeView>()
                .FirstOrDefault(node => node.NodeAsset == nodeAsset);
        }
        
        public void LoadGraph(InworldGraphAsset graphAsset)
        {
            if (!graphAsset) 
                return;

            ClearGraph();
            m_GraphAsset = graphAsset;
            if (m_GraphAsset.Nodes != null && m_GraphAsset.Nodes.Count > 0)
            {
                List<InworldNodeAsset> uniqueNodes = m_GraphAsset.Nodes.Distinct().ToList();
                if (uniqueNodes.Count != m_GraphAsset.Nodes.Count)
                {
                    m_GraphAsset.Nodes.Clear();
                    m_GraphAsset.Nodes.AddRange(uniqueNodes);
                    UnityEditor.EditorUtility.SetDirty(m_GraphAsset);
                }
            }
            for (int i = 0; i < graphAsset.Nodes.Count; i++)
            {
                Vector2 position = graphAsset.Nodes[i].EditorPosition != Vector2.zero
                    ? graphAsset.Nodes[i].EditorPosition
                    : SetNextPosition(graphAsset.Nodes[i], i);
                CreateNodeForAsset(graphAsset.Nodes[i], position);
            }

            for (int i = 0; i < graphAsset.Edges.Count; i++)
            {
                InworldEdgeAsset edgeAsset = graphAsset.Edges[i];
                InworldNodeView startNodeView = null;
                InworldNodeView endNodeView = null;
    
                foreach (InworldNodeView nodeView in nodes.Cast<InworldNodeView>())
                {
                    if (nodeView.NodeAsset == edgeAsset.StartNode)
                        startNodeView = nodeView;
                    else if (nodeView.NodeAsset == edgeAsset.EndNode)
                        endNodeView = nodeView;
                }
                if (startNodeView == null || endNodeView == null) 
                    continue;
                Port outputPort = startNodeView.outputContainer.Q<Port>();
                Port inputPort = endNodeView.inputContainer.Q<Port>();
                if (outputPort == null || inputPort == null) 
                    continue;
                outputPort.portColor = edgeAsset.EdgeColor;
                inputPort.portColor = edgeAsset.EdgeColor;
                InworldEdgeView edge = new InworldEdgeView
                {
                    output = outputPort,
                    input = inputPort
                };
                edge.LoadFromAsset(graphAsset, edgeAsset);
                AddElement(edge);
            }
        }

        Vector2 SetNextPosition(InworldNodeAsset asset, int i)
        {
            asset.EditorPosition = Vector2.right * (i * 200) + Vector2.up * 200;
            return asset.EditorPosition;
        }

        public void SaveGraph(InworldGraphAsset graphAsset)
        {
            SaveNodes(graphAsset);
            SaveEdges(graphAsset);
            SaveStartEndNode(graphAsset);
            SaveNewAssets(graphAsset);
        }

        void SaveStartEndNode(InworldGraphAsset graphAsset)
        {
            if (graphAsset == null)
		        return;
            
	        List<InworldNodeAsset> allNodeAssets = nodes
		        .Cast<InworldNodeView>()
		        .Where(v => v != null && v.NodeAsset != null)
		        .Select(v => v.NodeAsset)
		        .ToList();

            // Get In/OutDegree
	        Dictionary<InworldNodeAsset, int> indegree = new Dictionary<InworldNodeAsset, int>();
	        Dictionary<InworldNodeAsset, int> outdegree = new Dictionary<InworldNodeAsset, int>();
	        foreach (InworldNodeAsset n in allNodeAssets)
	        {
		        indegree[n] = 0;
		        outdegree[n] = 0;
	        }

	        foreach (EdgeView e in edges)
            {
                if (!(e is InworldEdgeView ev)) 
                    continue;
                InworldNodeView startView = ev.output?.node as InworldNodeView;
                InworldNodeView endView = ev.input?.node as InworldNodeView;
                InworldNodeAsset start = startView?.NodeAsset;
                InworldNodeAsset end = endView?.NodeAsset;
                if (start == null || end == null) 
                    continue;
                if (outdegree.ContainsKey(start)) 
                    outdegree[start]++;
                if (indegree.ContainsKey(end)) 
                    indegree[end]++;
            }
            
            // Update StartNode. 
	        List<InworldNodeAsset> startCandidates = allNodeAssets
		        .Where(n => indegree.TryGetValue(n, out int d) && d == 0)
		        .ToList();

	        bool currentStartValid = graphAsset.StartNode != null
		        && indegree.TryGetValue(graphAsset.StartNode, out int curIn) && curIn == 0;

	        if (!currentStartValid)
	        {
		        InworldNodeAsset newStart = startCandidates.FirstOrDefault();
		        if (newStart != null)
			        graphAsset.StartNode = newStart;
	        }
            // Update End Nodes
	        List<InworldNodeAsset> endCandidates = allNodeAssets
		        .Where(n => outdegree.TryGetValue(n, out int d) && d == 0)
		        .ToList();
            
	        for (int i = graphAsset.EndNodes.Count - 1; i >= 0; i--)
	        {
		        InworldNodeAsset n = graphAsset.EndNodes[i];
		        if (n == null) { graphAsset.EndNodes.RemoveAt(i); continue; }
		        if (outdegree.TryGetValue(n, out int d) && d > 0)
			        graphAsset.EndNodes.RemoveAt(i);
	        }

	        foreach (InworldNodeAsset n in endCandidates.Where(n => !graphAsset.EndNodes.Contains(n)))
            {
                graphAsset.EndNodes.Add(n);
            }
        }

        void SaveEdges(InworldGraphAsset graphAsset)
        {
            graphAsset.Edges.Clear();
            foreach (EdgeView edge in edges)
            {
                if (!(edge is InworldEdgeView edgeView)) 
                    continue;
                InworldEdgeAsset edgeAsset = edgeView.SaveToAsset();
                graphAsset.Edges.Add(edgeAsset);
            }
        }

        void SaveNodes(InworldGraphAsset graphAsset)
        {
            graphAsset.Nodes.Clear();
            foreach (NodeView node in nodes)
            {
                if (!(node is InworldNodeView nodeView)) 
                    continue;
                InworldNodeAsset nodeAsset = nodeView.SaveToAsset();
                graphAsset.Nodes.Add(nodeAsset);
            }
        }

        void ClearGraph()
        {
            foreach (NodeView node in nodes.ToList())
            {
                RemoveElement(node);
            }
            
            foreach (EdgeView edge in edges.ToList())
            {
                RemoveElement(edge);
            }
        }

        void AttachEdgeConnectors(InworldNodeView node)
        {
            if (node == null || m_EdgeListener == null)
                return;

            Port inputPort = node.inputContainer?.Q<Port>();
            if (inputPort != null)
            {
                inputPort.AddManipulator(new EdgeConnector<EdgeView>(m_EdgeListener));
            }

            Port outputPort = node.outputContainer?.Q<Port>();
            if (outputPort != null)
            {
                outputPort.AddManipulator(new EdgeConnector<EdgeView>(m_EdgeListener));
            }
        }
        
        InworldEdgeAsset CreateEdgeAsset(EdgeView edge)
        {
            InworldEdgeAsset edgeAsset = ScriptableObject.CreateInstance<InworldEdgeAsset>();
            if (edge.output.node is not InworldNodeView startNodeView || edge.input.node is not InworldNodeView endNodeView) 
                return edgeAsset;
            edgeAsset.StartNode = startNodeView.NodeAsset;
            edgeAsset.EndNode = endNodeView.NodeAsset;
            return edgeAsset;
        }
        
        void SaveNewAssets(InworldGraphAsset graphAsset)
        {
            string dataDir = $"Assets/Data/{graphAsset.Name}";
            if (!AssetDatabase.IsValidFolder(dataDir))
            {
                if (!AssetDatabase.IsValidFolder("Assets/Data"))
                    AssetDatabase.CreateFolder("Assets", "Data");
                AssetDatabase.CreateFolder("Assets/Data", graphAsset.Name);
            }

            foreach (InworldNodeAsset node in graphAsset.Nodes)
            {
                if (string.IsNullOrEmpty(AssetDatabase.GetAssetPath(node)))
                {
                    string nodeName = string.IsNullOrEmpty(node.NodeName) ? "Node" : node.NodeName;
                    string uniqueName = GetUniqueFileName(dataDir, nodeName);
                    AssetDatabase.CreateAsset(node, $"{dataDir}/{uniqueName}.asset");
                }
            }

            foreach (InworldEdgeAsset edge in graphAsset.Edges)
            {
                if (string.IsNullOrEmpty(AssetDatabase.GetAssetPath(edge)))
                {
                    string edgeName =
                        $"Edge_{edge.StartNode?.NodeName ?? "Unknown"}_To_{edge.EndNode?.NodeName ?? "Unknown"}";
                    string uniqueName = GetUniqueFileName(dataDir, edgeName);
                    AssetDatabase.CreateAsset(edge, $"{dataDir}/{uniqueName}.asset");
                }
            }
        }
        
        string GetUniqueFileName(string directory, string baseName)
        {
            string safeName = baseName.Replace(" ", "_").Replace("/", "_").Replace("\\", "_");
            string uniqueName = safeName;
            int counter = 1;
    
            while (AssetDatabase.LoadAssetAtPath($"{directory}/{uniqueName}.asset", typeof(ScriptableObject)) != null)
            {
                uniqueName = $"{safeName}_{counter}";
                counter++;
            }
    
            return uniqueName;
        }
    }
}
#endif