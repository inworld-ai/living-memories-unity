using System;

namespace Inworld.Framework
{
    public class CancellationContext : InworldContext
    {
        public CancellationContext(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_CancellationContext_delete);
        }

        public override bool IsValid => InworldInterop.inworld_CancellationContext_is_valid(m_DLLPtr);
        
        public bool IsCancelled => InworldInterop.inworld_CancellationContext_IsCancelled(m_DLLPtr);
    }
}