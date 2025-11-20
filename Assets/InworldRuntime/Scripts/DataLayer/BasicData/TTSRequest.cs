using System;

namespace Inworld.Framework
{
    public class TTSRequest : InworldBaseData
    {
        public TTSRequest(InworldText text, InworldVoice voice)
        {
            IntPtr optVoice = InworldInterop.inworld_optional_Voice_new_rcinworld_Voice(voice.ToDLL);
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_TTSRequest_new_std_shared_ptr_Sl_inworld_graphs_Text_Sg__std_optional_Sl_inworld_Voice_Sg_(text.ToDLL, optVoice),
                InworldInterop.inworld_TTSRequest_delete);
        }

        public TTSRequest(string input, InworldVoice voice)
        {
            IntPtr optVoice = InworldInterop.inworld_optional_Voice_new_rcinworld_Voice(voice.ToDLL);
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_TTSRequest_new_rcstd_string_std_optional_Sl_inworld_Voice_Sg_(input, optVoice),
                InworldInterop.inworld_TTSRequest_delete);
        }
        public TTSRequest(InworldDataStream<string> strStream, InworldVoice voice)
        {
            IntPtr optVoice = InworldInterop.inworld_optional_Voice_new_rcinworld_Voice(voice.ToDLL);
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_TTSRequest_new_std_shared_ptr_Sl_inworld_graphs_DataStream_Sl_std_string_Sg__Sg__std_optional_Sl_inworld_Voice_Sg_(strStream.ToDLL, optVoice),
                InworldInterop.inworld_TTSRequest_delete);
        }

        public TTSRequest(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_TTSRequest_delete);
        }

        public TTSRequest(InworldBaseData parent)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_TTSRequest(parent.ToDLL),
                InworldInterop.inworld_TTSRequest_delete);
        }
        
        public override bool IsValid => InworldInterop.inworld_TTSRequest_is_valid(m_DLLPtr);

        public override string ToString() => InworldInterop.inworld_TTSRequest_ToString(m_DLLPtr);

        public InworldInputStream<string> ContentStream =>
            new InworldInputStream<string>(InworldInterop.inworld_TTSRequest_get_content_stream(m_DLLPtr));

        public InworldText TextContent => new InworldText(InworldInterop.inworld_TTSRequest_get_text_content(m_DLLPtr));

        public InworldVoice GetVoice()
        {
            IntPtr optVoice = InworldInterop.inworld_TTSRequest_get_voice(m_DLLPtr);
            if (InworldInterop.inworld_optional_Voice_has_value(optVoice))
                return new InworldVoice(InworldInterop.inworld_optional_Voice_getConst(optVoice));
            return null;
        }

        public InworldVoice Voice
        {
            get
            {
                IntPtr optVoice = InworldInterop.inworld_TTSRequest_voice(m_DLLPtr);
                if (InworldInterop.inworld_optional_Voice_has_value(optVoice))
                    return new InworldVoice(InworldInterop.inworld_optional_Voice_getConst(optVoice));
                return null;
            }
        }

        public bool HasStreamContent => InworldInterop.inworld_TTSRequest_has_stream_content(m_DLLPtr);
        
        public bool HasTextContent => InworldInterop.inworld_TTSRequest_has_text_content(m_DLLPtr);

        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}