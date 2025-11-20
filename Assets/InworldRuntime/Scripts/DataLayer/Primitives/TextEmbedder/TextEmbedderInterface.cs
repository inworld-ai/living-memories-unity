/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.TextEmbedder
{
    /// <summary>
    /// Provides an interface for text embedding operations within the Inworld framework.
    /// Handles conversion of text into vector embeddings for machine learning and similarity analysis.
    /// Used for generating numerical representations of text content for various AI processing tasks.
    /// </summary>
    public class TextEmbedderInterface : InworldInterface
    {
        /// <summary>
        /// Initializes a new instance of the TextEmbedderInterface class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the text embedder interface object.</param>
        public TextEmbedderInterface(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_TextEmbedderInterface_delete);
        }

        /// <summary>
        /// Generates a vector embedding for a single text input.
        /// Converts the input text into a numerical vector representation using the configured embedding model.
        /// </summary>
        /// <param name="text">The text string to convert into an embedding vector.</param>
        /// <returns>A vector of float values representing the text embedding, or null if embedding fails.</returns>
        public InworldVector<float> Embed(string text)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_TextEmbedderInterface_Embed(m_DLLPtr, text),
                InworldInterop.inworld_StatusOr_vector_float_status,
                InworldInterop.inworld_StatusOr_vector_float_ok,
                InworldInterop.inworld_StatusOr_vector_float_value,
                InworldInterop.inworld_StatusOr_vector_float_delete);
            return result != IntPtr.Zero ? new InworldVector<float>(result) : null;
        }
        
        /// <summary>
        /// Generates vector embeddings for multiple text inputs in a batch operation.
        /// Efficiently processes multiple text strings and returns their corresponding embedding vectors.
        /// </summary>
        /// <param name="texts">A vector of text strings to convert into embedding vectors.</param>
        /// <returns>A vector of embedding vectors, where each inner vector represents one text input, or null if embedding fails.</returns>
        public InworldVector<InworldVector<float>> EmbedBatch(InworldVector<string> texts)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_TextEmbedderInterface_EmbedBatch(m_DLLPtr, texts.ToDLL),
                InworldInterop.inworld_StatusOr_vector_vector_float_status,
                InworldInterop.inworld_StatusOr_vector_vector_float_ok,
                InworldInterop.inworld_StatusOr_vector_vector_float_value,
                InworldInterop.inworld_StatusOr_vector_vector_float_delete);
            return result != IntPtr.Zero ? new InworldVector<InworldVector<float>>(result) : null;
        }
    }
}