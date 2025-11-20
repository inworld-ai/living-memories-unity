using System;
using Inworld.Framework.LLM;


namespace Inworld.Framework.Node
{
    public class LLMMatcherConfig : InworldConfig
    {
        public LLMMatcherConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LlmMatcherConfig_new(),
                InworldInterop.inworld_LlmMatcherConfig_delete);
        }
        
        public LLMMatcherConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_LlmMatcherConfig_delete);
        }

        public TextGenerationConfig GenerationConfig
        {
            get => new TextGenerationConfig(InworldInterop.inworld_LlmMatcherConfig_generation_config_get(m_DLLPtr));
            set => InworldInterop.inworld_LlmMatcherConfig_generation_config_set(m_DLLPtr, value.ToDLL);
        }

        public string PromptTemplate
        {
            get => InworldInterop.inworld_LlmMatcherConfig_prompt_template_get(m_DLLPtr);
            set => InworldInterop.inworld_LlmMatcherConfig_prompt_template_set(m_DLLPtr, value);
        }

        public float EmbeddingSimilarityThreshold
        {
            get
            {
                IntPtr optStr = InworldInterop.inworld_LlmMatcherConfig_embedding_similarity_threshold_get(m_DLLPtr);
                if (InworldInterop.inworld_optional_float_has_value(optStr))
                    return InworldInterop.inworld_optional_float_getConst(optStr);
                return -1;
            }
            set
            {
                IntPtr optStr = InworldInterop.inworld_optional_float_new_rcf(value);
                InworldInterop.inworld_LlmMatcherConfig_embedding_similarity_threshold_set(m_DLLPtr, optStr);
            }
        }

        public int MaxEmbeddingMatches
        {
            get
            {
                IntPtr optStr = InworldInterop.inworld_LlmMatcherConfig_max_embedding_matches_for_llm_get(m_DLLPtr);
                if (InworldInterop.inworld_optional_int_has_value(optStr))
                    return InworldInterop.inworld_optional_int_getConst(optStr);
                return -1;
            }
            set
            {
                IntPtr optStr = InworldInterop.inworld_optional_int_new_rci(value);
                InworldInterop.inworld_LlmMatcherConfig_max_embedding_matches_for_llm_set(m_DLLPtr, optStr);
            }
        }
    }
}