using System;

namespace Inworld.Framework.LLM
{
    public class LLMChatResponse : InworldBaseData
    {
        public LLMChatResponse(InworldContent content)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LLMChatResponse_new(content.ToDLL),
                InworldInterop.inworld_LLMChatResponse_delete);
        }
        public LLMChatResponse(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_LLMChatResponse_delete);
        }

        public LLMChatResponse(InworldBaseData parent)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_LLMChatResponse(parent.ToDLL),
                InworldInterop.inworld_LLMChatResponse_delete);
        }

        public override bool IsValid => InworldInterop.inworld_LLMChatResponse_is_valid(m_DLLPtr);

        public bool IsStreaming => InworldInterop.inworld_LLMChatResponse_is_streaming(m_DLLPtr);

        public InworldContent Content => new InworldContent(InworldInterop.inworld_LLMChatResponse_get_response_content(m_DLLPtr));
        
        public InworldInputStream<InworldContent> InputStream => new InworldInputStream<InworldContent>(InworldInterop.inworld_LLMChatResponse_get_response_stream(m_DLLPtr));

        public bool HasCompleteContent => InworldInterop.inworld_LLMChatResponse_has_complete_content(m_DLLPtr);
        public override string ToString() => InworldInterop.inworld_LLMChatResponse_ToString(m_DLLPtr);
    }
}