/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.LLM
{
    /// <summary>
    /// Provides configuration settings for LLM routing and failover within the Inworld framework.
    /// Defines priority, error handling, and routing behavior for LLM service selection.
    /// Used for implementing fault tolerance and load balancing across multiple LLM providers.
    /// </summary>
    public class LLMRoutingConfig : InworldConfig
    {
        /// <summary>
        /// Initializes a new instance of the LLMRoutingConfig class with default settings.
        /// </summary>
        public LLMRoutingConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LLMRoutingConfig_new(),
                InworldInterop.inworld_LLMRoutingConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the LLMRoutingConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the LLM routing config object.</param>
        public LLMRoutingConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_LLMRoutingConfig_delete);
        }
        
        /// <summary>
        /// Gets or sets the priority level for this LLM routing option.
        /// Higher priority routes are preferred when multiple LLM options are available.
        /// </summary>
        public int Priority
        {
            get => InworldInterop.inworld_LLMRoutingConfig_priority_get(m_DLLPtr);
            set => InworldInterop.inworld_LLMRoutingConfig_priority_set(m_DLLPtr, value);
        }
        
        /// <summary>
        /// Gets or sets the minimum number of errors required before triggering failover.
        /// Controls when the system switches to alternative LLM providers due to failures.
        /// </summary>
        public int MinError
        {
            get => InworldInterop.inworld_LLMRoutingConfig_min_errors_get(m_DLLPtr);
            set => InworldInterop.inworld_LLMRoutingConfig_min_errors_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the LLM creation configuration for this routing option.
        /// Defines how the LLM instance should be created and configured for this route.
        /// </summary>
        public string LLMComponentID
        {
            get => InworldInterop.inworld_LLMRoutingConfig_llm_component_id_get(m_DLLPtr);
            set => InworldInterop.inworld_LLMRoutingConfig_llm_component_id_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the cooldown duration after errors before this route can be used again.
        /// Defines how long to wait before attempting to use a failed LLM provider again.
        /// </summary>
        public InworldDuration ErrorCoolDown
        {
            get => new InworldDuration(InworldInterop.inworld_LLMRoutingConfig_error_cooldown_get(m_DLLPtr));
            set => InworldInterop.inworld_LLMRoutingConfig_error_cooldown_set(m_DLLPtr, value.ToDLL);
        }
    }
}