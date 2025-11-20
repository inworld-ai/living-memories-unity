using System;

namespace Inworld.Framework
{
    public class MCPClientCreationConfig : InworldConfig
    {
        public MCPClientCreationConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_MCPClientCreationConfig_new(),
                InworldInterop.inworld_MCPClientCreationConfig_delete);
        }
        
        public MCPClientCreationConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_MCPClientCreationConfig_delete);
        }

        public MCPSessionCreationConfig SessionConfig
        {
            get => new MCPSessionCreationConfig(
                InworldInterop.inworld_MCPClientCreationConfig_session_config_get(m_DLLPtr));
            set => InworldInterop.inworld_MCPClientCreationConfig_session_config_set(m_DLLPtr, value.ToDLL);
        }
    }
}