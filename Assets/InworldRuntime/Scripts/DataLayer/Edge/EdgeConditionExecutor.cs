/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Inworld.Framework.Edge
{
    /// <summary>
    /// Executes conditional logic for graph edges in a synchronous manner.
    /// Provides functionality to evaluate conditions that determine whether an edge should be traversed.
    /// Used for implementing custom conditional logic in graph-based AI workflows.
    /// </summary>
    public class EdgeConditionExecutor : InworldExecutor
    {
        // WARNING(Yan): Parameter order is reversed between CustomNodeProcessExecutor and EdgeConditionExecutor
        // CustomNodeProcessExecutor_new(user_data, callback) vs EdgeConditionExecutor_new_pv_pv(callback, user_data)
        
        /// <summary>
        /// Initializes a new instance of the InworldEdgeConditionExecutor class with a callback function and user data.
        /// Note: Parameter order is reversed compared to CustomNodeProcessExecutor.
        /// </summary>
        /// <param name="func">The delegate function to execute for condition evaluation.</param>
        /// <param name="data">User data to be passed to the callback function.</param>
        public EdgeConditionExecutor(ProcessBaseDataIODelegate func, object data)
        {
            m_Self = GCHandle.Alloc(data, GCHandleType.Pinned);
            IntPtr funcPtr = Marshal.GetFunctionPointerForDelegate(func);
            IntPtr selfPtr = GCHandle.ToIntPtr(m_Self);
            m_DLLPtr = MemoryManager.Register(
                InworldInterop.inworld_EdgeConditionExecutor_new_pv_pv(funcPtr, selfPtr),
                InworldInterop.inworld_EdgeConditionExecutor_delete);
        }
        
        public EdgeConditionExecutor(EdgeConditionExecutor rhs)
        {
            m_DLLPtr = MemoryManager.Register(
                InworldInterop.inworld_EdgeConditionExecutor_copy(rhs.ToDLL),
                InworldInterop.inworld_EdgeConditionExecutor_delete);
        }
        /// <summary>
        /// Initializes a new instance of the InworldEdgeConditionExecutor class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the edge condition executor object.</param>
        public EdgeConditionExecutor(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_EdgeConditionExecutor_delete);
        }
        
        /// <summary>
        /// Gets the last input data that was processed by this executor.
        /// Used to access the input that triggered the condition evaluation.
        /// </summary>
        /// <returns>The last input data processed by the executor.</returns>
        public static InworldBaseData LastIntput =>
            new InworldBaseData(InworldInterop.inworld_EdgeConditionExecutor_GetLastInput());
        

        /// <summary>
        /// Sets the output condition result for the next execution.
        /// Determines whether the edge condition is satisfied and the edge should be traversed.
        /// </summary>
        /// <param name="condition">True if the condition is met; otherwise, false.</param>
        public static void SetLastOutput(bool condition)
        {
            InworldInterop.inworld_EdgeConditionExecutor_SetNextOutput(condition);
        }
        
        public static void SetNextError(string error)
            => InworldInterop.inworld_EdgeConditionExecutor_SetNextError(error);
    }
}