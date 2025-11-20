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
    /// Provides functionality to remove text enclosed in brackets from text streams within the Inworld framework.
    /// Processes input text streams and filters out content within various bracket types (e.g., [], (), {}) to produce clean text output.
    /// Used for text sanitization by removing annotations, comments, or metadata that may be enclosed in brackets.
    /// </summary>
    public class TextInBracketsRemover : InworldRemover
    {
        /// <summary>
        /// Initializes a new instance of the TextInBracketsRemover class with the specified input stream.
        /// </summary>
        /// <param name="stream">The input stream containing text that may include bracketed content to be removed.</param>
        public TextInBracketsRemover(InworldInputStream<string> stream)
        {
            IntPtr result = InworldFrameworkUtil.Execute(InworldInterop.inworld_TextInBracketsRemover_Create(stream.ToDLL),
                InworldInterop.inworld_StatusOr_TextInBracketsRemover_status,
                InworldInterop.inworld_StatusOr_TextInBracketsRemover_ok,
                InworldInterop.inworld_StatusOr_TextInBracketsRemover_value,
                InworldInterop.inworld_StatusOr_TextInBracketsRemover_delete);
            if (result != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(result, InworldInterop.inworld_TextInBracketsRemover_delete);
        }

        /// <summary>
        /// Initializes a new instance of the TextInBracketsRemover class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the text in brackets remover object.</param>
        public TextInBracketsRemover(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_TextInBracketsRemover_delete);
        }
        
        /// <summary>
        /// Gets a value indicating whether this text in brackets remover instance is valid and ready for processing.
        /// </summary>
        public override bool IsValid => InworldInterop.inworld_TextInBracketsRemover_is_valid(m_DLLPtr);

        /// <summary>
        /// Gets a value indicating whether there are more processed text results available from the text in brackets remover.
        /// </summary>
        public override bool HasNext => InworldInterop.inworld_TextInBracketsRemover_HasNext(m_DLLPtr);

        /// <summary>
        /// Gets the next processed text result with bracketed content removed.
        /// Returns text that has been filtered to exclude content enclosed in brackets.
        /// </summary>
        public override string Result =>
            InworldFrameworkUtil.Execute(InworldInterop.inworld_TextInBracketsRemover_Read(m_DLLPtr),
                InworldInterop.inworld_StatusOr_string_status,
                InworldInterop.inworld_StatusOr_string_ok,
                InworldInterop.inworld_StatusOr_string_value,
                InworldInterop.inworld_StatusOr_string_delete);
    }
}