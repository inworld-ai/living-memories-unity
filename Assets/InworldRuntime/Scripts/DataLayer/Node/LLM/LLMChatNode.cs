/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.LLM;

namespace Inworld.Framework.Node
{
    /// <summary>
    /// Represents a Large Language Model (LLM) chat node for conversational AI within the Inworld framework.
    /// Processes chat requests through LLM services to generate conversational responses.
    /// Used for implementing AI-powered chat functionality in conversation workflows.
    /// </summary>
    public class LLMChatNode : InworldNode
    {
        public LLMChatNode(string nodeName, LLMChatNodeExecutionConfig executionConfig)
        {
            IntPtr genResult = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_LLMChatNode_Create(nodeName, executionConfig.ToDLL),
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatNode_delete);
            if (genResult != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(genResult, InworldInterop.inworld_LLMChatNode_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the LLMChatNode class from a native pointer.
        /// </summary>
        /// <param name="dllPtr">The native pointer to the LLM chat node object.</param>
        public LLMChatNode(IntPtr dllPtr)
        {
            m_DLLPtr = MemoryManager.Register(dllPtr, InworldInterop.inworld_LLMChatNode_delete);
        }
        
        /// <summary>
        /// Gets the unique identifier of this LLM chat node.
        /// Overrides the base implementation to provide LLM chat-specific identification.
        /// </summary>
        public override string ID => InworldInterop.inworld_LLMChatNode_id(m_DLLPtr);

        /// <summary>
        /// Gets a value indicating whether this LLM chat node is in a valid state for execution.
        /// Overrides the base implementation to provide LLM chat-specific validation logic.
        /// </summary>
        public override bool IsValid => InworldInterop.inworld_LLMChatNode_is_valid(m_DLLPtr);

        /// <summary>
        /// Processes an LLM chat request to generate a conversational response.
        /// Sends the request to the configured LLM service and returns the generated response.
        /// </summary>
        /// <param name="processContext"></param>
        /// <param name="request">The LLM chat request containing the conversation context and prompt.</param>
        /// <returns>The LLM chat response with generated text, or null if processing fails.</returns>
        public LLMChatResponse Process(ProcessContext processContext, LLMChatRequest request)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_LLMChatNode_Process(m_DLLPtr, processContext.ToDLL, request.ToDLL), 
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatResponse_status,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatResponse_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatResponse_value,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatResponse_delete
            );
            return result != IntPtr.Zero ? new LLMChatResponse(result) : null;
        }
    }
}