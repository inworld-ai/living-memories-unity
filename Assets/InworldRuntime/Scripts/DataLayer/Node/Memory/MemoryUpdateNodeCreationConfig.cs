using System;
using Inworld.Framework.Memory;

namespace Inworld.Framework.Node
{
    public class MemoryUpdateNodeCreationConfig : NodeCreationConfig
    {
        public MemoryUpdateNodeCreationConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_MemoryUpdateNodeCreationConfig_new(),
                InworldInterop.inworld_MemoryUpdateNodeCreationConfig_delete);
        }
        
        public MemoryUpdateNodeCreationConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_MemoryUpdateNodeCreationConfig_delete);
        }
        
        public string LLMComponentID
        {
            get => InworldInterop.inworld_MemoryUpdateNodeCreationConfig_llm_component_id_get(m_DLLPtr);
            set => InworldInterop.inworld_MemoryUpdateNodeCreationConfig_llm_component_id_set(m_DLLPtr, value);
        }

        public string EmbedderComponentID
        {
            get => InworldInterop.inworld_MemoryUpdateNodeCreationConfig_embedder_component_id_get(m_DLLPtr);
            set => InworldInterop.inworld_MemoryUpdateNodeCreationConfig_embedder_component_id_set(m_DLLPtr, value);
        }

        public MemoryUpdateConfig MemoryUpdateConfig
        {
            get => new MemoryUpdateConfig(InworldInterop.inworld_MemoryUpdateNodeCreationConfig_memory_update_config_get(m_DLLPtr));
            set => InworldInterop.inworld_MemoryUpdateNodeCreationConfig_memory_update_config_set(m_DLLPtr, value.ToDLL);
        }
    }
}