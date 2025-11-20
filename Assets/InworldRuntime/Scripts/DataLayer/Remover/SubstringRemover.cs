/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.TextOperation
{
    /// <summary>
    /// Provides functionality to remove specific substrings from text streams within the Inworld framework.
    /// Processes input text streams and filters out specified substring patterns to produce clean text output.
    /// Used for text sanitization by removing unwanted phrases, tags, or specific content patterns.
    /// </summary>
    public class SubstringRemover : InworldRemover
    {
        /// <summary>
        /// Initializes a new instance of the SubstringRemover class with the specified input stream and substring patterns.
        /// </summary>
        /// <param name="stream">The input stream containing text to be processed.</param>
        /// <param name="substrings">A vector containing the substring patterns to remove from the text.</param>
        public SubstringRemover(InworldInputStream<string> stream, InworldVector<string> substrings)
        {
            IntPtr result = InworldFrameworkUtil.Execute(InworldInterop.inworld_SubstringRemover_Create(stream.ToDLL, substrings.ToDLL),
                InworldInterop.inworld_StatusOr_SubstringRemover_status,
                InworldInterop.inworld_StatusOr_SubstringRemover_ok,
                InworldInterop.inworld_StatusOr_SubstringRemover_value,
                InworldInterop.inworld_StatusOr_SubstringRemover_delete);
            if (result != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(result, InworldInterop.inworld_SubstringRemover_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the SubstringRemover class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the substring remover object.</param>
        public SubstringRemover(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_SubstringRemover_delete);
        }

        /// <summary>
        /// Gets a value indicating whether this substring remover instance is valid and ready for processing.
        /// </summary>
        public override bool IsValid => InworldInterop.inworld_SubstringRemover_is_valid(m_DLLPtr);

        /// <summary>
        /// Gets a value indicating whether there are more processed text results available from the substring remover.
        /// </summary>
        public override bool HasNext => InworldInterop.inworld_SubstringRemover_HasNext(m_DLLPtr);

        /// <summary>
        /// Gets the next processed text result with specified substrings removed.
        /// Returns text that has been filtered to exclude the configured substring patterns.
        /// </summary>
        public override string Result =>
            InworldFrameworkUtil.Execute(InworldInterop.inworld_SubstringRemover_Read(m_DLLPtr),
                InworldInterop.inworld_StatusOr_string_status,
                InworldInterop.inworld_StatusOr_string_ok,
                InworldInterop.inworld_StatusOr_string_value,
                InworldInterop.inworld_StatusOr_string_delete);
    }
}