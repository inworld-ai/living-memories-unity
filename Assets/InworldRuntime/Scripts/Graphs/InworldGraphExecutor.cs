/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Inworld.Framework.Graph
{
    /// <summary>
    /// Component responsible for executing Inworld AI graph assets within Unity.
    /// Manages the lifecycle of graph compilation, execution, and result handling for AI-powered workflows.
    /// Provides event-driven architecture for monitoring graph state and receiving output data.
    /// </summary>
    public class InworldGraphExecutor : MonoBehaviour
    {
        [FormerlySerializedAs("m_GraphAsset")]
        [Header("Graph Configuration")]
        [SerializeField] protected InworldGraphAsset m_Graph;
        [SerializeField] bool m_AutoCompileOnStart = true;
        [SerializeField] bool m_IsStreaming = false;

        const string k_ErrGraphNull = "[InworldFramework] Cannot compile graph: GraphAsset is null";
        const string k_CompileFailed = "[InworldFramework] Compiling graph failed.";
        const string k_ExecuteFailed = "[InworldFramework] Executing graph failed.";
        
        // State
        bool m_IsCompiled = false;
        bool m_IsExecuting = false;
        
        string m_ErrorMessage;

        // Runtime result buffering to avoid main-thread blocking and spikes
        readonly ConcurrentQueue<InworldBaseData> m_ResultQueue = new ConcurrentQueue<InworldBaseData>();

        
        // Events
        /// <summary>
        /// Event triggered when a graph asset has been successfully compiled and is ready for execution.
        /// </summary>
        public event Action<InworldGraphAsset> OnGraphCompiled;
        
        /// <summary>
        /// Event triggered when graph execution begins.
        /// </summary>
        public event Action<InworldGraphAsset> OnGraphStarted;
        
        /// <summary>
        /// Event triggered when the graph produces a result during execution.
        /// Provides access to intermediate and final output data from the graph.
        /// </summary>
        public event Action<InworldBaseData> OnGraphResult;
        
        /// <summary>
        /// Event triggered when graph execution completes successfully.
        /// </summary>
        public event Action<InworldGraphAsset> OnGraphFinished;
        
        /// <summary>
        /// Event triggered when an error occurs during graph compilation or execution.
        /// </summary>
        public event Action<InworldGraphAsset, string> OnGraphError;

        #region Properties
        /// <summary>
        /// Gets or sets the graph asset to be executed by this component.
        /// When set, clears the current compilation state and runtime objects.
        /// </summary>
        public InworldGraphAsset Graph 
        { 
            get => m_Graph;
            set => m_Graph = value;
        }

        /// <summary>
        /// Gets or sets the current error message from graph operations.
        /// When set, automatically logs the error and triggers the OnGraphError event.
        /// </summary>
        public string Error
        {
            get => m_ErrorMessage;
            set
            {
                if (m_ErrorMessage == value)
                    return;
                m_ErrorMessage = value;
                Debug.LogError(m_ErrorMessage);
                OnGraphError?.Invoke(m_Graph, m_ErrorMessage);
            }
        }
        
        /// <summary>
        /// Gets a value indicating whether the current graph has been successfully compiled.
        /// </summary>
        public bool IsCompiled => m_IsCompiled;
        
        /// <summary>
        /// Gets a value indicating whether the graph is currently executing.
        /// </summary>
        public bool IsExecuting => m_IsExecuting;
        
        /// <summary>
        /// Gets the runtime instance of the compiled graph.
        /// Returns null if no graph is loaded or compilation failed.
        /// </summary>
        public InworldGraph RuntimeGraph => m_Graph.Runtime;
        #endregion

        #region Unity Lifecycle

        void OnEnable()
        {
            if (!InworldController.Instance)
                return;
            InworldController.Instance.OnFrameworkInitialized += Initialize;
        }

        void OnDisable()
        {
            if (!InworldController.Instance)
                return;
            InworldController.Instance.OnFrameworkInitialized -= Initialize;
        }

        void OnDestroy()
        {
            CleanupRuntimeObjects();
        }
        #endregion

        #region Public API
        /// <summary>
        /// Loads a new graph asset for execution, replacing the current one.
        /// Cannot be called while a graph is currently executing.
        /// Automatically clears compilation state and runtime objects.
        /// </summary>
        /// <param name="graphAsset">The graph asset to load for execution.</param>
        public void LoadData(InworldGraphAsset graphAsset)
        {
            if (m_IsExecuting)
            {
                Debug.LogWarning("[InworldFramework] Cannot load new graph while executing.");
                return;
            }
            
            m_Graph = graphAsset;
            m_IsCompiled = false;
            CleanupRuntimeObjects();
        }
        public virtual bool IsInitialized => m_IsCompiled;

        /// <summary>
        /// Compiles the current graph asset into a runtime executable form.
        /// Must be called before executing the graph.
        /// Triggers OnGraphCompiled event on success or OnGraphError on failure.
        /// </summary>
        /// <returns>True if compilation succeeded, false otherwise.</returns>
        public bool Compile(CompiledGraphInterface compiled = null)
        {
            CleanupRuntimeObjects();
            if (!m_Graph)
            {
                Error = k_ErrGraphNull;
                return false;
            }
            if (m_Graph.CreateRuntime())
                m_IsCompiled = m_Graph.CompileRuntime();
            if (m_IsCompiled)
                m_IsCompiled = m_Graph.LoadGameData();
            if (m_IsCompiled)
                OnGraphCompiled?.Invoke(Graph);
            else
                Error = k_CompileFailed;
            return m_IsCompiled;
        }

        /// <summary>
        /// Executes the compiled graph asynchronously with optional input data.
        /// The graph must be compiled before execution can begin.
        /// Supports both streaming and simplified execution modes.
        /// </summary>
        /// <param name="executorName">Optional identifier for this execution instance. If empty, a GUID will be generated.</param>
        /// <param name="input">Optional input data to provide to the graph. If null, an empty InworldBaseData will be used.</param>
        /// <returns>A task that completes with true if execution started successfully, false otherwise.</returns>
        public virtual async Awaitable<bool> ExecuteGraphAsync(string executorName = "", InworldBaseData input = null)
        {
            if (!m_IsCompiled)
            {
                Error = $"[InworldFramework] Cannot execute graph: Graph is not compiled";
                return false;
            }

            if (m_IsExecuting)
            {
                Debug.LogWarning("[InworldFramework] Graph is already executing");
                return false;
            }

            if (m_Graph.Executor == null && !m_Graph.CreateExecutorRuntime())
            {
                Error = $"[InworldFramework] Failed to Create Executor";
                return false;
            }
            m_IsExecuting = m_Graph.StartRuntime();
            if (m_IsExecuting)
                OnGraphStarted?.Invoke(Graph);
            
            if (string.IsNullOrEmpty(executorName))
                executorName = Guid.NewGuid().ToString();
            
            input ??= new InworldBaseData();
            try
            {
                if (InworldFrameworkUtil.IsDebugMode)
                    Debug.Log($"[InworldFramework] Graph '{m_Graph.name}' started execution");
                if (!m_IsStreaming)
                    await ExecuteGraphSimplified(executorName, input);
                else
                    await ExecuteGraphStream(executorName, input);
                if (InworldFrameworkUtil.IsDebugMode)
                    Debug.Log($"[InworldFramework] Graph '{m_Graph.name}' finished execution");
                return true;
            }
            catch (Exception ex)
            {
                Error = $"[InworldFramework] Failed to execute graph: {ex.Message}";
                m_IsExecuting = false;
                return false;
            }
        }
        /// <summary>
        /// Called to invoke the Graph's Execute API, if this executor instance is set to stream.
        /// </summary>
        /// <param name="executionId">An identifier for the execution call invoked. </param>
        /// <param name="inputData">Input data handle passed from ExecuteGraphAsync</param>
        async Awaitable ExecuteGraphStream(string executionId, InworldBaseData inputData)
        {
            if (!m_Graph || m_Graph.Executor == null)
            {
                Error = k_ExecuteFailed;
                m_IsExecuting = false;
                OnGraphFinished?.Invoke(Graph);
                return;
            }
            // Offload blocking stream reads to a background thread, enqueue results, and
            // consume them on main thread in LateUpdate to avoid spikes.
            ExecutionResult handle = m_Graph.Executor.Execute(inputData, executionId);
            InworldInputStream<InworldBaseData> stream = handle.ResultStream;
            await Awaitable.BackgroundThreadAsync();
            while (stream.HasNext)
            {
                InworldBaseData result = stream.Read();
                if (result != null)
                    m_ResultQueue.Enqueue(result);
            }
            await Awaitable.MainThreadAsync();
        }
        /// <summary>
        /// Called to invoke the Graph's Execute API, if this executor instance is set to non-stream.
        /// </summary>
        /// <param name="executionId">An identifier for the execution call invoked. </param>
        /// <param name="inputData">Input data handle passed from ExecuteGraphAsync</param>
        async Awaitable ExecuteGraphSimplified(string executionId, InworldBaseData inputData)
        {
            if (!m_Graph || m_Graph.Executor == null)
            {
                Error = k_ExecuteFailed;
                m_IsExecuting = false;
                OnGraphFinished?.Invoke(Graph);
                return;
            }
            ExecutorInterface executor = m_Graph.Executor;
            int handle = executor.ExecuteSimplified(inputData, executionId);
            await Awaitable.BackgroundThreadAsync();
            while (executor.HasMoreResults(handle))
            {
                InworldBaseData result = executor.GetNextResult(handle);
                if (result != null)
                    m_ResultQueue.Enqueue(result);
            }
            await Awaitable.MainThreadAsync();
        }
        
        /// <summary>
        /// Stops the current graph execution if one is running.
        /// Closes all active executions and triggers the OnGraphFinished event.
        /// </summary>
        public void StopExecution()
        {
            if (!m_Graph || !m_IsExecuting || m_Graph.Executor == null)
                return;
                
            m_IsExecuting = false;
            m_Graph.Executor.CloseAllExecutions();
            OnGraphFinished?.Invoke(Graph);
            Debug.Log($"[InworldFramework] Graph '{m_Graph.name}' execution stopped");
        }
        #endregion

        #region Private Methods
        protected virtual void Initialize()
        {
            if (m_AutoCompileOnStart && m_Graph != null)
            {
                Compile();
            }
        }

        /// <summary>
        /// Cleans up all runtime objects associated with the current graph.
        /// Resets compilation and execution state to allow for fresh graph loading.
        /// </summary>
        public virtual void CleanupRuntimeObjects()
        {
            if (m_Graph == null)
            {
                Error = k_ErrGraphNull;
                return;
            }
            if (m_Graph.NeedClearHistory)
                m_Graph.ClearHistory(); 
            m_Graph.ClearGraphRuntime();
            m_IsCompiled = false;
            m_IsExecuting = false;
        }
        #endregion

        void LateUpdate()
        {
            if (m_ResultQueue.TryDequeue(out var data))
            {
                OnGraphResult?.Invoke(data);
            }
            if (m_IsExecuting && m_ResultQueue.IsEmpty)
            {
                m_IsExecuting = false;
                OnGraphFinished?.Invoke(Graph);
            }
        }
    }
}