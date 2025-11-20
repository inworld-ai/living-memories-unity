/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.Safety;
using Inworld.Framework.TextEmbedder;

namespace Inworld.Framework.Node
{
    /// <summary>
    /// Provides configuration settings for creating safety checker nodes within the Inworld framework.
    /// Defines safety policies, embedder configurations, and other creation-time parameters.
    /// Used for initializing safety checker nodes with specific moderation settings.
    /// </summary>
    public class SafetyCheckerNodeCreationConfig : NodeCreationConfig
    {
        /// <summary>
        /// Initializes a new instance of the SafetyCheckerNodeCreationConfig class with default settings.
        /// </summary>
        public SafetyCheckerNodeCreationConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_SafetyCheckerNodeCreationConfig_new(),
                InworldInterop.inworld_SafetyCheckerNodeCreationConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the SafetyCheckerNodeCreationConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the safety checker node creation config object.</param>
        public SafetyCheckerNodeCreationConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_SafetyCheckerNodeCreationConfig_delete);
        }

        /// <summary>
        /// Gets or sets the safety configuration that defines moderation policies and thresholds.
        /// Specifies what types of content to check and their severity thresholds.
        /// </summary>
        public SafetyCheckerCreationConfig SafetyConfig
        {
            get => new SafetyCheckerCreationConfig(InworldInterop.inworld_SafetyCheckerNodeCreationConfig_safety_config_get(m_DLLPtr));
            set => InworldInterop.inworld_SafetyCheckerNodeCreationConfig_safety_config_set(m_DLLPtr, value.ToDLL);
        }

        /// <summary>
        /// Gets or sets the text embedder configuration for content analysis.
        /// Defines how text is embedded and processed for safety analysis.
        /// </summary>
        public string EmbedderComponentID
        {
            get => InworldInterop.inworld_SafetyCheckerNodeCreationConfig_embedder_component_id_get(m_DLLPtr);
            set => InworldInterop.inworld_SafetyCheckerNodeCreationConfig_embedder_component_id_set(m_DLLPtr, value);
        } 
    }
}