using System;

namespace Inworld.Framework.LLM
{
    public class LLMRoutingCreationConfig : InworldConfig
    {
        public LLMRoutingCreationConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LLMRoutingCreationConfig_new(),
                InworldInterop.inworld_LLMRoutingCreationConfig_delete);
        }
        
        public LLMRoutingCreationConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_LLMRoutingCreationConfig_delete);
        }
        
        public LLMRoutingConfig RoutingConfig
        {
            get => new(InworldInterop.inworld_LLMRoutingCreationConfig_routing_configs_get(m_DLLPtr));
            set => InworldInterop.inworld_LLMRoutingCreationConfig_routing_configs_set(m_DLLPtr, value.ToDLL);
        }
    }
}