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
    /// Provides local configuration settings for Text-to-Speech processing within the Inworld framework.
    /// Defines device-specific settings, model paths, and parameters for local TTS operations.
    /// Used for configuring TTS components that operate locally on the client device.
    /// </summary>
    public class TTSLocalConfig : InworldLocalConfig
    {
        /// <summary>
        /// Initializes a new instance of the TTSLocalConfig class with default settings.
        /// </summary>
        public TTSLocalConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LocalTTSConfig_new(), 
                InworldInterop.inworld_LocalTTSConfig_delete);
        }

        /// <summary>
        /// Gets or sets the file path to the local TTS model.
        /// Specifies the location of the speech synthesis model file used for local text-to-speech conversion.
        /// </summary>
        public override string ModelPath
        {
            get => InworldInterop.inworld_LocalTTSConfig_model_path_get(m_DLLPtr); 
            set => InworldInterop.inworld_LocalTTSConfig_model_path_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the device configuration for local TTS processing.
        /// Specifies the hardware device settings and capabilities for speech synthesis operations.
        /// </summary>
        public override InworldDevice Device
        {
            get => new InworldDevice(InworldInterop.inworld_LocalTTSConfig_device_get(m_DLLPtr)); 
            set => InworldInterop.inworld_LocalTTSConfig_device_set(m_DLLPtr, value.ToDLL);
        }
        
        /// <summary>
        /// Gets or sets the file path to the prompts directory for TTS.
        /// Specifies the location of prompt files or templates used for speech synthesis enhancement.
        /// </summary>
        public string PromptPath
        {
            get => InworldInterop.inworld_LocalTTSConfig_prompts_path_get(m_DLLPtr); 
            set => InworldInterop.inworld_LocalTTSConfig_prompts_path_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the synthesis configuration for local TTS processing.
        /// Defines speech synthesis parameters and settings for local text-to-speech operations.
        /// </summary>
        public override InworldConfig Config
        {
            get => new InworldSynthesizeConfig(InworldInterop.inworld_LocalTTSConfig_synthesis_config_get(m_DLLPtr)); 
            set => InworldInterop.inworld_LocalTTSConfig_synthesis_config_set(m_DLLPtr, value.ToDLL);
        }
    }
}