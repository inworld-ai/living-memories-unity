/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.LLM;

namespace Inworld.Framework.Memory
{
    /// <summary>
    /// Provides configuration settings for rolling summary functionality within the Inworld framework.
    /// Defines parameters that control how conversation summaries are generated and updated.
    /// Used for configuring the behavior of rolling summary components in memory management.
    /// </summary>
    public class RollingSummaryConfig : InworldConfig
    {
        /// <summary>
        /// Initializes a new instance of the RollingSummaryConfig class with default settings.
        /// </summary>
        public RollingSummaryConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_RollingSummaryConfig_new(),
                InworldInterop.inworld_RollingSummaryConfig_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the RollingSummaryConfig class from a native pointer.
        /// </summary>
        /// <param name="dllPtr">The native pointer to the rolling summary config object.</param>
        public RollingSummaryConfig(IntPtr dllPtr)
        {
            m_DLLPtr = MemoryManager.Register(dllPtr, InworldInterop.inworld_RollingSummaryConfig_delete);
        }
        
        /// <summary>
        /// Gets or sets the maximum number of sentences allowed in the summary.
        /// Controls the length and detail level of generated conversation summaries.
        /// </summary>
        public int MaxSummarySentences
        {
            get => InworldInterop.inworld_RollingSummaryConfig_max_summary_sentences_get(m_DLLPtr);
            set => InworldInterop.inworld_RollingSummaryConfig_max_summary_sentences_set(m_DLLPtr, value);
        }
        
        /// <summary>
        /// Gets or sets the number of conversation turns that must occur before triggering a summary update.
        /// Determines how frequently the rolling summary is updated based on conversation activity.
        /// </summary>
        public int NumberOfTurnsBeforeSummary
        {
            get => InworldInterop.inworld_RollingSummaryConfig_number_of_turns_before_summary_get(m_DLLPtr);
            set => InworldInterop.inworld_RollingSummaryConfig_number_of_turns_before_summary_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the initial window size for conversation content to include in summaries.
        /// Defines how much conversation history to consider when generating the initial summary.
        /// </summary>
        public int StartWindowSize
        {
            get => InworldInterop.inworld_RollingSummaryConfig_start_window_size_get(m_DLLPtr);
            set => InworldInterop.inworld_RollingSummaryConfig_start_window_size_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the text generation configuration used for creating summaries.
        /// Defines LLM parameters like temperature and max tokens for summary generation.
        /// </summary>
        public TextGenerationConfig TextGenerationConfig
        {
            get => new TextGenerationConfig(
                InworldInterop.inworld_RollingSummaryConfig_text_generation_config_get(m_DLLPtr));
            set => InworldInterop.inworld_RollingSummaryConfig_text_generation_config_set(m_DLLPtr, value.ToDLL);
        }
    }
}