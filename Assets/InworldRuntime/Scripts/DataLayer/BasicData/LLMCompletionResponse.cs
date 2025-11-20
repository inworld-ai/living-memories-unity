

using System;

namespace Inworld.Framework
{
    public class LLMCompletionResponse : InworldBaseData
    {
        public LLMCompletionResponse(string response)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LLMCompletionResponse_new(response),
                InworldInterop.inworld_LLMCompletionResponse_delete);
        }

        public LLMCompletionResponse(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_LLMCompletionResponse_delete);
        }

        public LLMCompletionResponse(InworldBaseData parent)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_LLMCompletionResponse(parent.ToDLL),
                InworldInterop.inworld_LLMCompletionResponse_delete);
        }
        
        public override bool IsValid => InworldInterop.inworld_LLMCompletionResponse_is_valid(m_DLLPtr);

        public override string ToString() => InworldInterop.inworld_LLMCompletionResponse_ToString(m_DLLPtr);
        
        public bool HasCompleteContent => InworldInterop.inworld_LLMCompletionResponse_has_complete_content(m_DLLPtr);

        public string Content => InworldInterop.inworld_LLMCompletionResponse_get_content_text(m_DLLPtr);

        public InworldInputStream<string> InputStream => new InworldInputStream<string>(InworldInterop.inworld_LLMCompletionResponse_get_input_stream(m_DLLPtr));
        
        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}