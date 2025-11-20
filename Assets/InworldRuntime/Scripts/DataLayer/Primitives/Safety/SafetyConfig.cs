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
    /// Provides configuration settings for safety checking operations within the Inworld framework.
    /// Defines safety policies, forbidden topics, and thresholds for content moderation.
    /// Used for configuring how safety checkers evaluate and filter content.
    /// </summary>
    public class SafetyConfig : InworldConfig
    {
        /// <summary>
        /// Initializes a new instance of the SafetyConfig class with default settings.
        /// </summary>
        public SafetyConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_SafetyConfig_new(),
                InworldInterop.inworld_SafetyConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the SafetyConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the safety config object.</param>
        public SafetyConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_SafetyConfig_delete);
        }
        
        /// <summary>
        /// Gets or sets the collection of forbidden topics with their associated thresholds.
        /// Defines which topics are considered unsafe and the confidence thresholds for blocking content.
        /// </summary>
        public InworldVector<TopicThreshold> ForbiddenTopics
        {
            get => new(InworldInterop.inworld_SafetyConfig_forbidden_topics_get(m_DLLPtr));
            set => InworldInterop.inworld_SafetyConfig_forbidden_topics_set(m_DLLPtr, value.ToDLL);
        }
    }
}