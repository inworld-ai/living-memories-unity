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
    /// <summary>
    /// Executes custom node processing logic in a synchronous manner within the Inworld framework.
    /// Provides functionality to implement custom data processing operations for nodes.
    /// Used for creating custom nodes with user-defined processing behavior in graph workflows.
    /// </summary>
    public class CustomNodeProcessExecutor : InworldExecutor
    {
        // WARNING(Yan): Parameter order is reversed between CustomNodeProcessExecutor and EdgeConditionExecutor
        // CustomNodeProcessExecutor_new(user_data, callback) vs EdgeConditionExecutor_new_pv_pv(callback, user_data)
        readonly ProcessBaseDataIODelegate m_Func;
        /// <summary>
        /// Initializes a new instance of the CustomNodeProcessExecutor class with a processing delegate.
        /// Note: Parameter order is different from EdgeConditionExecutor.
        /// </summary>
        /// <param name="func">The delegate function to execute for custom node processing.</param>
        /// <param name="userData">The user data pointer passed back to the callback by the native DLL.</param>
        public CustomNodeProcessExecutor(ProcessBaseDataIODelegate func, IntPtr userData)
        {
            m_Self = GCHandle.Alloc(this);
            m_Func = func;
            IntPtr funcPtr = Marshal.GetFunctionPointerForDelegate(func);
            m_DLLPtr = MemoryManager.Register(
                InworldInterop.inworld_CustomNodeProcessExecutor_new(userData, funcPtr),
                InworldInterop.inworld_CustomNodeProcessExecutor_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the CustomNodeProcessExecutor class from a native pointer.
        /// </summary>
        /// <param name="dllPtr">The native pointer to the custom node process executor object.</param>
        public CustomNodeProcessExecutor(IntPtr dllPtr)
        {
            m_DLLPtr = MemoryManager.Register(dllPtr, InworldInterop.inworld_CustomNodeProcessExecutor_delete);
        }

        /// <summary>
        /// Gets the last input data that was processed by this executor.
        /// Used to access the inputs that triggered the custom processing operation.
        /// </summary>
        /// <returns>A vector containing the last input data processed by the executor.</returns>
        public static InworldVector<InworldBaseData> LastIntputs =>
             new InworldVector<InworldBaseData>(InworldInterop.inworld_CustomNodeProcessExecutor_GetLastInputs());
        
        public static ProcessContext LastContext => new ProcessContext(InworldInterop.inworld_CustomNodeProcessExecutor_GetLastContext());
        
        /// <summary>
        /// Sets the output result for the last custom processing operation.
        /// Defines the result data to be passed to subsequent nodes in the graph.
        /// </summary>
        /// <param name="input">The output data to set as the result of custom processing.</param>
        public static void SetLastOutput(InworldBaseData input)
        {
            IntPtr optBaseData = InworldInterop.inworld_StatusOr_BaseData_new_rcstd_shared_ptr_Sl_inworld_graphs_BaseData_Sg_(input.ToDLL);
            InworldInterop.inworld_CustomNodeProcessExecutor_SetLastOutput(optBaseData);
        }

    }
}