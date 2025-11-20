using System;

namespace Inworld.Framework
{
    public class FunctionChoice : InworldFrameworkDllClass
    {
        public FunctionChoice()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_FunctionChoice_new(),
                InworldInterop.inworld_FunctionChoice_delete);
        }
        
        public FunctionChoice(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_FunctionChoice_delete);
        }

        public string Type
        {
            get => InworldInterop.inworld_FunctionChoice_type_get(m_DLLPtr);
            set => InworldInterop.inworld_FunctionChoice_type_set(m_DLLPtr, value);
        }
        
        public string Name
        {
            get => InworldInterop.inworld_FunctionChoice_name_get(m_DLLPtr);
            set => InworldInterop.inworld_FunctionChoice_name_set(m_DLLPtr, value);
        }
    }
}