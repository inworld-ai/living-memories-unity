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
    /// Provides functionality to process and stream text in sentence-based chunks within the Inworld framework.
    /// Breaks down input text streams into individual sentences for more granular text processing.
    /// Used for sentence-level text analysis, processing, and manipulation operations.
    /// </summary>
    public class SentenceStream : InworldRemover
    {
        /// <summary>
        /// Initializes a new instance of the SentenceStream class with the specified input stream.
        /// </summary>
        /// <param name="stream">The input stream containing text to be processed into sentences.</param>
        public SentenceStream(InworldInputStream<string> stream)
        {
            IntPtr result = InworldFrameworkUtil.Execute(InworldInterop.inworld_SentenceStream_Create(stream.ToDLL),
                InworldInterop.inworld_StatusOr_SentenceStream_status,
                InworldInterop.inworld_StatusOr_SentenceStream_ok,
                InworldInterop.inworld_StatusOr_SentenceStream_value,
                InworldInterop.inworld_StatusOr_SentenceStream_delete);
            if (result != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(result, InworldInterop.inworld_SentenceStream_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the SentenceStream class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the sentence stream object.</param>
        public SentenceStream(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_SentenceStream_delete);
        }

        /// <summary>
        /// Gets a value indicating whether this sentence stream instance is valid and ready for processing.
        /// </summary>
        public override bool IsValid => InworldInterop.inworld_SentenceStream_is_valid(m_DLLPtr);

        /// <summary>
        /// Gets a value indicating whether there are more sentence results available from the sentence stream.
        /// </summary>
        public override bool HasNext => InworldInterop.inworld_SentenceStream_HasNext(m_DLLPtr);

        /// <summary>
        /// Gets the next sentence result from the text stream.
        /// Returns individual sentences extracted from the input text for processing.
        /// </summary>
        public override string Result =>
            InworldFrameworkUtil.Execute(InworldInterop.inworld_SentenceStream_Read(m_DLLPtr),
                InworldInterop.inworld_StatusOr_string_status,
                InworldInterop.inworld_StatusOr_string_ok,
                InworldInterop.inworld_StatusOr_string_value,
                InworldInterop.inworld_StatusOr_string_delete);
    }
}