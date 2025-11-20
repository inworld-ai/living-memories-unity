using System;

namespace Inworld.Framework.Node
{
    public class CustomNodeDestructor : InworldFrameworkDllClass
    {
        public CustomNodeDestructor(IntPtr context, IntPtr callbackPtr)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_CustomNodeDestructor_new(context, callbackPtr),
                InworldInterop.inworld_CustomNodeDestructor_delete);
        }
    }
}