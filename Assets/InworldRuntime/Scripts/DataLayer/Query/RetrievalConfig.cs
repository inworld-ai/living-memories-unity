/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework
{
    /// <summary>
    /// Provides configuration settings for data retrieval operations within the Inworld framework.
    /// Defines parameters for querying and retrieving information from knowledge bases or document stores.
    /// Used for configuring search behavior, result limits, and quality thresholds for retrieval systems.
    /// </summary>
    public class RetrievalConfig : InworldConfig
    {
        /// <summary>
        /// Initializes a new instance of the InworldRetrievalConfig class with default settings.
        /// </summary>
        public RetrievalConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_RetrievalConfig_new(),
                InworldInterop.inworld_RetrievalConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldRetrievalConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the retrieval config object.</param>
        public RetrievalConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_RetrievalConfig_delete);
        }

        /// <summary>
        /// Gets or sets the maximum number of top results to retrieve.
        /// Determines how many of the best-matching results should be returned from a search query.
        /// Higher values provide more comprehensive results but may include less relevant items.
        /// </summary>
        public int TopK
        {
            get => InworldInterop.inworld_RetrievalConfig_top_k_get(m_DLLPtr);
            set => InworldInterop.inworld_RetrievalConfig_top_k_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the minimum relevance threshold for retrieval results.
        /// Only results with a relevance score above this threshold will be included in the response.
        /// Values typically range from 0.0 to 1.0, where higher values require stronger relevance matches.
        /// </summary>
        public float Threshold
        {
            get => InworldInterop.inworld_RetrievalConfig_threshold_get(m_DLLPtr);
            set => InworldInterop.inworld_RetrievalConfig_threshold_set(m_DLLPtr, value);
        }
    }
}