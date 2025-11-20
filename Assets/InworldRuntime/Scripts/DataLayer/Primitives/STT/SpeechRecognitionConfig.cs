/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.STT
{
    /// <summary>
    /// Provides configuration settings for speech recognition operations within the Inworld framework.
    /// Defines parameters and options for speech-to-text conversion processes.
    /// Used for configuring how speech recognition components process audio input and generate text output.
    /// </summary>
    public class SpeechRecognitionConfig : InworldConfig
    {
        /// <summary>
        /// Initializes a new instance of the SpeechRecognitionConfig class with default settings.
        /// </summary>
        public SpeechRecognitionConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_SpeechRecognitionConfig_new(),
                InworldInterop.inworld_SpeechRecognitionConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the SpeechRecognitionConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the speech recognition config object.</param>
        public SpeechRecognitionConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_SpeechRecognitionConfig_delete);
        }
    }
}