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
    /// Represents the output of a Text-to-Speech synthesis operation within the Inworld framework.
    /// Contains synthesized audio data along with phoneme information and timing data.
    /// Used for encapsulating the complete results of TTS processing including audio, text, and metadata.
    /// </summary>
    public class TTSOutput : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the TTSOutput class with specified synthesis data.
        /// </summary>
        /// <param name="text">The original text that was synthesized.</param>
        /// <param name="audio">The synthesized audio data.</param>
        public TTSOutput(string text, InworldAudio audio)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_TTSOutput_new(text, audio.ToDLL), 
                InworldInterop.inworld_TTSOutput_delete);
        }

        /// <summary>
        /// Initializes a new instance of the TTSOutput class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the TTS output object.</param>
        public TTSOutput(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_TTSOutput_delete);
        }

        /// <summary>
        /// Gets or sets the original text that was used for synthesis.
        /// Contains the input text that was converted to speech audio.
        /// </summary>
        public string Text
        {
            get => InworldInterop.inworld_TTSOutput_text_get(m_DLLPtr);
            set => InworldInterop.inworld_TTSOutput_text_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets the synthesized audio data.
        /// Contains the generated speech audio in the form of audio samples and metadata.
        /// </summary>
        public InworldAudio Audio => new InworldAudio(InworldInterop.inworld_TTSOutput_safe_audio(m_DLLPtr));
        
        // TODO(Yan): Add Components.
    }
}