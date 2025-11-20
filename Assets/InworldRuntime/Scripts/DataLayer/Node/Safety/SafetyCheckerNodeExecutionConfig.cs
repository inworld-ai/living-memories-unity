/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.Safety;

namespace Inworld.Framework.Node
{
    /// <summary>
    /// Provides configuration settings for executing safety checker nodes within the Inworld framework.
    /// Defines runtime parameters, safety policies, and reporting behavior for content moderation.
    /// Used for customizing safety checker node behavior during execution.
    /// </summary>
    public class SafetyCheckerNodeExecutionConfig : NodeExecutionConfig
    {
        /// <summary>
        /// Initializes a new instance of the SafetyCheckerNodeExecutionConfig class with default settings.
        /// </summary>
        public SafetyCheckerNodeExecutionConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_SafetyCheckerNodeExecutionConfig_new(),
                InworldInterop.inworld_SafetyCheckerNodeExecutionConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the SafetyCheckerNodeExecutionConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the safety checker node execution config object.</param>
        public SafetyCheckerNodeExecutionConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_SafetyCheckerNodeExecutionConfig_delete);
        }

        /// <summary>
        /// Gets a value indicating whether this safety checker execution configuration is valid.
        /// Overrides the base implementation to provide safety-specific validation logic.
        /// </summary>
        public override bool IsValid => InworldInterop.inworld_SafetyCheckerNodeExecutionConfig_is_valid(m_DLLPtr);
        
        /// <summary>
        /// Gets or sets the safety configuration for runtime moderation behavior.
        /// Defines safety policies and thresholds that override creation-time settings.
        /// Returns null if no runtime safety configuration is set.
        /// </summary>
        public SafetyConfig SafetyConfig
        {
            get
            {
                IntPtr optCfg = InworldInterop.inworld_SafetyCheckerNodeExecutionConfig_safety_config_get(m_DLLPtr);
                if (InworldInterop.inworld_optional_SafetyConfig_has_value(optCfg))
                    return new SafetyConfig(InworldInterop.inworld_optional_SafetyConfig_getConst(optCfg));
                return null;
            }
            set
            {
                IntPtr optCfg = InworldInterop.inworld_optional_SafetyConfig_new_rcinworld_SafetyConfig(value.ToDLL);
                InworldInterop.inworld_SafetyCheckerNodeExecutionConfig_safety_config_set(m_DLLPtr, optCfg);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether safety violations should be reported to the client.
        /// When true, safety results are sent back to the client application for handling.
        /// </summary>
        public override bool NeedReportToClient
        {
            get => InworldInterop.inworld_SafetyCheckerNodeExecutionConfig_report_to_client_get(m_DLLPtr);
            set => InworldInterop.inworld_SafetyCheckerNodeExecutionConfig_report_to_client_set(m_DLLPtr, value);
        } 

        public override InworldMap<string, string> Properties
        {
            get => new InworldMap<string, string>(InworldInterop.inworld_SafetyCheckerNodeExecutionConfig_properties_get(m_DLLPtr));
            set => InworldInterop.inworld_SafetyCheckerNodeExecutionConfig_properties_set(m_DLLPtr, value.ToDLL);
        }
    }
}