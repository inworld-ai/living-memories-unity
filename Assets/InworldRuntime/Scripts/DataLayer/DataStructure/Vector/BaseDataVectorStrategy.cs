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
    /// Implements vector strategy for InworldBaseData elements.
    /// Provides native C++ interop for vectors that store base data objects.
    /// Used for managing dynamic arrays of fundamental framework data structures.
    /// </summary>
    public class BaseDataVectorStrategy : IVectorStrategy<InworldBaseData>
    {
        /// <summary>
        /// Creates a new native vector instance for InworldBaseData elements.
        /// </summary>
        /// <returns>A native pointer to the newly created base data vector.</returns>
        public IntPtr CreateNew() => InworldInterop.inworld_vector_BaseData_new();
        
        /// <summary>
        /// Creates a copy of an existing native base data vector.
        /// </summary>
        /// <param name="source">The native pointer to the source vector to copy.</param>
        /// <returns>A native pointer to the newly created vector copy.</returns>
        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_BaseData_copy(source);
        
        /// <summary>
        /// Deletes a native base data vector instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_BaseData_delete(ptr);
        
        /// <summary>
        /// Reserves memory capacity for the specified number of base data elements.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="nSize">The number of base data elements to reserve capacity for.</param>
        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_BaseData_reserve(ptr, nSize);
        
        /// <summary>
        /// Removes all base data elements from the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_BaseData_clear(ptr);
        
        /// <summary>
        /// Adds an InworldBaseData element to the end of the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="value">The InworldBaseData value to add to the vector.</param>
        public void PushBack(IntPtr ptr, InworldBaseData value) => InworldInterop.inworld_vector_BaseData_push_back(ptr, value.ToDLL);
        
        /// <summary>
        /// Gets the number of base data elements in the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The number of elements in the vector.</returns>
        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_BaseData_size(ptr);
        
        /// <summary>
        /// Gets the capacity of the base data vector (total allocated space).
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The capacity of the vector in number of elements.</returns>
        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_BaseData_capacity(ptr);
        
        /// <summary>
        /// Determines whether the base data vector is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>True if the vector contains no elements; otherwise, false.</returns>
        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_BaseData_empty(ptr);
        
        /// <summary>
        /// Gets the InworldBaseData element at the specified index.
        /// Uses safe retrieval to prevent access violations.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The InworldBaseData element at the specified index.</returns>
        public InworldBaseData GetItem(IntPtr ptr, int index) => new InworldBaseData(InworldInterop.inworld_vector_BaseData_safe_get(ptr, index));
        
        /// <summary>
        /// Sets the InworldBaseData element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to set.</param>
        /// <param name="value">The InworldBaseData value to set at the specified index.</param>
        public void SetItem(IntPtr ptr, int index, InworldBaseData value) => InworldInterop.inworld_vector_BaseData_set(ptr, index, value.ToDLL);
    }
}