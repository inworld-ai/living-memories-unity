using System;

namespace Inworld.Framework.Node
{
    public class MemoryRetrieveNodeExecutionConfig : NodeExecutionConfig
    {
        public MemoryRetrieveNodeExecutionConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_MemoryRetrieveNodeExecutionConfig_new(),
                InworldInterop.inworld_MemoryRetrieveNodeExecutionConfig_delete);
        }
        
        public MemoryRetrieveNodeExecutionConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_MemoryRetrieveNodeExecutionConfig_delete);
        }

        public RetrievalConfig RetrievalConfig
        {
            get => new RetrievalConfig(
                InworldInterop.inworld_MemoryRetrieveNodeExecutionConfig_retrieval_config_get(m_DLLPtr));
            set => InworldInterop.inworld_MemoryRetrieveNodeExecutionConfig_retrieval_config_set(m_DLLPtr, value.ToDLL);
        }

        public bool ReturnRollingSummery
        {
            get => InworldInterop.inworld_MemoryRetrieveNodeExecutionConfig_return_rolling_summary_get(m_DLLPtr);
            set => InworldInterop.inworld_MemoryRetrieveNodeExecutionConfig_return_rolling_summary_set(m_DLLPtr, value);
        }
        
        public override bool IsValid => InworldInterop.inworld_MemoryRetrieveNodeExecutionConfig_is_valid(m_DLLPtr);
        
        public override bool NeedReportToClient
        {
            get => InworldInterop.inworld_MemoryRetrieveNodeExecutionConfig_report_to_client_get(m_DLLPtr);
            set => InworldInterop.inworld_MemoryRetrieveNodeExecutionConfig_report_to_client_set(m_DLLPtr, value);
        }

        public override InworldMap<string, string> Properties
        {
            get => new InworldMap<string, string>(InworldInterop.inworld_MemoryRetrieveNodeExecutionConfig_properties_get(m_DLLPtr));
            set => InworldInterop.inworld_MemoryRetrieveNodeExecutionConfig_properties_set(m_DLLPtr, value.ToDLL);
        }
    }
}