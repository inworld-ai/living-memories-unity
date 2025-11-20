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
    /// Provides configuration settings for Acoustic Echo Cancellation (AEC) within the Inworld framework.
    /// Defines parameters and settings for configuring AEC audio processing capabilities.
    /// Used for setting up echo cancellation to improve audio quality in real-time communication.
    /// </summary>
    public class AECConfig : InworldConfig
    {
        /// <summary>
        /// Initializes a new instance of the AECConfig class with default settings.
        /// </summary>
        public AECConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_AcousticEchoCancellationConfig_new(),
                InworldInterop.inworld_AcousticEchoCancellationConfig_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the AECConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the AEC configuration object.</param>
        public AECConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_AcousticEchoCancellationConfig_delete);
        }
    }
}