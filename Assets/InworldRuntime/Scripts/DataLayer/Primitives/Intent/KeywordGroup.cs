using System;

namespace Inworld.Framework
{
    public class KeywordGroup : InworldFrameworkDllClass
    {
        public KeywordGroup()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_KeywordGroup_new(),
                InworldInterop.inworld_KeywordGroup_delete);
        }

        public KeywordGroup(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_KeywordGroup_delete);
        }

        public string Name
        {
            get => InworldInterop.inworld_KeywordGroup_name_get(m_DLLPtr);
            set => InworldInterop.inworld_KeywordGroup_name_set(m_DLLPtr, value);
        }

        public InworldVector<string> Keywords
        {
            get => new InworldVector<string>(InworldInterop.inworld_KeywordGroup_keywords_get(m_DLLPtr));
            set => InworldInterop.inworld_KeywordGroup_keywords_set(m_DLLPtr, value.ToDLL);
        }
    }
}