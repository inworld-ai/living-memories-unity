/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.AEC
{
    /// <summary>
    /// Provides an interface for Acoustic Echo Cancellation (AEC) audio processing within the Inworld framework.
    /// Handles real-time echo cancellation operations on audio streams to improve audio quality.
    /// Used for processing audio data to remove echo artifacts and enhance communication clarity.
    /// </summary>
    public class AECInterface : InworldInterface
    {
        /// <summary>
        /// Initializes a new instance of the AECInterface class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the AEC interface object.</param>
        public AECInterface(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_AECFilterInterface_delete);
        }

        /// <summary>
        /// Gets the model properties and specifications for this AEC interface.
        /// Provides information about the AEC model's capabilities and configuration.
        /// </summary>
        public ModelProperties ModelProperties => new ModelProperties(InworldInterop.inworld_AECFilterInterface_model_properties(m_DLLPtr));

        /// <summary>
        /// Filters audio to remove echo using Acoustic Echo Cancellation processing.
        /// Processes near-end and far-end audio streams to eliminate echo artifacts.
        /// </summary>
        /// <param name="nearend">The near-end audio chunk (microphone input) that may contain echo.</param>
        /// <param name="farend">The far-end audio chunk (speaker output) used as reference for echo removal.</param>
        /// <returns>The filtered audio chunk with echo removed, or the original nearend audio if filtering fails.</returns>
        public AudioChunk FilterAudio(AudioChunk nearend, AudioChunk farend)
        {
            if (!IsValid || farend == null || !farend.IsValid || nearend == null || !nearend.IsValid)
                return null;
            IntPtr result = InworldFrameworkUtil.Execute(InworldInterop.inworld_AECFilterInterface_FilterAudio
                    (m_DLLPtr, nearend.ToDLL, farend.ToDLL),
                InworldInterop.inworld_StatusOr_AudioChunk_status,
                InworldInterop.inworld_StatusOr_AudioChunk_ok,
                InworldInterop.inworld_StatusOr_AudioChunk_value,
                InworldInterop.inworld_StatusOr_AudioChunk_delete
            );
            return result != IntPtr.Zero ? new AudioChunk(result) : nearend;
        }
    }
}