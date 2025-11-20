/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.Graph
{
    /// <summary>
    /// Represents a loop structure within a graph that defines cyclic execution paths.
    /// Contains information about loop entry points and the paths that form the loop.
    /// Used for managing iterative processing and cyclic workflows in graph execution.
    /// </summary>
    public class InworldLoop : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldLoop class with default settings.
        /// </summary>
        public InworldLoop()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_Loop_new(), InworldInterop.inworld_Loop_delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldLoop class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the loop object.</param>
        public InworldLoop(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_Loop_delete);
        }

        /// <summary>
        /// Gets or sets the identifier of the node that serves as the entry point for this loop.
        /// Execution enters the loop through this designated node.
        /// </summary>
        public string EntryNodeID
        {
            get => InworldInterop.inworld_Loop_entry_node_id_get(m_DLLPtr);
            set => InworldInterop.inworld_Loop_entry_node_id_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the collection of paths that define the loop structure.
        /// Maps node identifiers to sets of connected node identifiers that form loop cycles.
        /// </summary>
        public InworldHashMap<string, InworldHashSet<string>> LoopPath
        {
            get => new InworldHashMap<string, InworldHashSet<string>>(InworldInterop.inworld_Loop_loop_paths_get(m_DLLPtr));
            set => InworldInterop.inworld_Loop_loop_paths_set(m_DLLPtr, value.ToDLL);
        }
    }
}