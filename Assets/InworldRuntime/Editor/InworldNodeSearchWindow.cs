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

namespace Inworld.Framework.Editor
{
    /// <summary>
    /// Search window provider for creating new nodes in the Inworld graph editor.
    /// Provides a searchable interface for selecting and creating different types of graph nodes.
    /// </summary>
    public class InworldNodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        InworldGraphView m_GraphView;
        InworldGraphEditorWindow m_EditorWindow;
        Port m_PendingPort;
        Vector2 m_PendingScreenPosition;
        static readonly object k_CreateNewCustomNodeScriptToken = new object();
        /// <summary>
        /// Initializes the search window with references to the graph view and editor window.
        /// </summary>
        /// <param name="graphView">The graph view that will contain the created nodes</param>
        /// <param name="editorWindow">The editor window that hosts the graph view</param>
        public void Init(InworldGraphView graphView, InworldGraphEditorWindow editorWindow)
        {
            m_GraphView = graphView;
            m_EditorWindow = editorWindow;
        }
        
        public void SetPendingEdgeContext(Port startPort, Vector2 screenPosition)
        {
            m_PendingPort = startPort;
            m_PendingScreenPosition = screenPosition;
        }
        
        /// <summary>
        /// Creates the search tree structure that defines available node types.
        /// Each entry corresponds to a different type of node that can be created.
        /// </summary>
        /// <param name="context">The search window context</param>
        /// <returns>List of search tree entries representing available node types</returns>
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            // Custom Nodes section
            List<Type> customTypes = TypeCache.GetTypesDerivedFrom<CustomNodeAsset>()
                .Where(t => t != null && !t.IsAbstract && t.IsClass && typeof(ScriptableObject).IsAssignableFrom(t))
                .OrderBy(t => t.Name)
                .ToList();
            
            List<SearchTreeEntry> tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node"), 0),
                
