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
    /// Implements map strategy for string-to-string key-value pairs.
    /// Provides native C++ interop for maps that store string keys and string values.
    /// Used for organizing key-value relationships where both keys and values are strings
    /// within the Inworld framework data structures.
    /// </summary>
    public class StringStringMapStrategy : IMapStrategy<string, string>
    {
        /// <summary>
        /// Creates a new native map instance for string-to-string mappings.
        /// </summary>
        /// <returns>A native pointer to the newly created string-to-string map.</returns>
        public IntPtr CreateNew() => InworldInterop.inworld_map_string_string_new();

        /// <summary>
        /// Deletes a native string-to-string map instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the map to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_map_string_string_delete(ptr);

        /// <summary>
        /// Gets the number of string-to-string mappings in the map.
        /// </summary>
        /// <param name="ptr">The native pointer to the map.</param>
        /// <returns>The number of key-value pairs in the map.</returns>
        public int Size(IntPtr ptr) => InworldInterop.inworld_map_string_string_size(ptr);

        /// <summary>
        /// Determines whether the string-to-string map is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the map.</param>
        /// <returns>True if the map contains no elements; otherwise, false.</returns>
        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_map_string_string_empty(ptr);

        /// <summary>
        /// Removes all string-to-string mappings from the map.
        /// </summary>
        /// <param name="ptr">The native pointer to the map.</param>
        public void Clear(IntPtr ptr) => InworldInterop.inworld_map_string_string_clear(ptr);

        /// <summary>
        /// Determines whether the map contains the specified string key.
        /// </summary>
        /// <param name="ptr">The native pointer to the map.</param>
        /// <param name="key">The string key to locate in the map.</param>
        /// <returns>True if the map contains the specified key; otherwise, false.</returns>
        public bool ContainsKey(IntPtr ptr, string key) => InworldInterop.inworld_map_string_string_has_key(ptr, key);

        /// <summary>
        /// Removes the key-value pair with the specified string key from the map.
        /// </summary>
        /// <param name="ptr">The native pointer to the map.</param>
        /// <param name="key">The string key of the element to remove.</param>
        public void DeleteKey(IntPtr ptr, string key) => InworldInterop.inworld_map_string_string_del(ptr, key);

        /// <summary>
        /// Gets the string value associated with the specified string key.
        /// </summary>
        /// <param name="ptr">The native pointer to the map.</param>
        /// <param name="key">The string key whose associated value to retrieve.</param>
        /// <returns>The string value associated with the specified key.</returns>
        public string GetValue(IntPtr ptr, string key) => InworldInterop.inworld_map_string_string_get(ptr, key);

        /// <summary>
        /// Sets the string value associated with the specified string key.
        /// </summary>
        /// <param name="ptr">The native pointer to the map.</param>
        /// <param name="key">The string key with which to associate the value.</param>
        /// <param name="value">The string value to associate with the key.</param>
        public void SetValue(IntPtr ptr, string key, string value) => InworldInterop.inworld_map_string_string_set(ptr, key, value);
    }
}