using System;

namespace Inworld.Framework.Node
{
    public class KnowledgeNodeExecutionConfig : NodeExecutionConfig
    {
        public KnowledgeNodeExecutionConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_KnowledgeNodeExecutionConfig_new(),
                InworldInterop.inworld_KnowledgeNodeExecutionConfig_delete);
        }
        
        public KnowledgeNodeExecutionConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_KnowledgeNodeExecutionConfig_delete);
        }

        public override bool IsValid => InworldInterop.inworld_KnowledgeNodeExecutionConfig_is_valid(m_DLLPtr);
        
        public override bool NeedReportToClient
        {
            get => InworldInterop.inworld_KnowledgeNodeExecutionConfig_report_to_client_get(m_DLLPtr); 
            set => InworldInterop.inworld_KnowledgeNodeExecutionConfig_report_to_client_set(m_DLLPtr, value);
        }

        public override InworldMap<string, string> Properties
        {
            get => new InworldMap<string, string>(InworldInterop.inworld_KnowledgeNodeExecutionConfig_properties_get(m_DLLPtr)); 
            set => InworldInterop.inworld_KnowledgeNodeExecutionConfig_properties_set(m_DLLPtr, value.ToDLL);
        }
        
        public InworldVector<string> KnowledgeIDs
        {
            get => new InworldVector<string>(InworldInterop.inworld_KnowledgeNodeExecutionConfig_knowledge_ids_get(m_DLLPtr)); 
            set => InworldInterop.inworld_KnowledgeNodeExecutionConfig_knowledge_ids_set(m_DLLPtr, value.ToDLL); 
        }

        public RetrievalConfig RetrievalConfig
        {
            get => new RetrievalConfig(InworldInterop.inworld_KnowledgeNodeExecutionConfig_retrieval_config_get(m_DLLPtr)); 
            set => InworldInterop.inworld_KnowledgeNodeExecutionConfig_retrieval_config_set(m_DLLPtr, value.ToDLL); 
        }
    }
}