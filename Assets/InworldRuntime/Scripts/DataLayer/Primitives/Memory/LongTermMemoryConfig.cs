using System;
using Inworld.Framework.LLM;

namespace Inworld.Framework.Memory
{
    public class LongTermMemoryConfig : InworldConfig
    {
        public LongTermMemoryConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LongTermMemoryConfig_new(),
                InworldInterop.inworld_LongTermMemoryConfig_delete);
        }
        
        public LongTermMemoryConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_LongTermMemoryConfig_delete);
        }

        public int MaxTopicSummaryLenToAppend
        {
            get => InworldInterop.inworld_LongTermMemoryConfig_max_topic_summary_len_to_append_get(m_DLLPtr);
            set => InworldInterop.inworld_LongTermMemoryConfig_max_topic_summary_len_to_append_set(m_DLLPtr, value);
        }

        public int MaxNumberOfFlashMemory
        {
            get => InworldInterop.inworld_LongTermMemoryConfig_max_number_of_flash_memory_get(m_DLLPtr);
            set => InworldInterop.inworld_LongTermMemoryConfig_max_number_of_flash_memory_set(m_DLLPtr, value);
        }

        public int MaxNumberOfTopics
        {
            get => InworldInterop.inworld_LongTermMemoryConfig_max_number_of_topics_get(m_DLLPtr);
            set => InworldInterop.inworld_LongTermMemoryConfig_max_number_of_topics_set(m_DLLPtr, value);
        }
        
        public TextGenerationConfig TextGenerationConfig
        {
            get => new TextGenerationConfig(
                InworldInterop.inworld_LongTermMemoryConfig_text_generation_config_get(m_DLLPtr));
            set => InworldInterop.inworld_LongTermMemoryConfig_text_generation_config_set(m_DLLPtr, value.ToDLL);
        }
    }
}