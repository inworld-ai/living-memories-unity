/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework
{
    /// <summary>
    /// Represents a matched keyword from user input within the Inworld framework.
    /// Contains information about recognized keywords and their associated groupings.
    /// Used for keyword-based intent recognition and conversation trigger mechanisms.
    /// </summary>
    public class KeywordMatch : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldKeywordMatch class with default settings.
        /// </summary>
        public KeywordMatch()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_KeywordMatch_new(),
                InworldInterop.inworld_KeywordMatch_delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldKeywordMatch class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the keyword match object.</param>
        public KeywordMatch(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_KeywordMatch_delete);
        }

        /// <summary>
        /// Gets or sets the name of the keyword group that this match belongs to.
        /// Identifies the category or grouping of related keywords for organizational purposes.
        /// </summary>
        public string GroupName
        {
            get => InworldInterop.inworld_KeywordMatch_group_name_get(m_DLLPtr);
            set => InworldInterop.inworld_KeywordMatch_group_name_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the specific keyword that was matched.
        /// Contains the exact keyword text that was identified in the user input.
        /// </summary>
        public string Keyword
        {
            get => InworldInterop.inworld_KeywordMatch_keyword_get(m_DLLPtr);
            set => InworldInterop.inworld_KeywordMatch_keyword_set(m_DLLPtr, value);
        }
    }
}