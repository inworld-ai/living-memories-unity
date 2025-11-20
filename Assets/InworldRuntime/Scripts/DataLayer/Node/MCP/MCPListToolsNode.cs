using System;

namespace Inworld.Framework.Node
{
    public class MCPListToolsNode : InworldNode
    {
        public MCPListToolsNode(string nodeName, MCPListToolsNodeExecutionConfig executionConfig)
        {
            IntPtr genResult = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_MCPListToolsNode_Create(nodeName, executionConfig.ToDLL),
                InworldInterop.inworld_StatusOr_SharedPtr_MCPListToolsNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_MCPListToolsNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_MCPListToolsNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_MCPListToolsNode_delete);
            if (genResult != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(genResult, InworldInterop.inworld_MCPListToolsNode_delete);
        }
        
        public MCPListToolsNode(IntPtr dllPtr)
        {
            m_DLLPtr = MemoryManager.Register(dllPtr, InworldInterop.inworld_MCPListToolsNode_delete);
        }

        public override string ID => InworldInterop.inworld_MCPListToolsNode_id(m_DLLPtr);

        public override bool IsValid => InworldInterop.inworld_MCPListToolsNode_is_valid(m_DLLPtr);

        public override InworldBaseData Process(ProcessContext processContext, InworldVector<InworldBaseData> baseDatas)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_MCPListToolsNode_Process(m_DLLPtr, processContext.ToDLL, baseDatas.ToDLL), 
                InworldInterop.inworld_StatusOr_BaseData_status,
                InworldInterop.inworld_StatusOr_BaseData_ok,
                InworldInterop.inworld_StatusOr_BaseData_value,
                InworldInterop.inworld_StatusOr_BaseData_delete
            );
            return result != IntPtr.Zero ? new InworldBaseData(result) : null;
        }
    }
}