                // AI Processing Nodes
                new SearchTreeEntry(new GUIContent("     LLM Node"))
                {
                    userData = typeof(LLMNodeAsset), level = 1
                },
                new SearchTreeEntry(new GUIContent("     STT Node"))
                {
                    userData = typeof(STTNodeAsset), level = 1
                },
                new SearchTreeEntry(new GUIContent("     TTS Node"))
                {
                    userData = typeof(TTSNodeAsset), level = 1
                },
                new SearchTreeEntry(new GUIContent("     Safety Node"))
                {
                    userData = typeof(SafetyNodeAsset), level = 1
                },
                new SearchTreeEntry(new GUIContent("     Intent Node"))
                {
                    userData = typeof(IntentNodeAsset), level = 1
                },
                // Basic Node
                new SearchTreeEntry(new GUIContent("     Default Node"))
                {
                    userData = typeof(InworldNodeAsset), level = 1
                },
            };

            if (customTypes.Count > 0)
            {
                tree.Add(new SearchTreeGroupEntry(new GUIContent("Custom Nodes..."),1));
                foreach (Type t in customTypes)
                {
                    string displayName = $"     {GetNodeTypeName(t)}";
                    tree.Add(new SearchTreeEntry(new GUIContent(displayName))
                    {
                        userData = t, level = 2
                    });
                }
            }

            // New Custom Node Script action at bottom
            tree.Add(new SearchTreeEntry(new GUIContent("     Create New Custom Node Script..."))
            {
                userData = k_CreateNewCustomNodeScriptToken, level = 1
            });
            return tree;
        }

        /// <summary>
        /// Handles the selection of an entry from the search tree.
        /// Creates the corresponding node at the specified position in the graph.
        /// </summary>
        /// <param name="searchTreeEntry">The selected search tree entry</param>
        /// <param name="context">The search window context containing mouse position</param>
        /// <returns>True if the node was successfully created</returns>
        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            // Calculate the position in graph space
            Vector2 screenPos = m_PendingPort != null ? m_PendingScreenPosition : context.screenMousePosition;
            Vector2 windowMousePosition = screenPos - m_EditorWindow.position.position;
            Vector2 graphMousePosition = m_GraphView.contentViewContainer.WorldToLocal(windowMousePosition);
            
            // Handle special action: create new custom node script
            if (searchTreeEntry.userData == k_CreateNewCustomNodeScriptToken)
            {
                CreateCustomNodeScriptWindow.Show("Create CustomNode Script", createdClassName =>
                {
                    AssetDatabase.Refresh();
                    EditorApplication.delayCall += () =>
                    {
                        // Graph editor might have been closed per requirement; guard against nulls
                        if (m_GraphView == null || m_EditorWindow == null)
                            return;
                        Type newType = TypeCache.GetTypesDerivedFrom<CustomNodeAsset>()
                            .FirstOrDefault(t => string.Equals(t.Name, createdClassName, StringComparison.Ordinal) ||
                                                string.Equals(t.FullName, createdClassName, StringComparison.Ordinal));
                        if (newType != null)
                            CreateNodeOfTypeAt(newType, screenPos, m_PendingPort);
                    };
                });
                return true;
            }
            
            // Get the node asset type from the selected entry
            Type nodeAssetType = searchTreeEntry.userData as Type;
            if (nodeAssetType == null) 
                return false;
            
            // Create the node asset FIRST
            InworldNodeAsset nodeAsset = CreateInstance(nodeAssetType) as InworldNodeAsset;
            InworldNodeView node = null;
            if (nodeAsset != null)
            {
                // Set the node name based on its type
                nodeAsset.NodeName = GetNodeTypeName(nodeAssetType);

                // Ensure required component asset exists for non-custom node types
                EnsureRequiredComponent(nodeAssetType);

                // Keep the initial position set by mouse before LoadFromAsset applies it
                nodeAsset.EditorPosition = graphMousePosition;

                // Create the correct NodeView subclass and bind via LoadFromAsset (triggers OnNodeAssetAssigned)
                node = m_GraphView.CreateNodeForAsset(nodeAsset, graphMousePosition);

                // Auto-assign start/end node roles and update title
                CheckAndAssignStartEndNode(node);
            }

            // If invoked from edge dragging, auto connect the new node
            if (m_PendingPort == null || node == null) 
                return true;
            Port newNodeInput = node.inputContainer?.Q<Port>();
            Port newNodeOutput = node.outputContainer?.Q<Port>();
            if (m_PendingPort.direction == Direction.Output && newNodeInput != null)
            {
                InworldEdgeView e = new InworldEdgeView { output = m_PendingPort, input = newNodeInput };
                m_PendingPort.Connect(e);
                newNodeInput.Connect(e);
                m_GraphView.AddElement(e);
            }
            else if (m_PendingPort.direction == Direction.Input && newNodeOutput != null)
            {
                InworldEdgeView e = new InworldEdgeView { output = newNodeOutput, input = m_PendingPort };
                newNodeOutput.Connect(e);
                m_PendingPort.Connect(e);
                m_GraphView.AddElement(e);
            }
            m_PendingPort = null;
            return true;
        }
        public void CreateNodeOfTypeAt(Type nodeAssetType, Vector2 screenPosition, Port fromPort = null)
        {
            if (nodeAssetType == null)
                return;
            Vector2 windowMousePosition = screenPosition - m_EditorWindow.position.position;
            Vector2 graphMousePosition = m_GraphView.contentViewContainer.WorldToLocal(windowMousePosition);
            
            // Create the node asset FIRST
            InworldNodeAsset nodeAsset = CreateInstance(nodeAssetType) as InworldNodeAsset;
            InworldNodeView node = null;
            if (nodeAsset != null)
            {
                nodeAsset.NodeName = GetNodeTypeName(nodeAssetType);
                // Skip EnsureRequiredComponent for custom nodes by design
                if (nodeAssetType != typeof(CustomNodeAsset))
                    EnsureRequiredComponent(nodeAssetType);

                // Keep the initial position set by mouse before LoadFromAsset applies it
                nodeAsset.EditorPosition = graphMousePosition;

                // Create correct NodeView subclass and bind
                node = m_GraphView.CreateNodeForAsset(nodeAsset, graphMousePosition);

                // Auto-assign start/end and update title
                CheckAndAssignStartEndNode(node);
            }

            if (fromPort == null || node == null)
                return;
            Port newNodeInput = node.inputContainer?.Q<Port>();
            Port newNodeOutput = node.outputContainer?.Q<Port>();
            if (fromPort.direction == Direction.Output && newNodeInput != null)
            {
                InworldEdgeView e = new InworldEdgeView { output = fromPort, input = newNodeInput };
                fromPort.Connect(e);
                newNodeInput.Connect(e);
                m_GraphView.AddElement(e);
            }
            else if (fromPort.direction == Direction.Input && newNodeOutput != null)
            {
                InworldEdgeView e = new InworldEdgeView { output = newNodeOutput, input = fromPort };
                newNodeOutput.Connect(e);
                fromPort.Connect(e);
                m_GraphView.AddElement(e);
            }
        }
        
        /// <summary>
        /// Generates a display name for a node based on its asset type.
        /// Used to set the initial NodeName property of newly created nodes.
        /// </summary>
        /// <param name="nodeAssetType">The type of the node asset</param>
        /// <returns>A human-readable name for the node type</returns>
        string GetNodeTypeName(Type nodeAssetType)
        {
            if (nodeAssetType == null)
                return "Node";
            string typeName = nodeAssetType.Name;
            const string suffix = "Asset";
            if (typeName.EndsWith(suffix, StringComparison.Ordinal))
                typeName = typeName.Substring(0, typeName.Length - suffix.Length);
            if (string.IsNullOrEmpty(typeName))
                typeName = "Node";
            return typeName;
        }

        /// <summary>
        /// Automatically assigns start/end node roles based on the current number of nodes.
        /// The first node created becomes the start node, the second becomes an end node.
        /// </summary>
        /// <param name="newNode">The newly created node to potentially assign a role to</param>
        void CheckAndAssignStartEndNode(InworldNodeView newNode)
        {
            if (m_GraphView.GraphAsset == null) 
                return;
            
            int nodeCount = m_GraphView.nodes.Count();
            
            if (nodeCount == 1) // First node becomes start node
            {
                m_GraphView.SetStartNode(newNode);
            }
            else if (nodeCount == 2) // Second node becomes end node
            {
                m_GraphView.AddEndNode(newNode);
            }
        }
        
        void EnsureRequiredComponent(Type nodeAssetType)
        {
            if (m_GraphView == null || m_GraphView.GraphAsset == null)
                return;
            // Skip Custom nodes
            if (nodeAssetType == typeof(CustomNodeAsset))
                return;

            InworldGraphAsset graph = m_GraphView.GraphAsset;

            if (nodeAssetType == typeof(LLMNodeAsset))
            {
                EnsureComponent<LLMComponentAsset>(graph, "LLM_Component");
            }
            else if (nodeAssetType == typeof(STTNodeAsset))
            {
                EnsureComponent<STTComponentAsset>(graph, "STT_Component");
            }
            else if (nodeAssetType == typeof(TTSNodeAsset))
            {
                EnsureComponent<TTSComponentAsset>(graph, "TTS_Component");
            }
        }
        
        void EnsureComponent<TComp>(InworldGraphAsset graph, string baseName)
            where TComp : InworldComponentAsset
        {
            // If component of this runtime type already exists, skip
            if (graph.Components.Any(c => c is TComp))
                return;

            // Create component asset instance
            TComp comp = ScriptableObject.CreateInstance<TComp>();
            if (comp == null)
                return;
            graph.Components.Add(comp);

            // Persist as asset file under Assets/Data/<GraphName>
            string dataDir = $"Assets/Data/{graph.Name}";
            if (!AssetDatabase.IsValidFolder(dataDir))
            {
                if (!AssetDatabase.IsValidFolder("Assets/Data"))
                    AssetDatabase.CreateFolder("Assets", "Data");
                AssetDatabase.CreateFolder("Assets/Data", graph.Name);
            }

            string uniqueName = GetUniqueAssetName(dataDir, baseName);
            comp.name = uniqueName;
            AssetDatabase.CreateAsset(comp, $"{dataDir}/{uniqueName}.asset");
            AssetDatabase.SaveAssets();
        }

        string GetUniqueAssetName(string directory, string baseName)
        {
            string safeName = baseName.Replace(" ", "_").Replace("/", "_").Replace("\\", "_");
            string uniqueName = safeName;
            int counter = 1;
            while (AssetDatabase.LoadAssetAtPath<ScriptableObject>($"{directory}/{uniqueName}.asset") != null)
            {
                uniqueName = $"{safeName}_{counter}";
                counter++;
            }
            return uniqueName;
        }
    }
}
#endif