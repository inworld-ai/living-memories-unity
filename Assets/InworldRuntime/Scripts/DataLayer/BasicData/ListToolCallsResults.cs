using System;

namespace Inworld.Framework
{
    public class ListToolCallsResults : InworldBaseData
    {
        public ListToolCallsResults(InworldVector<ToolCallResult> toolCallResults)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_ListToolCallsResults_new_std_vector_Sl_std_shared_ptr_Sl_inworld_graphs_ToolCallResult_Sg__Sg_(toolCallResults.ToDLL),
                InworldInterop.inworld_ListToolCallsResults_delete);
        }
         
        public ListToolCallsResults(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_ListToolCallsResults_delete);
        }

        public ListToolCallsResults(InworldBaseData parent)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_ListToolCallsResults(parent.ToDLL),
                InworldInterop.inworld_ListToolCallsResults_delete);
        }
        
        public override bool IsValid => InworldInterop.inworld_ListToolCallsResults_is_valid(m_DLLPtr);

        public override string ToString() => InworldInterop.inworld_ListToolCallsResults_ToString(m_DLLPtr);

        public InworldVector<ToolCallResult> ToolCallResults => new InworldVector<ToolCallResult>(InworldInterop.inworld_ListToolCallsResults_tool_call_results_swig(m_DLLPtr));
        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}