using System;

namespace Inworld.Framework
{
    public class ListToolCallData : InworldBaseData
    {
        public ListToolCallData(InworldVector<ToolCallData> toolCallData)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_ListToolCallData_new_std_vector_Sl_std_shared_ptr_Sl_inworld_graphs_ToolCallData_Sg__Sg_(toolCallData.ToDLL),
                InworldInterop.inworld_ListToolCallData_delete);
        }
         
        public ListToolCallData(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_ListToolCallData_delete);
        }

        public ListToolCallData(InworldBaseData parent)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_ListToolCallData(parent.ToDLL),
                InworldInterop.inworld_ListToolCallData_delete);
        }
        
        public override bool IsValid => InworldInterop.inworld_ListToolCallData_is_valid(m_DLLPtr);

        public override string ToString() => InworldInterop.inworld_ListToolCallData_ToString(m_DLLPtr);

        public InworldVector<ToolCallData> ToolCallData => new InworldVector<ToolCallData>(InworldInterop.inworld_ListToolCallData_tool_calls_swig(m_DLLPtr));
        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}