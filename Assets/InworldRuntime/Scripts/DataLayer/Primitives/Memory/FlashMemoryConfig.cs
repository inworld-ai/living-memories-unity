using System;
using Inworld.Framework.LLM;

namespace Inworld.Framework.Memory
{
    public class FlashMemoryConfig : InworldConfig
    {
        public FlashMemoryConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_FlashMemoryConfig_new(),
                InworldInterop.inworld_FlashMemoryConfig_delete);
        }
        
        public FlashMemoryConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_FlashMemoryConfig_delete);
        }

        public int DialogueCutoffSize
        {
            get => InworldInterop.inworld_FlashMemoryConfig_dialogue_cutoff_size_get(m_DLLPtr);
            set => InworldInterop.inworld_FlashMemoryConfig_dialogue_cutoff_size_set(m_DLLPtr, value);
        }

        public int NHistoryTurns
        {
            get => InworldInterop.inworld_FlashMemoryConfig_n_history_turns_get(m_DLLPtr);
            set => InworldInterop.inworld_FlashMemoryConfig_n_history_turns_set(m_DLLPtr, value);
        }

        public float MemoriesSimilarityThreshold
        {
            get => InworldInterop.inworld_FlashMemoryConfig_memories_similarity_threshold_get(m_DLLPtr);
            set => InworldInterop.inworld_FlashMemoryConfig_memories_similarity_threshold_set(m_DLLPtr, value);
        }
        
        public int MaxFlashMemory
        {
            get => InworldInterop.inworld_FlashMemoryConfig_max_flash_memory_get(m_DLLPtr);
            set => InworldInterop.inworld_FlashMemoryConfig_max_flash_memory_set(m_DLLPtr, value);
        }

        public int MaxTopicsPerMemory
        {
            get => InworldInterop.inworld_FlashMemoryConfig_max_topics_per_memory_get(m_DLLPtr);
            set => InworldInterop.inworld_FlashMemoryConfig_max_topics_per_memory_set(m_DLLPtr, value);
        }

        public TextGenerationConfig TextGenerationConfig
        {
            get => new TextGenerationConfig(
                InworldInterop.inworld_FlashMemoryConfig_text_generation_config_get(m_DLLPtr));
            set => InworldInterop.inworld_FlashMemoryConfig_text_generation_config_set(m_DLLPtr, value.ToDLL);
        }
    }
}