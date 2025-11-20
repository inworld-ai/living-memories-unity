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
    /// Represents a message in the Inworld framework for communication between different components.
    /// Manages message content, roles, and tool call associations for conversational AI systems.
    /// Used for handling chat messages, tool responses, and structured communication within the framework.
    /// </summary>
    public class InworldMessage : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldMessage class with default settings.
        /// </summary>
        public InworldMessage()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_Message_new(),
                InworldInterop.inworld_Message_delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldMessage class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the message object.</param>
        public InworldMessage(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_Message_delete);
        }

        /// <summary>
        /// Gets or sets the content of the message.
        /// Contains the actual text or data payload of the message being communicated.
        /// </summary>
        public string Content
        {
            get => InworldInterop.inworld_Message_GetTextContent(m_DLLPtr);
            set => InworldInterop.inworld_Message_SetTextContent(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the role associated with this message.
        /// Defines the sender type or context for the message (e.g., user, assistant, system).
        /// </summary>
        public Role Role
        {
            get => Enum.Parse<Role>(InworldInterop.inworld_Message_role_get(m_DLLPtr), true);
            set => InworldInterop.inworld_Message_role_set(m_DLLPtr, value.ToString().ToLower());
        }

        /// <summary>
        /// Gets or sets the tool call identifier associated with this message.
        /// Links the message to a specific tool call for tracking and response correlation.
        /// </summary>
        public string ToolCallID
        {
            get => InworldInterop.inworld_Message_tool_call_id_get(m_DLLPtr);
            set => InworldInterop.inworld_Message_tool_call_id_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the collection of tool calls associated with this message.
        /// Contains multiple tool calls that may be requested or responded to within this message.
        /// </summary>
        public InworldVector<ToolCall> ToolCalls
        {
            get => new InworldVector<ToolCall>(InworldInterop.inworld_Message_tool_calls_get(m_DLLPtr));
            set => InworldInterop.inworld_Message_tool_calls_set(m_DLLPtr, value.ToDLL);
        }
    }
}