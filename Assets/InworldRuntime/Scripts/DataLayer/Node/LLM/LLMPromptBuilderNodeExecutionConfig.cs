using System;

namespace Inworld.Framework.Node
{
    public class LLMPromptBuilderNodeExecutionConfig : NodeExecutionConfig
    {
        public LLMPromptBuilderNodeExecutionConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LLMPromptBuilderNodeExecutionConfig_new(),
                InworldInterop.inworld_LLMPromptBuilderNodeExecutionConfig_delete);
        }
        
        public LLMPromptBuilderNodeExecutionConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_LLMPromptBuilderNodeExecutionConfig_delete);
        }

        public string PromptTemplate
        {
            get => InworldInterop.inworld_LLMPromptBuilderNodeExecutionConfig_prompt_template_get(m_DLLPtr);
            set => InworldInterop.inworld_LLMPromptBuilderNodeExecutionConfig_prompt_template_set(m_DLLPtr, value);
        }

        public override bool IsValid => InworldInterop.inworld_LLMPromptBuilderNodeExecutionConfig_is_valid(m_DLLPtr);

        public override bool NeedReportToClient
        {
            get => InworldInterop.inworld_LLMPromptBuilderNodeExecutionConfig_report_to_client_get(m_DLLPtr);
            set => InworldInterop.inworld_LLMPromptBuilderNodeExecutionConfig_report_to_client_set(m_DLLPtr, value);
        }

        public override InworldMap<string, string> Properties
        {
            get => new InworldMap<string, string>(InworldInterop.inworld_LLMPromptBuilderNodeExecutionConfig_properties_get(m_DLLPtr));
            set => InworldInterop.inworld_LLMPromptBuilderNodeExecutionConfig_properties_set(m_DLLPtr, value.ToDLL);
        }
    }
}