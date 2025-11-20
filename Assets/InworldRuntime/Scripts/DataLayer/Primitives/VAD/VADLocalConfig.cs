/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.VAD
{
    /// <summary>
    /// Provides local configuration settings for Voice Activity Detection processing within the Inworld framework.
    /// Defines device-specific settings, model paths, and parameters for local VAD operations.
    /// Used for configuring VAD components that operate locally on the client device.
    /// </summary>
    public class VADLocalConfig : InworldLocalConfig
    {
        /// <summary>
        /// Initializes a new instance of the VADLocalConfig class with default settings.
        /// </summary>
        public VADLocalConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LocalVADConfig_new(), InworldInterop.inworld_LocalVADConfig_delete);
        }

        /// <summary>
        /// Gets or sets the file path to the local VAD model.
        /// Specifies the location of the voice activity detection model file used for local speech detection.
        /// </summary>
        public override string ModelPath
        {
            get => InworldInterop.inworld_LocalVADConfig_model_path_get(m_DLLPtr);
            set => InworldInterop.inworld_LocalVADConfig_model_path_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the device configuration for local VAD processing.
        /// Specifies the hardware device settings and capabilities for voice activity detection operations.
        /// </summary>
        public override InworldDevice Device
        {
            get => new InworldDevice(InworldInterop.inworld_LocalVADConfig_device_get(m_DLLPtr)); 
            set => InworldInterop.inworld_LocalVADConfig_device_set(m_DLLPtr, value.ToDLL);
        }

        /// <summary>
        /// Gets or sets the default voice activity detection configuration for local VAD processing.
        /// Defines default parameters like thresholds and sensitivity settings for voice activity detection.
        /// </summary>
        public override InworldConfig Config
        {
            get => new VoiceActivityDetectionConfig(InworldInterop.inworld_LocalVADConfig_default_config_get(m_DLLPtr)); 
            set => InworldInterop.inworld_LocalVADConfig_default_config_set(m_DLLPtr, value.ToDLL);
        }
    }
}