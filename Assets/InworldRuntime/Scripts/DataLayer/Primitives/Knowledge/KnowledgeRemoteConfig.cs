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
    /// Provides remote configuration settings for knowledge services within the Inworld framework.
    /// Defines connection parameters and settings for accessing remote knowledge systems.
    /// Used for configuring knowledge components that connect to cloud-based knowledge services.
    /// </summary>
    public class KnowledgeRemoteConfig : InworldRemoteConfig
    {
        /// <summary>
        /// Initializes a new instance of the KnowledgeRemoteConfig class with default settings.
        /// </summary>
        public KnowledgeRemoteConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_RemoteKnowledgeConfig_new(),
                InworldInterop.inworld_RemoteKnowledgeConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the KnowledgeRemoteConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the remote knowledge configuration object.</param>
        public KnowledgeRemoteConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_RemoteKnowledgeConfig_delete);
        }

        /// <summary>
        /// Gets or sets the API key for accessing remote knowledge services.
        /// Provides authentication credentials for connecting to the knowledge service endpoints.
        /// </summary>
        public override string APIKey
        {
            get => InworldInterop.inworld_RemoteKnowledgeConfig_api_key_get(m_DLLPtr); 
            set => InworldInterop.inworld_RemoteKnowledgeConfig_api_key_set(m_DLLPtr, value);
        }

        public Language Language
        {
            get => new Language(InworldInterop.inworld_RemoteKnowledgeConfig_language_get(m_DLLPtr));
            set => InworldInterop.inworld_RemoteKnowledgeConfig_language_set(m_DLLPtr, value.ToDLL);
        }
        /// <summary>
        /// Gets or sets the knowledge compilation configuration.
        /// Defines how knowledge content should be processed and compiled for remote knowledge services.
        /// </summary>
        // Knowledge Compile Config
        public override InworldConfig Config
        {
            get => new KnowledgeCompileConfig(InworldInterop.inworld_RemoteKnowledgeConfig_knowledge_compile_config_get(m_DLLPtr)); 
            set => InworldInterop.inworld_RemoteKnowledgeConfig_knowledge_compile_config_set(m_DLLPtr, value.ToDLL);
        }
    }
}