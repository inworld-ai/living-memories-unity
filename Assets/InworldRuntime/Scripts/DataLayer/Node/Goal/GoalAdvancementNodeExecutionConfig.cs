using System;
using Inworld.Framework.LLM;

namespace Inworld.Framework.Node
{
    public class GoalAdvancementNodeExecutionConfig : NodeExecutionConfig
    {
        public GoalAdvancementNodeExecutionConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_GoalAdvancementNodeExecutionConfig_new(),
                InworldInterop.inworld_GoalAdvancementNodeExecutionConfig_delete);
        }
        
        public GoalAdvancementNodeExecutionConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_GoalAdvancementNodeExecutionConfig_delete);
        }

        public override bool IsValid => InworldInterop.inworld_GoalAdvancementNodeExecutionConfig_is_valid(m_DLLPtr);

        public TextGenerationConfig TextGenerationConfig
        {
            get
            {
                IntPtr optStr = InworldInterop.inworld_GoalAdvancementNodeExecutionConfig_text_generation_config_get(m_DLLPtr);
                if (InworldInterop.inworld_optional_TextGenerationConfig_has_value(optStr))
                    return new TextGenerationConfig(InworldInterop.inworld_optional_TextGenerationConfig_getConst(optStr));
                return null;
            }
            set
            {
                IntPtr optStr = InworldInterop.inworld_optional_TextGenerationConfig_new_rcinworld_TextGenerationConfig(value.ToDLL);
                InworldInterop.inworld_GoalAdvancementNodeExecutionConfig_text_generation_config_set(m_DLLPtr, optStr);
            }
        }

        public string LLMComponentID
        {
            get => InworldInterop.inworld_GoalAdvancementNodeExecutionConfig_llm_component_id_get(m_DLLPtr);
            set => InworldInterop.inworld_GoalAdvancementNodeExecutionConfig_llm_component_id_set(m_DLLPtr, value);
        }

        public override bool NeedReportToClient
        {
            get => InworldInterop.inworld_GoalAdvancementNodeExecutionConfig_report_to_client_get(m_DLLPtr);
            set => InworldInterop.inworld_GoalAdvancementNodeExecutionConfig_report_to_client_set(m_DLLPtr, value);
        }

        public override InworldMap<string, string> Properties
        {
            get => new InworldMap<string, string>(
                InworldInterop.inworld_GoalAdvancementNodeExecutionConfig_properties_get(m_DLLPtr));
            set => InworldInterop.inworld_GoalAdvancementNodeExecutionConfig_properties_set(m_DLLPtr, value.ToDLL);
        }
    }
}