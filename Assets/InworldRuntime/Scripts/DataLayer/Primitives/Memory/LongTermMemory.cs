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
    /// Represents long-term memory for persistent storage within the Inworld framework.
    /// Provides durable storage for important knowledge and information that should persist across sessions.
    /// Used for storing long-term memory that maintains context and learning over extended conversations.
    /// </summary>
    public class LongTermMemory : InworldMemory
    {
        /// <summary>
        /// Initializes a new instance of the LongTermMemory class with default settings.
        /// </summary>
        public LongTermMemory()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LongTermMemory_new(),
                InworldInterop.inworld_LongTermMemory_delete);
        }

        /// <summary>
        /// Initializes a new instance of the LongTermMemory class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the long-term memory object.</param>
        public LongTermMemory(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_LongTermMemory_delete);
        }

        /// <summary>
        /// Gets or sets the knowledge collection stored in long-term memory.
        /// Provides access to the persistent knowledge records maintained in this memory component.
        /// </summary>
        public override KnowledgeCollection KnowledgeCollection
        {
            get => new KnowledgeCollection(InworldInterop.inworld_LongTermMemory_knowledge_collection_get(m_DLLPtr)); 
            set => InworldInterop.inworld_LongTermMemory_knowledge_collection_set(m_DLLPtr, value.ToDLL);
        }
    }
}