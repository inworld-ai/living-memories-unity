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
    /// Represents a topic threshold for safety checking within the Inworld framework.
    /// Defines confidence thresholds for specific unsafe topics to determine when content should be blocked.
    /// Used for configuring granular safety policies for different types of potentially harmful content.
    /// </summary>
    public class TopicThreshold : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the TopicThreshold class with default settings.
        /// </summary>
        public TopicThreshold()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_TopicThreshold_new(),
                InworldInterop.inworld_TopicThreshold_delete);
        }

        /// <summary>
        /// Initializes a new instance of the TopicThreshold class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the topic threshold object.</param>
        public TopicThreshold(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_TopicThreshold_delete);
        }

        /// <summary>
        /// Gets or sets the confidence threshold for this topic.
        /// Content with confidence scores above this threshold for the associated topic will be considered unsafe.
        /// Values typically range from 0.0 to 1.0, where higher values indicate stricter filtering.
        /// </summary>
        public float Threshold
        {
            get => InworldInterop.inworld_TopicThreshold_threshold_get(m_DLLPtr);
            set => InworldInterop.inworld_TopicThreshold_threshold_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the unsafe topic type that this threshold applies to.
        /// Identifies the specific category of potentially harmful content this threshold controls.
        /// </summary>
        public UnsafeTopic TopicName
        {
            get => (UnsafeTopic)InworldInterop.inworld_TopicThreshold_topic_name_get(m_DLLPtr);
            set => InworldInterop.inworld_TopicThreshold_topic_name_set(m_DLLPtr, (int)value);
        }
    }
}