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
    /// Provides local configuration settings for text embedding processing within the Inworld framework.
    /// Defines device-specific settings, model paths, and parameters for local text embedding operations.
    /// Used for configuring text embedder components that operate locally on the client device.
    /// </summary>
    public class TextEmbedderLocalConfig : InworldLocalConfig
    {
        /// <summary>
        /// Initializes a new instance of the TextEmbedderLocalConfig class with default settings.
        /// </summary>
        public TextEmbedderLocalConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LocalTextEmbedderConfig_new(),
                InworldInterop.inworld_LocalTextEmbedderConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the TextEmbedderLocalConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the local text embedder config object.</param>
        public TextEmbedderLocalConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_LocalTextEmbedderConfig_delete);
        }

        /// <summary>
        /// Gets or sets the file path to the local text embedding model.
        /// Specifies the location of the embedding model file used for local text-to-vector conversion.
        /// </summary>
        public override string ModelPath
        {
            get => InworldInterop.inworld_LocalTextEmbedderConfig_model_path_get(m_DLLPtr); 
            set => InworldInterop.inworld_LocalTextEmbedderConfig_model_path_set(m_DLLPtr, value);
        }
        
        /// <summary>
        /// Gets or sets the device configuration for local text embedding processing.
        /// Specifies the hardware device settings and capabilities for text embedding operations.
        /// </summary>
        public override InworldDevice Device
        {
            get => new InworldDevice(InworldInterop.inworld_LocalTextEmbedderConfig_device_get(m_DLLPtr));
            set => InworldInterop.inworld_LocalTextEmbedderConfig_device_set(m_DLLPtr, value.ToDLL);
        }
        
        /// <summary>
        /// Gets or sets additional configuration settings for the local text embedder.
        /// This property is not currently used in the implementation.
        /// </summary>
        public override InworldConfig Config { get; set; } // Not used.
    }
}