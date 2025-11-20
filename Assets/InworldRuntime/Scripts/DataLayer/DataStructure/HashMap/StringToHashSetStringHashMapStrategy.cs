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
    /// Implements hash map strategy for mapping string keys to InworldHashSet&lt;string&gt; values.
    /// Provides native C++ interop for hash maps that store string hash sets indexed by string identifiers.
    /// Used for organizing collections of related strings within the Inworld framework data structures.
    /// </summary>
    public class StringToHashSetStringHashMapStrategy : IHashMapStrategy<string, InworldHashSet<string>>
    {
        /// <summary>
        /// Creates a new native hash map instance for string-to-string-hashset mappings.
        /// </summary>
        /// <returns>A native pointer to the newly created string-to-hashset hash map.</returns>
        public IntPtr CreateNew() => InworldInterop.inworld_HashMap_StringToHashSetString_new();

        /// <summary>
        /// Deletes a native string-to-hashset hash map instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_HashMap_StringToHashSetString_delete(ptr);

        /// <summary>
        /// Gets the number of string-to-hashset mappings in the hash map.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <returns>The number of key-value pairs in the hash map.</returns>
        public int Size(IntPtr ptr) => InworldInterop.inworld_HashMap_StringToHashSetString_size(ptr);

        /// <summary>
        /// Determines whether the string-to-hashset hash map is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <returns>True if the hash map contains no elements; otherwise, false.</returns>
        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_HashMap_StringToHashSetString_empty(ptr);

        /// <summary>
        /// Removes all string-to-hashset mappings from the hash map.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        public void Clear(IntPtr ptr) => InworldInterop.inworld_HashMap_StringToHashSetString_clear(ptr);

        /// <summary>
        /// Determines whether the hash map contains the specified string key.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <param name="key">The string key to locate in the hash map.</param>
        /// <returns>True if the hash map contains the specified key; otherwise, false.</returns>
        public bool ContainsKey(IntPtr ptr, string key) =>
            InworldInterop.inworld_HashMap_StringToHashSetString___contains__(ptr, key);

        /// <summary>
        /// Gets the InworldHashSet&lt;string&gt; value associated with the specified string key.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <param name="key">The string key whose associated hash set to retrieve.</param>
        /// <returns>The InworldHashSet&lt;string&gt; object associated with the specified key.</returns>
        public InworldHashSet<string> GetValue(IntPtr ptr, string key) =>
            new InworldHashSet<string>(InworldInterop.inworld_HashMap_StringToHashSetString___getitem__(ptr, key));

        /// <summary>
        /// Sets the InworldHashSet&lt;string&gt; value associated with the specified string key.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <param name="key">The string key with which to associate the hash set.</param>
        /// <param name="value">The InworldHashSet&lt;string&gt; object to associate with the key.</param>
        public void SetValue(IntPtr ptr, string key, InworldHashSet<string> value) =>
            InworldInterop.inworld_HashMap_StringToHashSetString___setitem__(ptr, key, value.ToDLL);
    }
}