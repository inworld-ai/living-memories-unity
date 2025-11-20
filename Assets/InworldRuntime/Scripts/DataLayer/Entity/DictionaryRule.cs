using System;

namespace Inworld.Framework
{
    public class DictionaryRule : InworldFrameworkDllClass
    {
        public DictionaryRule()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_DictionaryRule_new(),
                InworldInterop.inworld_DictionaryRule_delete);
        }
        
        public DictionaryRule(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_DictionaryRule_delete);
        }

        public string Name
        {
            get => InworldInterop.inworld_DictionaryRule_name_get(m_DLLPtr);
            set => InworldInterop.inworld_DictionaryRule_name_set(m_DLLPtr, value);
        }

        public string DisplayName
        {
            get => InworldInterop.inworld_DictionaryRule_display_name_get(m_DLLPtr);
            set => InworldInterop.inworld_DictionaryRule_display_name_set(m_DLLPtr, value);
        }

        public InworldVector<string> Synonyms
        {
            get => new InworldVector<string>(InworldInterop.inworld_DictionaryRule_synonyms_get(m_DLLPtr));
            set => InworldInterop.inworld_DictionaryRule_synonyms_set(m_DLLPtr, value.ToDLL);
        }
    }
}