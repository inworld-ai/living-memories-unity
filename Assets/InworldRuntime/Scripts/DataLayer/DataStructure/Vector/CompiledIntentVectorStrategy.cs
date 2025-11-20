using System;

namespace Inworld.Framework
{
    public class CompiledIntentVectorStrategy : IVectorStrategy<CompiledIntent>
    {
        public IntPtr CreateNew() => InworldInterop.inworld_vector_CompiledIntent_new();

        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_CompiledIntent_copy(source);

        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_CompiledIntent_delete(ptr);

        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_CompiledIntent_reserve(ptr, nSize);

        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_CompiledIntent_clear(ptr);

        public void PushBack(IntPtr ptr, CompiledIntent value) => InworldInterop.inworld_vector_CompiledIntent_push_back(ptr, value.ToDLL);

        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_CompiledIntent_size(ptr);

        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_CompiledIntent_capacity(ptr);

        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_CompiledIntent_empty(ptr);

        public CompiledIntent GetItem(IntPtr ptr, int index) => new CompiledIntent(InworldInterop.inworld_vector_CompiledIntent_get(ptr, index));

        public void SetItem(IntPtr ptr, int index, CompiledIntent value) => InworldInterop.inworld_vector_CompiledIntent_set(ptr, index, value.ToDLL);
    }
}