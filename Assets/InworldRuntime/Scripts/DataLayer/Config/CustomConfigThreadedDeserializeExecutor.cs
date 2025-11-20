using System;
using System.Runtime.InteropServices;
using Inworld.Framework.Node;

namespace Inworld.Framework
{
    public class CustomConfigThreadedDeserializeExecutor : InworldExecutor
    {
        ProcessBaseDataIODelegateExecutionID m_Func;
        public CustomConfigThreadedDeserializeExecutor(ProcessBaseDataIODelegateExecutionID func)
        {
            m_Self = GCHandle.Alloc(this, GCHandleType.Pinned);
            m_Func = func;
            IntPtr funcPtr = Marshal.GetFunctionPointerForDelegate(func);
            m_DLLPtr = MemoryManager.Register(
                InworldInterop.inworld_CustomConfigThreadedDeserializeExecutor_new(GCHandle.ToIntPtr(m_Self), funcPtr),
                InworldInterop.inworld_CustomConfigThreadedDeserializeExecutor_delete);
        }

        public static string GetInput(int threadID) =>
            InworldInterop.inworld_CustomConfigThreadedDeserializeExecutor_GetInput(threadID);
        
        public static void SetOutput(int threadID, CustomConfigWrapper wrapper)
        {
            IntPtr statusWrapper = InworldInterop
                .inworld_StatusOr_CustomConfigWrapper_new_rcstd_shared_ptr_Sl_inworld_swig_helpers_CustomConfigWrapper_Sg_(wrapper.ToDLL);
            InworldInterop.inworld_CustomConfigThreadedDeserializeExecutor_SetOutput(threadID, statusWrapper);
        }
    }
}