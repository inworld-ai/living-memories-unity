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
    /// Provides local configuration settings for Speech-to-Text processing within the Inworld framework.
    /// Defines device-specific settings, model paths, and parameters for local speech recognition operations.
    /// Used for configuring STT components that operate locally on the client device.
    /// </summary>
    public class STTLocalConfig : InworldLocalConfig
    {
        /// <summary>
        /// Initializes a new instance of the STTLocalConfig class with default settings.
        /// </summary>
        public STTLocalConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LocalSTTConfig_new(), 
                InworldInterop.inworld_LocalSTTConfig_delete);
        }

        /// <summary>
        /// Gets or sets the file path to the local STT model.
        /// Specifies the location of the speech recognition model file used for local audio-to-text conversion.
        /// </summary>
        public override string ModelPath
        {
            get => InworldInterop.inworld_LocalSTTConfig_model_path_get(m_DLLPtr); 
            set => InworldInterop.inworld_LocalSTTConfig_model_path_set(m_DLLPtr, value);
        }
        
        /// <summary>
        /// Gets or sets the device configuration for local STT processing.
        /// Specifies the hardware device settings and capabilities for speech recognition operations.
        /// </summary>
        public override InworldDevice Device
        {
            get => new InworldDevice(InworldInterop.inworld_LocalSTTConfig_device_get(m_DLLPtr)); 
            set => InworldInterop.inworld_LocalSTTConfig_device_set(m_DLLPtr, value.ToDLL);
        }

        /// <summary>
        /// Gets or sets the default speech recognition configuration for local STT processing.
        /// Defines default parameters like language models and recognition settings for local speech recognition.
        /// </summary>
        public override InworldConfig Config
        {
            get => new SpeechRecognitionConfig(InworldInterop.inworld_LocalSTTConfig_default_config_get(m_DLLPtr)); 
            set => InworldInterop.inworld_LocalSTTConfig_default_config_set(m_DLLPtr, value.ToDLL);
        }
    }
}