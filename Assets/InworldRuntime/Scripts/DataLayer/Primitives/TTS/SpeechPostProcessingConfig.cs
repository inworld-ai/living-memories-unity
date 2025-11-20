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
    /// Provides configuration settings for speech synthesis post-processing operations within the Inworld framework.
    /// Defines parameters for audio processing that occurs after speech generation.
    /// Used for configuring output audio format, quality, and processing options for synthesized speech.
    /// </summary>
    public class SpeechPostProcessingConfig : InworldConfig
    {
        /// <summary>
        /// Initializes a new instance of the SpeechPostProcessingConfig class with default settings.
        /// </summary>
        public SpeechPostProcessingConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_SpeechSynthesisPostprocessingConfig_new(),
                InworldInterop.inworld_SpeechSynthesisPostprocessingConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the SpeechPostProcessingConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the speech post-processing config object.</param>
        public SpeechPostProcessingConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs,
                InworldInterop.inworld_SpeechSynthesisPostprocessingConfig_delete);
        }

        /// <summary>
        /// Gets or sets the audio sample rate for the synthesized speech output.
        /// Determines the audio quality and file size of the generated speech audio.
        /// Common values include 16000, 22050, 44100, and 48000 Hz.
        /// </summary>
        public int SampleRate
        {
            set => InworldInterop.inworld_SpeechSynthesisPostprocessingConfig_sample_rate_set(m_DLLPtr, value);
            get => InworldInterop.inworld_SpeechSynthesisPostprocessingConfig_sample_rate_get(m_DLLPtr);
        }
    }
}