using System;

namespace Inworld.Framework.Node
{
    public class LLMChatRequestBuilderNodeExecutionConfig : NodeExecutionConfig
    {
        public LLMChatRequestBuilderNodeExecutionConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LLMChatRequestBuilderNodeExecutionConfig_new(),
                InworldInterop.inworld_LLMChatRequestBuilderNodeExecutionConfig_delete);
        }
        
        public LLMChatRequestBuilderNodeExecutionConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_LLMChatRequestBuilderNodeExecutionConfig_delete);
        }

        public InworldVector<ChatMessage> Messages
        {
            get => new InworldVector<ChatMessage>(
                InworldInterop.inworld_LLMChatRequestBuilderNodeExecutionConfig_messages_get(m_DLLPtr));
            set => InworldInterop.inworld_LLMChatRequestBuilderNodeExecutionConfig_messages_set(m_DLLPtr, value.ToDLL);
        }

        public InworldVector<InworldTool> Tools
        {
            get => new InworldVector<InworldTool>(
                InworldInterop.inworld_LLMChatRequestBuilderNodeExecutionConfig_tools_get(m_DLLPtr));
            set => InworldInterop.inworld_LLMChatRequestBuilderNodeExecutionConfig_tools_set(m_DLLPtr, value.ToDLL);
        }

        public ToolChoice ToolChoice
        {
            get
            {
                IntPtr optStr = InworldInterop.inworld_LLMChatRequestBuilderNodeExecutionConfig_tool_choice_get(m_DLLPtr);
                if (InworldInterop.inworld_OptionalToolChoice_has_value(optStr))
                    return new ToolChoice(InworldInterop.inworld_OptionalToolChoice_getConst(optStr));
                return null;
            }
            set
            {
                IntPtr optStr = InworldInterop.inworld_OptionalToolChoice_new_rcinworld_ToolChoice(value.ToDLL);
                InworldInterop.inworld_LLMChatRequestBuilderNodeExecutionConfig_tool_choice_set(m_DLLPtr, optStr);
            }
        }

        public ResponseFormat ResponseFormat
        {
            get => (ResponseFormat)InworldInterop.inworld_LLMChatRequestBuilderNodeExecutionConfig_response_format_get(m_DLLPtr);
            set => InworldInterop.inworld_LLMChatRequestBuilderNodeExecutionConfig_response_format_set(m_DLLPtr, Convert.ToInt32(value));
        }

        public override bool IsValid =>
            InworldInterop.inworld_LLMChatRequestBuilderNodeExecutionConfig_is_valid(m_DLLPtr);

        public override bool NeedReportToClient
        {
            get => InworldInterop.inworld_LLMChatRequestBuilderNodeExecutionConfig_report_to_client_get(m_DLLPtr);
            set => InworldInterop.inworld_LLMChatRequestBuilderNodeExecutionConfig_report_to_client_set(m_DLLPtr, value);
        }

        public override InworldMap<string, string> Properties
        {
            get => new InworldMap<string, string>(InworldInterop.inworld_LLMChatRequestBuilderNodeExecutionConfig_properties_get(m_DLLPtr));
            set => InworldInterop.inworld_LLMChatRequestBuilderNodeExecutionConfig_properties_set(m_DLLPtr, value.ToDLL);
        }
    }
}