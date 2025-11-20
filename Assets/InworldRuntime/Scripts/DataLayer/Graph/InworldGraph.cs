/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.Edge;
using Inworld.Framework.Knowledge;
using Inworld.Framework.LLM;
using Inworld.Framework.Node;
using Inworld.Framework.STT;
using Inworld.Framework.TextEmbedder;
using Inworld.Framework.TTS;
using UnityEngine;

namespace Inworld.Framework.Graph
{
    /// <summary>
    /// Represents a graph structure used for AI workflow execution within the Inworld framework.
    /// Provides functionality to create nodes, edges, and define execution flow in a directed graph.
    /// Used for building complex AI conversation and processing workflows.
    /// </summary>
    public class InworldGraph : InworldFrameworkDllClass, IAddInterfaceHandler
    {
        /// <summary>
        /// Initializes a new instance of the InworldGraph class with the specified name.
        /// </summary>
        /// <param name="graphName">The name identifier for this graph.</param>
        public InworldGraph(string graphName)
        {
            m_DLLPtr =
                MemoryManager.Register(InworldInterop.inworld_Graph_new(graphName),
                    InworldInterop.inworld_Graph_delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldGraph class from a native pointer.
        /// </summary>
        /// <param name="ptr">The native pointer to the graph object.</param>
        public InworldGraph(IntPtr ptr) => m_DLLPtr = MemoryManager.Register(ptr, InworldInterop.inworld_Graph_delete);
        
        /// <summary>
        /// Creates an edge connecting two nodes in the graph.
        /// The edge defines the execution flow and data transfer between nodes.
        /// </summary>
        /// <param name="node1">The source node for the edge.</param>
        /// <param name="node2">The destination node for the edge.</param>
        /// <returns>An InworldEdgeWrapper for configuring the created edge.</returns>
        public EdgeWrapper CreateEdge(InworldNode node1, InworldNode node2)
        {
            return new EdgeWrapper(InworldInterop.inworld_Graph_CreateEdge(m_DLLPtr, node1.ToDLL, node2.ToDLL));
        }
        
        /// <summary>
        /// Adds a node to the graph for inclusion in the execution workflow.
        /// The node must be added before it can be connected with edges.
        /// </summary>
        /// <param name="node">The node to add to the graph.</param>
        public void AddNode(InworldNode node)
        {
            InworldInterop.inworld_Graph_AddNode(m_DLLPtr, node.ToDLL);
        }

        /// <summary>
        /// Designates a node as the starting point for graph execution.
        /// Execution will begin from this node when the graph is run.
        /// </summary>
        /// <param name="node">The node to set as the start node.</param>
        public void SetStartNode(InworldNode node)
        {
            InworldInterop.inworld_Graph_SetAsStart(m_DLLPtr, node.ToDLL);
        }

        /// <summary>
        /// Designates a node as an ending point for graph execution.
        /// Execution will terminate when reaching this node.
        /// </summary>
        /// <param name="node">The node to set as an end node.</param>
        public void SetEndNode(InworldNode node)
        {
            InworldInterop.inworld_Graph_SetAsEnd(m_DLLPtr, node.ToDLL);
        }
        
        /// <summary>
        /// Compiles the graph and returns a low-level interface for execution.
        /// This prepares the graph for runtime execution and optimizes the structure.
        /// </summary>
        /// <returns>A CompiledGraphInterface for executing the compiled graph, or null if compilation fails.</returns>
        public CompiledGraphInterface CompileAndReturnRaw()
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_Graph_CompileAndReturnRaw(m_DLLPtr),
                InworldInterop.inworld_StatusOr_CompiledGraphInterfacePtr_status,
                InworldInterop.inworld_StatusOr_CompiledGraphInterfacePtr_ok,
                InworldInterop.inworld_StatusOr_CompiledGraphInterfacePtr_value,
                InworldInterop.inworld_StatusOr_CompiledGraphInterfacePtr_delete
            );
            return result == IntPtr.Zero ? null : new CompiledGraphInterface(result);
        }

        public InworldCreationContext CreationContext
        {
            get
            {            
                IntPtr result = InworldFrameworkUtil.Execute(
                    InworldInterop.inworld_Graph_GetCreationContextHelper(m_DLLPtr),
                    InworldInterop.inworld_StatusOr_CreationContext_status,
                    InworldInterop.inworld_StatusOr_CreationContext_ok,
                    InworldInterop.inworld_StatusOr_CreationContext_value,
                    InworldInterop.inworld_StatusOr_CreationContext_delete
                );
                return result == IntPtr.Zero ? null : new InworldCreationContext(result);
            }
        }

        public bool AddLLMInterface(string componentID, LLMInterface llmInterface)
        {
            IntPtr status = InworldInterop.inworld_Graph_AddComponent_LLMInterface(m_DLLPtr, componentID, llmInterface.ToDLL);
            if (InworldInterop.inworld_Status_ok(status))
                return true;
            Debug.LogError(InworldInterop.inworld_Status_ToString(status));
            return false;
        }

        public bool AddSTTInterface(string componentID, STTInterface sttInterface)
        {
            IntPtr status = InworldInterop.inworld_Graph_AddComponent_STTInterface(m_DLLPtr, componentID, sttInterface.ToDLL);
            if (InworldInterop.inworld_Status_ok(status))
                return true;
            Debug.LogError(InworldInterop.inworld_Status_ToString(status));
            return false;
        }
        
        public bool AddTTSInterface(string componentID, TTSInterface ttsInterface)
        {
            IntPtr status = InworldInterop.inworld_Graph_AddComponent_TTSInterface(m_DLLPtr, componentID, ttsInterface.ToDLL);
            if (InworldInterop.inworld_Status_ok(status))
                return true;
            Debug.LogError(InworldInterop.inworld_Status_ToString(status));
            return false;
        }
        
        public bool AddTextEmbedderInterface(string componentID, TextEmbedderInterface textEmbedderInterface)
        {
            IntPtr status = InworldInterop.inworld_Graph_AddComponent_TextEmbedderInterface(m_DLLPtr, componentID, textEmbedderInterface.ToDLL);
            if (InworldInterop.inworld_Status_ok(status))
                return true;
            Debug.LogError(InworldInterop.inworld_Status_ToString(status));
            return false;
        }
        
        public bool AddKnowledgeInterface(string componentID, KnowledgeInterface knowledgeInterface)
        {
            IntPtr status = InworldInterop.inworld_Graph_AddComponent_KnowledgeInterface(m_DLLPtr, componentID, knowledgeInterface.ToDLL);
            if (InworldInterop.inworld_Status_ok(status))
                return true;
            Debug.LogError(InworldInterop.inworld_Status_ToString(status));
            return false;
        }
        
        public bool AddMCPClientInterface(string componentID, MCPClientInterface mcpClientInterface)
        {
            IntPtr status = InworldInterop.inworld_Graph_AddComponent_MCPClientInterface(m_DLLPtr, componentID, mcpClientInterface.ToDLL);
            if (InworldInterop.inworld_Status_ok(status))
                return true;
            Debug.LogError(InworldInterop.inworld_Status_ToString(status));
            return false;
        }
    }
}