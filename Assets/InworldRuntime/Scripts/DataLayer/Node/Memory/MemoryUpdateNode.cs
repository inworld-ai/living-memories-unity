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
    /// Represents a memory update node for modifying stored memories within the Inworld framework.
    /// Handles memory storage and updating operations for conversational context management.
    /// Used for implementing persistent memory functionality in AI conversation workflows.
    /// </summary>
    public class MemoryUpdateNode : InworldNode
    {
        public MemoryUpdateNode(string nodeName, InworldCreationContext inworldCreationContext,
            MemoryUpdateNodeCreationConfig creationConfig)
        {
            IntPtr genResult = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_MemoryUpdateNode_Create_rcstd_string_rcinworld_CreationContext_rcinworld_graphs_MemoryUpdateNodeCreationConfig(nodeName, inworldCreationContext.ToDLL, creationConfig.ToDLL),
                InworldInterop.inworld_StatusOr_SharedPtr_MemoryUpdateNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_MemoryUpdateNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_MemoryUpdateNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_MemoryUpdateNode_delete);
            if (genResult != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(genResult, InworldInterop.inworld_MemoryUpdateNode_delete);
        }
        
        public MemoryUpdateNode(string nodeName, InworldCreationContext inworldCreationContext,
            MemoryUpdateNodeCreationConfig creationConfig, NodeExecutionConfig executionConfig)
        {
            IntPtr genResult = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_MemoryUpdateNode_Create_rcstd_string_rcinworld_CreationContext_rcinworld_graphs_MemoryUpdateNodeCreationConfig_rcstd_shared_ptr_Sl_inworld_graphs_NodeExecutionConfig_Sg_
                    (nodeName, inworldCreationContext.ToDLL, creationConfig.ToDLL, executionConfig.ToDLL),
                InworldInterop.inworld_StatusOr_SharedPtr_MemoryUpdateNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_MemoryUpdateNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_MemoryUpdateNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_MemoryUpdateNode_delete);
            if (genResult != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(genResult, InworldInterop.inworld_MemoryUpdateNode_delete);
        }
        /// <summary>
        /// Initializes a new instance of the MemoryUpdateNode class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the memory update node object.</param>
        public MemoryUpdateNode(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_MemoryUpdateNode_delete);
        }

        public override bool IsValid => InworldInterop.inworld_MemoryUpdateNode_is_valid(m_DLLPtr);
        /// <summary>
        /// Gets the unique identifier of this memory update node.
        /// Overrides the base implementation to provide memory-specific identification.
        /// </summary>
        public override string ID => InworldInterop.inworld_MemoryUpdateNode_id(m_DLLPtr);
        
        // TODO(Yan): Implement the Process Method. The Current DLL Aug 15 is still unavailable. 
    }
}