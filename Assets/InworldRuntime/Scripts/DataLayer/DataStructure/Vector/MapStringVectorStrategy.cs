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
    /// Implements vector strategy for string-to-string map elements.
    /// Provides native C++ interop for vectors that store map objects with string keys and string values.
    /// Used for managing dynamic arrays of key-value mappings within the Inworld framework data structures.
    /// </summary>
    public class MapStringVectorStrategy : IVectorStrategy<InworldMap<string, string>>
    {
        /// <summary>
        /// Creates a new native vector instance for string-to-string map elements.
        /// </summary>
        /// <returns>A native pointer to the newly created map vector.</returns>
        public IntPtr CreateNew() => InworldInterop.inworld_vector_map_string_string_new();
        
        /// <summary>
        /// Creates a copy of an existing native map vector.
        /// </summary>
        /// <param name="source">The native pointer to the source vector to copy.</param>
        /// <returns>A native pointer to the newly created vector copy.</returns>
        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_map_string_string_copy(source);
        
        /// <summary>
        /// Deletes a native map vector instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_map_string_string_delete(ptr);
        
        /// <summary>
        /// Reserves memory capacity for the specified number of map elements.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="nSize">The number of map elements to reserve capacity for.</param>
        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_map_string_string_reserve(ptr, nSize);
        
        /// <summary>
        /// Removes all map elements from the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_map_string_string_clear(ptr);
        
        /// <summary>
        /// Adds an InworldMap element to the end of the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="value">The InworldMap&lt;string, string&gt; value to add to the vector.</param>
        public void PushBack(IntPtr ptr, InworldMap<string, string> value) => InworldInterop.inworld_vector_map_string_string_push_back(ptr, value.ToDLL);
        
        /// <summary>
        /// Gets the number of map elements in the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The number of elements in the vector.</returns>
        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_map_string_string_size(ptr);
        
        /// <summary>
        /// Gets the capacity of the map vector (total allocated space).
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The capacity of the vector in number of elements.</returns>
        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_map_string_string_capacity(ptr);
        
        /// <summary>
        /// Determines whether the map vector is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>True if the vector contains no elements; otherwise, false.</returns>
        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_map_string_string_empty(ptr);
        
        /// <summary>
        /// Gets the InworldMap element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The InworldMap&lt;string, string&gt; element at the specified index.</returns>
        public InworldMap<string, string> GetItem(IntPtr ptr, int index) => new InworldMap<string, string>(InworldInterop.inworld_vector_map_string_string_get(ptr, index));
        
        /// <summary>
        /// Sets the InworldMap element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to set.</param>
        /// <param name="value">The InworldMap&lt;string, string&gt; value to set at the specified index.</param>
        public void SetItem(IntPtr ptr, int index, InworldMap<string, string> value) => InworldInterop.inworld_vector_map_string_string_set(ptr, index, value.ToDLL);
    }
}