/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.Knowledge;

namespace Inworld.Framework.Node
{
    /// <summary>
    /// Represents a knowledge retrieval node for querying information within the Inworld framework.
    /// Processes text queries and retrieves relevant knowledge records from the knowledge base.
    /// Used for implementing context-aware information retrieval in AI conversation workflows.
    /// </summary>
    public class KnowledgeNode : InworldNode
    {
        public KnowledgeNode(string nodeName, InworldCreationContext inworldCreationContext, KnowledgeNodeCreationConfig creationConfig, KnowledgeNodeExecutionConfig executionConfig)
        {
            IntPtr genResult = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_KnowledgeNode_Create(nodeName, inworldCreationContext.ToDLL, creationConfig.ToDLL,
                    executionConfig.ToDLL),
                InworldInterop.inworld_StatusOr_SharedPtr_KnowledgeNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_KnowledgeNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_KnowledgeNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_KnowledgeNode_delete);
            if (genResult != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(genResult, InworldInterop.inworld_KnowledgeNode_delete);
        }
        /// <summary>
        /// Initializes a new instance of the KnowledgeNode class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the knowledge node object.</param>
        public KnowledgeNode(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_KnowledgeNode_delete);
        }
        
        /// <summary>
        /// Gets the unique identifier of this knowledge node.
        /// Overrides the base implementation to provide knowledge-specific identification.
        /// </summary>
        public override string ID => InworldInterop.inworld_KnowledgeNode_id(m_DLLPtr);

        /// <summary>
        /// Gets a value indicating whether this knowledge node is in a valid state for execution.
        /// Overrides the base implementation to provide knowledge-specific validation logic.
        /// </summary>
        public override bool IsValid => InworldInterop.inworld_KnowledgeNode_is_valid(m_DLLPtr);

        /// <summary>
        /// Processes a text query and player information to retrieve relevant knowledge records.
        /// Searches the knowledge base for information matching the query context.
        /// </summary>
        /// <param name="query">The text query to search for in the knowledge base.</param>
        /// <param name="playerName">The player name for context-aware knowledge retrieval.</param>
        /// <returns>Knowledge records matching the query, or null if processing fails.</returns>
        public KnowledgeRecords Process(InworldText query, InworldText playerName)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_KnowledgeNode_Process(m_DLLPtr, query.ToDLL, playerName.ToDLL), 
                InworldInterop.inworld_StatusOr_SharedPtr_KnowledgeRecords_status,
                InworldInterop.inworld_StatusOr_SharedPtr_KnowledgeRecords_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_KnowledgeRecords_value,
                InworldInterop.inworld_StatusOr_SharedPtr_KnowledgeRecords_delete
            );
            return result != IntPtr.Zero ? new KnowledgeRecords(result) : null;
        }
    }
}