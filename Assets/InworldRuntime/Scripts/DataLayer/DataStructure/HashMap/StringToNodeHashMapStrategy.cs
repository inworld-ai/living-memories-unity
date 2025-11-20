/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.Node;

namespace Inworld.Framework
{
    /// <summary>
    /// Implements hash map strategy for mapping string keys to InworldNode values.
    /// Provides native C++ interop for hash maps that store node objects indexed by string identifiers.
    /// Used for organizing and accessing processing nodes within the Inworld graph system.
    /// </summary>
    public class StringToNodeHashMapStrategy : IHashMapStrategy<string, InworldNode>
    {
        /// <summary>
        /// Creates a new native hash map instance for string-to-node mappings.
        /// </summary>
        /// <returns>A native pointer to the newly created string-to-node hash map.</returns>
        public IntPtr CreateNew() => InworldInterop.inworld_HashMap_StringToNode_new();

        /// <summary>
        /// Deletes a native string-to-node hash map instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_HashMap_StringToNode_delete(ptr);

        /// <summary>
        /// Gets the number of string-to-node mappings in the hash map.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <returns>The number of key-value pairs in the hash map.</returns>
        public int Size(IntPtr ptr) => InworldInterop.inworld_HashMap_StringToNode_size(ptr);

        /// <summary>
        /// Determines whether the string-to-node hash map is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <returns>True if the hash map contains no elements; otherwise, false.</returns>
        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_HashMap_StringToNode_empty(ptr);

        /// <summary>
        /// Removes all string-to-node mappings from the hash map.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        public void Clear(IntPtr ptr) => InworldInterop.inworld_HashMap_StringToNode_clear(ptr);

        /// <summary>
        /// Determines whether the hash map contains the specified string key.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <param name="key">The string key to locate in the hash map.</param>
        /// <returns>True if the hash map contains the specified key; otherwise, false.</returns>
        public bool ContainsKey(IntPtr ptr, string key) 
            => InworldInterop.inworld_HashMap_StringToNode___contains__(ptr, key);

        /// <summary>
        /// Gets the InworldNode value associated with the specified string key.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <param name="key">The string key whose associated node to retrieve.</param>
        /// <returns>The InworldNode object associated with the specified key.</returns>
        public InworldNode GetValue(IntPtr ptr, string key) 
            => new InworldNode(InworldInterop.inworld_HashMap_StringToNode___getitem__(ptr, key));

        /// <summary>
        /// Sets the InworldNode value associated with the specified string key.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <param name="key">The string key with which to associate the node.</param>
        /// <param name="value">The InworldNode object to associate with the key.</param>
        public void SetValue(IntPtr ptr, string key, InworldNode value) 
            => InworldInterop.inworld_HashMap_StringToNode___setitem__(ptr, key, value.ToDLL);
    }
}