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
    /// Represents a chat request for Large Language Model (LLM) interactions.
    /// Contains conversation messages and optional tool definitions for AI chat completions.
    /// Used to structure input data for LLM processing and conversation management.
    /// </summary>
    public class LLMChatRequest : InworldBaseData
    {
        /// <summary>
        /// Initializes a new instance of the LLMChatRequest class with conversation messages.
        /// Creates a chat request containing the specified messages for LLM processing.
        /// </summary>
        /// <param name="messages">A vector of conversation messages to include in the request.</param>
        public LLMChatRequest(InworldVector<InworldMessage> messages)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LLMChatRequest_new_std_vector_Sl_inworld_Message_Sg_(messages.ToDLL),
                InworldInterop.inworld_LLMChatRequest_delete);
        }

        /// <summary>
        /// Initializes a new instance of the LLMChatRequest class with conversation messages and available tools.
        /// Creates a chat request that enables the LLM to use specified tools during conversation processing.
        /// </summary>
        /// <param name="messages">A vector of conversation messages to include in the request.</param>
        /// <param name="tools">A vector of tools available for the LLM to use during processing.</param>
        public LLMChatRequest(InworldVector<InworldMessage> messages, InworldVector<ToolCall> tools)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LLMChatRequest_new_std_vector_Sl_inworld_Message_Sg__std_vector_Sl_inworld_Tool_Sg_(messages.ToDLL, tools.ToDLL),
                InworldInterop.inworld_LLMChatRequest_delete); 
        }
        
        /// <summary>
        /// Initializes a new instance of the LLMChatRequest class from an existing native pointer.
        /// Used for wrapping existing native LLM chat request objects created by the C++ library.
        /// </summary>
        /// <param name="rhs">Native pointer to an existing LLM chat request instance.</param>
        public LLMChatRequest(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_LLMChatRequest_delete);
        }

        /// <summary>
        /// Initializes a new instance of the LLMChatRequest class from an existing base data object.
        /// This constructor is primarily for internal use when wrapping native objects.
        /// </summary>
        /// <param name="parent">The parent base data object to wrap.</param>
        public LLMChatRequest(InworldBaseData parent)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_LLMChatRequest(parent.ToDLL), InworldInterop.inworld_LLMChatRequest_delete);
        }

        /// <summary>
        /// Gets a value indicating whether the current LLMChatRequest object is valid.
        /// </summary>
        /// <value>True if the object is valid; otherwise, false.</value>
        public override bool IsValid => InworldInterop.inworld_LLMChatRequest_is_valid(m_DLLPtr);

        /// <summary>
        /// Gets the conversation messages contained in this chat request.
        /// Returns a vector of messages that represent the conversation history and current input.
        /// </summary>
        /// <value>An InworldVector containing the conversation messages.</value>
        public InworldVector<InworldMessage> Messages
            => new InworldVector<InworldMessage>(InworldInterop.inworld_LLMChatRequest_messages(m_DLLPtr));

        /// <summary>
        /// Returns a string representation of the LLMChatRequest object.
        /// </summary>
        /// <returns>A string containing the native pointer if available, otherwise an empty string.</returns>
        public override string ToString() =>
            m_DLLPtr == IntPtr.Zero ? "" : InworldInterop.inworld_LLMChatRequest_ToString(m_DLLPtr);

        /// <summary>
        /// Gets the tool available for use in this chat request.
        /// Returns a tool that the LLM can invoke during conversation processing.
        /// </summary>
        /// <value>An InworldTool containing the available tool.</value>
        public InworldTool Tool => new InworldTool(InworldInterop.inworld_LLMChatRequest_tools(m_DLLPtr));

        /// <summary>
        /// Accepts a visitor to visit this data object.
        /// </summary>
        /// <param name="visitor">The visitor to accept.</param>
        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}