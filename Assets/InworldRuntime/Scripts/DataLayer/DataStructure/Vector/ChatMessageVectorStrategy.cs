using System;

namespace Inworld.Framework
{
    public class ChatMessageVectorStrategy : IVectorStrategy<ChatMessage>
    {
        public IntPtr CreateNew() => InworldInterop.inworld_Vector_ChatMessage_new();

        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_Vector_ChatMessage_copy(source);

        public void Delete(IntPtr ptr) => InworldInterop.inworld_Vector_ChatMessage_delete(ptr);

        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_Vector_ChatMessage_reserve(ptr, nSize);

        public void Clear(IntPtr ptr) => InworldInterop.inworld_Vector_ChatMessage_clear(ptr);

        public void PushBack(IntPtr ptr, ChatMessage value) => InworldInterop.inworld_Vector_ChatMessage_push_back(ptr, value.ToDLL);

        public int GetSize(IntPtr ptr) => InworldInterop.inworld_Vector_ChatMessage_size(ptr);

        public int GetCapacity(IntPtr ptr) => InworldInterop.inworld_Vector_ChatMessage_capacity(ptr);

        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_Vector_ChatMessage_empty(ptr);

        public ChatMessage GetItem(IntPtr ptr, int index) => new ChatMessage(InworldInterop.inworld_Vector_ChatMessage_get(ptr, index));

        public void SetItem(IntPtr ptr, int index, ChatMessage value) => InworldInterop.inworld_Vector_ChatMessage_set(ptr, index, value.ToDLL);

    }
}