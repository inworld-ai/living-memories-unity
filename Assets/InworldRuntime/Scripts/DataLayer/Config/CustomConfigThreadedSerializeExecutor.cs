using System;
using System.Runtime.InteropServices;
using Inworld.Framework.Node;

namespace Inworld.Framework
{
    public class CustomConfigThreadedSerializeExecutor : InworldExecutor
    {
        ProcessBaseDataIODelegateExecutionID m_Func;
        public CustomConfigThreadedSerializeExecutor(ProcessBaseDataIODelegateExecutionID func)
        {
            m_Self = GCHandle.Alloc(this);
            m_Func = func;
            IntPtr funcPtr = Marshal.GetFunctionPointerForDelegate(func);
            m_DLLPtr = MemoryManager.Register(
                InworldInterop.inworld_CustomConfigThreadedSerializeExecutor_new(GCHandle.ToIntPtr(m_Self), funcPtr),
                InworldInterop.inworld_CustomConfigThreadedSerializeExecutor_delete);
        }

        public static CustomConfigWrapper GetInput(int threadID) =>
            new CustomConfigWrapper(InworldInterop.inworld_CustomConfigThreadedSerializeExecutor_GetInput(threadID));
        
        public static void SetOutput(int threadID, string json)
        {
            InworldInterop.inworld_CustomConfigThreadedSerializeExecutor_SetOutput(threadID, json);
        }
    }
}