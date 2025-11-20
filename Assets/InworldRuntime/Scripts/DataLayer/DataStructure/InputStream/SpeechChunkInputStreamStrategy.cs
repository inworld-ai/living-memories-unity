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
    /// Implements input stream strategy for InworldSpeechChunk elements.
    /// Provides native C++ interop for input streams that deliver speech audio data with enhanced metadata.
    /// Used for sequential processing of speech synthesis results and audio analysis data.
    /// </summary>
    public class SpeechChunkInputStreamStrategy : IInputStreamStrategy<SpeechChunk>
    {
        /// <summary>
        /// Deletes a native speech chunk input stream instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_InputStream_SpeechChunk_delete(ptr);

        /// <summary>
        /// Determines whether the speech chunk input stream contains valid data.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream.</param>
        /// <returns>True if the input stream is valid and functional; otherwise, false.</returns>
        public bool IsValid(IntPtr ptr) => InworldInterop.inworld_InputStream_SpeechChunk_is_valid(ptr);

        /// <summary>
        /// Determines whether there are more speech chunk items available to read from the input stream.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream.</param>
        /// <returns>True if more speech chunk data is available; otherwise, false.</returns>
        public bool HasNext(IntPtr ptr) => InworldInterop.inworld_InputStream_SpeechChunk_HasNext(ptr);

        /// <summary>
        /// Reads the next InworldSpeechChunk item from the input stream and advances the stream position.
        /// Uses error handling to safely extract the speech data from the native stream result.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream.</param>
        /// <returns>The next InworldSpeechChunk object from the stream, or null if an error occurs.</returns>
        public SpeechChunk Read(IntPtr ptr) 
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_InputStream_SpeechChunk_Read(ptr),
                InworldInterop.inworld_StatusOr_SpeechChunk_status,
                InworldInterop.inworld_StatusOr_SpeechChunk_ok,
                InworldInterop.inworld_StatusOr_SpeechChunk_value,
                InworldInterop.inworld_StatusOr_SpeechChunk_delete
            );
            return result == IntPtr.Zero ? null : new SpeechChunk(result);
        }
    }
}