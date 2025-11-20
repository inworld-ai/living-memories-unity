/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.Memory
{
    /// <summary>
    /// Represents a rolling summary of conversation history within the Inworld framework.
    /// Maintains a continuously updated summary of recent conversation turns to preserve context while managing memory.
    /// Used for condensing long conversations into manageable summaries that retain important information.
    /// </summary>
    public class RollingSummary : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the RollingSummary class with default settings.
        /// </summary>
        public RollingSummary()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_RollingSummary_new(),
                InworldInterop.inworld_RollingSummary_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the RollingSummary class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the rolling summary object.</param>
        public RollingSummary(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_RollingSummary_delete);
        }
        
        /// <summary>
        /// Gets or sets the current summarized text of the conversation.
        /// Contains the condensed version of recent conversation history.
        /// </summary>
        public string SummarizedText
        {
            get => InworldInterop.inworld_RollingSummary_summarized_text_get(m_DLLPtr);
            set => InworldInterop.inworld_RollingSummary_summarized_text_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the number of conversation turns that have occurred since the last summary update.
        /// Tracks when the next summary update should be triggered based on conversation activity.
        /// </summary>
        public int TurnsSinceLastUpdate
        {
            get => InworldInterop.inworld_RollingSummary_turns_since_last_update_get(m_DLLPtr);
            set => InworldInterop.inworld_RollingSummary_turns_since_last_update_set(m_DLLPtr, value);
        }
    }
}