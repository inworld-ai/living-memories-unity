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
    /// Provides local configuration settings for Large Language Model processing within the Inworld framework.
    /// Defines device-specific settings, model paths, and parameters for local LLM operations.
    /// Used for configuring LLM components that operate locally on the client device.
    /// </summary>
    public class LLMLocalConfig : InworldLocalConfig
    {
        /// <summary>
        /// Initializes a new instance of the LLMLocalConfig class with default settings.
        /// </summary>
        public LLMLocalConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LocalLLMConfig_new(), InworldInterop.inworld_LocalLLMConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the LLMLocalConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the local LLM config object.</param>
        public LLMLocalConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_LocalLLMConfig_delete);
        }

        /// <summary>
        /// Gets or sets the file path to the local LLM model.
        /// Specifies the location of the model file used for local text generation.
        /// </summary>
        public override string ModelPath
        {
            get => m_DLLPtr == IntPtr.Zero ? "" : InworldInterop.inworld_LocalLLMConfig_model_path_get(m_DLLPtr);
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_LocalLLMConfig_model_path_set(m_DLLPtr, value);
            }
        }

        /// <summary>
        /// Gets or sets the device configuration for local LLM processing.
        /// Specifies the hardware device settings and capabilities for text generation.
        /// </summary>
        public override InworldDevice Device
        {
            get => new(InworldInterop.inworld_LocalLLMConfig_device_get(m_DLLPtr));
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_LocalLLMConfig_device_set(m_DLLPtr, value.ToDLL);
            }
        }

        /// <summary>
        /// Gets or sets the default text generation configuration for this local LLM.
        /// Defines default parameters like temperature, max tokens, and penalties for local text generation.
        /// </summary>
        public override InworldConfig Config
        {
            get => new TextGenerationConfig(InworldInterop.inworld_LocalLLMConfig_default_config_get(m_DLLPtr));
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_LocalLLMConfig_default_config_set(m_DLLPtr, value.ToDLL);
            }
        }
    }
}