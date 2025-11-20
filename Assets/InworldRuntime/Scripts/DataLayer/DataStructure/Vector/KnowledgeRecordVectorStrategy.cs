/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.Knowledge;

namespace Inworld.Framework
{
    /// <summary>
    /// Implements vector strategy for KnowledgeRecord elements.
    /// Provides native C++ interop for vectors that store knowledge record objects.
    /// Used for managing dynamic arrays of knowledge entries within the Inworld framework knowledge system.
    /// </summary>
    public class KnowledgeRecordVectorStrategy : IVectorStrategy<KnowledgeRecord>
    {
        /// <summary>
        /// Creates a new native vector instance for KnowledgeRecord elements.
        /// </summary>
        /// <returns>A native pointer to the newly created knowledge record vector.</returns>
        public IntPtr CreateNew() => InworldInterop.inworld_vector_KnowledgeRecord_new();
        
        /// <summary>
        /// Creates a copy of an existing native knowledge record vector.
        /// </summary>
        /// <param name="source">The native pointer to the source vector to copy.</param>
        /// <returns>A native pointer to the newly created vector copy.</returns>
        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_KnowledgeRecord_copy(source);
        
        /// <summary>
        /// Deletes a native knowledge record vector instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_KnowledgeRecord_delete(ptr);
        
        /// <summary>
        /// Reserves memory capacity for the specified number of knowledge record elements.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="nSize">The number of knowledge record elements to reserve capacity for.</param>
        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_KnowledgeRecord_reserve(ptr, nSize);
        
        /// <summary>
        /// Removes all knowledge record elements from the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_KnowledgeRecord_clear(ptr);
        
        /// <summary>
        /// Adds a KnowledgeRecord element to the end of the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="value">The KnowledgeRecord value to add to the vector.</param>
        public void PushBack(IntPtr ptr, KnowledgeRecord value) => InworldInterop.inworld_vector_KnowledgeRecord_push_back(ptr, value.ToDLL);
        
        /// <summary>
        /// Gets the number of knowledge record elements in the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The number of elements in the vector.</returns>
        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_KnowledgeRecord_size(ptr);
        
        /// <summary>
        /// Gets the capacity of the knowledge record vector (total allocated space).
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The capacity of the vector in number of elements.</returns>
        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_KnowledgeRecord_capacity(ptr);
        
        /// <summary>
        /// Determines whether the knowledge record vector is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>True if the vector contains no elements; otherwise, false.</returns>
        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_KnowledgeRecord_empty(ptr);
        
        /// <summary>
        /// Gets the KnowledgeRecord element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The KnowledgeRecord element at the specified index.</returns>
        public KnowledgeRecord GetItem(IntPtr ptr, int index) => new KnowledgeRecord(InworldInterop.inworld_vector_KnowledgeRecord_get(ptr, index));
        
        /// <summary>
        /// Sets the KnowledgeRecord element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to set.</param>
        /// <param name="value">The KnowledgeRecord value to set at the specified index.</param>
        public void SetItem(IntPtr ptr, int index, KnowledgeRecord value) => InworldInterop.inworld_vector_KnowledgeRecord_set(ptr, index, value.ToDLL);
    }
}