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
    /// Represents an LLM prompt builder node for constructing prompts from structured data within the Inworld framework.
    /// Processes JSON input to build formatted text prompts suitable for LLM processing.
    /// Used for implementing dynamic prompt generation and template-based prompt construction.
    /// </summary>
    public class LLMPromptBuilderNode : InworldNode
    {
        public LLMPromptBuilderNode(string nodeName, LLMPromptBuilderNodeExecutionConfig executionConfig)
        {
            IntPtr genResult = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_LLMPromptBuilderNode_Create(nodeName, executionConfig.ToDLL),
                InworldInterop.inworld_StatusOr_SharedPtr_LLMPromptBuilderNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMPromptBuilderNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMPromptBuilderNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMPromptBuilderNode_delete);
            if (genResult != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(genResult, InworldInterop.inworld_LLMPromptBuilderNode_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the LLMPromptBuilderNode class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the LLM prompt builder node object.</param>
        public LLMPromptBuilderNode(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_LLMPromptBuilderNode_delete);
        }

        public override bool IsValid => InworldInterop.inworld_LLMPromptBuilderNode_is_valid(m_DLLPtr);
                        
        /// <summary>
        /// Gets the unique identifier of this LLM prompt builder node.
        /// Overrides the base implementation to provide prompt builder-specific identification.
        /// </summary>
        public override string ID => InworldInterop.inworld_LLMPromptBuilderNode_id(m_DLLPtr);
        
        /// <summary>
        /// Processes JSON input to build a formatted text prompt.
        /// Transforms structured JSON data into text suitable for LLM processing.
        /// </summary>
        /// <param name="json">The JSON input containing data to build the prompt from.</param>
        /// <returns>The built text prompt, or null if processing fails.</returns>
        public InworldText Process(ProcessContext processContext, InworldJson json)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_LLMPromptBuilderNode_Process(m_DLLPtr, processContext.ToDLL, json.ToDLL), 
                InworldInterop.inworld_StatusOr_SharedPtr_Text_status,
                InworldInterop.inworld_StatusOr_SharedPtr_Text_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_Text_value,
                InworldInterop.inworld_StatusOr_SharedPtr_Text_delete
            );
            return result != IntPtr.Zero ? new InworldText(result) : null;
        }
    }
}