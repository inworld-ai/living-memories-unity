/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Runtime.InteropServices;
using AOT;
using Inworld.Framework.Edge;
using Inworld.Framework.Node;
using UnityEngine;


namespace Inworld.Framework.Graph
{
    /// <summary>
    /// Base ScriptableObject that represents an edge connection between nodes in graph workflows within the Inworld framework.
    /// This asset can be created through Unity's Create menu and used to define connections and flow control between nodes.
    /// Used for implementing conditional logic, data flow routing, and execution control in AI conversation systems.
    /// </summary>
    [CreateAssetMenu(fileName = "New Edge", menuName = "Inworld/Create Edge/Default", order = -2700)]
    public class InworldEdgeAsset : ScriptableObject
    {
        /// <summary>
        /// The graph asset that contains this edge.
        /// Used to reference the parent graph for context and runtime operations.
        /// </summary>
        [SerializeField] protected InworldGraphAsset m_Graph;
        
        [SerializeField] protected InworldComponentAsset m_Component;
        /// <summary>
        /// The source node where this edge begins.
        /// Defines the starting point of the connection in the graph flow.
        /// </summary>
        [SerializeField] InworldNodeAsset m_StartNode;
        
        /// <summary>
        /// The destination node where this edge ends.
        /// Defines the target point of the connection in the graph flow.
        /// </summary>
        [SerializeField] InworldNodeAsset m_EndNode;
        
        /// <summary>
        /// Whether this edge is required for graph execution.
        /// When true, this edge must be traversed for the graph to complete successfully.
        /// </summary>
        [SerializeField] bool m_IsRequired;
        
        /// <summary>
        /// Whether this edge creates a loop in the graph.
        /// When true, this edge can create cycles in the execution flow.
        /// </summary>
        [SerializeField] bool m_IsLoop;
        
        /// <summary>
        /// Whether this edge should be executed in a separate thread.
        /// When true, edge condition checking will be performed asynchronously.
        /// </summary>
        [SerializeField] bool m_IsMultiThread = false;
        
        /// <summary>
        /// Whether to display the title of this edge in the editor.
        /// Controls the visual representation of edge information in the graph editor.
        /// </summary>
        [SerializeField] bool m_ShowTitle;
        
        /// <summary>
        /// The default behavior for edge traversal when conditions are met.
        /// When true, the edge allows passage by default; when false, it blocks by default.
        /// </summary>
        [SerializeField] protected bool m_AllowedPassByDefault = true;
        
        /// <summary>
        /// The display title for this edge in the editor.
        /// Used for identification and debugging purposes in the graph editor.
        /// </summary>
        [SerializeField] string m_EdgeTitle;
        
        /// <summary>
        /// The color used to display this edge in the editor.
        /// Provides visual differentiation between different types of edges.
        /// </summary>
        [SerializeField] Color m_EdgeColor = Color.white;

        /// <summary>
        /// The runtime edge instance created from this asset.
        /// Contains the actual edge logic used during graph execution.
        /// </summary>
        InworldEdge m_Runtime;
        
        /// <summary>
        /// The executor responsible for condition checking on this edge.
        /// Manages the evaluation logic that determines edge traversal.
        /// </summary>
        InworldExecutor m_Executor;
        
        /// <summary>
        /// The runtime wrapper that encapsulates edge functionality.
        /// Provides the interface between the asset and the runtime graph system.
        /// </summary>
        EdgeWrapper m_RuntimeWrapper;
        
        /// <summary>
        /// Delegate function for converting data types during edge processing.
        /// Allows custom data transformation during edge traversal.
        /// </summary>
        public ProcessBaseDataIODelegate dataTypeConverterFunc;

        /// <summary>
        /// Gets or sets the runtime wrapper for this edge.
        /// Provides access to the runtime edge functionality and state.
        /// </summary>
        public EdgeWrapper Wrapper
        {
            get => m_RuntimeWrapper;
            set => m_RuntimeWrapper = value;
        }

        public Color EdgeColor
        {
            get => m_EdgeColor;
            set => m_EdgeColor = value;
        }
        /// <summary>
        /// Gets the runtime edge instance.
        /// Provides access to the active edge logic during execution.
        /// </summary>
        public InworldEdge Runtime => m_Runtime;
        
        /// <summary>
        /// Gets whether this edge is required for graph completion.
        /// Indicates if the edge must be traversed for successful graph execution.
        /// </summary>
        public bool IsRequired => m_IsRequired;
        
        /// <summary>
        /// Gets whether this edge creates a loop in the graph.
        /// Indicates if the edge can create cycles in the execution flow.
        /// </summary>
        public bool IsLoop => m_IsLoop;
        
        /// <summary>
        /// Gets the identifier of the start node for this edge.
        /// Provides the unique ID of the source node in the connection.
        /// </summary>
        public string StartNodeName => m_StartNode?.NodeName;
        
        /// <summary>
        /// Gets the identifier of the end node for this edge.
        /// Provides the unique ID of the destination node in the connection.
        /// </summary>
        public string EndNodeName => m_EndNode?.NodeName;
        
        /// <summary>
        /// Gets the start node asset for this edge.
        /// Provides access to the source node configuration and settings.
        /// </summary>
        public InworldNodeAsset StartNode
        {
            get => m_StartNode;
            set => m_StartNode = value;
        }

