using System;
using System.Runtime.InteropServices;

namespace Inworld.Framework.Node
{
    public class CustomNodeThreadedCreateExecutor : InworldExecutor
    {
        readonly ProcessBaseDataIODelegateExecutionID m_Func;
        
        public CustomNodeThreadedCreateExecutor(ProcessBaseDataIODelegateExecutionID func)
        {
            m_Self = GCHandle.Alloc(this, GCHandleType.Pinned);
            m_Func = func;
            IntPtr funcPtr = Marshal.GetFunctionPointerForDelegate(func);
            m_DLLPtr = MemoryManager.Register(
                InworldInterop.inworld_CustomNodeThreadedCreateExecutor_new(GCHandle.ToIntPtr(m_Self), funcPtr),
                InworldInterop.inworld_CustomNodeThreadedCreateExecutor_delete);
        }
        
        public CustomNodeThreadedCreateExecutor(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_CustomNodeThreadedCreateExecutor_delete);
        }
        
        public static string GetInputID(int threadID) => InworldInterop.inworld_CustomNodeThreadedCreateExecutor_GetInputId(threadID);

        public static InworldCreationContext GetInputCreationContext(int threadID) =>
            new InworldCreationContext(
                InworldInterop.inworld_CustomNodeThreadedCreateExecutor_GetInputCreationContext(threadID));

        public static NodeExecutionConfig GetInputExecutionConfig(int threadID) =>
            new NodeExecutionConfig(
                InworldInterop.inworld_CustomNodeThreadedCreateExecutor_GetInputExecutionConfig(threadID));
        
        public static InworldVector<CustomConfigWrapper> GetInputConfigs(int threadID) =>
            new InworldVector<CustomConfigWrapper>(
                InworldInterop.inworld_CustomNodeThreadedCreateExecutor_GetInputConfigs(threadID));

        public static void SetOutput(int threadID, CustomNodeWrapper wrapper)
        {
            IntPtr statusWrapper = InworldInterop
                .inworld_StatusOr_CustomNodeWrapper_new_rcstd_shared_ptr_Sl_inworld_swig_helpers_CustomNodeWrapper_Sg_(wrapper.ToDLL);
            InworldInterop.inworld_CustomNodeThreadedCreateExecutor_SetOutput(threadID, statusWrapper);
        }
    }
}