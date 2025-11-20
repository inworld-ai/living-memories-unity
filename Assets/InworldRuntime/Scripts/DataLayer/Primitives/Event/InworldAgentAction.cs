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
    /// Represents an action performed by an agent within the Inworld framework.
    /// Contains action metadata, parameters, and agent information for behavioral interactions.
    /// Used for handling and processing agent-generated actions in conversation workflows.
    /// </summary>
    public class InworldAgentAction : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldAgentAction class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the agent action object.</param>
        public InworldAgentAction(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_AgentAction_delete);
        }

        /// <summary>
        /// Gets or sets the name of the action being performed.
        /// Identifies the specific type of action the agent is executing.
        /// </summary>
        public string ActionName
        {
            get => InworldInterop.inworld_AgentAction_name_get(m_DLLPtr);
            set => InworldInterop.inworld_AgentAction_name_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the name of the agent performing this action.
        /// Identifies which agent is executing the action in multi-agent scenarios.
        /// </summary>
        public string AgentName
        {
            get => InworldInterop.inworld_AgentAction_agent_name_get(m_DLLPtr);
            set => InworldInterop.inworld_AgentAction_agent_name_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets action parameters by name using indexer syntax.
        /// Provides access to custom parameters associated with the action.
        /// </summary>
        /// <param name="param">The name of the parameter to get or set.</param>
        /// <returns>The value of the specified parameter.</returns>
        public string this[string param]
        {
            get => InworldInterop.inworld_AgentAction_get_parameter(m_DLLPtr, param);
            set => InworldInterop.inworld_AgentAction_set_parameter(m_DLLPtr, param, value);
        }
        
        /// <summary>
        /// Checks whether the action contains a parameter with the specified name.
        /// Used to verify if a parameter exists before accessing its value.
        /// </summary>
        /// <param name="param">The name of the parameter to check for existence.</param>
        /// <returns>True if the parameter exists; otherwise, false.</returns>
        public bool ContainsKey(string param)
        {
            return InworldInterop.inworld_AgentAction_contains_parameter(m_DLLPtr, param);
        }
    }
}