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
    /// Represents text data within the Inworld framework.
    /// Wraps native C++ text functionality and provides managed access to text content
    /// for processing, analysis, and display within AI interactions.
    /// </summary>
    public class InworldText : InworldBaseData
    {
        /// <summary>
        /// Initializes a new instance of the InworldText class from an existing native pointer.
        /// Used for wrapping existing native text objects created by the C++ library.
        /// </summary>
        /// <param name="dllPtr">Native pointer to an existing text instance.</param>
        public InworldText(IntPtr dllPtr)
        { 
            m_DLLPtr = MemoryManager.Register(dllPtr, InworldInterop.inworld_Text_delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldText class by converting from base data.
        /// Extracts text data from a generic InworldBaseData object.
        /// </summary>
        /// <param name="baseData">The base data object to convert to text format.</param>
        public InworldText(InworldBaseData baseData)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_Text(baseData.ToDLL), InworldInterop.inworld_Text_delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldText class with the specified text content.
        /// Creates a new text object containing the provided string data.
        /// </summary>
        /// <param name="text">The text content to store in this text object.</param>
        public InworldText(string text)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_Text_new_std_string(text), InworldInterop.inworld_Text_delete);
        }

        /// <summary>
        /// Returns a string representation of this text object.
        /// Provides the formatted text content for display or debugging purposes.
        /// </summary>
        /// <returns>The text content as a formatted string.</returns>
        public override string ToString()
        {
            return InworldInterop.inworld_Text_ToString(m_DLLPtr);
        }

        /// <summary>
        /// Gets a value indicating whether this text object contains valid data.
        /// </summary>
        /// <value>True if the text object is valid and contains meaningful data; otherwise, false.</value>
        public override bool IsValid => InworldInterop.inworld_Text_is_valid(m_DLLPtr);

        /// <summary>
        /// Gets the raw text content as a string.
        /// Retrieves the actual text value stored within this text object.
        /// </summary>
        /// <value>The text content as a string.</value>
        public string Text => InworldInterop.inworld_Text_value(m_DLLPtr);

        /// <summary>
        /// Accepts a visitor for processing this text data object.
        /// Part of the visitor pattern implementation for data processing and analysis.
        /// </summary>
        /// <param name="visitor">The visitor to accept for processing this text object.</param>
        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}