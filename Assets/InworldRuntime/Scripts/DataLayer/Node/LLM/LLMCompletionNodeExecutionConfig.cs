using System;
using Inworld.Framework.LLM;

namespace Inworld.Framework.Node
{
    public class LLMCompletionNodeExecutionConfig : NodeExecutionConfig
    {
        public LLMCompletionNodeExecutionConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LLMCompletionNodeExecutionConfig_new(),
                InworldInterop.inworld_LLMCompletionNodeExecutionConfig_delete);
        }
        
        public LLMCompletionNodeExecutionConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_LLMCompletionNodeExecutionConfig_delete);
        }

        public string LLMComponentID
        {
            get => InworldInterop.inworld_LLMCompletionNodeExecutionConfig_llm_component_id_get(m_DLLPtr);
            set => InworldInterop.inworld_LLMCompletionNodeExecutionConfig_llm_component_id_set(m_DLLPtr, value);
        }
        
        public TextGenerationConfig TextGenerationConfig
        {
            get
            {
                IntPtr optStr = InworldInterop.inworld_LLMCompletionNodeExecutionConfig_text_generation_config_get(m_DLLPtr);
                if (InworldInterop.inworld_optional_TextGenerationConfig_has_value(optStr))
                    return new TextGenerationConfig(InworldInterop.inworld_optional_TextGenerationConfig_getConst(optStr));
                return null;
            }
            set
            {
                IntPtr optStr = InworldInterop.inworld_optional_TextGenerationConfig_new_rcinworld_TextGenerationConfig(value.ToDLL);
                InworldInterop.inworld_LLMCompletionNodeExecutionConfig_text_generation_config_set(m_DLLPtr, optStr);
            }
        }

        public bool IsStream
        {
            get => InworldInterop.inworld_LLMCompletionNodeExecutionConfig_stream_get(m_DLLPtr);
            set => InworldInterop.inworld_LLMCompletionNodeExecutionConfig_stream_set(m_DLLPtr, value);
        }

        public override bool IsValid => InworldInterop.inworld_LLMCompletionNodeExecutionConfig_is_valid(m_DLLPtr);

        public override bool NeedReportToClient
        {
            get => InworldInterop.inworld_LLMCompletionNodeExecutionConfig_report_to_client_get(m_DLLPtr);
            set => InworldInterop.inworld_LLMCompletionNodeExecutionConfig_report_to_client_set(m_DLLPtr, value);
        }

        public override InworldMap<string, string> Properties
        {
            get => new InworldMap<string, string>(InworldInterop.inworld_LLMCompletionNodeExecutionConfig_properties_get(m_DLLPtr));
            set => InworldInterop.inworld_LLMCompletionNodeExecutionConfig_properties_set(m_DLLPtr, value.ToDLL);
        }
    }
}