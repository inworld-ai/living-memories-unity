/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.Node;

namespace Inworld.Framework.Graph
{
    /// <summary>
    /// Provides an interface to a compiled graph ready for execution.
    /// Contains optimized graph structure information and creates execution interfaces.
    /// Used for introspecting compiled graphs and creating executors for runtime processing.
    /// </summary>
    public class CompiledGraphInterface : InworldInterface
    {
        // TODO(Yan): Put it in InworldGraphExecutor since This destructor will crash.
        /// <summary>
        /// Initializes a new instance of the CompiledGraphInterface class from a native pointer.
        /// Note: Memory management is handled manually due to destructor issues.
        /// </summary>
        /// <param name="dll">The native pointer to the compiled graph interface object.</param>
        public CompiledGraphInterface(IntPtr dll) =>
            m_DLLPtr = dll; //MemoryManager.Register(dll, InworldInterop.inworld_CompiledGraphInterface_delete);

        /// <summary>
        /// Gets the unique identifier of this compiled graph.
        /// Used for referencing and managing multiple compiled graphs.
        /// </summary>
        public string ID => InworldInterop.inworld_CompiledGraphInterface_id(m_DLLPtr);

        /// <summary>
        /// Gets a map of all nodes in the compiled graph, indexed by their string identifiers.
        /// Provides access to the complete node structure of the compiled graph.
        /// </summary>
        public InworldHashMap<string, InworldNode> Nodes => 
            new InworldHashMap<string, InworldNode>(InworldInterop.inworld_CompiledGraphInterface_Nodes(m_DLLPtr));

        /// <summary>
        /// Gets the set of nodes designated as starting points for graph execution.
        /// Execution will begin from these nodes when the graph is run.
        /// </summary>
        public InworldHashSet<InworldNode> StartNodes =>
            new InworldHashSet<InworldNode>(InworldInterop.inworld_CompiledGraphInterface_StartNodes(m_DLLPtr));
        
        /// <summary>
        /// Gets the set of nodes designated as ending points for graph execution.
        /// Execution will terminate when reaching any of these nodes.
        /// </summary>
        public InworldHashSet<InworldNode> EndNodes =>
            new InworldHashSet<InworldNode>(InworldInterop.inworld_CompiledGraphInterface_EndNodes(m_DLLPtr));

        /// <summary>
        /// Gets a map of all loops in the compiled graph, indexed by their string identifiers.
        /// Provides access to loop structures for cycle detection and analysis.
        /// </summary>
        public InworldHashMap<string, InworldLoop> Loops => new InworldHashMap<string, InworldLoop>(InworldInterop.inworld_CompiledGraphInterface_Loops(m_DLLPtr));

        /// <summary>
        /// Gets all edges with the specified name from the compiled graph.
        /// Used for analyzing edge connections and data flow patterns.
        /// </summary>
        /// <param name="edgeName">The name of the edges to retrieve.</param>
        /// <returns>A vector containing all edges with the specified name.</returns>
        public InworldVector<InworldEdge> Edges(string edgeName) =>
            new InworldVector<InworldEdge>(InworldInterop.inworld_CompiledGraphInterface_Edges(m_DLLPtr, edgeName));

        /// <summary>
        /// Gets the parent node identifiers for a specific graph or subgraph.
        /// Used for hierarchical graph analysis and navigation.
        /// </summary>
        /// <param name="graphName">The name of the graph to get parent nodes for.</param>
        /// <returns>A vector containing the identifiers of parent nodes.</returns>
        public InworldVector<string> ParentNodeIDs(string graphName)
        {
            return new InworldVector<string>(
                InworldInterop.inworld_CompiledGraphInterface_ParentNodeIds(m_DLLPtr, graphName));
        }

        /// <summary>
        /// Generates a visualization of the specified graph or subgraph.
        /// Creates a visual representation for debugging and analysis purposes.
        /// </summary>
        /// <param name="graphName">The name of the graph to visualize.</param>
        /// <returns>A status object indicating the success or failure of visualization generation.</returns>
        public InworldStatus Visualize(string graphName)
        {
            return new InworldStatus(InworldInterop.inworld_CompiledGraphInterface_Visualize(m_DLLPtr, graphName));
        }
        
        /// <summary>
        /// Creates an executor interface for running this compiled graph.
        /// The executor interface manages graph execution sessions and result retrieval.
        /// </summary>
        /// <returns>An InworldExecutorInterface for executing this compiled graph.</returns>
        public ExecutorInterface CreateExecuteInterface()
        {
            return new ExecutorInterface(InworldInterop
                .inworld_GraphExecutorFactoryHelper_Create_pinworld_graphs_CompiledGraphInterface(m_DLLPtr));
        }
    }
}