using System;
using Inworld.Framework.Node;

namespace Inworld.Framework
{
    public class DeviceVectorStrategy : IVectorStrategy<InworldDevice>
    {
        public IntPtr CreateNew() => InworldInterop.inworld_vector_Device_new();

        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_Device_copy(source);

        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_Device_delete(ptr);

        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_Device_reserve(ptr, nSize);

        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_Device_clear(ptr);

        public void PushBack(IntPtr ptr, InworldDevice value) => InworldInterop.inworld_vector_Device_push_back(ptr, value.ToDLL);

        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_Device_size(ptr);

        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_Device_capacity(ptr);

        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_Device_empty(ptr);

        public InworldDevice GetItem(IntPtr ptr, int index) => new InworldDevice(InworldInterop.inworld_vector_Device_get(ptr, index));

        public void SetItem(IntPtr ptr, int index, InworldDevice value) => InworldInterop.inworld_vector_Device_set(ptr, index, value.ToDLL);
    }
}