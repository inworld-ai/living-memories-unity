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
    /// Provides local configuration settings for knowledge processing within the Inworld framework.
    /// Defines device-specific settings and parameters for local knowledge operations.
    /// Used for configuring knowledge components that operate locally on the client device.
    /// </summary>
    public class KnowledgeLocalConfig : InworldLocalConfig
    {
        /// <summary>
        /// Initializes a new instance of the KnowledgeLocalConfig class with default settings.
        /// </summary>
        public KnowledgeLocalConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LocalKnowledgeConfig_new(),
                InworldInterop.inworld_LocalKnowledgeConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the KnowledgeLocalConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the local knowledge configuration object.</param>
        public KnowledgeLocalConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_LocalKnowledgeConfig_delete);
        }

        /// <summary>
        /// Gets or sets the model path for local knowledge processing.
        /// Specifies the file path to the knowledge model or database used for local operations.
        /// </summary>
        public override string ModelPath { get; set; }
        
        /// <summary>
        /// Gets or sets the device configuration for knowledge processing.
        /// Specifies the hardware device settings and capabilities for knowledge operations.
        /// </summary>
        public override InworldDevice Device { get; set; }

        
        /// <summary>
        /// Gets or sets the knowledge compilation configuration.
        /// Defines how knowledge content should be processed and compiled for local knowledge operations.
        /// </summary>
        // Knowledge Compile Config
        public override InworldConfig Config
        {
            get => new KnowledgeCompileConfig(
                InworldInterop.inworld_LocalKnowledgeConfig_knowledge_compile_config_get(m_DLLPtr));
            set => InworldInterop.inworld_LocalKnowledgeConfig_knowledge_compile_config_set(m_DLLPtr, value.ToDLL);
        }

        public string EmbedderComponentID
        {
            get => InworldInterop.inworld_LocalKnowledgeConfig_embedder_component_id_get(m_DLLPtr);
            set => InworldInterop.inworld_LocalKnowledgeConfig_embedder_component_id_set(m_DLLPtr, value);
        }
    }
}