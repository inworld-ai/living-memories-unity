using System;
using Inworld.Framework.Node;

namespace Inworld.Framework.Node
{
    public class MCPListToolsNodeExecutionConfig : NodeExecutionConfig
    {
        public MCPListToolsNodeExecutionConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_MCPListToolsNodeExecutionConfig_new(),
                InworldInterop.inworld_MCPListToolsNodeExecutionConfig_delete);
        }
        
        public MCPListToolsNodeExecutionConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_MCPListToolsNodeExecutionConfig_delete);
        }
        
        public string MCPComponentID
        {
            get => InworldInterop.inworld_MCPListToolsNodeExecutionConfig_mcp_component_id_get(m_DLLPtr);
            set => InworldInterop.inworld_MCPListToolsNodeExecutionConfig_mcp_component_id_set(m_DLLPtr, value);
        }

        public override bool IsValid => InworldInterop.inworld_MCPListToolsNodeExecutionConfig_is_valid(m_DLLPtr);

        public override bool NeedReportToClient
        {
            get => InworldInterop.inworld_MCPListToolsNodeExecutionConfig_report_to_client_get(m_DLLPtr);
            set => InworldInterop.inworld_MCPListToolsNodeExecutionConfig_report_to_client_set(m_DLLPtr, value);
        }

        public override InworldMap<string, string> Properties
        {
            get => new InworldMap<string, string>(InworldInterop.inworld_MCPListToolsNodeExecutionConfig_properties_get(m_DLLPtr));
            set => InworldInterop.inworld_MCPListToolsNodeExecutionConfig_properties_set(m_DLLPtr, value.ToDLL);
        }
    }
}