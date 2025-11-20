/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework
{
    /// <summary>
    /// Represents a chunk of speech audio data with enhanced metadata for speech processing.
    /// Extends basic audio functionality with speech-specific features like phoneme timing information.
    /// Used for text-to-speech synthesis, speech analysis, and advanced audio processing operations.
    /// </summary>
    public class SpeechChunk : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldSpeechChunk class with default settings.
        /// Creates a new native speech chunk object and registers it with the memory manager.
        /// </summary>
        public SpeechChunk()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_SpeechChunk_new(),InworldInterop.inworld_SpeechChunk_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the InworldSpeechChunk class from an existing native pointer.
        /// Used for wrapping existing native speech chunk objects created by the C++ library.
        /// </summary>
        /// <param name="rhs">Native pointer to an existing speech chunk instance.</param>
        public SpeechChunk(IntPtr rhs) => m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_SpeechChunk_delete);

        /// <summary>
        /// Gets or sets the audio waveform data as a vector of normalized floating-point samples.
        /// Contains the raw audio data for the speech segment, typically in the range of -1.0 to 1.0.
        /// Returns null if the native pointer is invalid, and setter is ignored for invalid pointers.
        /// </summary>
        /// <value>An InworldVector containing the speech waveform samples, or null if invalid.</value>
        public InworldVector<float> WaveForm 
        {
            get => m_DLLPtr != IntPtr.Zero ? 
                new InworldVector<float>(InworldInterop.inworld_SpeechChunk_waveform_get(m_DLLPtr)) : null;
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_SpeechChunk_waveform_set(m_DLLPtr, value.ToDLL);
            }
        }

        /// <summary>
        /// Gets or sets the sample rate of the speech audio data in samples per second (Hz).
        /// Determines the temporal resolution and frequency range of the speech data.
        /// Returns -1 if the native pointer is invalid, and setter is ignored for invalid pointers.
        /// </summary>
        /// <value>The sample rate in Hz, or -1 if the speech chunk is invalid.</value>
        public int SampleRate
        {
            get => m_DLLPtr != IntPtr.Zero ? InworldInterop.inworld_SpeechChunk_sample_rate_get(m_DLLPtr) : -1;
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_SpeechChunk_sample_rate_set(m_DLLPtr, value);
            }
        }
        
        /// <summary>
        /// Gets or sets the phoneme timestamp data for speech synthesis and analysis.
        /// Contains timing information for individual phonemes within the speech segment,
        /// enabling precise lip-sync and speech animation capabilities.
        /// Returns IntPtr.Zero if the native pointer is invalid.
        /// </summary>
        /// <value>A native pointer to phoneme timestamp data, or IntPtr.Zero if invalid.</value>
        public InworldVector<PhonemeStamp> Phonemes 
        {
            get => m_DLLPtr != IntPtr.Zero ? new InworldVector<PhonemeStamp>(InworldInterop.inworld_SpeechChunk_phoneme_timestamps_get(m_DLLPtr)) : null;
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_SpeechChunk_phoneme_timestamps_set(m_DLLPtr, value.ToDLL);
            }
        }
    }
}