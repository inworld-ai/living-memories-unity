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
    /// Provides configuration settings for creating text embedder instances within the Inworld framework.
    /// Manages both remote and local text embedder configurations and their availability.
    /// Used for configuring how text embedder components are created and initialized for vector generation.
    /// </summary>
    public class TextEmbedderCreationConfig : InworldConfig
    {
        /// <summary>
        /// Initializes a new instance of the TextEmbedderCreationConfig class with default settings.
        /// </summary>
        public TextEmbedderCreationConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_TextEmbedderCreationConfig_new(),
                InworldInterop.inworld_TextEmbedderCreationConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the TextEmbedderCreationConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the text embedder creation config object.</param>
        public TextEmbedderCreationConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_TextEmbedderCreationConfig_delete);
        }
        
        /// <summary>
        /// Gets or sets the local configuration settings for text embedding.
        /// Defines model path, device settings, and parameters for local text embedding operations.
        /// </summary>
        public InworldLocalConfig LocalConfig
        {
            get => new TextEmbedderLocalConfig(InworldInterop.inworld_TextEmbedderCreationConfig_get_local_config(m_DLLPtr));
            set => InworldInterop.inworld_TextEmbedderCreationConfig_set_local_config(m_DLLPtr, value.ToDLL);
        }
        
        /// <summary>
        /// Gets or sets the remote configuration settings for text embedding.
        /// Defines connection parameters and settings for accessing remote text embedding services.
        /// </summary>
        public InworldRemoteConfig RemoteConfig
        {
            get => new TextEmbedderRemoteConfig(InworldInterop.inworld_TextEmbedderCreationConfig_get_remote_config(m_DLLPtr));
            set => InworldInterop.inworld_TextEmbedderCreationConfig_set_remote_config(m_DLLPtr, value.ToDLL);
        }
        
        /// <summary>
        /// Gets a value indicating whether remote text embedder configuration is available and configured.
        /// Used to check if remote text embedding services can be utilized.
        /// </summary>
        public bool HasRemoteConfig => InworldInterop.inworld_TextEmbedderCreationConfig_has_remote_config(m_DLLPtr);

        /// <summary>
        /// Gets a value indicating whether local text embedder configuration is available and configured.
        /// Used to check if local text embedding models can be utilized.
        /// </summary>
        public bool HasLocalConfig => InworldInterop.inworld_TextEmbedderCreationConfig_has_local_config(m_DLLPtr);

    }
}