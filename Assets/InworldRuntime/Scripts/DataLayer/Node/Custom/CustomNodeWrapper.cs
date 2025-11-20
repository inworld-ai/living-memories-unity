/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.Node
{
    /// <summary>
    /// Provides a wrapper for custom nodes with user-defined processing logic within the Inworld framework.
    /// Encapsulates custom node executors and integrates them into the standard node processing pipeline.
    /// Used for creating nodes with custom behavior while maintaining compatibility with the graph system.
    /// </summary>
    public class CustomNodeWrapper : InworldNode
    {
        /// <summary>
        /// Initializes a new instance of the CustomNodeWrapper class with an executor.
        /// Supports both synchronous and threaded custom node executors.
        /// </summary>
        /// <param name="id">The unique identifier for this custom node.</param>
        /// <param name="executor">The custom executor that defines the node's processing behavior.</param>
        public CustomNodeWrapper(string id, InworldExecutor executor)
        {
            if (executor is CustomNodeProcessExecutor)
                m_DLLPtr = MemoryManager.Register(
                    InworldInterop
                        .inworld_CustomNodeWrapper_new_rcstd_string_rcinworld_swig_helpers_CustomNodeProcessExecutor(
                            id, executor.ToDLL), InworldInterop.inworld_CustomNodeWrapper_delete);
            else if (executor is CustomNodeThreadedProcessExecutor)
                m_DLLPtr = MemoryManager.Register(
                    InworldInterop
                        .inworld_CustomNodeWrapper_new_rcstd_string_rcinworld_swig_helpers_CustomNodeThreadedProcessExecutor(
                            id, executor.ToDLL), InworldInterop.inworld_CustomNodeWrapper_delete);
        }
        
        public CustomNodeWrapper(string id, InworldExecutor executor, CustomNodeDestructor destructor)
        {
            if (executor is CustomNodeProcessExecutor)
                m_DLLPtr = MemoryManager.Register(
                    InworldInterop
                        .inworld_CustomNodeWrapper_new_rcstd_string_rcinworld_swig_helpers_CustomNodeProcessExecutor_rcinworld_swig_helpers_CustomNodeDestructor(
                            id, executor.ToDLL, destructor.ToDLL), InworldInterop.inworld_CustomNodeWrapper_delete);
            else if (executor is CustomNodeThreadedProcessExecutor)
                m_DLLPtr = MemoryManager.Register(
                    InworldInterop
                        .inworld_CustomNodeWrapper_new_rcstd_string_rcinworld_swig_helpers_CustomNodeThreadedProcessExecutor_rcinworld_swig_helpers_CustomNodeDestructor(
                            id, executor.ToDLL, destructor.ToDLL), InworldInterop.inworld_CustomNodeWrapper_delete);
        }
        
        public CustomNodeWrapper(string id, InworldExecutor executor, NodeExecutionConfig executionConfig)
        {
            if (executor is CustomNodeProcessExecutor)
                m_DLLPtr = MemoryManager.Register(
                    InworldInterop
                        .inworld_CustomNodeWrapper_new_rcstd_string_rcstd_shared_ptr_Sl_inworld_graphs_NodeExecutionConfig_Sg__rcinworld_swig_helpers_CustomNodeProcessExecutor(
                            id, executionConfig.ToDLL, executor.ToDLL), InworldInterop.inworld_CustomNodeWrapper_delete);
            else if (executor is CustomNodeThreadedProcessExecutor)
                m_DLLPtr = MemoryManager.Register(
                    InworldInterop
                        .inworld_CustomNodeWrapper_new_rcstd_string_rcstd_shared_ptr_Sl_inworld_graphs_NodeExecutionConfig_Sg__rcinworld_swig_helpers_CustomNodeThreadedProcessExecutor(
                            id, executionConfig.ToDLL, executor.ToDLL), InworldInterop.inworld_CustomNodeWrapper_delete);
        }
        
        public CustomNodeWrapper(string id, InworldExecutor executor,  NodeExecutionConfig executionConfig, CustomNodeDestructor destructor)
        {
            if (executor is CustomNodeProcessExecutor)
                m_DLLPtr = MemoryManager.Register(
                    InworldInterop
                        .inworld_CustomNodeWrapper_new_rcstd_string_rcstd_shared_ptr_Sl_inworld_graphs_NodeExecutionConfig_Sg__rcinworld_swig_helpers_CustomNodeProcessExecutor_rcinworld_swig_helpers_CustomNodeDestructor(
                            id, executionConfig.ToDLL, executor.ToDLL, destructor.ToDLL), InworldInterop.inworld_CustomNodeWrapper_delete);
            else if (executor is CustomNodeThreadedProcessExecutor)
                m_DLLPtr = MemoryManager.Register(
                    InworldInterop
                        .inworld_CustomNodeWrapper_new_rcstd_string_rcstd_shared_ptr_Sl_inworld_graphs_NodeExecutionConfig_Sg__rcinworld_swig_helpers_CustomNodeThreadedProcessExecutor_rcinworld_swig_helpers_CustomNodeDestructor(
                            id, executionConfig.ToDLL, executor.ToDLL, destructor.ToDLL), InworldInterop.inworld_CustomNodeWrapper_delete);
        }

        /// <summary>
        /// Gets a value indicating whether this custom node wrapper is in a valid state for execution.
        /// Overrides the base implementation to provide custom node-specific validation logic.
        /// </summary>
        public override bool IsValid => InworldInterop.inworld_CustomNodeWrapper_is_valid(m_DLLPtr);

        /// <summary>
        /// Gets the unique identifier of this custom node wrapper.
        /// Returns the ID that was specified during construction.
        /// </summary>
        public override string ID => InworldInterop.inworld_CustomNodeWrapper_id(m_DLLPtr);

        /// <summary>
        /// Processes input data through the wrapped custom executor.
        /// Delegates processing to the underlying custom node executor implementation.
        /// </summary>
        /// <param name="processContext"></param>
        /// <param name="parameter">A vector containing the input data to process.</param>
        /// <returns>The processed data result from the custom executor, or null if processing fails.</returns>
        public override InworldBaseData Process(ProcessContext processContext, InworldVector<InworldBaseData> parameter)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_CustomNodeWrapper_Process(m_DLLPtr, processContext.ToDLL, parameter.ToDLL), 
                InworldInterop.inworld_StatusOr_BaseData_status,
                InworldInterop.inworld_StatusOr_BaseData_ok,
                InworldInterop.inworld_StatusOr_BaseData_value,
                InworldInterop.inworld_StatusOr_BaseData_delete
            );
            return result != IntPtr.Zero ? new InworldBaseData(result) : null;
        }
    }
}