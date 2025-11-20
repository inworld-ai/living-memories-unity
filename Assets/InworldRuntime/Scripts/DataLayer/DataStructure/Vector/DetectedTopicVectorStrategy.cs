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
    /// Implements vector strategy for DetectedTopic elements.
    /// Provides native C++ interop for vectors that store detected topic objects from safety analysis.
    /// Used for managing dynamic arrays of topic detection results within the Inworld framework safety system.
    /// </summary>
    public class DetectedTopicVectorStrategy : IVectorStrategy<DetectedTopic>
    {
        /// <summary>
        /// Creates a new native vector instance for DetectedTopic elements.
        /// </summary>
        /// <returns>A native pointer to the newly created detected topic vector.</returns>
        public IntPtr CreateNew() => InworldInterop.inworld_vector_DetectedTopic_new();

        /// <summary>
        /// Creates a copy of an existing native detected topic vector.
        /// </summary>
        /// <param name="source">The native pointer to the source vector to copy.</param>
        /// <returns>A native pointer to the newly created vector copy.</returns>
        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_DetectedTopic_copy(source);

        /// <summary>
        /// Deletes a native detected topic vector instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_DetectedTopic_delete(ptr);

        /// <summary>
        /// Reserves memory capacity for the specified number of detected topic elements.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="nSize">The number of detected topic elements to reserve capacity for.</param>
        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_DetectedTopic_reserve(ptr, nSize);

        /// <summary>
        /// Removes all detected topic elements from the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_DetectedTopic_clear(ptr);

        /// <summary>
        /// Adds a DetectedTopic element to the end of the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="value">The DetectedTopic value to add to the vector.</param>
        public void PushBack(IntPtr ptr, DetectedTopic value) =>
            InworldInterop.inworld_vector_DetectedTopic_push_back(ptr, value.ToDLL);

        /// <summary>
        /// Gets the number of detected topic elements in the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The number of elements in the vector.</returns>
        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_DetectedTopic_size(ptr);

        /// <summary>
        /// Gets the capacity of the detected topic vector (total allocated space).
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The capacity of the vector in number of elements.</returns>
        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_DetectedTopic_capacity(ptr);

        /// <summary>
        /// Determines whether the detected topic vector is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>True if the vector contains no elements; otherwise, false.</returns>
        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_DetectedTopic_empty(ptr);

        /// <summary>
        /// Gets the DetectedTopic element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The DetectedTopic element at the specified index.</returns>
        public DetectedTopic GetItem(IntPtr ptr, int index) => new DetectedTopic(InworldInterop.inworld_vector_DetectedTopic_get(ptr, index));

        /// <summary>
        /// Sets the DetectedTopic element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to set.</param>
        /// <param name="value">The DetectedTopic value to set at the specified index.</param>
        public void SetItem(IntPtr ptr, int index, DetectedTopic value) =>
            InworldInterop.inworld_vector_DetectedTopic_set(ptr, index, value.ToDLL);
    }
}