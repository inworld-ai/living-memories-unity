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
    /// Represents the response from a safety check operation within the Inworld framework.
    /// Contains the overall safety determination and details about detected unsafe topics.
    /// Used for reporting the results of content safety analysis and moderation decisions.
    /// </summary>
    public class CheckSafetyResponse : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the CheckSafetyResponse class with default settings.
        /// </summary>
        public CheckSafetyResponse()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_CheckSafetyResponse_new(),
                InworldInterop.inworld_CheckSafetyResponse_delete);
        }

        /// <summary>
        /// Initializes a new instance of the CheckSafetyResponse class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the check safety response object.</param>
        public CheckSafetyResponse(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_CheckSafetyResponse_delete);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the checked content is considered safe.
        /// True if the content passes all safety checks; false if unsafe content is detected.
        /// </summary>
        public bool IsSafe
        {
            get => InworldInterop.inworld_CheckSafetyResponse_is_safe_get(m_DLLPtr);
            set => InworldInterop.inworld_CheckSafetyResponse_is_safe_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the collection of unsafe topics detected in the content.
        /// Contains detailed information about each type of potentially harmful content found.
        /// </summary>
        public InworldVector<DetectedTopic> DetectedTopics
        {
            get => new InworldVector<DetectedTopic>(InworldInterop.inworld_CheckSafetyResponse_detected_topics_get(m_DLLPtr));
            set => InworldInterop.inworld_CheckSafetyResponse_detected_topics_set(m_DLLPtr, value.ToDLL);
        }
    }
}