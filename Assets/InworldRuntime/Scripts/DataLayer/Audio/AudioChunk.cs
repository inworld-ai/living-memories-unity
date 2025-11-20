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
    /// Represents a chunk of audio data containing raw audio samples and metadata.
    /// This class wraps native C++ audio functionality and provides managed access to audio data
    /// used for processing, transmission, and playback within the Inworld framework.
    /// </summary>
    public class AudioChunk : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldAudioChunk class with default settings.
        /// Creates a new native audio chunk object and registers it with the memory manager.
        /// </summary>
        public AudioChunk()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_AudioChunk_new(),
                InworldInterop.inworld_AudioChunk_delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldAudioChunk class from an existing native pointer.
        /// Used for wrapping existing native audio chunk objects created by the C++ library.
        /// </summary>
        /// <param name="rhs">Native pointer to an existing audio chunk instance.</param>
        public AudioChunk(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_AudioChunk_delete);
        }
        
        /// <summary>
        /// Gets or sets the sample rate of the audio data in samples per second (Hz).
        /// Common values include 16000 Hz for speech processing and 44100 Hz for high-quality audio.
        /// This determines the temporal resolution and frequency range of the audio data.
        /// </summary>
        /// <value>The sample rate in Hz as an integer value.</value>
        public int SampleRate
        {
            set => InworldInterop.inworld_AudioChunk_sample_rate_set(m_DLLPtr, value);
            get => InworldInterop.inworld_AudioChunk_sample_rate_get(m_DLLPtr);
        }
        
        /// <summary>
        /// Gets or sets the raw audio sample data as a vector of floating-point values.
        /// Each float represents a normalized audio sample typically in the range of -1.0 to 1.0.
        /// The data can be accessed and modified for audio processing operations.
        /// </summary>
        /// <value>An InworldVector containing the audio sample data as float values.</value>
        public InworldVector<float> Data
        {
            set => InworldInterop.inworld_AudioChunk_data_set(m_DLLPtr, value.ToDLL);
            get => new InworldVector<float>(InworldInterop.inworld_AudioChunk_data_get(m_DLLPtr));
        }
    }
}