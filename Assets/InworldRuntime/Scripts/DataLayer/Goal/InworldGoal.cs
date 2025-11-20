using System;

namespace Inworld.Framework
{
    public class InworldGoal : InworldFrameworkDllClass
    {
        public InworldGoal()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_Goal_new(), InworldInterop.inworld_Goal_delete);
        }
        
        public InworldGoal(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_Goal_delete);
        }

        public string Name
        {
            get => InworldInterop.inworld_Goal_name_get(m_DLLPtr);
            set => InworldInterop.inworld_Goal_name_set(m_DLLPtr, value);
        }

        public string Motivation
        {
            get
            {
                IntPtr optStr = InworldInterop.inworld_Goal_motivation_get(m_DLLPtr);
                if (InworldInterop.inworld_optional_string_has_value(optStr))
                    return InworldInterop.inworld_optional_string_getConst(optStr);
                return null;
            }
            set
            {
                IntPtr optStr = InworldInterop.inworld_optional_string_new_rcstd_string(value);
                InworldInterop.inworld_Goal_motivation_set(m_DLLPtr, optStr);
            }
        }

        public bool IsRepeatable
        {
            get => InworldInterop.inworld_Goal_repeatable_get(m_DLLPtr);
            set => InworldInterop.inworld_Goal_repeatable_set(m_DLLPtr, value);
        }

        public InworldCondition ActivationCondition
        {
            get
            {
                IntPtr optCondition = InworldInterop.inworld_Goal_activation_condition_get(m_DLLPtr);
                if (InworldInterop.inworld_optional_Condition_has_value(optCondition))
                    return new InworldCondition(InworldInterop.inworld_optional_Condition_getConst(optCondition));
                return null;
            }
            set
            {
                IntPtr optCondition = InworldInterop.inworld_optional_Condition_new_rcinworld_graphs_Condition(value.ToDLL);
                InworldInterop.inworld_Goal_activation_condition_set(m_DLLPtr, optCondition);
            }
        }
        
        public InworldCondition CompletionCondition
        {
            get
            {
                IntPtr optCondition = InworldInterop.inworld_Goal_completion_condition_get(m_DLLPtr);
                if (InworldInterop.inworld_optional_Condition_has_value(optCondition))
                    return new InworldCondition(InworldInterop.inworld_optional_Condition_getConst(optCondition));
                return null;
            }
            set
            {
                IntPtr optCondition = InworldInterop.inworld_optional_Condition_new_rcinworld_graphs_Condition(value.ToDLL);
                InworldInterop.inworld_Goal_completion_condition_set(m_DLLPtr, optCondition);
            }
        }
    }
}