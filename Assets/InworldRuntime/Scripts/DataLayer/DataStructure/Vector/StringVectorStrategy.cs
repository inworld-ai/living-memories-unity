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
    /// Implements vector strategy for string elements.
    /// Provides native C++ interop for vectors that store string values.
    /// Used for managing dynamic arrays of text content within the Inworld framework data structures.
    /// </summary>
    public class StringVectorStrategy : IVectorStrategy<string>
    {
        /// <summary>
        /// Creates a new native vector instance for string elements.
        /// </summary>
        /// <returns>A native pointer to the newly created string vector.</returns>
        public IntPtr CreateNew() => InworldInterop.inworld_vector_string_new();
        
        /// <summary>
        /// Creates a copy of an existing native string vector.
        /// </summary>
        /// <param name="source">The native pointer to the source vector to copy.</param>
        /// <returns>A native pointer to the newly created vector copy.</returns>
        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_string_copy(source);
        
        /// <summary>
        /// Deletes a native string vector instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_string_delete(ptr);
        
        /// <summary>
        /// Reserves memory capacity for the specified number of string elements.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="nSize">The number of string elements to reserve capacity for.</param>
        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_string_reserve(ptr, nSize);
        
        /// <summary>
        /// Removes all string elements from the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_string_clear(ptr);
        
        /// <summary>
        /// Adds a string element to the end of the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="value">The string value to add to the vector.</param>
        public void PushBack(IntPtr ptr, string value) => InworldInterop.inworld_vector_string_push_back(ptr, value);
        
        /// <summary>
        /// Gets the number of string elements in the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The number of elements in the vector.</returns>
        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_string_size(ptr);
        
        /// <summary>
        /// Gets the capacity of the string vector (total allocated space).
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The capacity of the vector in number of elements.</returns>
        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_string_capacity(ptr);
        
        /// <summary>
        /// Determines whether the string vector is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>True if the vector contains no elements; otherwise, false.</returns>
        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_string_empty(ptr);
        
        /// <summary>
        /// Gets the string element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The string element at the specified index.</returns>
        public string GetItem(IntPtr ptr, int index) => InworldInterop.inworld_vector_string_get(ptr, index);

        /// <summary>
        /// Sets the string element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to set.</param>
        /// <param name="value">The string value to set at the specified index.</param>
        public void SetItem(IntPtr ptr, int index, string value) =>
            InworldInterop.inworld_vector_string_set(ptr, index, value);
    }
}