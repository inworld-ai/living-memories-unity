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
    /// Represents an intent recognition node for analyzing user intentions within the Inworld framework.
    /// Processes text input to identify and match user intents for conversational understanding.
    /// Used for implementing natural language understanding in AI conversation workflows.
    /// </summary>
    public class IntentNode : InworldNode
    {
        public IntentNode(string nodeName, InworldCreationContext context, IntentNodeCreationConfig creationConfig, IntentNodeExecutionConfig executionConfig)
        {
            IntPtr genResult = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_IntentNode_Create(nodeName, context.ToDLL, creationConfig.ToDLL,
                    executionConfig.ToDLL),
                InworldInterop.inworld_StatusOr_SharedPtr_IntentNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_IntentNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_IntentNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_IntentNode_delete);
            if (genResult != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(genResult, InworldInterop.inworld_IntentNode_delete);
        }
        /// <summary>
        /// Initializes a new instance of the IntentNode class from a native pointer.
        /// </summary>
        /// <param name="dllPtr">The native pointer to the intent node object.</param>
        public IntentNode(IntPtr dllPtr)
        {
            m_DLLPtr = MemoryManager.Register(dllPtr, InworldInterop.inworld_IntentNode_delete);
        }
        
        /// <summary>
        /// Gets the unique identifier of this intent node.
        /// Overrides the base implementation to provide intent-specific identification.
        /// </summary>
        public override string ID => InworldInterop.inworld_IntentNode_id(m_DLLPtr);

        /// <summary>
        /// Gets a value indicating whether this intent node is in a valid state for execution.
        /// Overrides the base implementation to provide intent-specific validation logic.
        /// </summary>
        public override bool IsValid => InworldInterop.inworld_IntentNode_is_valid(m_DLLPtr);

        /// <summary>
        /// Processes text input to identify and match user intents.
        /// Analyzes the text to determine the user's intended actions or requests.
        /// </summary>
        /// <param name="text">The text input to analyze for intent recognition.</param>
        /// <returns>Matched intents with confidence scores, or null if processing fails.</returns>
        public MatchedIntents Process(ProcessContext processContext, InworldText text)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_IntentNode_Process(m_DLLPtr, processContext.ToDLL, text.ToDLL), 
                InworldInterop.inworld_StatusOr_SharedPtr_MatchedIntents_status,
                InworldInterop.inworld_StatusOr_SharedPtr_MatchedIntents_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_MatchedIntents_value,
                InworldInterop.inworld_StatusOr_SharedPtr_MatchedIntents_delete
            );
            return result != IntPtr.Zero ? new MatchedIntents(result) : null;
        }
    }
}