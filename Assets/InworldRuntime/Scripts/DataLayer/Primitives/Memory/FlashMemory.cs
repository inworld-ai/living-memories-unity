/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.Knowledge;

namespace Inworld.Framework.Memory
{
    /// <summary>
    /// Represents flash memory for temporary storage within the Inworld framework.
    /// Provides quick access to recently accessed or frequently used knowledge information.
    /// Used for storing short-term memory that may be quickly accessed or modified during conversations.
    /// </summary>
    public class FlashMemory : InworldMemory
    {
        /// <summary>
        /// Initializes a new instance of the FlashMemory class with default settings.
        /// </summary>
        public FlashMemory()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_FlashMemory_new(),
                InworldInterop.inworld_FlashMemory_delete);
        }

        /// <summary>
        /// Initializes a new instance of the FlashMemory class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the flash memory object.</param>
        public FlashMemory(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_FlashMemory_delete);
        }

        /// <summary>
        /// Gets or sets the knowledge collection stored in flash memory.
        /// Provides access to the temporary knowledge records maintained in this memory component.
        /// </summary>
        public override KnowledgeCollection KnowledgeCollection
        {
            get => new KnowledgeCollection(InworldInterop.inworld_FlashMemory_knowledge_collection_get(m_DLLPtr));
            set => InworldInterop.inworld_FlashMemory_knowledge_collection_set(m_DLLPtr, value.ToDLL);
        }
    }
}