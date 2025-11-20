using System;
using Inworld.Framework.Node;

namespace Inworld.Framework.Node
{
    public class MCPCallToolNodeExecutionConfig : NodeExecutionConfig
    {
        public MCPCallToolNodeExecutionConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_MCPCallToolNodeExecutionConfig_new(),
                InworldInterop.inworld_MCPCallToolNodeExecutionConfig_delete);
        }
        
        public MCPCallToolNodeExecutionConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_MCPCallToolNodeExecutionConfig_delete);
        }

        public string MCPComponentID
        {
            get => InworldInterop.inworld_MCPCallToolNodeExecutionConfig_mcp_component_id_get(m_DLLPtr);
            set => InworldInterop.inworld_MCPCallToolNodeExecutionConfig_mcp_component_id_set(m_DLLPtr, value);
        }

        public override bool IsValid => InworldInterop.inworld_MCPCallToolNodeExecutionConfig_is_valid(m_DLLPtr);

        public override bool NeedReportToClient
        {
            get => InworldInterop.inworld_MCPCallToolNodeExecutionConfig_report_to_client_get(m_DLLPtr);
            set => InworldInterop.inworld_MCPCallToolNodeExecutionConfig_report_to_client_set(m_DLLPtr, value);
        }

        public override InworldMap<string, string> Properties
        {
            get => new InworldMap<string, string>(InworldInterop.inworld_MCPCallToolNodeExecutionConfig_properties_get(m_DLLPtr));
            set => InworldInterop.inworld_MCPCallToolNodeExecutionConfig_properties_set(m_DLLPtr, value.ToDLL);
        }
    }
}