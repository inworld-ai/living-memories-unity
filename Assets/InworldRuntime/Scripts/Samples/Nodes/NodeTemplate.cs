/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Framework.Graph;
using UnityEngine;

namespace Inworld.Framework.Samples.Node
{
    /// <summary>
    /// Base template class for node-based sample implementations within the Inworld framework.
    /// Provides common functionality for integrating with graph execution and handling graph events.
    /// Serves as a foundation for building custom node behavior demonstrations and examples.
    /// </summary>
    public class NodeTemplate : MonoBehaviour
    {
        [SerializeField] protected InworldGraphExecutor m_InworldGraphExecutor;
        
        protected virtual void OnEnable()
        {
            if (!m_InworldGraphExecutor) 
                return;
            m_InworldGraphExecutor.OnGraphCompiled += OnGraphCompiled;
            m_InworldGraphExecutor.OnGraphResult += OnGraphResult;
            m_InworldGraphExecutor.OnGraphFinished += OnGraphFinished;
        }

        protected virtual void OnDisable()
        {
            if (!m_InworldGraphExecutor) 
                return;
            m_InworldGraphExecutor.OnGraphCompiled -= OnGraphCompiled;
            m_InworldGraphExecutor.OnGraphResult -= OnGraphResult;
            m_InworldGraphExecutor.OnGraphFinished -= OnGraphFinished;
        }

        /// <summary>
        /// Called when a graph has been successfully compiled and is ready for execution.
        /// Override this method to implement custom behavior when graph compilation completes.
        /// </summary>
        /// <param name="obj">The graph asset that was compiled.</param>
        protected virtual void OnGraphCompiled(InworldGraphAsset obj)
        {
           
        }
        
        /// <summary>
        /// Called when the graph produces a result during execution.
        /// Override this method to handle and process graph output data.
        /// </summary>
        /// <param name="obj">The result data produced by the graph.</param>
        protected virtual void OnGraphResult(InworldBaseData obj)
        {
            
        }
        
        /// <summary>
        /// Called when graph execution has finished or been terminated.
        /// Override this method to implement cleanup or completion logic.
        /// </summary>
        /// <param name="obj">The graph asset that finished execution.</param>
        protected virtual void OnGraphFinished(InworldGraphAsset obj)
        {
            
        }
    }
}