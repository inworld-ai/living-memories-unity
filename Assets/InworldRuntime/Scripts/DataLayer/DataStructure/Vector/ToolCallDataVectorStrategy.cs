using System;

namespace Inworld.Framework
{
    public class ToolCallDataVectorStrategy : IVectorStrategy<ToolCallData>
    {
        public IntPtr CreateNew() => InworldInterop.inworld_vector_ToolCallData_new();
        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_ToolCallData_copy(source);

        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_ToolCallData_delete(ptr);
        
        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_ToolCallData_reserve(ptr, nSize);
        
        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_ToolCallData_clear(ptr);

        public void PushBack(IntPtr ptr, ToolCallData value) => InworldInterop.inworld_vector_ToolCallData_push_back(ptr, value.ToDLL);

        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_ToolCallData_size(ptr);

        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_ToolCallData_capacity(ptr);

        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_ToolCallData_empty(ptr);

        public ToolCallData GetItem(IntPtr ptr, int index) =>
            new ToolCallData(InworldInterop.inworld_vector_ToolCallData_get(ptr, index));

        public void SetItem(IntPtr ptr, int index, ToolCallData value)
            => InworldInterop.inworld_vector_ToolCallData_set(ptr, index, value.ToDLL);
    }
}