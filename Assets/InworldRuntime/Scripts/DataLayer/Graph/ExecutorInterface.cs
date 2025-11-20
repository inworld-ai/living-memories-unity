/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Concurrent;
using Inworld.Framework.Event;
using Inworld.Framework.Goal;
using Inworld.Framework.Knowledge;
using Inworld.Framework.LLM;
using Inworld.Framework.Memory;
using Inworld.Framework.TTS;

namespace Inworld.Framework.Graph
{
    // TODO(Yan): Put it in InworldGraphExecutor
    /// <summary>
    /// Provides an interface for executing compiled graphs within the Inworld framework.
    /// Manages graph execution sessions, data streaming, and result retrieval.
    /// Used for running AI workflow graphs and handling asynchronous execution.
    /// </summary>
    public class ExecutorInterface : InworldInterface
    {
        string m_CurrentExecutorName;
        int m_CurrentExecutorID = -1;

        /// <summary>
        /// Initializes a new instance of the InworldExecutorInterface class from a native pointer.
        /// </summary>
        /// <param name="dllPtr">The native pointer to the executor interface object.</param>
        public ExecutorInterface(IntPtr dllPtr) =>
            m_DLLPtr = MemoryManager.Register(dllPtr, InworldInterop.inworld_ExecutorInterface_delete);

        /// <summary>
        /// Starts the executor interface and prepares it for execution.
        /// Must be called before executing any graphs or operations.
        /// </summary>
        public void Start()
        {
            InworldInterop.inworld_ExecutorInterface_Start(m_DLLPtr);
        }
        
        /// <summary>
        /// Stops the executor interface and cleans up active executions.
        /// Should be called when finished with graph execution.
        /// </summary>
        public void Stop()
        {
            InworldInterop.inworld_ExecutorInterface_Stop(m_DLLPtr);
        }

        public ExecutionResult Execute(InworldBaseData baseData, string executionID)
        {
            return new ExecutionResult(
                InworldInterop
                    .inworld_ExecutorInterface_Execute_std_shared_ptr_Sl_inworld_graphs_BaseData_Sg__rcstd_string(
                        m_DLLPtr, baseData.ToDLL, executionID));
        }

        public ExecutionResult Execute(InworldBaseData baseData, string executionID, UserContext userContext)
        {
            return new ExecutionResult(
                InworldInterop
                    .inworld_ExecutorInterface_Execute_std_shared_ptr_Sl_inworld_graphs_BaseData_Sg__rcstd_string_rcinworld_graphs_UserContext(
                        m_DLLPtr, baseData.ToDLL, executionID, userContext.ToDLL));
        }

        /// <summary>
        /// Executes a data item using simplified asynchronous execution.
        /// Returns an execution ID for tracking and retrieving results later.
        /// </summary>
        /// <param name="data">The input data to process.</param>
        /// <param name="executorName">The name of the executor to use. If empty, uses the current executor.</param>
        /// <returns>An execution ID for tracking this operation.</returns>
        public int ExecuteSimplified(InworldBaseData data, string executorName = "")
        {
            if (!string.IsNullOrEmpty(executorName))
                m_CurrentExecutorName = executorName;
            else if(!string.IsNullOrEmpty(m_CurrentExecutorName))
                executorName = m_CurrentExecutorName;
            m_CurrentExecutorID = InworldInterop.inworld_ExecutorInterface_ExecuteSimplified(m_DLLPtr, data.ToDLL, executorName);
            return m_CurrentExecutorID;
        }
        
        public int ExecuteSimplified(InworldBaseData data, string executorName, UserContext userContext)
        {
            if (!string.IsNullOrEmpty(executorName))
                m_CurrentExecutorName = executorName;
            else if(!string.IsNullOrEmpty(m_CurrentExecutorName))
                executorName = m_CurrentExecutorName;
            m_CurrentExecutorID = InworldInterop.inworld_ExecutorInterface_ExecuteSimplifiedWithUserContext(m_DLLPtr, data.ToDLL, executorName, userContext.ToDLL);
            return m_CurrentExecutorID;
        }

        public string GetExecutionVariant(int executionHandle) => InworldInterop.inworld_ExecutorInterface_GetExecutionVariant(m_DLLPtr, executionHandle); 

        /// <summary>
        /// Cancels an ongoing execution for the specified executor.
        /// Stops the execution and cleans up associated resources.
        /// </summary>
        /// <param name="executorName">The name of the executor to cancel. If empty, uses the current executor.</param>
        public void CancelExecution(string executorName = "")
        {
            if (!string.IsNullOrEmpty(executorName))
                m_CurrentExecutorName = executorName;
            else if(!string.IsNullOrEmpty(m_CurrentExecutorName))
                executorName = m_CurrentExecutorName;
            InworldInterop.inworld_ExecutorInterface_CancelExecution(m_DLLPtr, executorName);
            m_CurrentExecutorName = "";
        }

        /// <summary>
        /// Retrieves the next available result from an asynchronous execution.
        /// Used with simplified execution to get results as they become available.
        /// </summary>
        /// <param name="executorID">The execution ID to get results from. If -1, uses the current executor ID.</param>
        /// <returns>The next result data, or null if no results are available.</returns>
        public InworldBaseData GetNextResult(int executorID = -1)
        {
            if (executorID != -1)
                m_CurrentExecutorID = executorID;
            else if (m_CurrentExecutorID != -1)
                executorID = m_CurrentExecutorID;
            IntPtr baseData = InworldInterop.inworld_ExecutorInterface_GetNextResult(m_DLLPtr, executorID);
            return new InworldBaseData(baseData);
            // return InworldFrameworkUtil.TryConvert(baseData);
        }

        /// <summary>
        /// Checks if there are more results available from an asynchronous execution.
        /// Used to determine if GetNextResult should be called again.
        /// </summary>
        /// <param name="executorID">The execution ID to check. If -1, uses the current executor ID.</param>
        /// <returns>True if more results are available; otherwise, false.</returns>
        public bool HasMoreResults(int executorID = -1)
        {
            if (executorID != -1)
                m_CurrentExecutorID = executorID;
            else if (m_CurrentExecutorID != -1)
                executorID = m_CurrentExecutorID;
            return InworldInterop.inworld_ExecutorInterface_HasMoreResults(m_DLLPtr, executorID);
        }

        /// <summary>
        /// Closes a specific asynchronous execution and cleans up its resources.
        /// Should be called when finished retrieving results from an execution.
        /// </summary>
        /// <param name="executorID">The execution ID to close. If -1, uses the current executor ID.</param>
        public void CloseExecution(int executorID = -1)
        {
            if (executorID != -1)
                m_CurrentExecutorID = executorID;
            else if (m_CurrentExecutorID != -1)
                executorID = m_CurrentExecutorID;
            InworldInterop.inworld_ExecutorInterface_CloseExecution(m_DLLPtr, executorID);
            m_CurrentExecutorID = -1;
        }

        /// <summary>
        /// Closes all active executions and cleans up all associated resources.
        /// Used for cleanup when shutting down the executor interface.
        /// </summary>
        public void CloseAllExecutions()
        {
            InworldInterop.inworld_ExecutorInterface_CleanupAllExecutions(m_DLLPtr);
        }
    }
}