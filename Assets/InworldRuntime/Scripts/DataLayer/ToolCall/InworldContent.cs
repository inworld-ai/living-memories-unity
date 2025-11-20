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
    /// Represents content data with associated tool calls within the Inworld framework.
    /// Contains textual content and a collection of tool calls that can be executed
    /// as part of AI interactions and processing workflows.
    /// </summary>
    public class InworldContent : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldContent class with default settings.
        /// Creates a new native content object and registers it with the memory manager.
        /// </summary>
        public InworldContent()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_Content_new(),
                InworldInterop.inworld_Content_delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldContent class from an existing native pointer.
        /// Used for wrapping existing native content objects created by the C++ library.
        /// </summary>
        /// <param name="rhs">Native pointer to an existing content instance.</param>
        public InworldContent(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_Content_delete);
        }

        // public override bool IsValid => InworldInterop.inworld_Content_is_valid(m_DLLPtr);
        
        /// <summary>
        /// Gets or sets the textual content data.
        /// Contains the main content string that represents the data being processed or transmitted.
        /// </summary>
        /// <value>The content as a string value.</value>
        public string Content
        {
            get => InworldInterop.inworld_Content_content_get(m_DLLPtr);
            set => InworldInterop.inworld_Content_content_set(m_DLLPtr, value);
        }

        public override string ToString() => IsValid ? Content : "";

        /// <summary>
        /// Gets or sets the collection of tool calls associated with this content.
        /// Tool calls represent specific functions or operations that can be executed
        /// in response to or in conjunction with the content data.
        /// </summary>
        /// <value>An InworldVector containing the associated tool calls.</value>
        public InworldVector<ToolCall> ToolCalls
        {
            get => new InworldVector<ToolCall>(InworldInterop.inworld_Content_tool_calls_get(m_DLLPtr));
            set => InworldInterop.inworld_Content_tool_calls_set(m_DLLPtr, value.ToDLL);
        }
    }
}