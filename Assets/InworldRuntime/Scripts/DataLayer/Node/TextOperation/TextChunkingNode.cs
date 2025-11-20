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
    /// Represents a text chunking node for dividing large text into smaller segments within the Inworld framework.
    /// Breaks down text input into manageable chunks for processing and analysis.
    /// Used for implementing text segmentation and preprocessing operations in AI workflows.
    /// </summary>
    public class TextChunkingNode : InworldNode
    {
        /// <summary>
        /// Initializes a new instance of the TextChunkingNode class with the specified name.
        /// Creates a new text chunking node using the helper factory method.
        /// </summary>
        /// <param name="nodeName">The name identifier for this text chunking node.</param>
        /// <param name="executionConfig"></param>
        public TextChunkingNode(string nodeName, NodeExecutionConfig executionConfig)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_TextChunkingNode_Create(nodeName, executionConfig.ToDLL),
                InworldInterop.inworld_StatusOr_SharedPtr_TextChunkingNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_TextChunkingNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_TextChunkingNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_TextChunkingNode_delete
            );
            if (result != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(result, InworldInterop.inworld_TextChunkingNode_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the TextChunkingNode class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the text chunking node object.</param>
        public TextChunkingNode(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_TextChunkingNode_delete);
        }

        /// <summary>
        /// Gets a value indicating whether this text chunking node is in a valid state for execution.
        /// Overrides the base implementation to provide text chunking-specific validation logic.
        /// </summary>
        public override bool IsValid => InworldInterop.inworld_TextChunkingNode_is_valid(m_DLLPtr);

        /// <summary>
        /// Gets the unique identifier of this text chunking node.
        /// Overrides the base implementation to provide text chunking-specific identification.
        /// </summary>
        public override string ID => InworldInterop.inworld_TextChunkingNode_id(m_DLLPtr);
        
        // TODO(Yan): The process is unavailable in the current DLL (Aug 15)
    }
}