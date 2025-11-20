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
    /// Represents JSON data within the Inworld framework.
    /// Wraps native C++ JSON functionality and provides managed access to JSON content
    /// for configuration, data exchange, and structured data processing.
    /// </summary>
    public class InworldJson : InworldBaseData
    {
        /// <summary>
        /// Initializes a new instance of the InworldJson class with the specified JSON string.
        /// Parses and stores the JSON data for use within the Inworld framework.
        /// </summary>
        /// <param name="json">The JSON string to parse and store.</param>
        public InworldJson(string json)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_Json_new_rcstd_string(json),
                InworldInterop.inworld_Json_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the InworldJson class from an existing native pointer.
        /// Used for wrapping existing native JSON objects created by the C++ library.
        /// </summary>
        /// <param name="rhs">Native pointer to an existing JSON instance.</param>
        public InworldJson(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_Json_delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldJson class by converting from base data.
        /// Extracts JSON data from a generic InworldBaseData object.
        /// </summary>
        /// <param name="rhs">The base data object to convert to JSON format.</param>
        public InworldJson(InworldBaseData rhs)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_Json(rhs.ToDLL),
                InworldInterop.inworld_Json_delete);
        }

        /// <summary>
        /// Returns the JSON content as a formatted string.
        /// Provides a human-readable representation of the JSON data.
        /// </summary>
        /// <returns>The JSON content as a string, or empty string if invalid.</returns>
        public override string ToString()
        {
            return InworldInterop.inworld_Json_ToString(m_DLLPtr);
        }

        /// <summary>
        /// Gets a value indicating whether this JSON object contains valid data.
        /// </summary>
        /// <value>True if the JSON object is valid; otherwise, false.</value>
        public override bool IsValid => InworldInterop.inworld_Json_is_valid(m_DLLPtr);

        /// <summary>
        /// Accepts a visitor for processing this JSON data object.
        /// Part of the visitor pattern implementation for data processing.
        /// </summary>
        /// <param name="visitor">The visitor to accept for processing this JSON object.</param>
        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}