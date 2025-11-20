using System;

namespace Inworld.Framework.Graph
{
    public class UserContext : InworldFrameworkDllClass
    {
        public UserContext()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_UserContext_new(),
                InworldInterop.inworld_UserContext_delete);
        }
        
        public UserContext(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_UserContext_delete);
        }

        public InworldBTreeMap<string, string> Attributes
        {
            get => new InworldBTreeMap<string, string>(InworldInterop.inworld_UserContext_attributes_get(m_DLLPtr));
            set => InworldInterop.inworld_UserContext_attributes_set(m_DLLPtr, value.ToDLL);
        }

        public string TargetingKey
        {
            get => InworldInterop.inworld_UserContext_targeting_key_get(m_DLLPtr);
            set => InworldInterop.inworld_UserContext_targeting_key_set(m_DLLPtr, value);
        }
    }
}