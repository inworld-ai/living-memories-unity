/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using UnityEngine;
using Inworld.Framework.Graph;
using Inworld.Framework.Node;
using UnityEngine.UIElements;


namespace Inworld.Framework.Graph
{
    /// <summary>
    /// Base ScriptableObject that represents a node within graph workflows in the Inworld framework.
    /// This asset can be created through Unity's Create menu and used to define processing nodes in conversation flows.
    /// Used for implementing various AI processing operations, data transformations, and workflow logic in conversation systems.
    /// </summary>
    [CreateAssetMenu(fileName = "New Node", menuName = "Inworld/Create Node/Default", order = -2701)]
    public class InworldNodeAsset : ScriptableObject
    {
        [SerializeField] protected InworldComponentAsset m_Component;
        /// <summary>
        /// The display name for this node.
        /// Used for identification and debugging purposes in the graph editor and runtime.
        /// </summary>
        [Header("General Configuration:")]
        [SerializeField] protected string m_NodeName = "New Node";
        
        /// <summary>
        /// The creation configuration asset for this node.
        /// Hidden in inspector, contains model deployment and configuration settings.
        /// </summary>
        [HideInInspector][SerializeField] protected CreationConfigAsset m_CreationConfigAsset;
        
        //[Header("Type Information")]
        /// <summary>
        /// The list of accepted input data types for this node.
        /// Hidden in inspector, defines what types of data this node can process.
        /// </summary>
        [HideInInspector][SerializeField] List<string> m_InputTypes = new List<string>();
        
        /// <summary>
        /// The output data type produced by this node.
        /// Hidden in inspector, defines what type of data this node generates.
        /// </summary>
        [HideInInspector][SerializeField] string m_OutputType;
        
        //[Header("Editor Properties")]
        /// <summary>
        /// The background color for this node in the graph editor.
        /// Hidden in inspector, used for visual representation and differentiation.
        /// </summary>
        [SerializeField] Color m_BackgroundColor = Color.clear;
        
        /// <summary>
        /// The position of this node in the graph editor.
        /// Hidden in inspector, stores the visual layout coordinates for the node.
        /// </summary>
        [SerializeField] Vector2 m_EditorPosition;
        
        /// <summary>
        /// The graph asset that contains this node.
        /// Used to reference the parent graph for context and runtime operations.
        /// </summary>
        [Header("Graph Relationships")]
        [SerializeField] protected InworldGraphAsset m_Graph;
        
        /// <summary>
        /// The list of parent nodes that provide input to this node.
        /// Defines the incoming connections in the graph flow.
        /// </summary>
        [SerializeField] protected List<InworldNodeAsset> m_ParentNodes = new List<InworldNodeAsset>();
        
        /// <summary>
        /// The list of child nodes that receive output from this node.
        /// Defines the outgoing connections in the graph flow.
        /// </summary>
        [SerializeField] protected List<InworldNodeAsset> m_ChildrenNodes = new List<InworldNodeAsset>();
        
        /// <summary>
        /// Dictionary mapping child nodes to their connecting edges.
        /// Stores the edge relationships for data flow and condition checking.
        /// </summary>
        [SerializeField] protected Dictionary<InworldNodeAsset, InworldEdgeAsset> m_Edges = new Dictionary<InworldNodeAsset, InworldEdgeAsset>();
    
        /// <summary>
        /// The runtime node instance created from this asset.
        /// Contains the actual processing logic used during graph execution.
        /// </summary>
        InworldNode m_RuntimeNode;
        protected NodeCreationConfig m_CreationConfig;
        protected NodeExecutionConfig m_ExecutionConfig;

        public const string DefaultNodeType = "Node";
        
        /// <summary>
        /// Gets or sets the runtime node instance.
        /// Provides access to the active processing logic during execution.
        /// </summary>
        public InworldNode Runtime
        {
            get => m_RuntimeNode;
            set => m_RuntimeNode = value;
        }

        /// <summary>
        /// Gets whether this node has been successfully validated and is ready for execution.
        /// Indicates the current state of node initialization and configuration.
        /// </summary>
        public bool IsValid => Runtime != null && Runtime.IsValid;
        
