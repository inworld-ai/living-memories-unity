/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.Node;

namespace Inworld.Framework.LLM
{
    /// <summary>
    /// Provides configuration settings for creating Large Language Model (LLM) instances within the Inworld framework.
    /// Manages both remote and local LLM configurations and their availability.
    /// Used for configuring how LLM components are created and initialized for text generation.
    /// </summary>
    public class LLMCreationConfig : NodeCreationConfig
    {
        /// <summary>
        /// Initializes a new instance of the LLMCreationConfig class with default settings.
        /// </summary>
        public LLMCreationConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LLMCreationConfig_new(),
                InworldInterop.inworld_LLMCreationConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the LLMCreationConfig class from a native pointer.
        /// </summary>
        /// <param name="dllPtr">The native pointer to the LLM creation config object.</param>
        public LLMCreationConfig(IntPtr dllPtr)
        {
            m_DLLPtr = MemoryManager.Register(dllPtr, InworldInterop.inworld_LLMCreationConfig_delete);
        }

        /// <summary>
        /// Gets a value indicating whether remote LLM configuration is available and configured.
        /// Used to check if remote LLM services can be utilized.
        /// </summary>
        public bool HasRemote => InworldInterop.inworld_LLMCreationConfig_has_remote_llm_config(m_DLLPtr);
        
        /// <summary>
        /// Gets a value indicating whether local LLM configuration is available and configured.
        /// Used to check if local LLM models can be utilized.
        /// </summary>
        public bool HasLocal => InworldInterop.inworld_LLMCreationConfig_has_local_llm_config(m_DLLPtr);

        /// <summary>
        /// Gets or sets the remote LLM configuration settings.
        /// Defines connection parameters and settings for accessing remote LLM services.
        /// </summary>
        public LLMRemoteConfig Remote
        {
            get => new LLMRemoteConfig(InworldInterop.inworld_LLMCreationConfig_get_remote_llm_config(m_DLLPtr));
            set => InworldInterop.inworld_LLMCreationConfig_set_remote_llm_config(m_DLLPtr, value.ToDLL);
        }

        /// <summary>
        /// Gets or sets the local LLM configuration settings.
        /// Defines model path, device settings, and parameters for local LLM processing.
        /// </summary>
        public LLMLocalConfig Local
        {
            get => new LLMLocalConfig(InworldInterop.inworld_LLMCreationConfig_get_local_llm_config(m_DLLPtr));
            set => InworldInterop.inworld_LLMCreationConfig_set_remote_llm_config(m_DLLPtr, value.ToDLL);
        }
    }
}