using System;

namespace Inworld.Framework
{
    public class MCPAuthConfig : InworldConfig
    {
        public MCPAuthConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_MCPAuthConfig_new(),
                InworldInterop.inworld_MCPAuthConfig_delete);
        }
        
        public MCPAuthConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_MCPAuthConfig_delete);
        }

        public MCPHttpAuthConfig HttpAuthConfig
        {
            set => InworldInterop.inworld_MCPAuthConfig_set_http_auth_config(m_DLLPtr, value.ToDLL);
        }

        public MCPStdioAuthConfig StdioAuthConfig
        {
            set => InworldInterop.inworld_MCPAuthConfig_set_stdio_auth_config(m_DLLPtr, value.ToDLL);
        }
    }
}