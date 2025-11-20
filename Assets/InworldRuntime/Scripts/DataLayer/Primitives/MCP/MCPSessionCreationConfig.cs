using System;

namespace Inworld.Framework
{
    public class MCPSessionCreationConfig : InworldConfig
    {
        public MCPSessionCreationConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_MCPSessionCreationConfig_new(),
                InworldInterop.inworld_MCPSessionCreationConfig_delete);
        }
        
        public MCPSessionCreationConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_MCPSessionCreationConfig_delete);
        }

        public string Transport
        {
            get => InworldInterop.inworld_MCPSessionCreationConfig_transport_get(m_DLLPtr);
            set => InworldInterop.inworld_MCPSessionCreationConfig_transport_set(m_DLLPtr, value);
        }

        public string Endpoint
        {
            get => InworldInterop.inworld_MCPSessionCreationConfig_endpoint_get(m_DLLPtr);
            set => InworldInterop.inworld_MCPSessionCreationConfig_endpoint_set(m_DLLPtr, value);
        }

        public MCPAuthConfig AuthConfig
        {
            get => new MCPAuthConfig(InworldInterop.inworld_MCPSessionCreationConfig_auth_config_get(m_DLLPtr));
            set => InworldInterop.inworld_MCPSessionCreationConfig_auth_config_set(m_DLLPtr, value.ToDLL);
        }
    }
}