using System;

namespace Inworld.Framework
{
    public class InworldIntentVectorStrategy : IVectorStrategy<InworldIntent>
    {
        public IntPtr CreateNew() => InworldInterop.inworld_vector_Intent_new();

        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_Intent_copy(source);

        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_Intent_delete(ptr);

        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_Intent_reserve(ptr, nSize);

        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_Intent_clear(ptr);

        public void PushBack(IntPtr ptr, InworldIntent value) => InworldInterop.inworld_vector_Intent_push_back(ptr, value.ToDLL);

        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_Intent_size(ptr);

        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_Intent_capacity(ptr);

        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_Intent_empty(ptr);

        public InworldIntent GetItem(IntPtr ptr, int index) => new InworldIntent(InworldInterop.inworld_vector_Intent_get(ptr, index));

        public void SetItem(IntPtr ptr, int index, InworldIntent value) => InworldInterop.inworld_vector_Intent_set(ptr, index, value.ToDLL);

    }
}