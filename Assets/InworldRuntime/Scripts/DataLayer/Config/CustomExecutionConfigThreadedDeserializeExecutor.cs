using System;
using System.Runtime.InteropServices;
using Inworld.Framework.Node;

namespace Inworld.Framework
{
    public class CustomExecutionConfigThreadedDeserializeExecutor : InworldExecutor
    {
        ProcessBaseDataIODelegateExecutionID m_Func;
        public CustomExecutionConfigThreadedDeserializeExecutor(ProcessBaseDataIODelegateExecutionID func)
        {
            m_Self = GCHandle.Alloc(this, GCHandleType.Pinned);
            m_Func = func;
            IntPtr funcPtr = Marshal.GetFunctionPointerForDelegate(func);
            m_DLLPtr = MemoryManager.Register(
                InworldInterop.inworld_CustomExecutionConfigThreadedDeserializeExecutor_new(GCHandle.ToIntPtr(m_Self), funcPtr),
                InworldInterop.inworld_CustomExecutionConfigThreadedDeserializeExecutor_delete);
        }

        public static string GetInput(int threadID) =>
            InworldInterop.inworld_CustomExecutionConfigThreadedDeserializeExecutor_GetInput(threadID);
        
        public static void SetOutput(int threadID, CustomExecutionConfigWrapper wrapper)
        {
            IntPtr statusWrapper = InworldInterop
                .inworld_StatusOr_CustomExecutionConfigWrapper_new_rcstd_shared_ptr_Sl_inworld_swig_helpers_CustomExecutionConfigWrapper_Sg_(wrapper.ToDLL);
            InworldInterop.inworld_CustomExecutionConfigThreadedDeserializeExecutor_SetOutput(threadID, statusWrapper);
        }
    }
}