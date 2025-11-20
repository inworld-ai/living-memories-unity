using System;

namespace Inworld.Framework
{
    public class InworldIntent : InworldFrameworkDllClass
    {
        public InworldIntent()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_Intent_new(),
                InworldInterop.inworld_Intent_delete);
        }
        
        public InworldIntent(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_Intent_delete);
        }

        public string Name
        {
            get => InworldInterop.inworld_Intent_name_get(m_DLLPtr);
            set => InworldInterop.inworld_Intent_name_set(m_DLLPtr, value);
        }

        public InworldVector<string> Phrases
        {
            get => new InworldVector<string>(InworldInterop.inworld_Intent_phrases_get(m_DLLPtr));
            set => InworldInterop.inworld_Intent_phrases_set(m_DLLPtr, value.ToDLL);
        }
    }
}