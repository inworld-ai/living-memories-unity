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
    /// Implements vector strategy for InworldEdge elements.
    /// Provides native C++ interop for vectors that store graph edge objects.
    /// Used for managing dynamic arrays of graph connections within the Inworld framework graph system.
    /// </summary>
    public class EdgeVectorStrategy : IVectorStrategy<InworldEdge>
    {
        /// <summary>
        /// Creates a new native vector instance for InworldEdge elements.
        /// </summary>
        /// <returns>A native pointer to the newly created edge vector.</returns>
        public IntPtr CreateNew() => InworldInterop.inworld_vector_Edge_new();
        
        /// <summary>
        /// Creates a copy of an existing native edge vector.
        /// </summary>
        /// <param name="source">The native pointer to the source vector to copy.</param>
        /// <returns>A native pointer to the newly created vector copy.</returns>
        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_Edge_copy(source);
        
        /// <summary>
        /// Deletes a native edge vector instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_Edge_delete(ptr);
        
        /// <summary>
        /// Reserves memory capacity for the specified number of edge elements.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="nSize">The number of edge elements to reserve capacity for.</param>
        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_Edge_reserve(ptr, nSize);
        
        /// <summary>
        /// Removes all edge elements from the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_Edge_clear(ptr);
        
        /// <summary>
        /// Adds an InworldEdge element to the end of the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="value">The InworldEdge value to add to the vector.</param>
        public void PushBack(IntPtr ptr, InworldEdge value) => InworldInterop.inworld_vector_Edge_push_back(ptr, value.ToDLL);
        
        /// <summary>
        /// Gets the number of edge elements in the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The number of elements in the vector.</returns>
        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_Edge_size(ptr);
        
        /// <summary>
        /// Gets the capacity of the edge vector (total allocated space).
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The capacity of the vector in number of elements.</returns>
        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_Edge_capacity(ptr);
        
        /// <summary>
        /// Determines whether the edge vector is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>True if the vector contains no elements; otherwise, false.</returns>
        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_Edge_empty(ptr);
        
        /// <summary>
        /// Gets the InworldEdge element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The InworldEdge element at the specified index.</returns>
        public InworldEdge GetItem(IntPtr ptr, int index) => new InworldEdge(InworldInterop.inworld_vector_Edge_get(ptr, index));
        
        /// <summary>
        /// Sets the InworldEdge element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to set.</param>
        /// <param name="value">The InworldEdge value to set at the specified index.</param>
        public void SetItem(IntPtr ptr, int index, InworldEdge value) => InworldInterop.inworld_vector_Edge_set(ptr, index, value.ToDLL);
    }
}