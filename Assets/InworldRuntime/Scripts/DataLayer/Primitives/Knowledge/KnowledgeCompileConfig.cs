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
    /// Provides configuration settings for knowledge compilation within the Inworld framework.
    /// Defines parameters and options for processing and indexing knowledge content.
    /// Used for configuring how knowledge data is parsed, processed, and prepared for retrieval.
    /// </summary>
    public class KnowledgeCompileConfig : InworldConfig
    {
        /// <summary>
        /// Initializes a new instance of the KnowledgeCompileConfig class with default settings.
        /// </summary>
        public KnowledgeCompileConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_KnowledgeCompileConfig_new(),
                InworldInterop.inworld_KnowledgeCompileConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the KnowledgeCompileConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the knowledge compile configuration object.</param>
        public KnowledgeCompileConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_KnowledgeCompileConfig_delete);
        }

        /// <summary>
        /// Gets or sets the parsing configuration for knowledge compilation.
        /// Defines how knowledge content should be parsed and structured during the compilation process.
        /// Controls text processing, chunking, and formatting options.
        /// </summary>
        public InworldParsingConfig ParsingConfig
        {
            get => new InworldParsingConfig(InworldInterop.inworld_KnowledgeCompileConfig_parsing_config_get(m_DLLPtr));
            set => InworldInterop.inworld_KnowledgeCompileConfig_parsing_config_set(m_DLLPtr, value.ToDLL);
        }
    }
}