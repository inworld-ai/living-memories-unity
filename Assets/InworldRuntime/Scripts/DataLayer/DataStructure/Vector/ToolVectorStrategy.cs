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
    /// Implements vector strategy for InworldTool elements.
    /// Provides native C++ interop for vectors that store tool definition objects.
    /// Used for managing dynamic arrays of available tools within the Inworld framework tool system.
    /// </summary>
    public class ToolVectorStrategy : IVectorStrategy<InworldTool>
    {
        /// <summary>
        /// Creates a new native vector instance for InworldTool elements.
        /// </summary>
        /// <returns>A native pointer to the newly created tool vector.</returns>
        public IntPtr CreateNew() => InworldInterop.inworld_Tools_new();
        
        /// <summary>
        /// Creates a copy of an existing native tool vector.
        /// </summary>
        /// <param name="source">The native pointer to the source vector to copy.</param>
        /// <returns>A native pointer to the newly created vector copy.</returns>
        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_Tools_copy(source);
        
        /// <summary>
        /// Deletes a native tool vector instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_Tools_delete(ptr);
        
        /// <summary>
        /// Reserves memory capacity for the specified number of tool elements.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="nSize">The number of tool elements to reserve capacity for.</param>
        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_Tools_reserve(ptr, nSize);
        
        /// <summary>
        /// Removes all tool elements from the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        public void Clear(IntPtr ptr) => InworldInterop.inworld_Tools_clear(ptr);
        
        /// <summary>
        /// Adds an InworldTool element to the end of the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="value">The InworldTool value to add to the vector.</param>
        public void PushBack(IntPtr ptr, InworldTool value) => InworldInterop.inworld_Tools_push_back(ptr, value.ToDLL);
        
        /// <summary>
        /// Gets the number of tool elements in the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The number of elements in the vector.</returns>
        public int GetSize(IntPtr ptr) => InworldInterop.inworld_Tools_size(ptr);
        
        /// <summary>
        /// Gets the capacity of the tool vector (total allocated space).
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The capacity of the vector in number of elements.</returns>
        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_Tools_capacity(ptr);
        
        /// <summary>
        /// Determines whether the tool vector is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>True if the vector contains no elements; otherwise, false.</returns>
        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_Tools_empty(ptr);
        
        /// <summary>
        /// Gets the InworldTool element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The InworldTool element at the specified index.</returns>
        public InworldTool GetItem(IntPtr ptr, int index) => new InworldTool(InworldInterop.inworld_Tools_get(ptr, index));
        
        /// <summary>
        /// Sets the InworldTool element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to set.</param>
        /// <param name="value">The InworldTool value to set at the specified index.</param>
        public void SetItem(IntPtr ptr, int index, InworldTool value) => InworldInterop.inworld_Tools_set(ptr, index, value.ToDLL);
    }
}