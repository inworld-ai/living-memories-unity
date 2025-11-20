using System;

namespace Inworld.Framework
{
    public class InworldCondition : InworldFrameworkDllClass
    {
        public InworldCondition()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_Condition_new(),
                InworldInterop.inworld_Condition_delete);
        }
        
        public InworldCondition(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_Condition_delete);
        }

        public InworldVector<string> Intents
        {
            get => new InworldVector<string>(InworldInterop.inworld_Condition_intents_get(m_DLLPtr));
            set => InworldInterop.inworld_Condition_intents_set(m_DLLPtr, value.ToDLL);
        }

        public string Detect
        {
            get
            {
                IntPtr optStr = InworldInterop.inworld_Condition_detect_get(m_DLLPtr);
                if (InworldInterop.inworld_optional_string_has_value(optStr))
                    return InworldInterop.inworld_optional_string_getConst(optStr);
                return null;
            }
            set
            {
                IntPtr optStr = InworldInterop.inworld_optional_string_new_rcstd_string(value);
                InworldInterop.inworld_Condition_detect_set(m_DLLPtr, optStr);
            }
        }

        public InworldVector<string> RequiredGoals
        {
            get => new InworldVector<string>(InworldInterop.inworld_Condition_required_goals_get(m_DLLPtr));
            set => InworldInterop.inworld_Condition_required_goals_set(m_DLLPtr, value.ToDLL);
        }
    }
}