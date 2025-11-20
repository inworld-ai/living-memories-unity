using System;

namespace Inworld.Framework
{
    public class KeywordGroupVectorStrategy : IVectorStrategy<KeywordGroup>
    {
        public IntPtr CreateNew() => InworldInterop.inworld_vector_KeywordGroup_new();

        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_KeywordGroup_copy(source);

        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_KeywordGroup_delete(ptr);

        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_KeywordGroup_reserve(ptr, nSize);

        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_KeywordGroup_clear(ptr);

        public void PushBack(IntPtr ptr, KeywordGroup value) => InworldInterop.inworld_vector_KeywordGroup_push_back(ptr, value.ToDLL);

        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_KeywordGroup_size(ptr);

        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_KeywordGroup_capacity(ptr);

        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_KeywordGroup_empty(ptr);

        public KeywordGroup GetItem(IntPtr ptr, int index) => new KeywordGroup(InworldInterop.inworld_vector_KeywordGroup_get(ptr, index));

        public void SetItem(IntPtr ptr, int index, KeywordGroup value) => InworldInterop.inworld_vector_KeywordGroup_set(ptr, index, value.ToDLL);

    }
}