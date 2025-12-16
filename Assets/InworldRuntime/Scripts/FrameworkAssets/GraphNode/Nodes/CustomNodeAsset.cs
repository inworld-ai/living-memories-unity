/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using AOT;
using System.Runtime.InteropServices;
using Inworld.Framework.Node;
using UnityEngine;

namespace Inworld.Framework.Graph
{
    // Input: Any InworldBaseData (Based on your design)
    // Output: Any InworldBaseData (Based on your design)
    
    /// <summary>
    /// Specialized node asset for custom processing operations within graph workflows in the Inworld framework.
    /// Extends the base node functionality to provide customizable data processing capabilities.
    /// This asset can be created through Unity's Create menu and used to implement user-defined processing logic.
    /// Used for implementing custom data transformations, business logic, and specialized processing operations in AI workflows.
    /// </summary>
    [CreateAssetMenu(fileName = "Node_Custom", menuName = "Inworld/Create Node/Custom", order = -2850)]
    public class CustomNodeAsset : InworldNodeAsset
    {
        static readonly Dictionary<string, CustomNodeAsset> s_NodeIdToAsset = new Dictionary<string, CustomNodeAsset>();
        static readonly HashSet<string> s_RegisteredTypes = new HashSet<string>();
        public override string NodeTypeName => "Custom";

        /// <summary>
        /// Delegate function for custom data processing operations.
        /// Allows external assignment of custom processing logic for flexible node behavior.
        /// </summary>
        public ProcessBaseDataIODelegate processBaseDataFunc;
        
        /// <summary>
        /// The executor responsible for managing custom node processing operations.
        /// Handles the execution context and data flow for custom processing logic.
        /// </summary>
        CustomNodeProcessExecutor m_Executor;
        GCHandle m_SelfHandle;
        bool m_HandleAllocated;

        static readonly ProcessBaseDataIODelegate s_StaticProcessDelegate = StaticProcessBaseDataIO;
        
        /// <summary>
        /// Creates the runtime representation of this custom node within the specified graph.
        /// Initializes the custom node executor and creates a wrapper for processing operations.
        /// Sets up the processing delegate and validates the runtime node creation.
        /// </summary>
        /// <param name="graphAsset">The graph asset that will contain this custom node.</param>
        /// <returns>True if runtime creation succeeded; otherwise, false.</returns>
        public override bool CreateRuntime(InworldGraphAsset graphAsset)
        {
            m_Graph = graphAsset;
            m_Executor = new CustomNodeProcessExecutor(s_StaticProcessDelegate, GetUserDataPtr());
            Runtime = new CustomNodeWrapper(NodeName, m_Executor);
            return Runtime?.IsValid ?? false;
        }
        
        /// <summary>
        /// Virtual method for processing input data in custom nodes.
        /// Override this method in derived classes to implement specific data processing logic.
        /// The default implementation returns the first input data or an error if no input is provided.
        /// Please override this function for most use cases to implement custom processing behavior.
        /// </summary>
        /// <param name="inputs">The vector of input data to process.</param>
        /// <returns>The processed data result, or an error if processing fails.</returns>
        // YAN: Please override this function for most use case.
        protected virtual InworldBaseData ProcessBaseData(InworldVector<InworldBaseData> inputs)
        {
            if (inputs.Size == 0)
            {
                return new InworldError("No input data", StatusCode.DataLoss);
            }
            // YAN: Please override it by replacing inputs[0] to your own BaseData.
            return inputs[0];
        }
        
        /// <summary>
        /// Protected method for handling data input/output processing with error handling.
        /// Manages the execution context and provides comprehensive error handling for processing operations.
        /// Calls the ProcessBaseData method and handles any exceptions that occur during processing.
        /// </summary>
        /// <param name="contextPtr">Pointer to the execution context for data processing operations.</param>
        protected virtual void ProcessBaseDataIO(IntPtr contextPtr)
        {
            try
            {
                CustomNodeProcessExecutor.SetLastOutput(ProcessBaseData(CustomNodeProcessExecutor.LastIntputs));
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{NodeName}] Error Processing: {ex.Message}");
                try
                {
                    InworldError errorOutput = new InworldError($"Error: {ex.Message}");
                    CustomNodeProcessExecutor.SetLastOutput(errorOutput);
                }
                catch (Exception innerEx)
                {
                    Debug.LogError($"[{NodeName}] Error: {innerEx.Message}");
                }
            }
        }

        [MonoPInvokeCallback(typeof(ProcessBaseDataIODelegate))]
        static void StaticProcessBaseDataIO(IntPtr contextPtr)
        {
            if (contextPtr == IntPtr.Zero)
                return;
            CustomNodeAsset asset = null;
            try
            {
                GCHandle handle = GCHandle.FromIntPtr(contextPtr);
                asset = handle.Target as CustomNodeAsset;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[InworldFramework] CustomNodeAsset StaticProcessBaseDataIO GCHandle error: {ex.Message}");
                return;
            }

            asset?.ProcessBaseDataIO(IntPtr.Zero);
        }
        [MonoPInvokeCallback(typeof(ProcessBaseDataIODelegateExecutionID))]
        static void ThreadedCreateCallback(IntPtr ctxPtr, int threadID)
        {
            string nodeId = CustomNodeThreadedCreateExecutor.GetInputID(threadID);
            CustomNodeThreadedCreateExecutor.GetInputCreationContext(threadID);
            NodeExecutionConfig execCfg =
                CustomNodeThreadedCreateExecutor.GetInputExecutionConfig(threadID) ??
                new NodeExecutionConfig();

            if (!s_NodeIdToAsset.TryGetValue(nodeId, out CustomNodeAsset asset) || asset == null)
            {
                Debug.LogError($"[InworldFramework] No CustomNodeAsset found for nodeId: {nodeId}");
                return;
            }

            InworldVector<CustomConfigWrapper> extraConfigs =
                CustomNodeThreadedCreateExecutor.GetInputConfigs(threadID);
            if (extraConfigs != null && !extraConfigs.IsEmpty)
            {
                // TODO: Process the incoming data.
            }

            CustomNodeProcessExecutor processExec =
                new CustomNodeProcessExecutor(s_StaticProcessDelegate, asset.GetUserDataPtr());
            CustomNodeWrapper wrapper = new CustomNodeWrapper(nodeId, processExec, execCfg);
            CustomNodeThreadedCreateExecutor.SetOutput(threadID, wrapper);
        }

        IntPtr GetUserDataPtr()
        {
            if (!m_HandleAllocated || !m_SelfHandle.IsAllocated)
            {
                m_SelfHandle = GCHandle.Alloc(this);
                m_HandleAllocated = true;
            }
            return GCHandle.ToIntPtr(m_SelfHandle);
        }

        void OnDisable()
        {
            if (m_HandleAllocated && m_SelfHandle.IsAllocated)
            {
                m_SelfHandle.Free();
                m_HandleAllocated = false;
            }
        }

        protected virtual void RegisterCustomNode()
        {
            if (s_RegisteredTypes.Contains(NodeTypeName))
                return;
            CustomNodeThreadedCreateExecutor createExec = new CustomNodeThreadedCreateExecutor(ThreadedCreateCallback);
            InworldComponentManager.RegisterCustomNode(NodeTypeName, createExec);
        }

        public override bool RegisterJson(InworldGraphAsset graphAsset)
        {
            m_Graph = graphAsset;
            s_NodeIdToAsset[NodeName] = this;
            RegisterCustomNode();
            return m_Graph?.RegisterJsonNode(NodeName) ?? false;
        }
    }
}