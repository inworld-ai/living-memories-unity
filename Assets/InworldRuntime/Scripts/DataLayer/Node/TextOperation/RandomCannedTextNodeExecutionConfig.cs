using System;

namespace Inworld.Framework.Node
{
    public class RandomCannedTextNodeExecutionConfig : NodeExecutionConfig
    {
        public RandomCannedTextNodeExecutionConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_RandomCannedTextNodeExecutionConfig_new(),
                InworldInterop.inworld_RandomCannedTextNodeExecutionConfig_delete);
        }
        
        public RandomCannedTextNodeExecutionConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_RandomCannedTextNodeExecutionConfig_delete);
        }

        public InworldVector<string> CannedPhrases
        {
            get => new InworldVector<string>(
                InworldInterop.inworld_RandomCannedTextNodeExecutionConfig_canned_phrases_get(m_DLLPtr));
            set => InworldInterop.inworld_RandomCannedTextNodeExecutionConfig_canned_phrases_set(m_DLLPtr, value.ToDLL);
        }

        public override bool IsValid => InworldInterop.inworld_RandomCannedTextNodeExecutionConfig_is_valid(m_DLLPtr);
        
        public override bool NeedReportToClient
        {
            get => InworldInterop.inworld_RandomCannedTextNodeExecutionConfig_report_to_client_get(m_DLLPtr);
            set => InworldInterop.inworld_RandomCannedTextNodeExecutionConfig_report_to_client_set(m_DLLPtr, value);
        } 

        public override InworldMap<string, string> Properties
        {
            get => new InworldMap<string, string>(InworldInterop.inworld_RandomCannedTextNodeExecutionConfig_properties_get(m_DLLPtr));
            set => InworldInterop.inworld_RandomCannedTextNodeExecutionConfig_properties_set(m_DLLPtr, value.ToDLL);
        }
    }
}