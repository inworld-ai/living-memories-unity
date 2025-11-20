using System;
using Inworld.Framework.Node;

namespace Inworld.Framework
{
    public class StatusCodeVectorStrategy : IVectorStrategy<StatusCode>
    {
        public IntPtr CreateNew() => InworldInterop.inworld_vector_StatusCode_new();

        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_StatusCode_copy(source);

        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_StatusCode_delete(ptr);

        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_StatusCode_reserve(ptr, nSize);

        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_StatusCode_clear(ptr);

        public void PushBack(IntPtr ptr, StatusCode value) => InworldInterop.inworld_vector_StatusCode_push_back(ptr, Convert.ToInt32(value));

        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_StatusCode_size(ptr);

        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_StatusCode_capacity(ptr);

        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_StatusCode_empty(ptr);

        public StatusCode GetItem(IntPtr ptr, int index) => (StatusCode)InworldInterop.inworld_vector_StatusCode_get(ptr, index);

        public void SetItem(IntPtr ptr, int index, StatusCode value) => InworldInterop.inworld_vector_StatusCode_set(ptr, index, Convert.ToInt32(value));

    }
}