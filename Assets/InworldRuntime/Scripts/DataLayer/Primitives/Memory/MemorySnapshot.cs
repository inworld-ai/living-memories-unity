/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.Memory
{
    /// <summary>
    /// Represents a snapshot of the complete memory state within the Inworld framework.
    /// Captures the current state of all memory components including flash memory, long-term memory, and rolling summary.
    /// Used for saving, restoring, and managing the overall memory state of conversational agents.
    /// </summary>
    public class MemorySnapshot : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the MemorySnapshot class with default settings.
        /// </summary>
        public MemorySnapshot()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_MemorySnapshot_new(),
                InworldInterop.inworld_MemorySnapshot_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the MemorySnapshot class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the memory snapshot object.</param>
        public MemorySnapshot(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_MemorySnapshot_delete);
        }

        /// <summary>
        /// Gets or sets the flash memory component of this memory snapshot.
        /// Contains the temporary memory state captured in this snapshot.
        /// </summary>
        public FlashMemory FlashMemory
        {
            get => new FlashMemory(InworldInterop.inworld_MemorySnapshot_flash_memory_get(m_DLLPtr));
            set => InworldInterop.inworld_MemorySnapshot_flash_memory_set(m_DLLPtr, value.ToDLL);
        }

        /// <summary>
        /// Gets or sets the long-term memory component of this memory snapshot.
        /// Contains the persistent memory state captured in this snapshot.
        /// </summary>
        public LongTermMemory LongTermMemory
        {
            get => new LongTermMemory(InworldInterop.inworld_MemorySnapshot_long_term_memory_get(m_DLLPtr));
            set => InworldInterop.inworld_MemorySnapshot_long_term_memory_set(m_DLLPtr, value.ToDLL);
        }

        /// <summary>
        /// Gets or sets the rolling summary component of this memory snapshot.
        /// Contains the conversation summary state captured in this snapshot.
        /// </summary>
        public RollingSummary RollingSummary
        {
            get => new RollingSummary(InworldInterop.inworld_MemorySnapshot_rolling_summary_get(m_DLLPtr));
            set => InworldInterop.inworld_MemorySnapshot_rolling_summary_set(m_DLLPtr, value.ToDLL);
        }
    }
}