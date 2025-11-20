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
    /// Represents a collection of knowledge records within the Inworld framework.
    /// Manages a group of knowledge records for memory storage and retrieval operations.
    /// Used for organizing and accessing multiple knowledge records in memory components.
    /// </summary>
    public class KnowledgeCollection : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the KnowledgeCollection class with default settings.
        /// </summary>
        public KnowledgeCollection()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_KnowledgeCollection_new(),
                InworldInterop.inworld_KnowledgeCollection_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the KnowledgeCollection class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the knowledge collection object.</param>
        public KnowledgeCollection(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_KnowledgeCollection_delete);
        }

        /// <summary>
        /// Gets or sets the vector of knowledge records in this collection.
        /// Contains the individual knowledge records that make up this collection.
        /// </summary>
        public InworldVector<KnowledgeRecord> KnowledgeRecords
        {
            get => new InworldVector<KnowledgeRecord>(InworldInterop.inworld_KnowledgeCollection_knowledge_records_get(m_DLLPtr));
            set => InworldInterop.inworld_KnowledgeCollection_knowledge_records_set(m_DLLPtr, value.ToDLL);
        }
    }
}