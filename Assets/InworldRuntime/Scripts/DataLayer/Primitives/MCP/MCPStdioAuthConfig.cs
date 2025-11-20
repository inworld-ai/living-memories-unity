using System;

namespace Inworld.Framework
{
    public class MCPStdioAuthConfig : InworldConfig
    {
        public MCPStdioAuthConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_MCPStdioAuthConfig_new(),
                InworldInterop.inworld_MCPStdioAuthConfig_delete);
        }
        
        public MCPStdioAuthConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_MCPStdioAuthConfig_delete);
        }

        public void Set(string key, string value)
        {
            InworldInterop.inworld_MCPStdioAuthConfig_set(m_DLLPtr, key, value);
        }
    }
}