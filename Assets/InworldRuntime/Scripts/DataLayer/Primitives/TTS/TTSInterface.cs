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
    /// Provides an interface for Text-to-Speech (TTS) synthesis within the Inworld framework.
    /// Handles text-to-speech conversion operations using various speech synthesis services.
    /// Used for converting text input into speech audio output for conversation systems.
    /// </summary>
    public class TTSInterface : InworldInterface
    {
        /// <summary>
        /// Initializes a new instance of the TTSInterface class from a native pointer.
        /// </summary>
        /// <param name="ptr">The native pointer to the TTS interface object.</param>
        public TTSInterface(IntPtr ptr) => m_DLLPtr = MemoryManager.Register(ptr, InworldInterop.inworld_TTSInterface_delete);

        /// <summary>
        /// Synthesizes speech from text input using the specified configuration.
        /// Converts the input text into speech chunks through the configured text-to-speech service.
        /// </summary>
        /// <param name="input">The text string to convert into speech.</param>
        /// <param name="config">The speech synthesis configuration specifying voice and synthesis parameters.</param>
        /// <returns>An input stream containing the synthesized speech chunks, or null if synthesis fails.</returns>
        public InworldInputStream<SpeechChunk> SynthesizeSpeech(string input, InworldConfig config)
        {
            if (!IsValid || config == null || !config.IsValid || string.IsNullOrEmpty(input))
                return null;
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_TTSInterface_SynthesizeSpeech_rcinworld_Voice_rcstd_string
                    (m_DLLPtr, config.ToDLL, input),
                InworldInterop.inworld_StatusOr_InputStream_SpeechChunk_status,
                InworldInterop.inworld_StatusOr_InputStream_SpeechChunk_ok,
                InworldInterop.inworld_StatusOr_InputStream_SpeechChunk_value,
                InworldInterop.inworld_StatusOr_InputStream_SpeechChunk_delete
            );
            return result != IntPtr.Zero ? new InworldInputStream<SpeechChunk>(result) : null;
        }
    }
}