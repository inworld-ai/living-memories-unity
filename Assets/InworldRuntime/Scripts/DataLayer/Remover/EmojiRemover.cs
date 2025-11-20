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
    /// Provides functionality to remove emoji characters from text streams within the Inworld framework.
    /// Processes input text streams and filters out emoji characters to produce clean text output.
    /// Used for text sanitization in contexts where emoji characters may interfere with processing.
    /// </summary>
    public class EmojiRemover : InworldRemover
    {
        /// <summary>
        /// Initializes a new instance of the EmojiRemover class with the specified input stream.
        /// </summary>
        /// <param name="stream">The input stream containing text that may include emoji characters.</param>
        public EmojiRemover(InworldInputStream<string> stream)
        {
            IntPtr result = InworldFrameworkUtil.Execute(InworldInterop.inworld_EmojiRemover_Create(stream.ToDLL),
                InworldInterop.inworld_StatusOr_EmojiRemover_status,
                InworldInterop.inworld_StatusOr_EmojiRemover_ok,
                InworldInterop.inworld_StatusOr_EmojiRemover_value,
                InworldInterop.inworld_StatusOr_EmojiRemover_delete);
            if (result != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(result, InworldInterop.inworld_EmojiRemover_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the EmojiRemover class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the emoji remover object.</param>
        public EmojiRemover(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_EmojiRemover_delete);
        }

        /// <summary>
        /// Gets a value indicating whether this emoji remover instance is valid and ready for processing.
        /// </summary>
        public override bool IsValid => InworldInterop.inworld_EmojiRemover_is_valid(m_DLLPtr);

        /// <summary>
        /// Gets a value indicating whether there are more processed text results available from the emoji remover.
        /// </summary>
        public override bool HasNext => InworldInterop.inworld_EmojiRemover_HasNext(m_DLLPtr);

        /// <summary>
        /// Gets the next processed text result with emoji characters removed.
        /// Returns text that has been filtered to exclude emoji characters.
        /// </summary>
        public override string Result =>
            InworldFrameworkUtil.Execute(InworldInterop.inworld_EmojiRemover_Read(m_DLLPtr),
                InworldInterop.inworld_StatusOr_string_status,
                InworldInterop.inworld_StatusOr_string_ok,
                InworldInterop.inworld_StatusOr_string_value,
                InworldInterop.inworld_StatusOr_string_delete);

        
    }
}