/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Runtime.InteropServices;

namespace Inworld.Framework.Edge
{
    /// <summary>
    /// Executes conditional logic for graph edges in a threaded, asynchronous manner.
    /// Provides functionality to evaluate conditions across multiple execution contexts simultaneously.
    /// Used for implementing custom conditional logic in multi-threaded graph-based AI workflows.
    /// </summary>
    public class EdgeConditionThreadedExecutor : InworldExecutor
    {
        /// <summary>
        /// Initializes a new instance of the InworldEdgeConditionThreadedExecutor class with a callback function.
        /// Creates a threaded executor that can handle multiple concurrent condition evaluations.
        /// </summary>
        /// <param name="func">The delegate function to execute for condition evaluation.</param>
        public EdgeConditionThreadedExecutor(ProcessBaseDataIODelegate func)
        {
            m_Self = GCHandle.Alloc(this);
            IntPtr funcPtr = Marshal.GetFunctionPointerForDelegate(func);
            IntPtr selfPtr = GCHandle.ToIntPtr(m_Self);
            m_DLLPtr = MemoryManager.Register(
                InworldInterop.inworld_EdgeConditionThreadedExecutor_new(funcPtr, selfPtr),
                InworldInterop.inworld_EdgeConditionExecutor_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the InworldEdgeConditionThreadedExecutor class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the threaded edge condition executor object.</param>
        public EdgeConditionThreadedExecutor(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_EdgeConditionThreadedExecutor_delete);
        }
        
        /// <summary>
        /// Gets the last input data for a specific execution context.
        /// Used to access the input that triggered the condition evaluation in a threaded environment.
        /// </summary>
        /// <param name="executionID">The unique identifier of the execution context.</param>
        /// <returns>The last input data processed for the specified execution context.</returns>
        public static InworldBaseData GetLastIntput(int executionID)
        {
            return new InworldBaseData(InworldInterop.inworld_EdgeConditionThreadedExecutor_GetInput(executionID));
        }

        public static void SetupInputs(int executionID, InworldBaseData baseData)
        {
            InworldInterop.inworld_EdgeConditionThreadedExecutor_SetupInputs(executionID, baseData.ToDLL);
        }

        public static void SetError(int executionID, string error)
        {
            InworldInterop.inworld_EdgeConditionThreadedExecutor_SetError(executionID, error);
        }

        /// <summary>
        /// Sets the output condition result for a specific execution context.
        /// Determines whether the edge condition is satisfied for the given execution thread.
        /// </summary>
        /// <param name="executionID">The unique identifier of the execution context.</param>
        /// <param name="condition">True if the condition is met; otherwise, false.</param>
        public static void SetLastOutput(int executionID, bool condition)
        {
            InworldInterop.inworld_EdgeConditionThreadedExecutor_SetOutput(executionID, condition);
        }
    }
}