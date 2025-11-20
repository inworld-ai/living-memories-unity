using System;

namespace Inworld.Framework
{
    public class ToolCallResult : InworldBaseData
    {
        public ToolCallResult(string toolCallID, string result)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_ToolCallResult_new(toolCallID, result),
                InworldInterop.inworld_ToolCallResult_delete);
        }
         
        public ToolCallResult(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_ToolCallResult_delete);
        }

        public ToolCallResult(InworldBaseData parent)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_ToolCallResult(parent.ToDLL),
                InworldInterop.inworld_ToolCallResult_delete);
        }
        
        public override bool IsValid => InworldInterop.inworld_ToolCallResult_is_valid(m_DLLPtr);

        public override string ToString() => InworldInterop.inworld_ToolCallResult_ToString(m_DLLPtr);

        public string ToolCallID => InworldInterop.inworld_ToolCallResult_tool_call_id(m_DLLPtr);
        
        public string Result => InworldInterop.inworld_ToolCallResult_result(m_DLLPtr);
        
        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}