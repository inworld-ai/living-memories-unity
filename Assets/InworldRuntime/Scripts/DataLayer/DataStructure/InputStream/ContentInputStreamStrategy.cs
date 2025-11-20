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
    /// Implements input stream strategy for InworldContent elements.
    /// Provides native C++ interop for input streams that deliver content objects with associated tool calls.
    /// Used for sequential processing of content data within AI interaction workflows.
    /// </summary>
    public class ContentInputStreamStrategy : IInputStreamStrategy<InworldContent>
    {
        /// <summary>
        /// Deletes a native content input stream instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_InputStream_Content_delete(ptr);

        /// <summary>
        /// Determines whether the content input stream contains valid data.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream.</param>
        /// <returns>True if the input stream is valid and functional; otherwise, false.</returns>
        public bool IsValid(IntPtr ptr) => InworldInterop.inworld_InputStream_Content_is_valid(ptr);

        /// <summary>
        /// Determines whether there are more content items available to read from the input stream.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream.</param>
        /// <returns>True if more content data is available; otherwise, false.</returns>
        public bool HasNext(IntPtr ptr) => InworldInterop.inworld_InputStream_Content_HasNext(ptr);

        /// <summary>
        /// Reads the next InworldContent item from the input stream and advances the stream position.
        /// Uses error handling to safely extract the content data from the native stream result.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream.</param>
        /// <returns>The next InworldContent object from the stream, or null if an error occurs.</returns>
        public InworldContent Read(IntPtr ptr)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_InputStream_Content_Read(ptr),
                InworldInterop.inworld_StatusOr_Content_status,
                InworldInterop.inworld_StatusOr_Content_ok,
                InworldInterop.inworld_StatusOr_Content_value,
                InworldInterop.inworld_StatusOr_Content_delete
            );
            return result == IntPtr.Zero ? null : new InworldContent(result);
        }
    }
}