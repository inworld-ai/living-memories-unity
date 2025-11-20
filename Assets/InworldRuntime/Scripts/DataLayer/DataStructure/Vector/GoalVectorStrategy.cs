using System;

namespace Inworld.Framework
{
    public class GoalVectorStrategy : IVectorStrategy<InworldGoal>
    {
        public IntPtr CreateNew() => InworldInterop.inworld_vector_Goal_new();

        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_Goal_copy(source);

        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_Goal_delete(ptr);

        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_Goal_reserve(ptr, nSize);

        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_Goal_clear(ptr);

        public void PushBack(IntPtr ptr, InworldGoal value) => InworldInterop.inworld_vector_Goal_push_back(ptr, value.ToDLL);

        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_Goal_size(ptr);

        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_Goal_capacity(ptr);

        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_Goal_empty(ptr);

        public InworldGoal GetItem(IntPtr ptr, int index) => new InworldGoal(InworldInterop.inworld_vector_Goal_get(ptr, index));

        public void SetItem(IntPtr ptr, int index, InworldGoal value) => InworldInterop.inworld_vector_Goal_set(ptr, index, value.ToDLL);

    }
}