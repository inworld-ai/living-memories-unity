using System;

namespace Inworld.Framework.Node
{
    public class ConfigWrapperDestructor : InworldFrameworkDllClass
    {
        public ConfigWrapperDestructor(IntPtr context, IntPtr callbackPtr)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_ConfigWrapperDestructor_new(context, callbackPtr),
                InworldInterop.inworld_ConfigWrapperDestructor_delete);
        }
    }
}