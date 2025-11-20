using System;

namespace Inworld.Framework.Node
{
    public class IntentNodeCreationConfig : NodeCreationConfig
    {
        public IntentNodeCreationConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_IntentNodeCreationConfig_new(),
                InworldInterop.inworld_IntentNodeCreationConfig_delete);
        }
        
        public IntentNodeCreationConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_IntentNodeCreationConfig_delete);
        }
        
        public InworldVector<InworldIntent> Intents
        {
            get => new InworldVector<InworldIntent>(InworldInterop.inworld_IntentNodeCreationConfig_intents_get(m_DLLPtr));
            set => InworldInterop.inworld_IntentNodeCreationConfig_intents_set(m_DLLPtr, value.ToDLL);
        }

        public string EmbedderComponentID
        {
            get
            {
                IntPtr optStr = InworldInterop.inworld_IntentNodeCreationConfig_embedder_component_id_get(m_DLLPtr);
                if (InworldInterop.inworld_optional_string_has_value(optStr))
                    return InworldInterop.inworld_optional_string_getConst(optStr);
                return null;
            }
            set
            {
                IntPtr optStr = InworldInterop.inworld_optional_string_new_rcstd_string(value);
                InworldInterop.inworld_IntentNodeCreationConfig_embedder_component_id_set(m_DLLPtr, optStr);
            }
        }
        
        public string LLMComponentID
        {
            get
            {
                IntPtr optStr = InworldInterop.inworld_IntentNodeCreationConfig_llm_component_id_get(m_DLLPtr);
                if (InworldInterop.inworld_optional_string_has_value(optStr))
                    return InworldInterop.inworld_optional_string_getConst(optStr);
                return null;
            }
            set
            {
                IntPtr optStr = InworldInterop.inworld_optional_string_new_rcstd_string(value);
                InworldInterop.inworld_IntentNodeCreationConfig_llm_component_id_set(m_DLLPtr, optStr);
            }
        }
    }
}