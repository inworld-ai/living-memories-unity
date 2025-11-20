/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.Edge
{
    /// <summary>
    /// Provides a wrapper for graph edge configuration and management.
    /// Allows setting edge properties such as loop behavior, optional traversal, and conditional execution.
    /// Used for building and configuring edges in graph-based AI workflows.
    /// </summary>
    public class EdgeWrapper : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldEdgeWrapper class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the edge wrapper object.</param>
        public EdgeWrapper(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_EdgeWrapper_delete);
        }
        public EdgeWrapper(EdgeWrapper rhs)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_EdgeWrapper_copy(rhs.ToDLL), InworldInterop.inworld_EdgeWrapper_delete);
        }
        /// <summary>
        /// Configures this edge to be a loop edge.
        /// Loop edges can be traversed multiple times during graph execution.
        /// </summary>
        public void SetToLoop() => InworldInterop.inworld_EdgeWrapper_SetToLoop(m_DLLPtr);
        
        /// <summary>
        /// Configures this edge to be optional.
        /// Optional edges may be skipped during graph traversal without affecting execution flow.
        /// </summary>
        public void SetToOptional() => InworldInterop.inworld_EdgeWrapper_SetToOptional(m_DLLPtr);
        
        /// <summary>
        /// Finalizes the edge configuration and builds the edge for use in graph execution.
        /// Must be called after setting all edge properties.
        /// </summary>
        public void Build() => InworldInterop.inworld_EdgeWrapper_Build(m_DLLPtr);

        /// <summary>
        /// Sets a condition executor that determines whether this edge should be traversed.
        /// Supports both synchronous and threaded condition executors.
        /// </summary>
        /// <param name="executor">The condition executor to evaluate edge traversal conditions.</param>
        public void SetCondition(InworldExecutor executor)
        {
            if (executor is EdgeConditionExecutor)
                InworldInterop.inworld_EdgeWrapper_SetCondition_rcinworld_swig_helpers_EdgeConditionExecutor(m_DLLPtr, executor.ToDLL);
            else if (executor is EdgeConditionThreadedExecutor)
                InworldInterop.inworld_EdgeWrapper_SetCondition_rcinworld_swig_helpers_EdgeConditionThreadedExecutor(m_DLLPtr, executor.ToDLL);
        }
        // TODO(Yan): Set Condition Callback.
        // Currently it will not work because it requires adding code in the c++ to generate the wrapper for the callback ptr.
        public void SetConditionCallback(IntPtr callbackPtr)
        {
            InworldInterop.inworld_EdgeWrapper_SetConditionCallback(m_DLLPtr, callbackPtr);
        }
    }
}