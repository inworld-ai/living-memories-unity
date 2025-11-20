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
    /// Implements hash set strategy for string elements.
    /// Provides native C++ interop for hash sets that store string values.
    /// Used for organizing collections of unique strings within the Inworld framework data structures.
    /// </summary>
    public class StringHashSetStrategy : IHashSetStrategy<string>
    {
        /// <summary>
        /// Creates a new native hash set instance for string elements.
        /// </summary>
        /// <returns>A native pointer to the newly created string hash set.</returns>
        public IntPtr CreateNew() => InworldInterop.inworld_HashSet_String_new();

        /// <summary>
        /// Deletes a native string hash set instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash set to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_HashSet_String_delete(ptr);

        /// <summary>
        /// Removes all string elements from the hash set.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash set.</param>
        public void Clear(IntPtr ptr) => InworldInterop.inworld_HashSet_String_clear(ptr);

        /// <summary>
        /// Determines whether the hash set contains the specified string.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash set.</param>
        /// <param name="value">The string to locate in the hash set.</param>
        /// <returns>True if the hash set contains the specified string; otherwise, false.</returns>
        public bool Contains(IntPtr ptr, string value) =>
            InworldInterop.inworld_HashSet_String___contains__(ptr, value);

        /// <summary>
        /// Adds the specified string to the hash set.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash set.</param>
        /// <param name="value">The string to add to the hash set.</param>
        public void Add(IntPtr ptr, string value) => InworldInterop.inworld_HashSet_String_add(ptr, value);

        /// <summary>
        /// Determines whether the string hash set is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash set.</param>
        /// <returns>True if the hash set contains no elements; otherwise, false.</returns>
        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_HashSet_String_empty(ptr);

        /// <summary>
        /// Gets the number of string elements in the hash set.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash set.</param>
        /// <returns>The number of elements in the hash set.</returns>
        public int Size(IntPtr ptr) => InworldInterop.inworld_HashSet_String_size(ptr);
    }
}