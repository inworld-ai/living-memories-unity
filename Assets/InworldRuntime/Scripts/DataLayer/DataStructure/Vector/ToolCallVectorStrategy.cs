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
    /// Implements vector strategy for ToolCall elements.
    /// Provides native C++ interop for vectors that store tool call objects.
    /// Used for managing dynamic arrays of tool invocations within the Inworld framework tool system.
    /// </summary>
    public class ToolCallVectorStrategy : IVectorStrategy<ToolCall>
    {
        /// <summary>
        /// Creates a new native vector instance for ToolCall elements.
        /// </summary>
        /// <returns>A native pointer to the newly created tool call vector.</returns>
        public IntPtr CreateNew() => InworldInterop.inworld_ToolCalls_new();
        
        /// <summary>
        /// Creates a copy of an existing native tool call vector.
        /// </summary>
        /// <param name="source">The native pointer to the source vector to copy.</param>
        /// <returns>A native pointer to the newly created vector copy.</returns>
        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_ToolCalls_copy(source);
        
        /// <summary>
        /// Deletes a native tool call vector instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_ToolCalls_delete(ptr);
        
        /// <summary>
        /// Reserves memory capacity for the specified number of tool call elements.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="nSize">The number of tool call elements to reserve capacity for.</param>
        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_ToolCalls_reserve(ptr, nSize);
        
        /// <summary>
        /// Removes all tool call elements from the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        public void Clear(IntPtr ptr) => InworldInterop.inworld_ToolCalls_clear(ptr);
        
        /// <summary>
        /// Adds a ToolCall element to the end of the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="value">The ToolCall value to add to the vector.</param>
        public void PushBack(IntPtr ptr, ToolCall value) => InworldInterop.inworld_ToolCalls_push_back(ptr, value.ToDLL);
        
        /// <summary>
        /// Gets the number of tool call elements in the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The number of elements in the vector.</returns>
        public int GetSize(IntPtr ptr) => InworldInterop.inworld_ToolCalls_size(ptr);
        
        /// <summary>
        /// Gets the capacity of the tool call vector (total allocated space).
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The capacity of the vector in number of elements.</returns>
        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_ToolCalls_capacity(ptr);
        
        /// <summary>
        /// Determines whether the tool call vector is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>True if the vector contains no elements; otherwise, false.</returns>
        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_ToolCalls_empty(ptr);
        
        /// <summary>
        /// Gets the ToolCall element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The ToolCall element at the specified index.</returns>
        public ToolCall GetItem(IntPtr ptr, int index) => new ToolCall(InworldInterop.inworld_ToolCalls_get(ptr, index));
        
        /// <summary>
        /// Sets the ToolCall element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to set.</param>
        /// <param name="value">The ToolCall value to set at the specified index.</param>
        public void SetItem(IntPtr ptr, int index, ToolCall value) => InworldInterop.inworld_ToolCalls_set(ptr, index, value.ToDLL);
    }
}