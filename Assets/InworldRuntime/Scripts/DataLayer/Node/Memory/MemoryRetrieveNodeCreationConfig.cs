using System;

namespace Inworld.Framework.Node
{
    public class MemoryRetrieveNodeCreationConfig : NodeCreationConfig
    {
        public MemoryRetrieveNodeCreationConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_MemoryRetrieveNodeCreationConfig_new(),
                InworldInterop.inworld_MemoryRetrieveNodeCreationConfig_delete);
        }
        
        public MemoryRetrieveNodeCreationConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_MemoryRetrieveNodeCreationConfig_delete);
        }

        public string LLMComponentID
        {
            get => InworldInterop.inworld_MemoryRetrieveNodeCreationConfig_llm_component_id_get(m_DLLPtr);
            set => InworldInterop.inworld_MemoryRetrieveNodeCreationConfig_llm_component_id_set(m_DLLPtr, value);
        }

        public string EmbedderComponentID
        {
            get => InworldInterop.inworld_MemoryRetrieveNodeCreationConfig_embedder_component_id_get(m_DLLPtr);
            set => InworldInterop.inworld_MemoryRetrieveNodeCreationConfig_embedder_component_id_set(m_DLLPtr, value);
        }
    }
}