/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Runtime.InteropServices;

namespace Inworld.Framework.Node
{
    // WARNING(Yan): Parameter order is reversed between CustomNodeProcessExecutor and EdgeConditionExecutor
    // CustomNodeProcessExecutor_new(user_data, callback) vs EdgeConditionExecutor_new_pv_pv(callback, user_data)
    /// <summary>
    /// Executes custom node processing logic in a threaded, asynchronous manner within the Inworld framework.
    /// Provides functionality to implement custom data processing operations across multiple execution contexts.
    /// Used for creating custom nodes with user-defined processing behavior in multi-threaded graph workflows.
    /// </summary>
    public class CustomNodeThreadedProcessExecutor : InworldExecutor
    {
        readonly ProcessBaseDataIODelegate m_Func;
        /// <summary>
        /// Initializes a new instance of the CustomNodeThreadedProcessExecutor class with a processing delegate.
        /// Creates a threaded executor that can handle multiple concurrent custom processing operations.
        /// </summary>
        /// <param name="func">The delegate function to execute for custom node processing.</param>
        public CustomNodeThreadedProcessExecutor(ProcessBaseDataIODelegate func)
        {
            m_Self = GCHandle.Alloc(this);
            m_Func = func;
            IntPtr funcPtr = Marshal.GetFunctionPointerForDelegate(func);
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_CustomNodeThreadedProcessExecutor_new(GCHandle.ToIntPtr(m_Self), funcPtr),
                InworldInterop.inworld_CustomNodeThreadedProcessExecutor_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the CustomNodeThreadedProcessExecutor class from a native pointer.
        /// </summary>
        /// <param name="dllPtr">The native pointer to the custom node threaded process executor object.</param>
        public CustomNodeThreadedProcessExecutor(IntPtr dllPtr)
        {
            m_DLLPtr = MemoryManager.Register(dllPtr, InworldInterop.inworld_CustomNodeThreadedProcessExecutor_delete);
        }
        
        /// <summary>
        /// Gets the last input data for a specific thread execution context.
        /// Used to access the inputs that triggered the custom processing operation in a threaded environment.
        /// </summary>
        /// <param name="threadID">The unique identifier of the thread execution context.</param>
        /// <returns>A vector containing the last input data processed for the specified thread.</returns>
        public static InworldVector<InworldBaseData> GetLastIntputs(int threadID)
        {
            return new InworldVector<InworldBaseData>(InworldInterop.inworld_CustomNodeThreadedProcessExecutor_GetInputs(threadID));
        }

        public static ProcessContext GetProcessContext(int threadID)
        {
            return new ProcessContext(InworldInterop.inworld_CustomNodeThreadedProcessExecutor_GetContext(threadID));
        }

        /// <summary>
        /// Sets the output result for a specific thread execution context.
        /// Defines the result data to be passed to subsequent nodes for the given thread.
        /// </summary>
        /// <param name="threadID">The unique identifier of the thread execution context.</param>
        /// <param name="input">The output data to set as the result of custom processing for this thread.</param>
        public static void SetLastOutput(int threadID, InworldBaseData input)
        {
            IntPtr optBaseData = InworldInterop.inworld_StatusOr_BaseData_new_rcstd_shared_ptr_Sl_inworld_graphs_BaseData_Sg_(input.ToDLL);
            InworldInterop.inworld_CustomNodeThreadedProcessExecutor_SetOutput(threadID, optBaseData);
        }
    }
}