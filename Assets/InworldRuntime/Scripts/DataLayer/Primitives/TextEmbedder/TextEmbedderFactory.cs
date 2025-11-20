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
    /// Factory class for creating text embedder interfaces within the Inworld framework.
    /// Manages the creation and initialization of text embedding components for vector generation.
    /// Used for instantiating text embedder interfaces with proper configuration settings.
    /// </summary>
    public class TextEmbedderFactory : InworldFactory
    {
        /// <summary>
        /// Initializes a new instance of the TextEmbedderFactory class with default settings.
        /// </summary>
        public TextEmbedderFactory()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_TextEmbedderFactory_new(),
                InworldInterop.inworld_TextEmbedderFactory_delete);
        }
        
        /// <summary>
        /// Creates a text embedder interface instance using the provided configuration.
        /// Instantiates and configures a text embedder interface for embedding operations.
        /// Currently accepts TextEmbedderCreationConfig as the configuration type.
        /// </summary>
        /// <param name="config">The configuration settings for the text embedder interface. Should be a TextEmbedderCreationConfig instance.</param>
        /// <returns>A TextEmbedderInterface instance if creation succeeds; otherwise, null.</returns>
        // TODO(Yan): Now it only accept TextEmbedderCreationConfig
        public override InworldInterface CreateInterface(InworldConfig config)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_TextEmbedderFactory_CreateTextEmbedder(m_DLLPtr, config.ToDLL),
                InworldInterop.inworld_StatusOr_TextEmbedderInterface_status,
                InworldInterop.inworld_StatusOr_TextEmbedderInterface_ok,
                InworldInterop.inworld_StatusOr_TextEmbedderInterface_value,
                InworldInterop.inworld_StatusOr_TextEmbedderInterface_delete
            );
            return result != IntPtr.Zero ? new TextEmbedderInterface(result) : null;
        }
    }
}