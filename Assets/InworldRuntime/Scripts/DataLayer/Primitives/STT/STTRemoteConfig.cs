/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.STT
{
    /// <summary>
    /// Provides remote configuration settings for Speech-to-Text services within the Inworld framework.
    /// Defines connection parameters and authentication for remote speech recognition APIs.
    /// Used for configuring connections to cloud-based speech recognition services.
    /// </summary>
    public class STTRemoteConfig : InworldRemoteConfig
    {
        /// <summary>
        /// Initializes a new instance of the STTRemoteConfig class with default settings.
        /// </summary>
        public STTRemoteConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_RemoteSTTConfig_new(),
                InworldInterop.inworld_RemoteSTTConfig_delete);
        }

        /// <summary>
        /// Gets or sets the API key for accessing remote STT services.
        /// Provides authentication credentials for connecting to the speech recognition service endpoints.
        /// </summary>
        public override string APIKey
        {
            get => m_DLLPtr == IntPtr.Zero ? "" : InworldInterop.inworld_RemoteSTTConfig_api_key_get(m_DLLPtr);
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_RemoteSTTConfig_api_key_set(m_DLLPtr, value);
            }
        }
        
        /// <summary>
        /// Gets or sets the default speech recognition configuration for this remote STT service.
        /// Defines default parameters for speech recognition operations like language models and recognition settings.
        /// </summary>
        public override InworldConfig Config
        {
            get => new SpeechRecognitionConfig(InworldInterop.inworld_RemoteSTTConfig_default_config_get(m_DLLPtr)); 
            set => InworldInterop.inworld_RemoteSTTConfig_default_config_set(m_DLLPtr, value.ToDLL);
        }
    }
}