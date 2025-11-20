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
    /// Represents an LLM chat request builder node for constructing chat requests from input data within the Inworld framework.
    /// Processes various input data to build structured LLM chat requests.
    /// Used for implementing chat request preparation and context assembly for LLM processing.
    /// </summary>
    public class LLMChatRequestBuilderNode : InworldNode
    {
        public LLMChatRequestBuilderNode(string nodeName, LLMChatRequestBuilderNodeExecutionConfig executionConfig)
        {
            IntPtr genResult = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_LLMChatRequestBuilderNode_Create(nodeName, executionConfig.ToDLL),
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatRequestBuilderNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatRequestBuilderNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatRequestBuilderNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatRequestBuilderNode_delete);
            if (genResult != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(genResult, InworldInterop.inworld_LLMChatRequestBuilderNode_delete);
        }
        /// <summary>
        /// Initializes a new instance of the LLMChatRequestBuilderNode class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the LLM chat request builder node object.</param>
        public LLMChatRequestBuilderNode(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_LLMChatRequestBuilderNode_delete);
        }
                        
        /// <summary>
        /// Gets the unique identifier of this LLM chat request builder node.
        /// Overrides the base implementation to provide chat request builder-specific identification.
        /// </summary>
        public override string ID => InworldInterop.inworld_LLMChatRequestBuilderNode_id(m_DLLPtr);
        
        /// <summary>
        /// Processes input data to build a structured LLM chat request.
        /// Assembles conversation context, messages, and parameters into a chat request.
        /// </summary>
        /// <param name="data">The input data vector containing elements to build the chat request from.</param>
        /// <returns>The constructed LLM chat request, or null if processing fails.</returns>
        public LLMChatRequest Process(ProcessContext processContext, InworldJson data)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_LLMChatRequestBuilderNode_Process(m_DLLPtr, processContext.ToDLL, data.ToDLL), 
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatRequest_status,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatRequest_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatRequest_value,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatRequest_delete
            );
            return result != IntPtr.Zero ? new LLMChatRequest(result) : null;
        }
    }
}