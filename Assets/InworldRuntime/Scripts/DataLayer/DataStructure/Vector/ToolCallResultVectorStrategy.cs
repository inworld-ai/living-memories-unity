using System;

namespace Inworld.Framework
{
    public class ToolCallResultVectorStrategy : IVectorStrategy<ToolCallResult>
    {
        public IntPtr CreateNew() => InworldInterop.inworld_vector_ToolCallResult_new();
        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_ToolCallResult_copy(source);

        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_ToolCallResult_delete(ptr);
        
        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_ToolCallResult_reserve(ptr, nSize);
        
        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_ToolCallResult_clear(ptr);

        public void PushBack(IntPtr ptr, ToolCallResult value) => InworldInterop.inworld_vector_ToolCallResult_push_back(ptr, value.ToDLL);

        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_ToolCallResult_size(ptr);

        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_ToolCallResult_capacity(ptr);

        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_ToolCallResult_empty(ptr);

        public ToolCallResult GetItem(IntPtr ptr, int index) =>
            new ToolCallResult(InworldInterop.inworld_vector_ToolCallResult_get(ptr, index));

        public void SetItem(IntPtr ptr, int index, ToolCallResult value)
            => InworldInterop.inworld_vector_ToolCallResult_set(ptr, index, value.ToDLL);
    }
}