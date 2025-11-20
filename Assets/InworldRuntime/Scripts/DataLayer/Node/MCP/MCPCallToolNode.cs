using System;
using Inworld.Framework.Node;

namespace Inworld.Framework.Node
{
    public class MCPCallToolNode : InworldNode
    {
        public MCPCallToolNode(string nodeName, MCPCallToolNodeExecutionConfig executionConfig)
        {
            IntPtr genResult = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_MCPCallToolNode_Create(nodeName, executionConfig.ToDLL),
                InworldInterop.inworld_StatusOr_SharedPtr_MCPCallToolNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_MCPCallToolNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_MCPCallToolNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_MCPCallToolNode_delete);
            if (genResult != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(genResult, InworldInterop.inworld_MCPCallToolNode_delete);
        }
        
        public MCPCallToolNode(IntPtr dllPtr)
        {
            m_DLLPtr = MemoryManager.Register(dllPtr, InworldInterop.inworld_MCPCallToolNode_delete);
        }

        public override string ID => InworldInterop.inworld_MCPCallToolNode_id(m_DLLPtr);

        public override bool IsValid => InworldInterop.inworld_MCPCallToolNode_is_valid(m_DLLPtr);

        public ListToolCallsResults Process(ProcessContext processContext, ListToolCallData listToolCallData)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_MCPCallToolNode_Process(m_DLLPtr, processContext.ToDLL, listToolCallData.ToDLL), 
                InworldInterop.inworld_StatusOr_SharedPtr_ListToolCallsResults_status,
                InworldInterop.inworld_StatusOr_SharedPtr_ListToolCallsResults_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_ListToolCallsResults_value,
                InworldInterop.inworld_StatusOr_SharedPtr_ListToolCallsResults_delete
            );
            return result != IntPtr.Zero ? new ListToolCallsResults(result) : null;
        }
    }
}