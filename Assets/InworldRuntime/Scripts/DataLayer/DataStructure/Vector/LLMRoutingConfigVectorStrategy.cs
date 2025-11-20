/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.LLM;

namespace Inworld.Framework
{
    /// <summary>
    /// Implements vector strategy for LLMRoutingConfig elements.
    /// Provides native C++ interop for vectors that store LLM routing configuration objects.
    /// Used for managing dynamic arrays of LLM routing configurations within the Inworld framework AI system.
    /// </summary>
    public class LLMRoutingConfigVectorStrategy : IVectorStrategy<LLMRoutingConfig>
    {
        /// <summary>
        /// Creates a new native vector instance for LLMRoutingConfig elements.
        /// </summary>
        /// <returns>A native pointer to the newly created LLM routing config vector.</returns>
        public IntPtr CreateNew() => InworldInterop.inworld_vector_LLMRoutingConfig_new();
        
        /// <summary>
        /// Creates a copy of an existing native LLM routing config vector.
        /// </summary>
        /// <param name="source">The native pointer to the source vector to copy.</param>
        /// <returns>A native pointer to the newly created vector copy.</returns>
        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_LLMRoutingConfig_copy(source);
        
        /// <summary>
        /// Deletes a native LLM routing config vector instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_LLMRoutingConfig_delete(ptr);
        
        /// <summary>
        /// Reserves memory capacity for the specified number of LLM routing config elements.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="nSize">The number of LLM routing config elements to reserve capacity for.</param>
        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_LLMRoutingConfig_reserve(ptr, nSize);
        
        /// <summary>
        /// Removes all LLM routing config elements from the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_LLMRoutingConfig_clear(ptr);
        
        /// <summary>
        /// Adds an LLMRoutingConfig element to the end of the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="value">The LLMRoutingConfig value to add to the vector.</param>
        public void PushBack(IntPtr ptr, LLMRoutingConfig value) => InworldInterop.inworld_vector_LLMRoutingConfig_push_back(ptr, value.ToDLL);
        
        /// <summary>
        /// Gets the number of LLM routing config elements in the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The number of elements in the vector.</returns>
        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_LLMRoutingConfig_size(ptr);
        
        /// <summary>
        /// Gets the capacity of the LLM routing config vector (total allocated space).
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The capacity of the vector in number of elements.</returns>
        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_LLMRoutingConfig_capacity(ptr);
        
        /// <summary>
        /// Determines whether the LLM routing config vector is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>True if the vector contains no elements; otherwise, false.</returns>
        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_LLMRoutingConfig_empty(ptr);
        
        /// <summary>
        /// Gets the LLMRoutingConfig element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The LLMRoutingConfig element at the specified index.</returns>
        public LLMRoutingConfig GetItem(IntPtr ptr, int index) => new LLMRoutingConfig(InworldInterop.inworld_vector_LLMRoutingConfig_get(ptr, index));
        
        /// <summary>
        /// Sets the LLMRoutingConfig element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to set.</param>
        /// <param name="value">The LLMRoutingConfig value to set at the specified index.</param>
        public void SetItem(IntPtr ptr, int index, LLMRoutingConfig value) => InworldInterop.inworld_vector_LLMRoutingConfig_set(ptr, index, value.ToDLL);
    }
}