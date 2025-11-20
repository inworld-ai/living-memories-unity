using System;

namespace Inworld.Framework.TextEmbedder
{
    public class EmbeddingMatcherConfig : InworldConfig
    {
        public EmbeddingMatcherConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_EmbeddingMatcherConfig_new(),
                InworldInterop.inworld_EmbeddingMatcherConfig_delete);
        }
        
        public EmbeddingMatcherConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_EmbeddingMatcherConfig_delete);
        }
        
        public float SimilarityThreshold
        {
            get => InworldInterop.inworld_EmbeddingMatcherConfig_similarity_threshold_get(m_DLLPtr);
            set => InworldInterop.inworld_EmbeddingMatcherConfig_similarity_threshold_set(m_DLLPtr, value);
        }
    }
}