        /// <summary>
        /// Gets/Sets the display name of this node.
        /// Provides the identifier used for debugging and graph visualization.
        /// </summary>
        public string NodeName
        {
            get => string.IsNullOrEmpty(m_NodeName) ? "UnknownNode" : m_NodeName;
            set => m_NodeName = value;
        }

        public virtual string NodeTypeName => DefaultNodeType;

        public Color BackgroundColor
        {
            get => m_BackgroundColor;
            set => m_BackgroundColor = value;
        }
        /// <summary>
        /// Gets/Sets the position displayed on the graph editor.
        /// </summary>
        public Vector2 EditorPosition
        {
            get => m_EditorPosition;
            set => m_EditorPosition = value;
        }

        /// <summary>
        /// Retrieves the edge asset connecting this node to the specified child node.
        /// </summary>
        /// <param name="childNode">The child node to find the connecting edge for.</param>
        /// <returns>The edge asset connecting to the child node, or null if no connection exists.</returns>
        public InworldEdgeAsset GetEdge(InworldNodeAsset childNode)
        {
            return m_Edges.GetValueOrDefault(childNode);
        }

        /// <summary>
        /// Gets whether this node is a leaf node (has no children).
        /// Indicates if this node is an endpoint in the graph flow.
        /// </summary>
        public bool IsLeaf => m_ChildrenNodes.Count == 0;

        /// <summary>
        /// Gets the list of parent nodes that provide input to this node.
        /// Provides access to the incoming connections in the graph flow.
        /// </summary>
        public List<InworldNodeAsset> Parents => m_ParentNodes;
        
        /// <summary>
        /// Gets the list of child nodes that receive output from this node.
        /// Provides access to the outgoing connections in the graph flow.
        /// </summary>
        public List<InworldNodeAsset> Children => m_ChildrenNodes;
        
        /// <summary>
        /// Gets the dictionary mapping child nodes to their connecting edges.
        /// Provides access to the edge relationships for data flow and condition checking.
        /// </summary>
        public Dictionary<InworldNodeAsset, InworldEdgeAsset> Edges => m_Edges;

        public string ComponentID => string.IsNullOrEmpty(m_Component?.ID) ? m_NodeName : m_Component.ID;
        
        public virtual ComponentData ComponentData => new ComponentData
        {
            // YAN: For custom edges, it's like that.
            //      For the components, it's implemented in the child class.
            id = NodeName,
            type = NodeTypeName
        };

        /// <summary>
        /// Virtual method for providing node creation configuration.
        /// Override this method in derived classes to specify node-specific configuration.
        /// The default implementation returns null and should be implemented in detailed classes only.
        /// </summary>
        /// <returns>The configuration object for creating this node, or null if not implemented.</returns>
        public virtual NodeCreationConfig GetNodeCreationConfig()
        {
            m_CreationConfig ??= new NodeCreationConfig();
            return m_CreationConfig; // YAN: Implemented in detailed class only.
        }
        
        public virtual ConfigData CreationConfigData { get; }

        public virtual ConfigData ExecutionConfigData { get; }
        
        public virtual NodeExecutionConfig GetNodeExecutionConfig()
        {
            m_ExecutionConfig ??= new NodeExecutionConfig();
            return m_ExecutionConfig; // YAN: Implemented in detailed class only.
        }
        
        /// <summary>
        /// Virtual method for creating the runtime representation of this node.
        /// Override this method in derived classes to implement node-specific runtime creation logic.
        /// The default implementation returns false and should be implemented in detailed classes only.
        /// </summary>
        /// <param name="graphAsset">The graph asset that will contain this node.</param>
        /// <returns>True if runtime creation succeeded; otherwise, false.</returns>
        public virtual bool CreateRuntime(InworldGraphAsset graphAsset)
        {
            m_Graph = graphAsset;
            return false; // YAN: Implemented in detailed class only.
        }

        public virtual bool RegisterJson(InworldGraphAsset graphAsset)
        {
            m_Graph = graphAsset;
            // Handled by each child class.
            // For example, if it's an official node, such as LLM, TTS, return true directly.
            // Otherwise, Use InworldConfigManager.RegisterCustomConfig to its config first.
            // Then use InworldComponentManager.RegisterCustomNode for its node.
            return m_Graph?.RegisterJsonNode(NodeName) ?? false;
        }
    }
}