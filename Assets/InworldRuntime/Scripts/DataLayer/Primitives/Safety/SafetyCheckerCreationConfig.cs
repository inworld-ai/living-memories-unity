/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.Node
{
    /// <summary>
    /// Provides configuration settings for creating safety checker instances within the Inworld framework.
    /// Defines model paths and parameters needed for initializing safety checking components.
    /// Used for configuring how safety checker components are created and initialized.
    /// </summary>
    public class SafetyCheckerCreationConfig : InworldConfig
    {
        /// <summary>
        /// Initializes a new instance of the SafetyCheckerCreationConfig class with default settings.
        /// </summary>
        public SafetyCheckerCreationConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_SafetyCheckerCreationConfig_new(),
                InworldInterop.inworld_SafetyCheckerCreationConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the SafetyCheckerCreationConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the safety checker creation config object.</param>
        public SafetyCheckerCreationConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_SafetyCheckerCreationConfig_delete);
        }

        /// <summary>
        /// Gets or sets the file path to the safety model weights.
        /// Specifies the location of the model file used for safety classification operations.
        /// </summary>
        public string ModelPath
        {
            get => InworldInterop.inworld_SafetyCheckerCreationConfig_model_weights_path_get(m_DLLPtr);
            set => InworldInterop.inworld_SafetyCheckerCreationConfig_model_weights_path_set(m_DLLPtr, value);
        } 
    }
}