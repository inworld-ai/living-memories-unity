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
    /// Represents a tool call instance in the Inworld framework.
    /// This class manages the execution of specific tool calls with their parameters and identifiers.
    /// </summary>
    public class ToolCall : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the ToolCall class with default settings.
        /// </summary>
        public ToolCall()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_ToolCall_new(),
                InworldInterop.inworld_ToolCall_delete);
        }

        /// <summary>
        /// Initializes a new instance of the ToolCall class from an existing native pointer.
        /// </summary>
        /// <param name="rhs">Native pointer to an existing tool call instance.</param>
        public ToolCall(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_ToolCall_delete);
        }

        /// <summary>
        /// Gets or sets the unique identifier for this tool call.
        /// </summary>
        /// <value>The tool call's unique ID as a string.</value>
        public string ID
        {
            get => InworldInterop.inworld_ToolCall_id_get(m_DLLPtr);
            set => InworldInterop.inworld_ToolCall_id_set(m_DLLPtr, value);
        }
        
        /// <summary>
        /// Gets or sets the name of the tool being called.
        /// </summary>
        /// <value>The tool's name as a string.</value>
        public string Name
        {
            get => InworldInterop.inworld_ToolCall_name_get(m_DLLPtr);
            set => InworldInterop.inworld_ToolCall_name_set(m_DLLPtr, value);
        }
        
        /// <summary>
        /// Gets or sets the arguments passed to the tool call, typically in JSON format.
        /// </summary>
        /// <value>The tool call arguments as a string.</value>
        public string Args
        {
            get => InworldInterop.inworld_ToolCall_args_get(m_DLLPtr);
            set => InworldInterop.inworld_ToolCall_args_set(m_DLLPtr, value);
        }
    }
}