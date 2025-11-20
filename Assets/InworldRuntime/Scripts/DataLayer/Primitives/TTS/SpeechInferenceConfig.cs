/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.TTS
{
    /// <summary>
    /// Provides configuration settings for speech synthesis inference operations within the Inworld framework.
    /// Defines parameters that control the model inference process during text-to-speech generation.
    /// Used for fine-tuning speech synthesis quality, speed, and characteristics during model execution.
    /// </summary>
    public class SpeechInferenceConfig : InworldConfig
    {
        /// <summary>
        /// Initializes a new instance of the SpeechInferenceConfig class with default settings.
        /// </summary>
        public SpeechInferenceConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_SpeechSynthesisInferenceConfig_new(),
                InworldInterop.inworld_SpeechSynthesisPostprocessingConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the SpeechInferenceConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the speech inference config object.</param>
        public SpeechInferenceConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs,
                InworldInterop.inworld_SpeechSynthesisPostprocessingConfig_delete);
        }
        // TODO(Yan): Add Components.
    }
}