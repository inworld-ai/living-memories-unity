/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework
{
    /// <summary>
    /// Provides configuration settings for text parsing operations within the Inworld framework.
    /// Defines parameters for how documents and text are parsed, chunked, and processed.
    /// Used for configuring document ingestion and text processing pipelines.
    /// </summary>
    public class InworldParsingConfig : InworldConfig
    {
        /// <summary>
        /// Initializes a new instance of the InworldParsingConfig class with default settings.
        /// </summary>
        public InworldParsingConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_ParsingConfig_new(), 
                InworldInterop.inworld_ParsingConfig_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the InworldParsingConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the parsing config object.</param>
        public InworldParsingConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_ParsingConfig_delete);
        }

        /// <summary>
        /// Gets or sets the maximum number of characters allowed per text chunk.
        /// Determines how large individual text chunks can be when documents are split for processing.
        /// Smaller values create more granular chunks but may break semantic coherence.
        /// </summary>
        public int MaxCharsPerChunk
        {
            get => InworldInterop.inworld_ParsingConfig_max_chars_per_chunk_get(m_DLLPtr);
            set => InworldInterop.inworld_ParsingConfig_max_chars_per_chunk_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the maximum number of chunks allowed per document.
        /// Limits how many chunks a single document can be split into during parsing.
        /// Helps control memory usage and processing time for large documents.
        /// </summary>
        public int MaxChunksPerDoc
        {
            get => InworldInterop.inworld_ParsingConfig_max_chunks_per_document_get(m_DLLPtr);
            set => InworldInterop.inworld_ParsingConfig_max_chunks_per_document_set(m_DLLPtr, value);
        }
    }
}