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
    /// Provides configuration settings for executing LLM chat nodes within the Inworld framework.
    /// Defines runtime parameters, generation settings, and execution behavior for LLM chat processing.
    /// Used for customizing LLM chat node behavior during execution and response generation.
    /// </summary>
    public class LLMChatNodeExecutionConfig : NodeExecutionConfig
    {
        /// <summary>
        /// Initializes a new instance of the LLMChatNodeExecutionConfig class with default settings.
        /// </summary>
        public LLMChatNodeExecutionConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LLMChatNodeExecutionConfig_new(), InworldInterop.inworld_LLMChatNodeExecutionConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the LLMChatNodeExecutionConfig class from a native pointer.
        /// </summary>
        /// <param name="dllPtr">The native pointer to the LLM chat node execution config object.</param>
        public LLMChatNodeExecutionConfig(IntPtr dllPtr)
        {
            m_DLLPtr = MemoryManager.Register(dllPtr, InworldInterop.inworld_LLMChatNodeExecutionConfig_delete);
        }

        /// <summary>
        /// Gets or sets the text generation configuration for LLM processing.
        /// Defines parameters like temperature, max tokens, and other generation settings.
        /// </summary>
        public TextGenerationConfig TextGenerationConfig
        {
            get
            {
                IntPtr optStr = InworldInterop.inworld_LLMChatNodeExecutionConfig_text_generation_config_get(m_DLLPtr);
                if (InworldInterop.inworld_optional_TextGenerationConfig_has_value(optStr))
                    return new TextGenerationConfig(InworldInterop.inworld_optional_TextGenerationConfig_getConst(optStr));
                return null;
            }
            set
            {
                IntPtr optStr = InworldInterop.inworld_optional_TextGenerationConfig_new_rcinworld_TextGenerationConfig(value.ToDLL);
                InworldInterop.inworld_LLMChatNodeExecutionConfig_text_generation_config_set(m_DLLPtr, optStr);
            }
        }
        
        /// <summary>
        /// Gets or sets the identifier of the LLM component to use for processing.
        /// Specifies which LLM service or model should handle the chat requests.
        /// </summary>
        public string LLMComponentID
        {
            get => InworldInterop.inworld_LLMChatNodeExecutionConfig_llm_component_id_get(m_DLLPtr);
            set => InworldInterop.inworld_LLMChatNodeExecutionConfig_llm_component_id_set(m_DLLPtr, value);
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether to use streaming mode for LLM responses.
        /// When true, responses are streamed as they are generated rather than waiting for completion.
        /// </summary>
        public bool IsStream
        {
            get => InworldInterop.inworld_LLMChatNodeExecutionConfig_stream_get(m_DLLPtr);
            set => InworldInterop.inworld_LLMChatNodeExecutionConfig_stream_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether LLM responses should be reported to the client.
        /// When true, generated responses are sent back to the client application for handling.
        /// </summary>
        public override bool NeedReportToClient
        {
            get => InworldInterop.inworld_LLMChatNodeExecutionConfig_report_to_client_get(m_DLLPtr);
            set => InworldInterop.inworld_LLMChatNodeExecutionConfig_report_to_client_set(m_DLLPtr, value);
        }

        public override bool IsValid => InworldInterop.inworld_LLMChatNodeExecutionConfig_is_valid(m_DLLPtr);

        public override InworldMap<string, string> Properties
        {
            get => new InworldMap<string, string>(InworldInterop.inworld_LLMChatNodeExecutionConfig_properties_get(m_DLLPtr));
            set => InworldInterop.inworld_LLMChatNodeExecutionConfig_properties_set(m_DLLPtr, value.ToDLL);
        }
    }
} 