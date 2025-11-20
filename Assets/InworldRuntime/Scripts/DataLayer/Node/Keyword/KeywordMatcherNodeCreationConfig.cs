using System;

namespace Inworld.Framework.Node
{
    public class KeywordMatcherNodeCreationConfig : NodeCreationConfig
    {
        public KeywordMatcherNodeCreationConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_KeywordMatcherNodeCreationConfig_new(),
                InworldInterop.inworld_KeywordMatcherNodeCreationConfig_delete);
        }
        
        public KeywordMatcherNodeCreationConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_KeywordMatcherNodeCreationConfig_delete);
        }

        public InworldVector<KeywordGroup> KeywordGroups
        {
            get => new InworldVector<KeywordGroup>(InworldInterop.inworld_KeywordMatcherNodeCreationConfig_keyword_groups_get(m_DLLPtr)); 
            set => InworldInterop.inworld_KeywordMatcherNodeCreationConfig_keyword_groups_set(m_DLLPtr, value.ToDLL);
        } 
    }
}