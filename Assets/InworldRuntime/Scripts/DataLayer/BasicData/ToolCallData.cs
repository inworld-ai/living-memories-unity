using System;

namespace Inworld.Framework
{
    public class ToolCallData : InworldBaseData
    {
        public ToolCallData(ToolCall toolCall)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_ToolCallData_new(toolCall.ToDLL),
                InworldInterop.inworld_ToolCallData_delete);
        }
         
        public ToolCallData(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_ToolCallData_delete);
        }

        public ToolCallData(InworldBaseData parent)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_ToolCallData(parent.ToDLL),
                InworldInterop.inworld_ToolCallData_delete);
        }
        
        public override bool IsValid => InworldInterop.inworld_ToolCallData_is_valid(m_DLLPtr);

        public override string ToString() => InworldInterop.inworld_ToolCallData_ToString(m_DLLPtr);

        public ToolCall ToolCall => new ToolCall(InworldInterop.inworld_ToolCallData_tool_call(m_DLLPtr));
        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}