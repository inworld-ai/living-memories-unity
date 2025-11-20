using System;

namespace Inworld.Framework.Node
{
    public class KnowledgeNodeCreationConfig : NodeCreationConfig
    {
        public KnowledgeNodeCreationConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_KnowledgeNodeCreationConfig_new(),
                InworldInterop.inworld_KnowledgeNodeCreationConfig_delete);
        }
        
        public KnowledgeNodeCreationConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_KnowledgeNodeCreationConfig_delete);
        }

        public string KnowledgeID
        {
            get => InworldInterop.inworld_KnowledgeNodeCreationConfig_knowledge_id_get(m_DLLPtr);
            set => InworldInterop.inworld_KnowledgeNodeCreationConfig_knowledge_id_set(m_DLLPtr, value);
        }

        public InworldVector<string> KnowledgeRecords
        {
            get => new InworldVector<string>(
                InworldInterop.inworld_KnowledgeNodeCreationConfig_knowledge_records_get(m_DLLPtr));
            set => InworldInterop.inworld_KnowledgeNodeCreationConfig_knowledge_records_set(m_DLLPtr, value.ToDLL);
        }

        public string KnowledgeComponentID
        {
            get => InworldInterop.inworld_KnowledgeNodeCreationConfig_knowledge_component_id_get(m_DLLPtr);
            set => InworldInterop.inworld_KnowledgeNodeCreationConfig_knowledge_component_id_set(m_DLLPtr, value);
        }
    }
}