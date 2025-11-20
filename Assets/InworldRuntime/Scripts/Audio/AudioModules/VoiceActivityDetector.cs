/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;

namespace Inworld.Framework.Audio
{
    /// <summary>
    /// Advanced voice activity detection module that uses the Inworld VAD system for speech detection.
    /// Extends PlayerVoiceDetector to provide more sophisticated voice activity analysis using native DLL functions.
    /// This detector provides more accurate results than simple volume-based detection.
    /// </summary>
    public class VoiceActivityDetector : PlayerVoiceDetector
    {
        /// <summary>
        /// Detects whether the player is currently speaking using the Inworld VAD (Voice Activity Detection) system.
        /// Uses native DLL functions to analyze audio characteristics beyond simple volume thresholds.
        /// </summary>
        /// <returns>True if voice activity is detected in the current audio data; otherwise, false.</returns>
        protected override bool DetectPlayerSpeaking()
        {
            if (!InworldController.Instance || !InworldController.VAD)
            {
                return false;
            }
            if (m_CurrentWaveData.Count == 0)
            {
                return false;
            }
                
            AudioChunk audioChunk = GetAudioChunk(m_CurrentWaveData);
            // YAN: In that DLL, -1 is negative, 0 and above is positive.
            return InworldController.VAD.DetectVoiceActivity(audioChunk) >= 0;
        }
        
        /// <summary>
        /// Converts a list of float audio samples into an InworldAudioChunk for processing by the VAD system.
        /// Creates the appropriate data structure required by the native DLL voice activity detection functions.
        /// </summary>
        /// <param name="audioData">The audio sample data as normalized float values.</param>
        /// <returns>An InworldAudioChunk object containing the audio data formatted for VAD processing.</returns>
        public AudioChunk GetAudioChunk(List<float> audioData)
        {
            AudioChunk chunk = new AudioChunk();
            InworldVector<float> floatArray = new InworldVector<float>();
            foreach (float data in audioData)
            {
                floatArray.Add(data);
            }
            chunk.SampleRate = 16000;
            chunk.Data = floatArray;
            return chunk;
        }
    }
}