using System;

namespace Inworld.Framework
{
    public class ListToolsData : InworldBaseData
    {
        public ListToolsData(InworldVector<ToolData> toolData)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_ListToolsData_new_std_vector_Sl_std_shared_ptr_Sl_inworld_graphs_ToolData_Sg__Sg_(toolData.ToDLL),
                InworldInterop.inworld_ListToolsData_delete);
        }
         
        public ListToolsData(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_ListToolsData_delete);
        }

        public ListToolsData(InworldBaseData parent)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_ListToolsData(parent.ToDLL),
                InworldInterop.inworld_ListToolsData_delete);
        }
        
        public override bool IsValid => InworldInterop.inworld_ListToolsData_is_valid(m_DLLPtr);

        public override string ToString() => InworldInterop.inworld_ListToolsData_ToString(m_DLLPtr);

        public InworldVector<ToolData> ToolData => new InworldVector<ToolData>(InworldInterop.inworld_ListToolsData_list_tools_swig(m_DLLPtr));
        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}