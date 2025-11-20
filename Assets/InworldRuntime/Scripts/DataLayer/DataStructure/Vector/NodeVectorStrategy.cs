using System;
using Inworld.Framework.Node;

namespace Inworld.Framework
{
    public class NodeVectorStrategy : IVectorStrategy<InworldNode>
    {
        public IntPtr CreateNew() => InworldInterop.inworld_vector_Node_new();

        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_Node_copy(source);

        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_Node_delete(ptr);

        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_Node_reserve(ptr, nSize);

        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_Node_clear(ptr);

        public void PushBack(IntPtr ptr, InworldNode value) => InworldInterop.inworld_vector_Node_push_back(ptr, value.ToDLL);

        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_Node_size(ptr);

        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_vector_Node_capacity(ptr);

        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_Node_empty(ptr);

        public InworldNode GetItem(IntPtr ptr, int index) => new InworldNode(InworldInterop.inworld_vector_Node_get(ptr, index));

        public void SetItem(IntPtr ptr, int index, InworldNode value) => InworldInterop.inworld_vector_Node_set(ptr, index, value.ToDLL);
    }
}