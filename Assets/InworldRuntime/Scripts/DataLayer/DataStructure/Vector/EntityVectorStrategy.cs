using System;

namespace Inworld.Framework
{
    public class EntityVectorStrategy : IVectorStrategy<Entity>
    {
        public IntPtr CreateNew() => InworldInterop.inworld_vector_Entity_new();

        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_Entity_copy(source);

        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_Entity_delete(ptr);

        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_Entity_reserve(ptr, nSize);

        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_Entity_clear(ptr);

        public void PushBack(IntPtr ptr, Entity value) => InworldInterop.inworld_vector_Entity_push_back(ptr, value.ToDLL);

        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_Entity_size(ptr);

        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_Entity_capacity(ptr);

        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_Entity_empty(ptr);

        public Entity GetItem(IntPtr ptr, int index) => new Entity(InworldInterop.inworld_vector_Entity_get(ptr, index));

        public void SetItem(IntPtr ptr, int index, Entity value) => InworldInterop.inworld_vector_Entity_set(ptr, index, value.ToDLL);
    }
}