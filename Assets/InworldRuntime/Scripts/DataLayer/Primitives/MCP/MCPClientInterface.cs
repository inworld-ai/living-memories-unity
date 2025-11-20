using System;

namespace Inworld.Framework
{
    public class MCPClientInterface : InworldInterface
    {
        public MCPClientInterface(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_MCPClientInterface_delete);
        }

        public override bool IsValid => InworldInterop.inworld_MCPClientInterface_is_valid(m_DLLPtr);
        
        public string Call(ToolCall toolCall)
        {
            return InworldFrameworkUtil.Execute(
                InworldInterop.inworld_MCPClientInterface_Call(m_DLLPtr, toolCall.ToDLL),
                InworldInterop.inworld_StatusOr_string_status,
                InworldInterop.inworld_StatusOr_string_ok,
                InworldInterop.inworld_StatusOr_string_value,
                InworldInterop.inworld_StatusOr_string_delete
            );
        }
        
        public InworldVector<InworldTool> ListTools
        {
            get
            {
                IntPtr result = InworldFrameworkUtil.Execute(
                    InworldInterop.inworld_MCPClientInterface_ListTools(m_DLLPtr),
                    InworldInterop.inworld_StatusOr_Tools_status,
                    InworldInterop.inworld_StatusOr_Tools_ok,
                    InworldInterop.inworld_StatusOr_Tools_value,
                    InworldInterop.inworld_StatusOr_Tools_delete
                );
                return result == IntPtr.Zero ? null : new InworldVector<InworldTool>(result); 
            }
        }
    }
}