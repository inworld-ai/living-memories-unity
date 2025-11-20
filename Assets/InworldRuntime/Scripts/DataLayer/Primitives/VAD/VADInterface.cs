/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;


namespace Inworld.Framework.VAD
{
    /// <summary>
    /// Provides an interface for Voice Activity Detection (VAD) operations within the Inworld framework.
    /// Handles detection of speech activity in audio streams for voice processing applications.
    /// Used for determining when speech is present in audio input to optimize processing and reduce noise.
    /// </summary>
    public class VADInterface : InworldInterface
    {
        /// <summary>
        /// Initializes a new instance of the VADInterface class from a native pointer.
        /// </summary>
        /// <param name="ptr">The native pointer to the VAD interface object.</param>
        public VADInterface(IntPtr ptr) => m_DLLPtr = MemoryManager.Register(ptr, InworldInterop.inworld_LLMInterface_delete);

        /// <summary>
        /// Detects voice activity in the provided audio chunk using the specified configuration.
        /// Analyzes the audio data to determine the presence and intensity of speech activity.
        /// </summary>
        /// <param name="audioChunk">The audio chunk to analyze for voice activity.</param>
        /// <param name="vadConfig">The voice activity detection configuration specifying detection parameters.</param>
        /// <returns>An integer value indicating the level of voice activity detected in the audio chunk.</returns>
        public int DetectVoiceActivity(AudioChunk audioChunk, VoiceActivityDetectionConfig vadConfig)
        {
            return InworldFrameworkUtil.Execute(
                InworldInterop.inworld_VADInterface_DetectVoiceActivity(m_DLLPtr, audioChunk.ToDLL, vadConfig.ToDLL),
                InworldInterop.inworld_StatusOr_int_status,
                InworldInterop.inworld_StatusOr_int_ok,
                InworldInterop.inworld_StatusOr_int_value,
                InworldInterop.inworld_StatusOr_int_delete
            );
        }

    }
}