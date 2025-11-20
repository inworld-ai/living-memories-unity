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
    /// Represents a text aggregation node for combining multiple text inputs within the Inworld framework.
    /// Collects and merges text data from various sources into a unified output.
    /// Used for implementing text consolidation and merging operations in AI workflows.
    /// </summary>
    public class TextAggregatorNode : InworldNode
    {
        /// <summary>
        /// Initializes a new instance of the TextAggregatorNode class with the specified name.
        /// Creates a new text aggregator node using the helper factory method.
        /// </summary>
        /// <param name="nodeName">The name identifier for this text aggregator node.</param>
        /// <param name="executionConfig"></param>
        public TextAggregatorNode(string nodeName, NodeExecutionConfig executionConfig)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_TextAggregatorNode_Create(nodeName, executionConfig.ToDLL),
                InworldInterop.inworld_StatusOr_SharedPtr_TextAggregatorNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_TextAggregatorNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_TextAggregatorNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_TextAggregatorNode_delete
            );
            if (result != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(result, InworldInterop.inworld_TextAggregatorNode_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the TextAggregatorNode class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the text aggregator node object.</param>
        public TextAggregatorNode(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_TextAggregatorNode_delete);
        }
                        
        /// <summary>
        /// Gets a value indicating whether this text aggregator node is in a valid state for execution.
        /// Overrides the base implementation to provide text aggregator-specific validation logic.
        /// </summary>
        public override bool IsValid => InworldInterop.inworld_TextAggregatorNode_is_valid(m_DLLPtr);
        
        /// <summary>
        /// Gets the unique identifier of this text aggregator node.
        /// Overrides the base implementation to provide text aggregator-specific identification.
        /// </summary>
        public override string ID => InworldInterop.inworld_TextAggregatorNode_id(m_DLLPtr);
        
        // TODO(Yan): The process is unavailable in the current DLL (Aug 15)
    }
}