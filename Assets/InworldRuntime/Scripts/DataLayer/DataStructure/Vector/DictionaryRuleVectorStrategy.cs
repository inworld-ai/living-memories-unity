using System;

namespace Inworld.Framework
{
    public class DictionaryRuleVectorStrategy : IVectorStrategy<DictionaryRule>
    {
        public IntPtr CreateNew() => InworldInterop.inworld_vector_DictionaryRule_new();

        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_DictionaryRule_copy(source);

        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_DictionaryRule_delete(ptr);

        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_DictionaryRule_reserve(ptr, nSize);

        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_DictionaryRule_clear(ptr);

        public void PushBack(IntPtr ptr, DictionaryRule value) => InworldInterop.inworld_vector_DictionaryRule_push_back(ptr, value.ToDLL);

        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_DictionaryRule_size(ptr);

        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_DictionaryRule_capacity(ptr);

        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_DictionaryRule_empty(ptr);

        public DictionaryRule GetItem(IntPtr ptr, int index) => new DictionaryRule(InworldInterop.inworld_vector_DictionaryRule_get(ptr, index));

        public void SetItem(IntPtr ptr, int index, DictionaryRule value) => InworldInterop.inworld_vector_DictionaryRule_set(ptr, index, value.ToDLL);
    }
}