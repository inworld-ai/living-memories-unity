/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using UnityEngine;

namespace Inworld.Framework
{
    /// <summary>
    /// Implements input stream strategy for string elements.
    /// Provides native C++ interop for input streams that deliver string data.
    /// Used for sequential processing of text content and string-based data within the Inworld framework.
    /// </summary>
    public class StringInputStreamStrategy : IInputStreamStrategy<string>
    {
        /// <summary>
        /// Deletes a native string input stream instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_InputStream_string_delete(ptr);

        /// <summary>
        /// Determines whether the string input stream contains valid data.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream.</param>
        /// <returns>True if the input stream is valid and functional; otherwise, false.</returns>
        public bool IsValid(IntPtr ptr) => InworldInterop.inworld_InputStream_string_is_valid(ptr);

        /// <summary>
        /// Determines whether there are more string items available to read from the input stream.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream.</param>
        /// <returns>True if more string data is available; otherwise, false.</returns>
        public bool HasNext(IntPtr ptr) => InworldInterop.inworld_InputStream_string_HasNext(ptr);

        /// <summary>
        /// Reads the next string item from the input stream and advances the stream position.
        /// Includes error handling to check the operation status and log any errors that occur.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream.</param>
        /// <returns>The next string from the stream, or empty string if an error occurs.</returns>
        public string Read(IntPtr ptr)
        {
            IntPtr optStr = InworldInterop.inworld_InputStream_string_Read(ptr);
            if (!InworldInterop.inworld_StatusOr_string_ok(optStr))
            {
                IntPtr status = InworldInterop.inworld_StatusOr_string_status(optStr);
                if (status != IntPtr.Zero)
                    Debug.LogError(InworldInterop.inworld_Status_ToString(status));
                return "";
            }
            return InworldInterop.inworld_StatusOr_string_value(optStr);
        }
    }
}