using System;

namespace Inworld.Framework
{
    public class EntityMatchVectorStrategy : IVectorStrategy<EntityMatch>
    {
        public IntPtr CreateNew() => InworldInterop.inworld_vector_EntityMatch_new();

        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_EntityMatch_copy(source);

        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_EntityMatch_delete(ptr);

        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_EntityMatch_reserve(ptr, nSize);

        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_EntityMatch_clear(ptr);

        public void PushBack(IntPtr ptr, EntityMatch value) => InworldInterop.inworld_vector_EntityMatch_push_back(ptr, value.ToDLL);

        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_EntityMatch_size(ptr);

        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_EntityMatch_capacity(ptr);

        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_EntityMatch_empty(ptr);

        public EntityMatch GetItem(IntPtr ptr, int index) => new EntityMatch(InworldInterop.inworld_vector_EntityMatch_get(ptr, index));

        public void SetItem(IntPtr ptr, int index, EntityMatch value) => InworldInterop.inworld_vector_EntityMatch_set(ptr, index, value.ToDLL);
    }
}