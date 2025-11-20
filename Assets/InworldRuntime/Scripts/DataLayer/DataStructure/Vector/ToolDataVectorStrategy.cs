using System;

namespace Inworld.Framework
{
    public class ToolDataVectorStrategy : IVectorStrategy<ToolData>
    {
        public IntPtr CreateNew() => InworldInterop.inworld_vector_ToolData_new();
        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_ToolData_copy(source);

        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_ToolData_delete(ptr);
        
        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_ToolData_reserve(ptr, nSize);
        
        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_ToolData_clear(ptr);

        public void PushBack(IntPtr ptr, ToolData value) => InworldInterop.inworld_vector_ToolData_push_back(ptr, value.ToDLL);

        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_ToolData_size(ptr);

        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_ToolData_capacity(ptr);

        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_ToolData_empty(ptr);

        public ToolData GetItem(IntPtr ptr, int index) =>
            new ToolData(InworldInterop.inworld_vector_ToolData_get(ptr, index));

        public void SetItem(IntPtr ptr, int index, ToolData value)
            => InworldInterop.inworld_vector_ToolData_set(ptr, index, value.ToDLL);
    }
}