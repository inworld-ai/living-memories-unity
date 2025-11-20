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
    /// Represents a detected unsafe topic in content analysis within the Inworld framework.
    /// Contains information about the type of unsafe content detected and the confidence level.
    /// Used for providing detailed feedback about specific safety violations found in content.
    /// </summary>
    public class DetectedTopic : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the DetectedTopic class with default settings.
        /// </summary>
        public DetectedTopic()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_DetectedTopic_new(),
                InworldInterop.inworld_DetectedTopic_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the DetectedTopic class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the detected topic object.</param>
        public DetectedTopic(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_DetectedTopic_delete);
        }

        /// <summary>
        /// Gets or sets the confidence level for this detected topic.
        /// Represents how confident the safety system is that this topic is present in the content.
        /// Values typically range from 0.0 to 1.0, where higher values indicate stronger detection confidence.
        /// </summary>
        public float Confidence
        {
            get => InworldInterop.inworld_DetectedTopic_confidence_get(m_DLLPtr);
            set => InworldInterop.inworld_DetectedTopic_confidence_set(m_DLLPtr, value);
        }
        
        /// <summary>
        /// Gets or sets the type of unsafe topic that was detected.
        /// Identifies the specific category of potentially harmful content found in the analyzed text.
        /// </summary>
        public UnsafeTopic TopicName
        {
            get => (UnsafeTopic)InworldInterop.inworld_DetectedTopic_topic_name_get(m_DLLPtr);
            set => InworldInterop.inworld_DetectedTopic_topic_name_set(m_DLLPtr, (int)value);
        }
    }
}