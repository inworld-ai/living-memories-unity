/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.TextEmbedder
{
    /// <summary>
    /// Provides remote configuration settings for text embedding services within the Inworld framework.
    /// Defines connection parameters, provider settings, and authentication for remote text embedding APIs.
    /// Used for configuring connections to cloud-based text embedding services and models.
    /// </summary>
    public class TextEmbedderRemoteConfig : InworldRemoteConfig
    {
        /// <summary>
        /// Initializes a new instance of the TextEmbedderRemoteConfig class with default settings.
        /// </summary>
        public TextEmbedderRemoteConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_RemoteTextEmbedderConfig_new(),
                InworldInterop.inworld_RemoteTextEmbedderConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the TextEmbedderRemoteConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the remote text embedder config object.</param>
        public TextEmbedderRemoteConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_RemoteTextEmbedderConfig_delete);
        }
        
        /// <summary>
        /// Gets or sets the API key for accessing remote text embedding services.
        /// Provides authentication credentials for connecting to the text embedding service endpoints.
        /// </summary>
        public override string APIKey
        {
            get => InworldInterop.inworld_RemoteTextEmbedderConfig_api_key_get(m_DLLPtr); 
            set => InworldInterop.inworld_RemoteTextEmbedderConfig_api_key_set(m_DLLPtr, value);
        }
        
        /// <summary>
        /// Gets or sets the specific model name to use from the text embedding provider.
        /// Identifies the exact embedding model variant to use for text vectorization.
        /// </summary>
        public string ModelName
        {
            get => InworldInterop.inworld_RemoteTextEmbedderConfig_model_name_get(m_DLLPtr); 
            set => InworldInterop.inworld_RemoteTextEmbedderConfig_model_name_set(m_DLLPtr, value);
        }
        
        /// <summary>
        /// Gets or sets the text embedding service provider identifier.
        /// Specifies which text embedding service provider to use (e.g., "openai", "cohere", etc.).
        /// </summary>
        public string Provider
        {
            get => InworldInterop.inworld_RemoteTextEmbedderConfig_provider_get(m_DLLPtr); 
            set => InworldInterop.inworld_RemoteTextEmbedderConfig_provider_set(m_DLLPtr, value);
        }
        
        /// <summary>
        /// Gets or sets additional configuration settings for the text embedder.
        /// This property is not currently used in the implementation.
        /// </summary>
        public override InworldConfig Config { get; set; } // Not Used.
    }
}