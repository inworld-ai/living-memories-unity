using System;
using Inworld.Framework.TTS;

namespace Inworld.Framework.Node
{
    public class TTSNodeExecutionConfig : NodeExecutionConfig
    {
        public TTSNodeExecutionConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_TTSNodeExecutionConfig_new(),
                InworldInterop.inworld_TTSNodeExecutionConfig_delete);
        }
        
        public TTSNodeExecutionConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_TTSNodeExecutionConfig_delete);
        }

        public string TTSComponentID
        {
            get => InworldInterop.inworld_TTSNodeExecutionConfig_tts_component_id_get(m_DLLPtr);
            set => InworldInterop.inworld_TTSNodeExecutionConfig_tts_component_id_set(m_DLLPtr, value);
        }

        public InworldVoice Voice
        {
            get => new InworldVoice(InworldInterop.inworld_TTSNodeExecutionConfig_voice_get(m_DLLPtr));
            set => InworldInterop.inworld_TTSNodeExecutionConfig_voice_set(m_DLLPtr, value.ToDLL);
        }
        
        public SpeechSynthesisConfig SpeechSynthesisConfig
        {
            get
            {
                IntPtr optStr = InworldInterop.inworld_TTSNodeExecutionConfig_synthesis_config_get(m_DLLPtr);
                if (InworldInterop.inworld_optional_SpeechSynthesisConfig_has_value(optStr))
                    return new SpeechSynthesisConfig(InworldInterop.inworld_optional_SpeechSynthesisConfig_getConst(optStr));
                return null;
            }
            set
            {
                IntPtr optStr = InworldInterop.inworld_optional_SpeechSynthesisConfig_new_rcinworld_SpeechSynthesisConfig(value.ToDLL);
                InworldInterop.inworld_TTSNodeExecutionConfig_synthesis_config_set(m_DLLPtr, optStr);
            }
        }

        public override bool IsValid => InworldInterop.inworld_TTSNodeExecutionConfig_is_valid(m_DLLPtr);
        
        public override bool NeedReportToClient
        {
            get => InworldInterop.inworld_TTSNodeExecutionConfig_report_to_client_get(m_DLLPtr);
            set => InworldInterop.inworld_TTSNodeExecutionConfig_report_to_client_set(m_DLLPtr, value);
        } 

        public override InworldMap<string, string> Properties
        {
            get => new InworldMap<string, string>(InworldInterop.inworld_TTSNodeExecutionConfig_properties_get(m_DLLPtr));
            set => InworldInterop.inworld_TTSNodeExecutionConfig_properties_set(m_DLLPtr, value.ToDLL);
        }
    }
}