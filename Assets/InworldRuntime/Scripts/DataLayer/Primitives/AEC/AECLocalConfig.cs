/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;


namespace Inworld.Framework.AEC
{
    /// <summary>
    /// Provides local configuration settings for Acoustic Echo Cancellation (AEC) within the Inworld framework.
    /// Defines device-specific settings and parameters for local AEC processing.
    /// Used for configuring AEC components that operate locally on the client device.
    /// </summary>
    public class AECLocalConfig : InworldLocalConfig
    {
        /// <summary>
        /// Initializes a new instance of the AECLocalConfig class with default settings.
        /// </summary>
        public AECLocalConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LocalAECFilterConfig_new(), InworldInterop.inworld_LocalAECFilterConfig_delete);
        }

        /// <summary>
        /// Gets or sets the model path for AEC processing.
        /// Note: AEC does not require a model file, so this property is not used.
        /// </summary>
        public override string ModelPath { get; set; } // YAN: AEC Does not have model.

        /// <summary>
        /// Gets or sets the device configuration for AEC processing.
        /// Specifies the audio device settings and capabilities for echo cancellation.
        /// </summary>
        public override InworldDevice Device
        {
            get => new(InworldInterop.inworld_LocalAECFilterConfig_device_get(m_DLLPtr));
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_LocalAECFilterConfig_device_set(m_DLLPtr, value.ToDLL);
            }
        }

        /// <summary>
        /// Gets or sets the default AEC configuration settings.
        /// Provides access to the core AEC parameters and processing options.
        /// </summary>
        public override InworldConfig Config
        {
            get => new AECConfig(InworldInterop.inworld_LocalAECFilterConfig_default_config_get(m_DLLPtr));
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_LocalAECFilterConfig_default_config_set(m_DLLPtr, value.ToDLL);
            }
        }
    }
}