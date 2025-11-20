using System;
using Inworld.Framework.Node;

namespace Inworld.Framework
{
    public class CustomConfigWrapperVectorStrategy : IVectorStrategy<CustomConfigWrapper>
    {
        public IntPtr CreateNew() => InworldInterop.inworld_vector_CustomConfigWrapper_new();

        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_CustomConfigWrapper_copy(source);

        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_CustomConfigWrapper_delete(ptr);

        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_CustomConfigWrapper_reserve(ptr, nSize);

        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_CustomConfigWrapper_clear(ptr);

        public void PushBack(IntPtr ptr, CustomConfigWrapper value) => InworldInterop.inworld_vector_CustomConfigWrapper_push_back(ptr, value.ToDLL);

        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_CustomConfigWrapper_size(ptr);

        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_CustomConfigWrapper_capacity(ptr);

        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_CustomConfigWrapper_empty(ptr);

        public CustomConfigWrapper GetItem(IntPtr ptr, int index) => new CustomConfigWrapper(InworldInterop.inworld_vector_CustomConfigWrapper_get(ptr, index));

        public void SetItem(IntPtr ptr, int index, CustomConfigWrapper value) => InworldInterop.inworld_vector_CustomConfigWrapper_set(ptr, index, value.ToDLL);

    }
}