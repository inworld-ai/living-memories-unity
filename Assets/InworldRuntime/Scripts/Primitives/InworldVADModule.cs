/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Framework.VAD;
using Inworld.Framework.Node;
using UnityEngine;

namespace Inworld.Framework.Primitive
{
    /// <summary>
    /// Module for Voice Activity Detection (VAD) within the Inworld framework.
    /// Analyzes audio input to determine when speech is present versus silence or background noise.
    /// Used for optimizing speech processing by filtering out non-speech audio segments.
    /// Note: Only supports local model execution; remote VAD is not available.
    /// </summary>
    public class InworldVADModule : InworldFrameworkModule
    {
        [Range(0, 1)] [SerializeField] float m_Threshold = 0.3f;

        VoiceActivityDetectionConfig m_VADConfig;
        
        /// <summary>
        /// Creates and returns a VADFactory for this module.
        /// </summary>
        /// <returns>A factory instance for creating voice activity detection objects.</returns>
        public override InworldFactory CreateFactory() => m_Factory ??= new VADFactory();

        /// <summary>
        /// Sets up the configuration for voice activity detection operations.
        /// Configures the local VAD model path and detection threshold.
        /// Note: Remote VAD is not supported and will return null.
        /// </summary>
        /// <returns>A VAD configuration instance for module initialization, or null if remote mode is selected.</returns>
        public override InworldConfig SetupConfig()
        {
            if (ModelType == ModelType.Remote)
            {
                Debug.LogError("VAD Does not support Remote Mode.");
                return null;
            }
            VADLocalConfig localConfig = new VADLocalConfig();
            localConfig.ModelPath = $"{Application.streamingAssetsPath}/{InworldFrameworkUtil.VADModelPath}";
            localConfig.Device = InworldFrameworkUtil.GetDevice(ModelType);
            m_VADConfig = new VoiceActivityDetectionConfig();
            m_VADConfig.Threshold = m_Threshold;
            localConfig.Config = m_VADConfig;
            return localConfig;
        }

        /// <summary>
        /// Detects voice activity in the provided audio chunk.
        /// Analyzes the audio data to determine if speech is present based on the configured threshold.
        /// </summary>
        /// <param name="audioChunk">The audio data to analyze for voice activity.</param>
        /// <returns>1 if voice activity is detected, 0 if no voice activity, or -1 if detection failed.</returns>
        public int DetectVoiceActivity(AudioChunk audioChunk)
        {
            if (!Initialized || !(m_Interface is VADInterface vadInterface))
                return -1;
            return vadInterface.DetectVoiceActivity(audioChunk, m_VADConfig);
        }
    }
}