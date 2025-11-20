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
    /// Represents a tool in the Inworld framework that can be used for various operations.
    /// This class wraps native C++ tool functionality through the Inworld DLL.
    /// </summary>
    public class InworldTool : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldTool class with default settings.
        /// </summary>
        public InworldTool()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_Tool_new(), InworldInterop.inworld_Tool_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the InworldTool class from an existing native pointer.
        /// </summary>
        /// <param name="rhs">Native pointer to an existing tool instance.</param>
        public InworldTool(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_Tool_delete);
        }

        /// <summary>
        /// Gets or sets the name of the tool.
        /// </summary>
        /// <value>The tool's name as a string.</value>
        public string Name
        {
            get => InworldInterop.inworld_Tool_name_get(m_DLLPtr);
            set => InworldInterop.inworld_Tool_name_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the description of the tool.
        /// </summary>
        /// <value>The tool's description as a string.</value>
        public string Description
        {
            get => InworldInterop.inworld_Tool_description_get(m_DLLPtr);
            set => InworldInterop.inworld_Tool_description_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the properties of the tool as a JSON string.
        /// </summary>
        /// <value>The tool's properties serialized as a JSON string.</value>
        public string Properties
        {
            get => InworldInterop.inworld_Tool_GetPropertiesAsString(m_DLLPtr);
            set => InworldInterop.inworld_Tool_SetPropertiesFromString(m_DLLPtr, value);
        }
    }
}