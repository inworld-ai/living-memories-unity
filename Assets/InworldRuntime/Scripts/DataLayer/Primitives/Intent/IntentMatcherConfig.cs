using System;
using Inworld.Framework.Node;
using Inworld.Framework.TextEmbedder;

namespace Inworld.Framework
{
    public class IntentMatcherConfig : InworldConfig
    {
        public IntentMatcherConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_IntentMatcherConfig_new(),
                InworldInterop.inworld_IntentMatcherConfig_delete);
        }
        
        public IntentMatcherConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_IntentMatcherConfig_delete);
        } 
        
        public EmbeddingMatcherConfig EmbeddingMatcherConfig
        {
            get
            {
                IntPtr optStr = InworldInterop.inworld_IntentMatcherConfig_embedding_get(m_DLLPtr);
                if (InworldInterop.inworld_optional_EmbeddingMatcherConfig_has_value(optStr))
                    return new EmbeddingMatcherConfig(InworldInterop.inworld_optional_EmbeddingMatcherConfig_getConst(optStr));
                return null;
            }
            set
            {
                IntPtr optStr = InworldInterop.inworld_optional_EmbeddingMatcherConfig_new_rcinworld_EmbeddingMatcherConfig(value.ToDLL);
                InworldInterop.inworld_IntentMatcherConfig_embedding_set(m_DLLPtr, optStr);
            }
        }
        
        public LLMMatcherConfig LLMMatcherConfig
        {
            get
            {
                IntPtr optStr = InworldInterop.inworld_IntentMatcherConfig_llm_get(m_DLLPtr);
                if (InworldInterop.inworld_optional_LlmMatcherConfig_has_value(optStr))
                    return new LLMMatcherConfig(InworldInterop.inworld_optional_LlmMatcherConfig_getConst(optStr));
                return null;
            }
            set
            {
                IntPtr optStr = InworldInterop.inworld_optional_LlmMatcherConfig_new_rcinworld_LlmMatcherConfig(value.ToDLL);
                InworldInterop.inworld_IntentMatcherConfig_llm_set(m_DLLPtr, optStr);
            }
        }

        public int TopNIntents
        {
            get => InworldInterop.inworld_IntentMatcherConfig_top_n_intents_get(m_DLLPtr);
            set => InworldInterop.inworld_IntentMatcherConfig_top_n_intents_set(m_DLLPtr, value);
        }
    }
}