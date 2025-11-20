/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.Event
{
    /// <summary>
    /// Represents an event within the Inworld framework that contains agent interactions.
    /// Encapsulates both speech and action events that can occur during agent communication.
    /// Used for handling and processing various types of agent-generated events in conversations.
    /// </summary>
    public class InworldEvent : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldEvent class with default settings.
        /// </summary>
        public InworldEvent()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_Event_new(), InworldInterop.inworld_Event_delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldEvent class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the event object.</param>
        public InworldEvent(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_Event_delete);
        }

        /// <summary>
        /// Gets the agent action component of this event.
        /// Provides access to action-related information when the event represents an agent action.
        /// </summary>
        public InworldAgentAction Action => new InworldAgentAction(InworldInterop.inworld_Event_action(m_DLLPtr));
        
        /// <summary>
        /// Gets the agent speech component of this event.
        /// Provides access to speech-related information when the event represents agent speech.
        /// </summary>
        public InworldAgentSpeech Speech => new InworldAgentSpeech(InworldInterop.inworld_Event_speech(m_DLLPtr));

    }
}