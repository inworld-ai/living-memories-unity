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
    /// Implements vector strategy for InworldKeywordMatch elements.
    /// Provides native C++ interop for vectors that store keyword matching result objects.
    /// Used for managing dynamic arrays of keyword detection results within the Inworld framework text analysis system.
    /// </summary>
    public class KeywordMatchVectorStrategy : IVectorStrategy<KeywordMatch>
    {
        /// <summary>
        /// Creates a new native vector instance for InworldKeywordMatch elements.
        /// </summary>
        /// <returns>A native pointer to the newly created keyword match vector.</returns>
        public IntPtr CreateNew() => InworldInterop.inworld_vector_KeywordMatch_new();

        /// <summary>
        /// Creates a copy of an existing native keyword match vector.
        /// </summary>
        /// <param name="source">The native pointer to the source vector to copy.</param>
        /// <returns>A native pointer to the newly created vector copy.</returns>
        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_KeywordMatch_copy(source);

        /// <summary>
        /// Deletes a native keyword match vector instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_KeywordMatch_delete(ptr);

        /// <summary>
        /// Reserves memory capacity for the specified number of keyword match elements.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="nSize">The number of keyword match elements to reserve capacity for.</param>
        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_KeywordMatch_reserve(ptr, nSize);

        /// <summary>
        /// Removes all keyword match elements from the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_KeywordMatch_clear(ptr);

        /// <summary>
        /// Adds an InworldKeywordMatch element to the end of the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="value">The InworldKeywordMatch value to add to the vector.</param>
        public void PushBack(IntPtr ptr, KeywordMatch value) =>
            InworldInterop.inworld_vector_KeywordMatch_push_back(ptr, value.ToDLL);

        /// <summary>
        /// Gets the number of keyword match elements in the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The number of elements in the vector.</returns>
        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_KeywordMatch_size(ptr);

        /// <summary>
        /// Gets the capacity of the keyword match vector (total allocated space).
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The capacity of the vector in number of elements.</returns>
        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_KeywordMatch_capacity(ptr);

        /// <summary>
        /// Determines whether the keyword match vector is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>True if the vector contains no elements; otherwise, false.</returns>
        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_KeywordMatch_empty(ptr);

        /// <summary>
        /// Gets the InworldKeywordMatch element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The InworldKeywordMatch element at the specified index.</returns>
        public KeywordMatch GetItem(IntPtr ptr, int index) =>
            new KeywordMatch(InworldInterop.inworld_vector_KeywordMatch_get(ptr, index));

        /// <summary>
        /// Sets the InworldKeywordMatch element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to set.</param>
        /// <param name="value">The InworldKeywordMatch value to set at the specified index.</param>
        public void SetItem(IntPtr ptr, int index, KeywordMatch value) =>
            InworldInterop.inworld_vector_KeywordMatch_set(ptr, index, value.ToDLL);
    }
}