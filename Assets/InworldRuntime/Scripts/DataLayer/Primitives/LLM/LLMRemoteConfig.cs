/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.LLM
{
    /// <summary>
    /// Provides remote configuration settings for Large Language Model services within the Inworld framework.
    /// Defines connection parameters, provider settings, and authentication for remote LLM access.
    /// Used for configuring connections to cloud-based LLM services and APIs.
    /// </summary>
    public class LLMRemoteConfig : InworldRemoteConfig
    {
        /// <summary>
        /// Initializes a new instance of the LLMRemoteConfig class with default settings.
        /// </summary>
        public LLMRemoteConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_RemoteLLMConfig_new(), InworldInterop.inworld_RemoteLLMConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the LLMRemoteConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the remote LLM config object.</param>
        public LLMRemoteConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_RemoteLLMConfig_delete);
        }

        /// <summary>
        /// Gets or sets the LLM service provider identifier.
        /// Specifies which LLM service provider to use (e.g., "openai", "anthropic", etc.).
        /// </summary>
        public string Provider
        {
            get => m_DLLPtr == IntPtr.Zero ? "" : InworldInterop.inworld_RemoteLLMConfig_provider_get(m_DLLPtr);
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_RemoteLLMConfig_provider_set(m_DLLPtr, value);
            }
        }
        
        /// <summary>
        /// Gets or sets the specific model name to use from the LLM provider.
        /// Identifies the exact model variant for text generation (e.g., "gpt-4", "claude-3", etc.).
        /// </summary>
        public string ModelName
        {
            get => m_DLLPtr == IntPtr.Zero ? "" : InworldInterop.inworld_RemoteLLMConfig_model_name_get(m_DLLPtr);
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_RemoteLLMConfig_model_name_set(m_DLLPtr, value);
            }
        }
        
        /// <summary>
        /// Gets or sets the API key for accessing the remote LLM service.
        /// Provides authentication credentials for connecting to the LLM service endpoints.
        /// </summary>
        public override string APIKey
        {
            get => m_DLLPtr == IntPtr.Zero ? "" : InworldInterop.inworld_RemoteLLMConfig_api_key_get(m_DLLPtr);
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_RemoteLLMConfig_api_key_set(m_DLLPtr, value);
            }
        }
        
        /// <summary>
        /// Gets or sets the default text generation configuration for this remote LLM.
        /// Defines default parameters like temperature, max tokens, and penalties for text generation.
        /// </summary>
        //TextGenerationConfig
        public override InworldConfig Config
        {
            get => new TextGenerationConfig(InworldInterop.inworld_RemoteLLMConfig_default_config_get(m_DLLPtr));
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_RemoteLLMConfig_default_config_set(m_DLLPtr, value.ToDLL);
            }
        }
    }
}