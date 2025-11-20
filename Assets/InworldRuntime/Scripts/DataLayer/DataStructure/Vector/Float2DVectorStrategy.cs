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
    /// Implements vector strategy for two-dimensional float array elements.
    /// Provides native C++ interop for vectors that store float vector objects, creating a 2D array structure.
    /// Used for managing dynamic arrays of numerical matrices within the Inworld framework calculations.
    /// </summary>
    public class Float2DVectorStrategy : IVectorStrategy<InworldVector<float>>
    {
        /// <summary>
        /// Creates a new native vector instance for 2D float array elements.
        /// </summary>
        /// <returns>A native pointer to the newly created 2D float vector.</returns>
        public IntPtr CreateNew() => InworldInterop.inworld_vector_vector_float_new();
        
        /// <summary>
        /// Creates a copy of an existing native 2D float vector.
        /// </summary>
        /// <param name="source">The native pointer to the source vector to copy.</param>
        /// <returns>A native pointer to the newly created vector copy.</returns>
        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_vector_float_copy(source);
        
        /// <summary>
        /// Deletes a native 2D float vector instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_vector_float_delete(ptr);
        
        /// <summary>
        /// Reserves memory capacity for the specified number of float vector elements.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="nSize">The number of float vector elements to reserve capacity for.</param>
        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_vector_float_reserve(ptr, nSize);
        
        /// <summary>
        /// Removes all float vector elements from the 2D vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_vector_float_clear(ptr);
        
        /// <summary>
        /// Adds a float vector element to the end of the 2D vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="value">The InworldVector&lt;float&gt; value to add to the vector.</param>
        public void PushBack(IntPtr ptr, InworldVector<float> value) => InworldInterop.inworld_vector_vector_float_push_back(ptr, value.ToDLL);
        
        /// <summary>
        /// Gets the number of float vector elements in the 2D vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The number of elements in the vector.</returns>
        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_vector_float_size(ptr);
        
        /// <summary>
        /// Gets the capacity of the 2D float vector (total allocated space).
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The capacity of the vector in number of elements.</returns>
        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_vector_float_capacity(ptr);
        
        /// <summary>
        /// Determines whether the 2D float vector is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>True if the vector contains no elements; otherwise, false.</returns>
        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_vector_float_empty(ptr);
        
        /// <summary>
        /// Gets the float vector element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The InworldVector&lt;float&gt; element at the specified index.</returns>
        public InworldVector<float> GetItem(IntPtr ptr, int index) => new InworldVector<float>(InworldInterop.inworld_vector_vector_float_get(ptr, index));
        
        /// <summary>
        /// Sets the float vector element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to set.</param>
        /// <param name="value">The InworldVector&lt;float&gt; value to set at the specified index.</param>
        public void SetItem(IntPtr ptr, int index, InworldVector<float> value) => InworldInterop.inworld_vector_vector_float_set(ptr, index, value.ToDLL);
    }
}