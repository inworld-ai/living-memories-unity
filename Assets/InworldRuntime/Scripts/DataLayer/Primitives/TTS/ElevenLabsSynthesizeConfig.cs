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
    /// Provides configuration settings specific to ElevenLabs speech synthesis within the Inworld framework.
    /// Defines parameters for integrating with ElevenLabs text-to-speech services.
    /// Used for configuring ElevenLabs-specific synthesis options including query and request parameters.
    /// </summary>
    public class ElevenLabsSynthesizeConfig : InworldConfig
    {
        /// <summary>
        /// Initializes a new instance of the ElevenLabsSynthesizeConfig class with default settings.
        /// </summary>
        public ElevenLabsSynthesizeConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_ElevenLabsSpeechSynthesisConfig_new(),
                InworldInterop.inworld_ElevenLabsSpeechSynthesisConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the ElevenLabsSynthesizeConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the ElevenLabs synthesis config object.</param>
        public ElevenLabsSynthesizeConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_ElevenLabsSpeechSynthesisConfig_delete);
        }

        /// <summary>
        /// Gets or sets the query parameters for ElevenLabs API requests.
        /// Defines URL query parameters and options for API communication with ElevenLabs services.
        /// </summary>
        public InworldQueryParams QueryParams
        {
            set => InworldInterop.inworld_ElevenLabsSpeechSynthesisConfig_query_params_set(m_DLLPtr, value.ToDLL);
            get => new InworldQueryParams(InworldInterop.inworld_ElevenLabsSpeechSynthesisConfig_query_params_get(m_DLLPtr)); 
        }
        
        /// <summary>
        /// Gets or sets the request parameters for ElevenLabs API calls.
        /// Defines request body parameters and headers for API communication with ElevenLabs services.
        /// </summary>
        public InworldRequestParams RequestParams
        {
            set => InworldInterop.inworld_ElevenLabsSpeechSynthesisConfig_request_params_set(m_DLLPtr, value.ToDLL);
            get => new InworldRequestParams(InworldInterop.inworld_ElevenLabsSpeechSynthesisConfig_request_params_get(m_DLLPtr)); 
        }
    }
}