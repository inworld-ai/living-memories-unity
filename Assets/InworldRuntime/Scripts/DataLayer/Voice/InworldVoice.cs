/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework
{
    /// <summary>
    /// Represents voice configuration settings within the Inworld framework.
    /// Contains voice synthesis parameters including language codes and speaker identifiers
    /// used for text-to-speech operations and voice customization.
    /// </summary>
    public class InworldVoice : InworldConfig
    {
        /// <summary>
        /// Initializes a new instance of the InworldVoice class with default settings.
        /// Creates a new native voice configuration object and registers it with the memory manager.
        /// </summary>
        public InworldVoice()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_Voice_new(), InworldInterop.inworld_Voice_delete);
        }

        public InworldVoice(IntPtr rhs) => m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_Voice_delete);

        public Language Language
        {
            get
            {
                IntPtr optLang = InworldInterop.inworld_Voice_language_get(m_DLLPtr);
                if (InworldInterop.inworld_optional_Language_has_value(optLang))
                    return new Language(InworldInterop.inworld_optional_Language_getConst(optLang));
                return null;
            }
            set
            {
                IntPtr optLang = InworldInterop.inworld_optional_Language_new_rcinworld_library_Language(value.ToDLL);
                InworldInterop.inworld_Voice_language_set(m_DLLPtr, optLang);
            }
        }

        /// <summary>
        /// Gets or sets the speaker identifier for voice synthesis.
        /// Specifies which voice model or speaker to use for generating speech.
        /// This allows selection of different voice characteristics and personalities.
        /// </summary>
        /// <value>The speaker ID as a string, or null if the native pointer is invalid.</value>
        public string SpeakerID
        {
            get
            {
                if (m_DLLPtr == IntPtr.Zero)
                    return null;
                string speaker = InworldInterop.inworld_Voice_speaker_id_get(m_DLLPtr);
                return speaker;
            }
            set
            {
                if (m_DLLPtr == IntPtr.Zero)
                    return;
                InworldInterop.inworld_Voice_speaker_id_set(m_DLLPtr, value);
            }
        }
    }
}