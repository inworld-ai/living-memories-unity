/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.TextEmbedder;

namespace Inworld.Framework.Safety
{
    /// <summary>
    /// Factory class for creating safety checker interfaces within the Inworld framework.
    /// Manages the creation and initialization of safety checking components for content moderation.
    /// Used for instantiating safety checker interfaces with proper configuration and text embedding support.
    /// </summary>
    public class SafetyCheckerFactory : InworldFactory
    {
        TextEmbedderInterface m_TextEmbedder;
        
        /// <summary>
        /// Initializes a new instance of the SafetyCheckerFactory class with default settings.
        /// </summary>
        public SafetyCheckerFactory()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_SafetyCheckerFactory_new(), InworldInterop.inworld_SafetyCheckerFactory_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the SafetyCheckerFactory class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the safety checker factory object.</param>
        public SafetyCheckerFactory(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_SafetyCheckerFactory_delete);
        }

        /// <summary>
        /// Gets or sets the text embedder interface used for safety analysis.
        /// The text embedder is required for converting text into vector embeddings for safety classification.
        /// </summary>
        public TextEmbedderInterface TextEmbedder
        {
            get => m_TextEmbedder;
            set => m_TextEmbedder = value;
        }

        /// <summary>
        /// Creates a safety checker interface instance using the provided configuration.
        /// Instantiates and configures a safety checker interface for content moderation operations.
        /// </summary>
        /// <param name="config">The configuration settings for the safety checker interface. Should be a SafetyCheckerCreationConfig instance.</param>
        /// <returns>A SafetyCheckerInterface instance if creation succeeds; otherwise, null.</returns>
        public override InworldInterface CreateInterface(InworldConfig config) //inworld_SafetyCheckerCreationConfig
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_SafetyCheckerFactory_CreateSafetyChecker
                    (m_DLLPtr, m_TextEmbedder.ToDLL, config.ToDLL),
                InworldInterop.inworld_StatusOr_SafetyCheckerInterface_status,
                InworldInterop.inworld_StatusOr_SafetyCheckerInterface_ok,
                InworldInterop.inworld_StatusOr_SafetyCheckerInterface_value,
                InworldInterop.inworld_StatusOr_SafetyCheckerInterface_delete
            );
            return result != IntPtr.Zero ? new SafetyCheckerInterface(result) : null;
        }
    }
}