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
    /// Implements vector strategy for InworldMessage elements.
    /// Provides native C++ interop for vectors that store message objects with capacity management.
    /// Used for managing dynamic arrays of messages within the Inworld framework communication system.
    /// </summary>
    public class MessagesVectorStrategy : IVectorStrategy<InworldMessage>
    {
        /// <summary>
        /// Creates a new native vector instance for InworldMessage elements.
        /// </summary>
        /// <returns>A native pointer to the newly created message vector.</returns>
        public IntPtr CreateNew() => InworldInterop.inworld_Messages_new();
        
        /// <summary>
        /// Creates a copy of an existing native message vector.
        /// </summary>
        /// <param name="source">The native pointer to the source vector to copy.</param>
        /// <returns>A native pointer to the newly created vector copy.</returns>
        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_Messages_copy(source);
        
        /// <summary>
        /// Deletes a native message vector instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_Messages_delete(ptr);
        
        /// <summary>
        /// Reserves memory capacity for the specified number of message elements.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="nSize">The number of message elements to reserve capacity for.</param>
        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_Messages_reserve(ptr, nSize);
        
        /// <summary>
        /// Removes all message elements from the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        public void Clear(IntPtr ptr) => InworldInterop.inworld_Messages_clear(ptr);
        
        /// <summary>
        /// Adds an InworldMessage element to the end of the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="value">The InworldMessage value to add to the vector.</param>
        public void PushBack(IntPtr ptr, InworldMessage value) => InworldInterop.inworld_Messages_push_back(ptr, value.ToDLL);
        
        /// <summary>
        /// Gets the number of message elements in the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The number of elements in the vector.</returns>
        public int GetSize(IntPtr ptr) => InworldInterop.inworld_Messages_size(ptr);
        
        /// <summary>
        /// Gets the capacity of the message vector (total allocated space).
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The capacity of the vector in number of elements.</returns>
        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_Messages_capacity(ptr);
        
        /// <summary>
        /// Determines whether the message vector is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>True if the vector contains no elements; otherwise, false.</returns>
        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_Messages_empty(ptr);
        
        /// <summary>
        /// Gets the InworldMessage element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The InworldMessage element at the specified index.</returns>
        public InworldMessage GetItem(IntPtr ptr, int index) => new InworldMessage(InworldInterop.inworld_Messages_get(ptr, index));
        
        /// <summary>
        /// Sets the InworldMessage element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to set.</param>
        /// <param name="value">The InworldMessage value to set at the specified index.</param>
        public void SetItem(IntPtr ptr, int index, InworldMessage value) => InworldInterop.inworld_Messages_set(ptr, index, value.ToDLL);
    }
}