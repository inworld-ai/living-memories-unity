using System;
using Inworld.Framework.Memory;

namespace Inworld.Framework
{
    public class KnowledgeCollectionVectorStrategy : IVectorStrategy<KnowledgeCollection>
    {
        public IntPtr CreateNew() => InworldInterop.inworld_vector_KnowledgeCollection_new();

        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_KnowledgeCollection_copy(source);

        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_KnowledgeCollection_delete(ptr);

        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_KnowledgeCollection_reserve(ptr, nSize);

        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_KnowledgeCollection_clear(ptr);

        public void PushBack(IntPtr ptr, KnowledgeCollection value) => InworldInterop.inworld_vector_KnowledgeCollection_push_back(ptr, value.ToDLL);

        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_KnowledgeCollection_size(ptr);

        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_KnowledgeCollection_capacity(ptr);

        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_KnowledgeCollection_empty(ptr);

        public KnowledgeCollection GetItem(IntPtr ptr, int index) => new KnowledgeCollection(InworldInterop.inworld_vector_KnowledgeCollection_get(ptr, index));

        public void SetItem(IntPtr ptr, int index, KnowledgeCollection value) => InworldInterop.inworld_vector_KnowledgeCollection_set(ptr, index, value.ToDLL);

    }
}