        /// <summary>
        /// Gets the end node asset for this edge.
        /// Provides access to the destination node configuration and settings.
        /// </summary>
        public InworldNodeAsset EndNode
        {
            get => m_EndNode;
            set => m_EndNode = value;
        }

        /// <summary>
        /// Gets or sets whether this edge has been successfully validated and is ready for execution.
        /// Indicates the current state of edge initialization and configuration.
        /// </summary>
        public bool IsValid => Runtime != null && Runtime.IsValid;

        public virtual string EdgeTypeName => "";

        public string ComponentID => string.IsNullOrEmpty(m_Component?.ID) ? m_EdgeTitle : m_Component.ID;
        
        /// <summary>
        /// Creates the runtime representation of this edge within the specified graph.
        /// Establishes node connections and initializes the edge runtime wrapper.
        /// </summary>
        /// <param name="graphAsset">The graph asset that will contain this edge.</param>
        /// <returns>True if runtime creation succeeded; otherwise, false.</returns>
        public bool CreateRuntime(InworldGraphAsset graphAsset)
        {
            m_Graph = graphAsset;
            if (!m_StartNode.Children.Contains(m_EndNode))
                m_StartNode.Children.Add(m_EndNode);
            
            if (!m_EndNode.Parents.Contains(m_StartNode))
                m_EndNode.Parents.Add(m_StartNode);
            
            InworldNode startNodeRuntime = m_StartNode.Runtime;
            InworldNode endNodeRuntime = m_EndNode.Runtime;

            if (startNodeRuntime == null)
            {
                Debug.LogError("[InworldFramework] Failed to create edge: Runtime StartNode Null");
                return false;
            }

            if (endNodeRuntime == null)
            {
                Debug.LogError("[InworldFramework] Failed to create edge: Runtime EndNode Null");
                return false;
            }
            EdgeWrapper edgeWrapper = m_Graph.Runtime.CreateEdge(startNodeRuntime, endNodeRuntime);
            if (CreateRuntime(edgeWrapper))
            {
                Debug.Log($"[InworldFramework] Created edge: {startNodeRuntime.ID} -> {endNodeRuntime.ID}");
                return true;
            }
            Debug.LogError("[InworldFramework] Failed to create edge: Wrapper Creation Failed.");
            return false;
        }
        
        /// <summary>
        /// Creates the runtime representation of this edge using an existing wrapper.
        /// Configures the edge properties and establishes condition checking logic.
        /// </summary>
        /// <param name="wrapper">The edge wrapper to use for runtime operations.</param>
        /// <returns>True if runtime creation succeeded; otherwise, false.</returns>
        public bool CreateRuntime(EdgeWrapper wrapper)
        {
            if (wrapper == null || !wrapper.IsValid)
                return false;
            m_RuntimeWrapper = wrapper;
            if (IsLoop)
                m_RuntimeWrapper.SetToLoop();
            if (!IsRequired)
                m_RuntimeWrapper.SetToOptional();
            SetEdgeCondition();
            m_RuntimeWrapper.SetCondition(m_Executor);
            m_RuntimeWrapper.Build();
            return true;
        }

        protected void SetEdgeCondition(string customEdgeName = "")
        {
            if (!m_IsMultiThread)
            {
                EdgeConditionExecutor executor = new EdgeConditionExecutor(OnConditionCheck, this);
                if (!string.IsNullOrEmpty(customEdgeName))
                    InworldComponentManager.RegisterCustomEdgeCondition(customEdgeName, executor);
                
                m_Executor = executor;
            }
            else
            {
                EdgeConditionThreadedExecutor executor = new EdgeConditionThreadedExecutor(OnConditionCheck);
                if (!string.IsNullOrEmpty(customEdgeName))
                    InworldComponentManager.RegisterCustomEdgeCondition(customEdgeName, executor);
                m_Executor = executor;
            }
        }
        
        /// <summary>
        /// Static callback method for edge condition checking.
        /// Handles the evaluation of edge conditions during graph execution.
        /// </summary>
        /// <param name="data">Pointer to the edge asset data for condition evaluation.</param>
        [MonoPInvokeCallback(typeof(ProcessBaseDataIODelegate))]
        static void OnConditionCheck(IntPtr data)
        {
            InworldEdgeAsset edgeAsset = GCHandle.FromIntPtr(data).Target as InworldEdgeAsset;
            if (edgeAsset == null)
                return;
            InworldBaseData inputData = new InworldBaseData(InworldInterop.inworld_EdgeConditionExecutor_GetLastInput());
            InworldInterop.inworld_EdgeConditionExecutor_SetNextOutput(edgeAsset.MeetsCondition(inputData));
        }

        /// <summary>
        /// Virtual method for evaluating edge traversal conditions.
        /// Override this method in derived classes to implement custom condition logic.
        /// The default implementation returns the value of m_AllowedPassByDefault.
        /// </summary>
        /// <param name="inputData">The input data to evaluate for edge traversal.</param>
        /// <returns>True if the condition is met and the edge should allow passage; otherwise, false.</returns>
        protected virtual bool MeetsCondition(InworldBaseData inputData)
        {
            return m_AllowedPassByDefault;
        }
        
        public virtual bool RegisterJson(InworldGraphAsset graphAsset)
        {
            m_Graph = graphAsset;
            if (!string.IsNullOrEmpty(EdgeTypeName))
                SetEdgeCondition(EdgeTypeName);
            return m_Graph?.RegisterJsonEdge(StartNodeName, EndNodeName) ?? false;
        }
    }
}