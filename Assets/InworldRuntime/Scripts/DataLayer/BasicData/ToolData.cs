using System;

namespace Inworld.Framework
{
    public class ToolData : InworldBaseData
    {
        public ToolData(InworldTool tool)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_ToolData_new(tool.ToDLL),
                InworldInterop.inworld_ToolData_delete);
        }

        public ToolData(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_ToolData_delete);
        }

        public ToolData(InworldBaseData parent)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_ToolData(parent.ToDLL),
                InworldInterop.inworld_ToolData_delete);
        }
        
        public override bool IsValid => InworldInterop.inworld_ToolData_is_valid(m_DLLPtr);

        public override string ToString() => InworldInterop.inworld_ToolData_ToString(m_DLLPtr);

        public InworldTool Tool => new InworldTool(InworldInterop.inworld_ToolData_tool(m_DLLPtr));

        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}