using System;

namespace Inworld.Framework.Node
{
    public class IntentNodeExecutionConfig : NodeExecutionConfig
    {
        public IntentNodeExecutionConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_IntentNodeExecutionConfig_new(),
                InworldInterop.inworld_IntentNodeExecutionConfig_delete);
        }
        
        public IntentNodeExecutionConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_IntentNodeExecutionConfig_delete);
        }

        public override bool IsValid => InworldInterop.inworld_IntentNodeExecutionConfig_is_valid(m_DLLPtr);

        public IntentMatcherConfig MatcherConfig
        {
            get => new IntentMatcherConfig(InworldInterop.inworld_IntentNodeExecutionConfig_matcher_config_get(m_DLLPtr));
            set => InworldInterop.inworld_IntentNodeExecutionConfig_matcher_config_set(m_DLLPtr, value.ToDLL);
        }

        public override bool NeedReportToClient
        {
            get => InworldInterop.inworld_IntentNodeExecutionConfig_report_to_client_get(m_DLLPtr);
            set => InworldInterop.inworld_IntentNodeExecutionConfig_report_to_client_set(m_DLLPtr, value);
        }

        public override InworldMap<string, string> Properties
        {
            get => new InworldMap<string, string>(InworldInterop.inworld_IntentNodeExecutionConfig_properties_get(m_DLLPtr));
            set => InworldInterop.inworld_IntentNodeExecutionConfig_properties_set(m_DLLPtr, value.ToDLL);
        }
    }
}