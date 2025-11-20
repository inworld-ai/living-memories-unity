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
    /// Implements vector strategy for float elements.
    /// Provides native C++ interop for vectors that store floating-point numeric values.
    /// Used for managing dynamic arrays of numerical data within the Inworld framework calculations.
    /// </summary>
    public class FloatVectorStrategy : IVectorStrategy<float>
    {
        /// <summary>
        /// Creates a new native vector instance for float elements.
        /// </summary>
        /// <returns>A native pointer to the newly created float vector.</returns>
        public IntPtr CreateNew() => InworldInterop.inworld_vector_float_new();
        
        /// <summary>
        /// Creates a copy of an existing native float vector.
        /// </summary>
        /// <param name="source">The native pointer to the source vector to copy.</param>
        /// <returns>A native pointer to the newly created vector copy.</returns>
        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_float_copy(source);
        
        /// <summary>
        /// Deletes a native float vector instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_float_delete(ptr);
        
        /// <summary>
        /// Reserves memory capacity for the specified number of float elements.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="nSize">The number of float elements to reserve capacity for.</param>
        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_float_reserve(ptr, nSize);
        
        /// <summary>
        /// Removes all float elements from the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_float_clear(ptr);
        
        /// <summary>
        /// Adds a float element to the end of the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="value">The float value to add to the vector.</param>
        public void PushBack(IntPtr ptr, float value) => InworldInterop.inworld_vector_float_push_back(ptr, value);
        
        /// <summary>
        /// Gets the number of float elements in the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The number of elements in the vector.</returns>
        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_float_size(ptr);
        
        /// <summary>
        /// Gets the capacity of the float vector (total allocated space).
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The capacity of the vector in number of elements.</returns>
        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_float_capacity(ptr);
        
        /// <summary>
        /// Determines whether the float vector is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>True if the vector contains no elements; otherwise, false.</returns>
        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_float_empty(ptr);
        
        /// <summary>
        /// Gets the float element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The float element at the specified index.</returns>
        public float GetItem(IntPtr ptr, int index) => InworldInterop.inworld_vector_float_get(ptr, index);
        
        /// <summary>
        /// Sets the float element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to set.</param>
        /// <param name="value">The float value to set at the specified index.</param>
        public void SetItem(IntPtr ptr, int index, float value) => InworldInterop.inworld_vector_float_set(ptr, index, value);
    }
}