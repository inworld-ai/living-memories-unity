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
    /// Implements input stream strategy for InworldBaseData elements.
    /// Provides native C++ interop for input streams that deliver base data objects.
    /// Used for sequential processing of fundamental Inworld framework data structures.
    /// </summary>
    public class BaseDataInputStreamStrategy : IInputStreamStrategy<InworldBaseData>
    {
        /// <summary>
        /// Deletes a native base data input stream instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_InputStream_BaseData_delete(ptr);

        /// <summary>
        /// Determines whether the base data input stream contains valid data.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream.</param>
        /// <returns>True if the input stream is valid and functional; otherwise, false.</returns>
        public bool IsValid(IntPtr ptr) => InworldInterop.inworld_InputStream_BaseData_is_valid(ptr);

        /// <summary>
        /// Determines whether there are more base data items available to read from the input stream.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream.</param>
        /// <returns>True if more base data is available; otherwise, false.</returns>
        public bool HasNext(IntPtr ptr) => InworldInterop.inworld_InputStream_BaseData_HasNext(ptr);

        /// <summary>
        /// Reads the next InworldBaseData item from the input stream and advances the stream position.
        /// Uses error handling to safely extract the data from the native stream result.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream.</param>
        /// <returns>The next InworldBaseData object from the stream, or null if an error occurs.</returns>
        public InworldBaseData Read(IntPtr ptr)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_InputStream_BaseData_Read(ptr),
                InworldInterop.inworld_StatusOr_BaseData_status,
                InworldInterop.inworld_StatusOr_BaseData_ok,
                InworldInterop.inworld_StatusOr_BaseData_value,
                InworldInterop.inworld_StatusOr_BaseData_delete
            );
            return result == IntPtr.Zero ? null : new InworldBaseData(result);
        }
    }
}