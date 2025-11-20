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
    /// Represents a Large Language Model (LLM) completion node for text completion within the Inworld framework.
    /// Processes text prompts through LLM services to generate text completions.
    /// Used for implementing AI-powered text completion functionality in workflows.
    /// </summary>
    public class LLMCompletionNode : InworldNode
    {
        public LLMCompletionNode(string nodeName, LLMCompletionNodeExecutionConfig executionConfig)
        {
            IntPtr genResult = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_LLMCompletionNode_Create(nodeName, executionConfig.ToDLL),
                InworldInterop.inworld_StatusOr_SharedPtr_LLMCompletionNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMCompletionNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMCompletionNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMCompletionNode_delete);
            if (genResult != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(genResult, InworldInterop.inworld_LLMCompletionNode_delete);
        }
        /// <summary>
        /// Initializes a new instance of the LLMCompletionNode class from a native pointer.
        /// </summary>
        /// <param name="dllPtr">The native pointer to the LLM completion node object.</param>
        public LLMCompletionNode(IntPtr dllPtr)
        {
            m_DLLPtr = MemoryManager.Register(dllPtr, InworldInterop.inworld_LLMCompletionNode_delete);
        }
                
        /// <summary>
        /// Gets the unique identifier of this LLM completion node.
        /// Overrides the base implementation to provide LLM completion-specific identification.
        /// </summary>
        public override string ID => InworldInterop.inworld_LLMCompletionNode_id(m_DLLPtr);

        public override bool IsValid => InworldInterop.inworld_LLMCompletionNode_is_valid(m_DLLPtr);
        
        /// <summary>
        /// Processes text input to generate a completion using the LLM service.
        /// Takes a text prompt and returns the LLM-generated completion.
        /// </summary>
        /// <param name="text">The input text prompt to complete.</param>
        /// <returns>The LLM completion response as base data, or null if processing fails.</returns>
        public InworldBaseData Process(ProcessContext processContext, InworldText text)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_LLMCompletionNode_Process(m_DLLPtr, processContext.ToDLL, text.ToDLL), 
                InworldInterop.inworld_StatusOr_SharedPtr_LLMCompletionResponse_status,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMCompletionResponse_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMCompletionResponse_value,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMCompletionResponse_delete
            );
            return result != IntPtr.Zero ? new InworldBaseData(result) : null;
        }
    }
}