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
    /// Represents an edge connection between nodes in a graph structure.
    /// Defines the flow of execution and data transfer between connected nodes.
    /// Used for building graph relationships and controlling execution pathways.
    /// </summary>
    public class InworldEdge : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldEdge class with default settings.
        /// </summary>
        public InworldEdge()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_Edge_new(), InworldInterop.inworld_Edge_delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldEdge class from a native pointer.
        /// </summary>
        /// <param name="dllPtr">The native pointer to the edge object.</param>
        public InworldEdge(IntPtr dllPtr)
        {
            m_DLLPtr = MemoryManager.Register(dllPtr, InworldInterop.inworld_Edge_delete);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this edge is part of a loop.
        /// Loop edges can be traversed multiple times during graph execution.
        /// </summary>
        public bool IsLoop
        {
            get => InworldInterop.inworld_Edge_is_loop_edge_get(m_DLLPtr);
            set => InworldInterop.inworld_Edge_is_loop_edge_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this edge is required for execution.
        /// Required edges must be traversed, while optional edges may be skipped.
        /// </summary>
        public bool IsRequired
        {
            get => InworldInterop.inworld_Edge_is_required_get(m_DLLPtr);
            set => InworldInterop.inworld_Edge_is_required_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the identifier of the destination node for this edge.
        /// Defines where the execution flow will proceed when this edge is traversed.
        /// </summary>
        public string ToNodeID
        {
            get => InworldInterop.inworld_Edge_to_node_id_get(m_DLLPtr);
            set => InworldInterop.inworld_Edge_to_node_id_set(m_DLLPtr, value);
        }
    }
}