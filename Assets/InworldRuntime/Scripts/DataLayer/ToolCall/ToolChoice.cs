using System;

namespace Inworld.Framework
{
    public class ToolChoice : InworldFrameworkDllClass
    {
        public ToolChoice()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_ToolChoice_new(),
                InworldInterop.inworld_ToolChoice_delete);
        }
        
        public ToolChoice(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_ToolChoice_delete);
        }

        public bool IsStringChoice => InworldInterop.inworld_ToolChoice_IsStringChoice(m_DLLPtr);
        
        public bool IsFunctionChoice => InworldInterop.inworld_ToolChoice_IsFunctionChoice(m_DLLPtr);

        public string StringChoice
        {
            get => InworldInterop.inworld_ToolChoice_GetStringChoice(m_DLLPtr);
            set => InworldInterop.inworld_ToolChoice_SetStringChoice(m_DLLPtr, value);
        }

        public FunctionChoice FunctionChoice
        {
            get => new FunctionChoice(InworldInterop.inworld_ToolChoice_GetFunctionChoice(m_DLLPtr));
            set => InworldInterop.inworld_ToolChoice_SetFunctionChoice(m_DLLPtr, value.ToDLL);
        }
    }
}