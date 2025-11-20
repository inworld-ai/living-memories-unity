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
    /// Provides unified configuration settings for speech synthesis operations within the Inworld framework.
    /// Acts as a container for different speech synthesis provider configurations.
    /// Used for accessing both Inworld native and ElevenLabs speech synthesis configurations.
    /// </summary>
    public class SpeechSynthesisConfig : InworldConfig
    {
        /// <summary>
        /// Initializes a new instance of the SpeechSynthesizeConfig class with default settings.
        /// </summary>
        public SpeechSynthesisConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_SpeechSynthesisConfig_new(), InworldInterop.inworld_SpeechSynthesisConfig_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the SpeechSynthesizeConfig class from a native pointer.
        /// </summary>
        /// <param name="ptr">The native pointer to the speech synthesis config object.</param>
        public SpeechSynthesisConfig(IntPtr ptr) => m_DLLPtr = MemoryManager.Register(ptr, InworldInterop.inworld_SpeechSynthesisConfig_delete);

        /// <summary>
        /// Gets the configuration settings for Inworld's native speech synthesis.
        /// Provides access to Inworld-specific synthesis parameters and model settings.
        /// </summary>
        public InworldSynthesizeConfig Inworld => m_DLLPtr != IntPtr.Zero 
            ? new InworldSynthesizeConfig(InworldInterop.inworld_SpeechSynthesisConfig_inworld_config(m_DLLPtr)) 
            : null;
        
        /// <summary>
        /// Gets the configuration settings for ElevenLabs speech synthesis.
        /// Provides access to ElevenLabs-specific synthesis parameters and API settings.
        /// </summary>
        public ElevenLabsSynthesizeConfig ElevenLabs => m_DLLPtr != IntPtr.Zero 
            ? new ElevenLabsSynthesizeConfig(InworldInterop.inworld_SpeechSynthesisConfig_eleven_labs_config(m_DLLPtr)) 
            : null;
        
    }
}