using System;

namespace Inworld.Framework
{
    public class EntityMatch : InworldFrameworkDllClass
    {
        public EntityMatch()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_EntityMatch_new(),
                InworldInterop.inworld_EntityMatch_delete);
        }
        
        public EntityMatch(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_EntityMatch_delete);
        }

        public string EntityName
        {
            get => InworldInterop.inworld_EntityMatch_entity_name_get(m_DLLPtr);
            set => InworldInterop.inworld_EntityMatch_entity_name_set(m_DLLPtr, value);
        }

        public string RuleName
        {
            get => InworldInterop.inworld_EntityMatch_rule_name_get(m_DLLPtr);
            set => InworldInterop.inworld_EntityMatch_rule_name_set(m_DLLPtr, value);
        }

        public string Text
        {
            get => InworldInterop.inworld_EntityMatch_text_get(m_DLLPtr);
            set => InworldInterop.inworld_EntityMatch_text_set(m_DLLPtr, value);
        }
    }
}