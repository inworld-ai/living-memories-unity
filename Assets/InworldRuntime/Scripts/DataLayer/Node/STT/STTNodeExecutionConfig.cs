using System;

namespace Inworld.Framework.Node
{
    public class STTNodeExecutionConfig : NodeExecutionConfig
    {
        public STTNodeExecutionConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_STTNodeExecutionConfig_new(),
                InworldInterop.inworld_STTNodeExecutionConfig_delete);
        }
        
        public STTNodeExecutionConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_STTNodeExecutionConfig_delete);
        }

        public string STTComponentID
        {
            get => InworldInterop.inworld_STTNodeExecutionConfig_stt_component_id_get(m_DLLPtr);
            set => InworldInterop.inworld_STTNodeExecutionConfig_stt_component_id_set(m_DLLPtr, value);
        }

        public override bool IsValid => InworldInterop.inworld_STTNodeExecutionConfig_is_valid(m_DLLPtr);
        
        public override bool NeedReportToClient
        {
            get => InworldInterop.inworld_STTNodeExecutionConfig_report_to_client_get(m_DLLPtr);
            set => InworldInterop.inworld_STTNodeExecutionConfig_report_to_client_set(m_DLLPtr, value);
        } 

        public override InworldMap<string, string> Properties
        {
            get => new InworldMap<string, string>(InworldInterop.inworld_STTNodeExecutionConfig_properties_get(m_DLLPtr));
            set => InworldInterop.inworld_STTNodeExecutionConfig_properties_set(m_DLLPtr, value.ToDLL);
        }
    }
}