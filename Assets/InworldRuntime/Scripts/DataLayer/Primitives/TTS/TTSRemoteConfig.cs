/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;


namespace Inworld.Framework.TTS
{
    /// <summary>
    /// Provides remote configuration settings for Text-to-Speech services within the Inworld framework.
    /// Defines connection parameters and authentication for remote speech synthesis APIs.
    /// Used for configuring connections to cloud-based speech synthesis services.
    /// </summary>
    public class TTSRemoteConfig : InworldRemoteConfig
    {
        /// <summary>
        /// Initializes a new instance of the TTSRemoteConfig class with default settings.
        /// </summary>
        public TTSRemoteConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_RemoteTTSConfig_new(), InworldInterop.inworld_RemoteTTSConfig_delete);
        }
        
        /// <summary>
        /// Gets or sets the API key for accessing remote TTS services.
        /// Provides authentication credentials for connecting to the speech synthesis service endpoints.
        /// </summary>
        public override string APIKey
        {
            get => m_DLLPtr == IntPtr.Zero ? "" : InworldInterop.inworld_RemoteTTSConfig_api_key_get(m_DLLPtr);
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_RemoteTTSConfig_api_key_set(m_DLLPtr, value);
            }
        }
        
        /// <summary>
        /// Gets or sets the speech synthesis configuration for this remote TTS service.
        /// Defines default parameters for speech synthesis operations using SpeechSynthesizeConfig.
        /// </summary>
        //SpeechSynthesizeConfig
        public override InworldConfig Config
        {
            get => m_DLLPtr == IntPtr.Zero ? null : new SpeechSynthesisConfig(InworldInterop.inworld_RemoteTTSConfig_synthesis_config_get(m_DLLPtr));
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_RemoteTTSConfig_synthesis_config_set(m_DLLPtr, value.ToDLL);
            }
        }
    }
}