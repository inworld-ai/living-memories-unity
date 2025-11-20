using System;

namespace Inworld.Framework
{
    public class MCPHttpAuthConfig : InworldConfig
    {
        public MCPHttpAuthConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_MCPHttpAuthConfig_new(),
                InworldInterop.inworld_MCPHttpAuthConfig_delete);
        }
        
        public MCPHttpAuthConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_MCPHttpAuthConfig_delete);
        }

        public string APIKey
        {
            get => InworldInterop.inworld_MCPHttpAuthConfig_api_key_get(m_DLLPtr);
            set => InworldInterop.inworld_MCPHttpAuthConfig_api_key_set(m_DLLPtr, value);
        }
    }
}