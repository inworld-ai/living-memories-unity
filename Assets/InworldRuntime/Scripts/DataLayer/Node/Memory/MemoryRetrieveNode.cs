/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.Event;
using Inworld.Framework.Knowledge;
using Inworld.Framework.Memory;

namespace Inworld.Framework.Node
{
    /// <summary>
    /// Represents a memory retrieval node for accessing stored memories within the Inworld framework.
    /// Handles memory lookup and retrieval operations for conversational context access.
    /// Used for implementing memory recall functionality in AI conversation workflows.
    /// </summary>
    public class MemoryRetrieveNode : InworldNode
    {
        public MemoryRetrieveNode(string nodeName, InworldCreationContext inworldCreationContext,
            MemoryRetrieveNodeCreationConfig creationConfig, MemoryRetrieveNodeExecutionConfig executionConfig)
        {
            IntPtr genResult = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_MemoryRetrieveNode_Create(nodeName, inworldCreationContext.ToDLL, creationConfig.ToDLL, executionConfig.ToDLL),
                InworldInterop.inworld_StatusOr_SharedPtr_MemoryRetrieveNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_MemoryRetrieveNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_MemoryRetrieveNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_MemoryRetrieveNode_delete);
            if (genResult != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(genResult, InworldInterop.inworld_MemoryRetrieveNode_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the MemoryRetrievalNode class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the memory retrieval node object.</param>
        public MemoryRetrieveNode(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_MemoryRetrieveNode_delete);
        }

        public override bool IsValid => InworldInterop.inworld_MemoryRetrieveNode_is_valid(m_DLLPtr);
        
        /// <summary>
        /// Gets the unique identifier of this memory retrieval node.
        /// Overrides the base implementation to provide memory-specific identification.
        /// </summary>
        public override string ID => InworldInterop.inworld_MemoryRetrieveNode_id(m_DLLPtr);

        public KnowledgeRecords Process(ProcessContext processContext, EventHistory eventHistory,
            MemoryState memoryState, InworldText playerInput, InworldText playerName)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_MemoryRetrieveNode_Process(m_DLLPtr, processContext.ToDLL, eventHistory.ToDLL, memoryState.ToDLL, playerInput.ToDLL, playerName.ToDLL), 
                InworldInterop.inworld_StatusOr_SharedPtr_KnowledgeRecords_status,
                InworldInterop.inworld_StatusOr_SharedPtr_KnowledgeRecords_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_KnowledgeRecords_value,
                InworldInterop.inworld_StatusOr_SharedPtr_KnowledgeRecords_delete
            );
            return result != IntPtr.Zero ? new KnowledgeRecords(result) : null;
        }
    }
}