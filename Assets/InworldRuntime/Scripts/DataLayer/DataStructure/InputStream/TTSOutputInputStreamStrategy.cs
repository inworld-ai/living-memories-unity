/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.TTS;

namespace Inworld.Framework
{
    /// <summary>
    /// Implements input stream strategy for TTSOutput elements.
    /// Provides native C++ interop for input streams that deliver text-to-speech output data.
    /// Used for sequential processing of synthesized speech results from TTS operations.
    /// </summary>
    public class TTSOutputInputStreamStrategy : IInputStreamStrategy<TTSOutput>
    {
        /// <summary>
        /// Deletes a native TTS output input stream instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_InputStream_TTSOutput_delete(ptr);

        /// <summary>
        /// Determines whether the TTS output input stream contains valid data.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream.</param>
        /// <returns>True if the input stream is valid and functional; otherwise, false.</returns>
        public bool IsValid(IntPtr ptr) => InworldInterop.inworld_InputStream_TTSOutput_is_valid(ptr);

        /// <summary>
        /// Determines whether there are more TTS output items available to read from the input stream.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream.</param>
        /// <returns>True if more TTS output data is available; otherwise, false.</returns>
        public bool HasNext(IntPtr ptr) => InworldInterop.inworld_InputStream_TTSOutput_HasNext(ptr);

        /// <summary>
        /// Reads the next TTSOutput item from the input stream and advances the stream position.
        /// Uses error handling to safely extract the TTS data from the native stream result.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream.</param>
        /// <returns>The next TTSOutput object from the stream, or null if an error occurs.</returns>
        public TTSOutput Read(IntPtr ptr) 
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_InputStream_TTSOutput_Read(ptr),
                InworldInterop.inworld_StatusOr_TTSOutput_status,
                InworldInterop.inworld_StatusOr_TTSOutput_ok,
                InworldInterop.inworld_StatusOr_TTSOutput_value,
                InworldInterop.inworld_StatusOr_TTSOutput_delete
            );
            return result == IntPtr.Zero ? null : new TTSOutput(result);
        }
    }
}