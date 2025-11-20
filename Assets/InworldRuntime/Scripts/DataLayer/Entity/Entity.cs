using System;

namespace Inworld.Framework
{
    public class Entity : InworldFrameworkDllClass
    {
        public Entity()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_Entity_new(),
                InworldInterop.inworld_Entity_delete);
        }
        
        public Entity(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_Entity_delete);
        }

        public string Name
        {
            get => InworldInterop.inworld_Entity_name_get(m_DLLPtr);
            set => InworldInterop.inworld_Entity_name_set(m_DLLPtr, value);
        }

        public InworldVector<DictionaryRule> Rules
        {
            get => new InworldVector<DictionaryRule>(InworldInterop.inworld_Entity_rules_get(m_DLLPtr));
            set => InworldInterop.inworld_Entity_rules_set(m_DLLPtr, value.ToDLL);
        }
    }
}