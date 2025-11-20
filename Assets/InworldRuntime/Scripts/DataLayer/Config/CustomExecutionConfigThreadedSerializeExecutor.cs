using System;
using System.Runtime.InteropServices;

namespace Inworld.Framework
{
    public class CustomExecutionConfigThreadedSerializeExecutor : InworldExecutor
    {
        ProcessBaseDataIODelegateExecutionID m_Func;
        public CustomExecutionConfigThreadedSerializeExecutor(ProcessBaseDataIODelegateExecutionID func)
        {
            m_Self = GCHandle.Alloc(this, GCHandleType.Pinned);
            m_Func = func;
            IntPtr funcPtr = Marshal.GetFunctionPointerForDelegate(func);
            m_DLLPtr = MemoryManager.Register(
                InworldInterop.inworld_CustomExecutionConfigThreadedSerializeExecutor_new(GCHandle.ToIntPtr(m_Self), funcPtr),
                InworldInterop.inworld_CustomExecutionConfigThreadedSerializeExecutor_delete);
        }

        public static CustomExecutionConfigWrapper GetInput(int threadID) =>
            new CustomExecutionConfigWrapper(InworldInterop.inworld_CustomExecutionConfigThreadedSerializeExecutor_GetInput(threadID));
        
        public static void SetOutput(int threadID, string json)
        {
            InworldInterop.inworld_CustomExecutionConfigThreadedSerializeExecutor_SetOutput(threadID, json);
        }
    }
}