/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.Safety;

namespace Inworld.Framework
{
    /// <summary>
    /// Implements vector strategy for TopicThreshold elements.
    /// Provides native C++ interop for vectors that store topic threshold configuration objects.
    /// Used for managing dynamic arrays of safety threshold settings within the Inworld framework safety system.
    /// </summary>
    public class TopicThresholdVectorStrategy : IVectorStrategy<TopicThreshold>
    {
        /// <summary>
        /// Creates a new native vector instance for TopicThreshold elements.
        /// </summary>
        /// <returns>A native pointer to the newly created topic threshold vector.</returns>
        public IntPtr CreateNew() => InworldInterop.inworld_vector_TopicThreshold_new();

        /// <summary>
        /// Creates a copy of an existing native topic threshold vector.
        /// </summary>
        /// <param name="source">The native pointer to the source vector to copy.</param>
        /// <returns>A native pointer to the newly created vector copy.</returns>
        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_TopicThreshold_copy(source);
        
        /// <summary>
        /// Deletes a native topic threshold vector instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_TopicThreshold_delete(ptr);
        
        /// <summary>
        /// Reserves memory capacity for the specified number of topic threshold elements.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="nSize">The number of topic threshold elements to reserve capacity for.</param>
        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_TopicThreshold_reserve(ptr, nSize);

        /// <summary>
        /// Removes all topic threshold elements from the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_TopicThreshold_clear(ptr);

        /// <summary>
        /// Adds a TopicThreshold element to the end of the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="value">The TopicThreshold value to add to the vector.</param>
        public void PushBack(IntPtr ptr, TopicThreshold value) => InworldInterop.inworld_vector_TopicThreshold_push_back(ptr, value.ToDLL);

        /// <summary>
        /// Gets the number of topic threshold elements in the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The number of elements in the vector.</returns>
        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_TopicThreshold_size(ptr);

        /// <summary>
        /// Gets the capacity of the topic threshold vector (total allocated space).
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The capacity of the vector in number of elements.</returns>
        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_TopicThreshold_capacity(ptr);

        /// <summary>
        /// Determines whether the topic threshold vector is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>True if the vector contains no elements; otherwise, false.</returns>
        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_TopicThreshold_empty(ptr);

        /// <summary>
        /// Gets the TopicThreshold element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The TopicThreshold element at the specified index.</returns>
        public TopicThreshold GetItem(IntPtr ptr, int index) =>
            new TopicThreshold(InworldInterop.inworld_vector_TopicThreshold_get(ptr, index));

        /// <summary>
        /// Sets the TopicThreshold element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to set.</param>
        /// <param name="value">The TopicThreshold value to set at the specified index.</param>
        public void SetItem(IntPtr ptr, int index, TopicThreshold value) =>
            InworldInterop.inworld_vector_TopicThreshold_set(ptr, index, value.ToDLL);
        
    }
}