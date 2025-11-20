/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.STT
{
    /// <summary>
    /// Provides an interface for Speech-to-Text (STT) recognition within the Inworld framework.
    /// Handles audio-to-text conversion operations using various speech recognition services.
    /// Used for converting spoken audio input into text transcriptions for conversation processing.
    /// </summary>
    public class STTInterface : InworldInterface
    {
        /// <summary>
        /// Initializes a new instance of the STTInterface class from a native pointer.
        /// </summary>
        /// <param name="ptr">The native pointer to the STT interface object.</param>
        public STTInterface(IntPtr ptr) => m_DLLPtr = MemoryManager.Register(ptr, InworldInterop.inworld_STTInterface_delete);

        /// <summary>
        /// Recognizes speech from audio input and converts it to text.
        /// Processes audio chunks through the configured speech recognition service to generate text transcriptions.
        /// </summary>
        /// <param name="input">The audio chunk containing the speech data to be recognized.</param>
        /// <param name="config">The speech recognition configuration specifying recognition parameters.</param>
        /// <returns>An input stream containing the recognized text, or null if recognition fails.</returns>
        public InworldInputStream<string> RecognizeSpeech(AudioChunk input, InworldConfig config)
        {
            if (!IsValid || config == null || !config.IsValid || !input.IsValid)
                return null;
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_STTInterface_RecognizeSpeech(m_DLLPtr, input.ToDLL, config.ToDLL),
                InworldInterop.inworld_StatusOr_InputStream_string_status,
                InworldInterop.inworld_StatusOr_InputStream_string_ok,
                InworldInterop.inworld_StatusOr_InputStream_string_value,
                InworldInterop.inworld_StatusOr_InputStream_string_delete
            );
            return result != IntPtr.Zero ? new InworldInputStream<string>(result) : null;
        }
    }
}