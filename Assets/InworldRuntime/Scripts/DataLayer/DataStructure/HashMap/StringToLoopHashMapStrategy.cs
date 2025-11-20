/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.Graph;

namespace Inworld.Framework
{
    /// <summary>
    /// Implements hash map strategy for mapping string keys to InworldLoop values.
    /// Provides native C++ interop for hash maps that store loop objects indexed by string identifiers.
    /// Used for organizing and accessing processing loops within the Inworld graph system.
    /// </summary>
    public class StringToLoopHashMapStrategy : IHashMapStrategy<string, InworldLoop>
    {
        /// <summary>
        /// Creates a new native hash map instance for string-to-loop mappings.
        /// </summary>
        /// <returns>A native pointer to the newly created string-to-loop hash map.</returns>
        public IntPtr CreateNew() => InworldInterop.inworld_HashMap_StringToLoop_new();

        /// <summary>
        /// Deletes a native string-to-loop hash map instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_HashMap_StringToLoop_delete(ptr);

        /// <summary>
        /// Gets the number of string-to-loop mappings in the hash map.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <returns>The number of key-value pairs in the hash map.</returns>
        public int Size(IntPtr ptr) => InworldInterop.inworld_HashMap_StringToLoop_size(ptr);

        /// <summary>
        /// Determines whether the string-to-loop hash map is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <returns>True if the hash map contains no elements; otherwise, false.</returns>
        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_HashMap_StringToLoop_empty(ptr);

        /// <summary>
        /// Removes all string-to-loop mappings from the hash map.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        public void Clear(IntPtr ptr) => InworldInterop.inworld_HashMap_StringToLoop_clear(ptr);

        /// <summary>
        /// Determines whether the hash map contains the specified string key.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <param name="key">The string key to locate in the hash map.</param>
        /// <returns>True if the hash map contains the specified key; otherwise, false.</returns>
        public bool ContainsKey(IntPtr ptr, string key) =>
            InworldInterop.inworld_HashMap_StringToLoop___contains__(ptr, key);

        /// <summary>
        /// Gets the InworldLoop value associated with the specified string key.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <param name="key">The string key whose associated loop to retrieve.</param>
        /// <returns>The InworldLoop object associated with the specified key.</returns>
        public InworldLoop GetValue(IntPtr ptr, string key) =>
            new InworldLoop(InworldInterop.inworld_HashMap_StringToLoop___getitem__(ptr, key));

        /// <summary>
        /// Sets the InworldLoop value associated with the specified string key.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <param name="key">The string key with which to associate the loop.</param>
        /// <param name="value">The InworldLoop object to associate with the key.</param>
        public void SetValue(IntPtr ptr, string key, InworldLoop value) =>
            InworldInterop.inworld_HashMap_StringToLoop___setitem__(ptr, key, value.ToDLL);
    }
}