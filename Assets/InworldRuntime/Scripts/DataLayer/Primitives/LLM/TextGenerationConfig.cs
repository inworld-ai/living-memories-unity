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
    /// Provides configuration settings for text generation using Large Language Models within the Inworld framework.
    /// Defines parameters that control the behavior and quality of generated text output.
    /// Used for fine-tuning LLM text generation with parameters like temperature, token limits, and penalties.
    /// </summary>
    public class TextGenerationConfig : InworldConfig
    {
        /// <summary>
        /// Initializes a new instance of the TextGenerationConfig class with default settings.
        /// </summary>
        public TextGenerationConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_TextGenerationConfig_new(), InworldInterop.inworld_TextGenerationConfig_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the TextGenerationConfig class from a native pointer.
        /// </summary>
        /// <param name="ptr">The native pointer to the text generation config object.</param>
        public TextGenerationConfig(IntPtr ptr) => m_DLLPtr = MemoryManager.Register(ptr, InworldInterop.inworld_TextGenerationConfig_delete);

        /// <summary>
        /// Finalizes an instance of the TextGenerationConfig class.
        /// Ensures proper cleanup of native resources.
        /// </summary>
        ~TextGenerationConfig()
        {
            if (m_DLLPtr != IntPtr.Zero)
                InworldInterop.inworld_TextGenerationConfig_delete(m_DLLPtr);
            m_DLLPtr = IntPtr.Zero;
        }

        /// <summary>
        /// Gets or sets the maximum number of new tokens to generate.
        /// Controls the length of the generated text output.
        /// </summary>
        public int MaxToken
        {
            get => m_DLLPtr == IntPtr.Zero ? -1 : InworldInterop.inworld_TextGenerationConfig_max_new_tokens_get(m_DLLPtr);
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_TextGenerationConfig_max_new_tokens_set(m_DLLPtr, value);
            }
        }

        /// <summary>
        /// Gets or sets the maximum length of the input prompt.
        /// Limits the size of the context that can be processed by the LLM.
        /// </summary>
        public int MaxPromptLength
        {
            get => m_DLLPtr == IntPtr.Zero ? -1 : InworldInterop.inworld_TextGenerationConfig_max_prompt_length_get(m_DLLPtr);
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_TextGenerationConfig_max_prompt_length_set(m_DLLPtr, value);
            }
        }

        /// <summary>
        /// Gets or sets the temperature value for text generation.
        /// Controls the randomness and creativity of the generated text. Higher values produce more creative but less predictable output.
        /// </summary>
        public float Temperature
        {
            get => m_DLLPtr == IntPtr.Zero ? -1 : InworldInterop.inworld_TextGenerationConfig_temperature_get(m_DLLPtr);
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_TextGenerationConfig_temperature_set(m_DLLPtr, value);
            }
        }

        /// <summary>
        /// Gets or sets the top-p value for nucleus sampling.
        /// Controls the diversity of generated text by limiting the cumulative probability of token choices.
        /// </summary>
        public float TopP
        {
            get => m_DLLPtr == IntPtr.Zero ? -1 : InworldInterop.inworld_TextGenerationConfig_top_p_get(m_DLLPtr);
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_TextGenerationConfig_top_p_set(m_DLLPtr, value);
            }
        }

        /// <summary>
        /// Gets or sets the repetition penalty to reduce repetitive text generation.
        /// Penalizes tokens that have been generated recently to encourage more varied output.
        /// </summary>
        public float RepetitionPenalty
        {
            get => m_DLLPtr == IntPtr.Zero ? -1 : InworldInterop.inworld_TextGenerationConfig_repetition_penalty_get(m_DLLPtr);
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_TextGenerationConfig_repetition_penalty_set(m_DLLPtr, value);
            }
        }

        /// <summary>
        /// Gets or sets the frequency penalty to reduce repetition of frequent tokens.
        /// Penalizes tokens based on their frequency in the generated text to promote diversity.
        /// </summary>
        public float FrequencyPenalty
        {
            get => m_DLLPtr == IntPtr.Zero ? -1 : InworldInterop.inworld_TextGenerationConfig_frequency_penalty_get(m_DLLPtr);
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_TextGenerationConfig_frequency_penalty_set(m_DLLPtr, value);
            }
        }

        /// <summary>
        /// Gets or sets the presence penalty to encourage topic diversity.
        /// Penalizes tokens that have already appeared in the text to encourage new topics and concepts.
        /// </summary>
        public float PresencePenalty
        {
            get => m_DLLPtr == IntPtr.Zero ? -1 : InworldInterop.inworld_TextGenerationConfig_presence_penalty_get(m_DLLPtr);
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_TextGenerationConfig_presence_penalty_set(m_DLLPtr, value);
            }
        }
        
        /// <summary>
        /// Gets or sets the stop sequences that will terminate text generation.
        /// Contains strings that, when encountered during generation, will cause the process to stop.
        /// </summary>
        public InworldVector<string> StopSequences
        {
            get => m_DLLPtr == IntPtr.Zero ? null : new InworldVector<string>(InworldInterop.inworld_TextGenerationConfig_stop_sequences_get(m_DLLPtr));
            set
            {
                if (m_DLLPtr != IntPtr.Zero)
                    InworldInterop.inworld_TextGenerationConfig_stop_sequences_set(m_DLLPtr, value.ToDLL);
            }
        }
    }
}