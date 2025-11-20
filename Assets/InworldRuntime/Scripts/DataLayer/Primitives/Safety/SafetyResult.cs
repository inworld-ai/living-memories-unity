/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.Safety
{
    /// <summary>
    /// Represents the result of a safety analysis operation within the Inworld framework.
    /// Contains the analyzed text and the safety determination for content moderation purposes.
    /// Used for encapsulating safety check results and providing access to both input and outcome.
    /// </summary>
    public class SafetyResult : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the SafetyResult class with specified text and safety status.
        /// </summary>
        /// <param name="text">The text content that was analyzed for safety.</param>
        /// <param name="isSafe">True if the text is considered safe; false if unsafe content was detected.</param>
        public SafetyResult(string text, bool isSafe)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_SafetyResult_new(text, isSafe),
                InworldInterop.inworld_SafetyResult_delete);
        }

        /// <summary>
        /// Initializes a new instance of the SafetyResult class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the safety result object.</param>
        public SafetyResult(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_SafetyResult_delete);
        }

        /// <summary>
        /// Gets the text content that was analyzed for safety violations.
        /// Contains the original input text that underwent safety evaluation.
        /// </summary>
        public string Text => InworldInterop.inworld_SafetyResult_text(m_DLLPtr);

        /// <summary>
        /// Gets a value indicating whether the analyzed text is considered safe.
        /// True if the text passes all safety checks; false if unsafe content was detected.
        /// </summary>
        public bool IsSafe => InworldInterop.inworld_SafetyResult_is_safe(m_DLLPtr);
    }
}