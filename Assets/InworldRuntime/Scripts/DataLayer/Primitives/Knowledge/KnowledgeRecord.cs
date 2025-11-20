/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.Knowledge
{
    /// <summary>
    /// Represents a single knowledge record within the Inworld framework.
    /// Contains text content, vector embeddings, and metadata for knowledge retrieval operations.
    /// Used for storing and accessing individual pieces of knowledge in the knowledge base.
    /// </summary>
    public class KnowledgeRecord : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the KnowledgeRecord class with default settings.
        /// </summary>
        public KnowledgeRecord()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_KnowledgeRecord_new(),
                InworldInterop.inworld_KnowledgeRecord_delete);
        }

        /// <summary>
        /// Initializes a new instance of the KnowledgeRecord class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the knowledge record object.</param>
        public KnowledgeRecord(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_KnowledgeRecord_delete);
        }

        /// <summary>
        /// Gets or sets the text content of this knowledge record.
        /// Contains the actual textual information stored in this knowledge entry.
        /// </summary>
        public string Text
        {
            get => InworldInterop.inworld_KnowledgeRecord_text_get(m_DLLPtr); 
            set => InworldInterop.inworld_KnowledgeRecord_text_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the vector embedding representation of this knowledge record.
        /// Contains the numerical vector that represents the semantic meaning of the text content.
        /// Used for similarity search and semantic matching operations.
        /// </summary>
        public InworldVector<float> Embedding
        {
            get => new InworldVector<float>(InworldInterop.inworld_KnowledgeRecord_embedding_get(m_DLLPtr)); 
            set => InworldInterop.inworld_KnowledgeRecord_embedding_set(m_DLLPtr, value.ToDLL);
        }

        /// <summary>
        /// Gets or sets the metadata associated with this knowledge record.
        /// Contains additional information and tags that provide context about the knowledge content.
        /// Used for categorization, filtering, and enhanced retrieval operations.
        /// </summary>
        public InworldVector<string> MetaData
        {
            get => new InworldVector<string>(InworldInterop.inworld_KnowledgeRecord_metadata_get(m_DLLPtr)); 
            set => InworldInterop.inworld_KnowledgeRecord_metadata_set(m_DLLPtr, value.ToDLL);
        }
    }
}