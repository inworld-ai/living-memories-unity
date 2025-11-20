using System;

namespace Inworld.Framework.Memory
{
    public class MemoryUpdateConfig : InworldConfig
    {
        public MemoryUpdateConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_MemoryUpdateConfig_new(),
                InworldInterop.inworld_MemoryUpdateConfig_delete);
        }
        
        public MemoryUpdateConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_MemoryUpdateConfig_delete);
        }
        
        public RollingSummaryConfig RollingSummaryConfig
        {
            get
            {
                IntPtr optStr = InworldInterop.inworld_MemoryUpdateConfig_rolling_summary_config_get(m_DLLPtr);
                if (InworldInterop.inworld_optional_RollingSummaryConfig_has_value(optStr))
                    return new RollingSummaryConfig(InworldInterop.inworld_optional_RollingSummaryConfig_getConst(optStr));
                return null;
            }
            set
            {
                IntPtr optStr = InworldInterop.inworld_optional_RollingSummaryConfig_new_rcinworld_RollingSummaryConfig(value.ToDLL);
                InworldInterop.inworld_MemoryUpdateConfig_rolling_summary_config_set(m_DLLPtr, optStr);
            }
        }
        
        public FlashMemoryConfig FlashMemoryConfig
        {
            get
            {
                IntPtr optStr = InworldInterop.inworld_MemoryUpdateConfig_flash_memory_config_get(m_DLLPtr);
                if (InworldInterop.inworld_optional_FlashMemoryConfig_has_value(optStr))
                    return new FlashMemoryConfig(InworldInterop.inworld_optional_FlashMemoryConfig_getConst(optStr));
                return null;
            }
            set
            {
                IntPtr optStr = InworldInterop.inworld_optional_FlashMemoryConfig_new_rcinworld_FlashMemoryConfig(value.ToDLL);
                InworldInterop.inworld_MemoryUpdateConfig_flash_memory_config_set(m_DLLPtr, optStr);
            }
        }

        public LongTermMemoryConfig LongTermMemoryConfig
        {
            get
            {
                IntPtr optStr = InworldInterop.inworld_MemoryUpdateConfig_long_term_memory_config_get(m_DLLPtr);
                if (InworldInterop.inworld_optional_LongTermMemoryConfig_has_value(optStr))
                    return new LongTermMemoryConfig(InworldInterop.inworld_optional_LongTermMemoryConfig_getConst(optStr));
                return null;
            }
            set
            {
                IntPtr optStr = InworldInterop.inworld_optional_LongTermMemoryConfig_new_rcinworld_LongTermMemoryConfig(value.ToDLL);
                InworldInterop.inworld_MemoryUpdateConfig_long_term_memory_config_set(m_DLLPtr, optStr);
            }
        }
    }
}