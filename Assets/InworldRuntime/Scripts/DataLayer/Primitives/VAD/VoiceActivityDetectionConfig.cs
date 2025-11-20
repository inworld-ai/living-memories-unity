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
    /// Provides configuration settings for voice activity detection operations within the Inworld framework.
    /// Defines parameters and thresholds for detecting speech activity in audio streams.
    /// Used for configuring the sensitivity and behavior of voice activity detection algorithms.
    /// </summary>
    public class VoiceActivityDetectionConfig : InworldConfig
    {
        /// <summary>
        /// Initializes a new instance of the VoiceActivityDetectionConfig class with default settings.
        /// </summary>
        public VoiceActivityDetectionConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_VoiceActivityDetectionConfig_new(), 
                InworldInterop.inworld_VoiceActivityDetectionConfig_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the VoiceActivityDetectionConfig class from a native pointer.
        /// </summary>
        /// <param name="dllPtr">The native pointer to the voice activity detection config object.</param>
        public VoiceActivityDetectionConfig(IntPtr dllPtr)
        {
            m_DLLPtr = MemoryManager.Register(dllPtr, InworldInterop.inworld_VoiceActivityDetectionConfig_delete);
        }

        /// <summary>
        /// Gets or sets the speech detection threshold for voice activity detection.
        /// Determines the minimum confidence level required to classify audio as containing speech.
        /// Values typically range from 0.0 to 1.0, where higher values require stronger speech signals for detection.
        /// </summary>
        public float Threshold
        {
            get => InworldInterop.inworld_VoiceActivityDetectionConfig_speech_threshold_get(m_DLLPtr); 
            set => InworldInterop.inworld_VoiceActivityDetectionConfig_speech_threshold_set(m_DLLPtr, value);
        }
    }
}