/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.LLM
{
    /// <summary>
    /// Provides an interface for Large Language Model (LLM) content generation within the Inworld framework.
    /// Handles text generation, conversation processing, and tool-assisted content creation.
    /// Used for accessing LLM services to generate responses, completions, and conversational content.
    /// </summary>
    public class LLMInterface : InworldInterface
    {
        /// <summary>
        /// Initializes a new instance of the LLMInterface class from a native pointer.
        /// </summary>
        /// <param name="ptr">The native pointer to the LLM interface object.</param>
        public LLMInterface(IntPtr ptr) => m_DLLPtr = MemoryManager.Register(ptr, InworldInterop.inworld_LLMInterface_delete);

        /// <summary>
        /// Generates content from a single text input using the specified configuration.
        /// Processes the input text through the LLM and returns a stream of generated content.
        /// </summary>
        /// <param name="input">The input text prompt to generate content from.</param>
        /// <param name="config">The text generation configuration specifying parameters like temperature and max tokens.</param>
        /// <returns>An input stream containing the generated content, or null if generation fails.</returns>
        public InworldInputStream<InworldContent> GenerateContent(string input, TextGenerationConfig config)
        {
            if (!IsValid || config == null || !config.IsValid || string.IsNullOrEmpty(input))
                return null;
            IntPtr result = InworldFrameworkUtil.Execute(InworldInterop.inworld_LLMInterface_GenerateContent_rcstd_string_rcinworld_TextGenerationConfig
                    (m_DLLPtr, input, config.ToDLL),
                InworldInterop.inworld_StatusOr_InputStream_Content_status,
                InworldInterop.inworld_StatusOr_InputStream_Content_ok,
                InworldInterop.inworld_StatusOr_InputStream_Content_value,
                InworldInterop.inworld_StatusOr_InputStream_Content_delete
            );
            return result != IntPtr.Zero ? new InworldInputStream<InworldContent>(result) : null;
        }
        
        /// <summary>
        /// Generates content from a conversation context using multiple messages.
        /// Processes a sequence of conversation messages through the LLM to generate contextual responses.
        /// </summary>
        /// <param name="messages">A vector of conversation messages providing context for content generation.</param>
        /// <param name="config">The text generation configuration specifying parameters like temperature and max tokens.</param>
        /// <returns>An input stream containing the generated content, or null if generation fails.</returns>
        public InworldInputStream<InworldContent> GenerateContent(InworldVector<InworldMessage> messages, TextGenerationConfig config)
        {
            if (!IsValid || config == null || !config.IsValid || messages == null || !messages.IsValid || messages.Size <= 0)
                return null;
            IntPtr result = InworldFrameworkUtil.Execute(InworldInterop.inworld_LLMInterface_GenerateContent_rcstd_vector_Sl_inworld_Message_Sg__rcinworld_TextGenerationConfig
                (m_DLLPtr, messages.ToDLL, config.ToDLL),
                InworldInterop.inworld_StatusOr_InputStream_Content_status,
                InworldInterop.inworld_StatusOr_InputStream_Content_ok,
                InworldInterop.inworld_StatusOr_InputStream_Content_value,
                InworldInterop.inworld_StatusOr_InputStream_Content_delete
            );
            return result != IntPtr.Zero ? new InworldInputStream<InworldContent>(result) : null;
        }
        
        /// <summary>
        /// Generates content from conversation messages with tool assistance capabilities.
        /// Processes conversation context with access to external tools for enhanced content generation.
        /// </summary>
        /// <param name="messages">A vector of conversation messages providing context for content generation.</param>
        /// <param name="config">The text generation configuration specifying parameters like temperature and max tokens.</param>
        /// <param name="tools">A vector of tools available for the LLM to use during content generation.</param>
        /// <returns>An input stream containing the generated content, or null if generation fails.</returns>
        public InworldInputStream<InworldContent> GenerateContent(InworldVector<InworldMessage> messages, 
            TextGenerationConfig config, InworldVector<InworldTool> tools)
        {
            if (!IsValid || config == null || !config.IsValid || messages == null || !messages.IsValid || messages.Size <= 0 || tools == null || !tools.IsValid)
                return null;
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_LLMInterface_GenerateContent_rcstd_vector_Sl_inworld_Message_Sg__rcinworld_TextGenerationConfig_rcstd_vector_Sl_inworld_Tool_Sg_
                    (m_DLLPtr, messages.ToDLL, config.ToDLL, tools.ToDLL),
                InworldInterop.inworld_StatusOr_InputStream_Content_status,
                InworldInterop.inworld_StatusOr_InputStream_Content_ok,
                InworldInterop.inworld_StatusOr_InputStream_Content_value,
                InworldInterop.inworld_StatusOr_InputStream_Content_delete
            );
            return result != IntPtr.Zero ? new InworldInputStream<InworldContent>(result) : null;
        }
    }
}