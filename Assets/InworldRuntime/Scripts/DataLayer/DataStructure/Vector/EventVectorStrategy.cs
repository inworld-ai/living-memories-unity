/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.Event;

namespace Inworld.Framework
{
    /// <summary>
    /// Implements vector strategy for InworldEvent elements.
    /// Provides native C++ interop for vectors that store event objects.
    /// Used for managing dynamic arrays of events within the Inworld framework event system.
    /// </summary>
    public class EventVectorStrategy : IVectorStrategy<InworldEvent>
    {
        /// <summary>
        /// Creates a new native vector instance for InworldEvent elements.
        /// </summary>
        /// <returns>A native pointer to the newly created event vector.</returns>
        public IntPtr CreateNew() => InworldInterop.inworld_vector_Event_new();
        
        /// <summary>
        /// Creates a copy of an existing native event vector.
        /// </summary>
        /// <param name="source">The native pointer to the source vector to copy.</param>
        /// <returns>A native pointer to the newly created vector copy.</returns>
        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_Event_copy(source);
        
        /// <summary>
        /// Deletes a native event vector instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_Event_delete(ptr);
        
        /// <summary>
        /// Reserves memory capacity for the specified number of event elements.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="nSize">The number of event elements to reserve capacity for.</param>
        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_Event_reserve(ptr, nSize);
        
        /// <summary>
        /// Removes all event elements from the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_Event_clear(ptr);
        
        /// <summary>
        /// Adds an InworldEvent element to the end of the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="value">The InworldEvent value to add to the vector.</param>
        public void PushBack(IntPtr ptr, InworldEvent value) => InworldInterop.inworld_vector_Event_push_back(ptr, value.ToDLL);
        
        /// <summary>
        /// Gets the number of event elements in the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The number of elements in the vector.</returns>
        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_Event_size(ptr);
        
        /// <summary>
        /// Gets the capacity of the event vector (total allocated space).
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The capacity of the vector in number of elements.</returns>
        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_Event_capacity(ptr);
        
        /// <summary>
        /// Determines whether the event vector is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>True if the vector contains no elements; otherwise, false.</returns>
        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_Event_empty(ptr);
        
        /// <summary>
        /// Gets the InworldEvent element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The InworldEvent element at the specified index.</returns>
        public InworldEvent GetItem(IntPtr ptr, int index) => new InworldEvent(InworldInterop.inworld_vector_Event_get(ptr, index));
        
        /// <summary>
        /// Sets the InworldEvent element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to set.</param>
        /// <param name="value">The InworldEvent value to set at the specified index.</param>
        public void SetItem(IntPtr ptr, int index, InworldEvent value) => InworldInterop.inworld_vector_Event_set(ptr, index, value.ToDLL);
    }
}