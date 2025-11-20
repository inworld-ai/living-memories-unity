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
    /// Provides configuration settings for Inworld's native speech synthesis within the Inworld framework.
    /// Defines parameters for speech synthesis including preprocessing, inference, and post-processing stages.
    /// Used for configuring Inworld's proprietary text-to-speech synthesis pipeline and model settings.
    /// </summary>
    public class InworldSynthesizeConfig : InworldConfig
    {
        /// <summary>
        /// Initializes a new instance of the InworldSynthesizeConfig class with default settings.
        /// </summary>
        public InworldSynthesizeConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_InworldSpeechSynthesisConfig_new(), 
                InworldInterop.inworld_InworldSpeechSynthesisConfig_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the InworldSynthesizeConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the Inworld synthesis config object.</param>
        public InworldSynthesizeConfig(IntPtr rhs) => 
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_InworldSpeechSynthesisConfig_delete);

        /// <summary>
        /// Gets or sets the model identifier for speech synthesis.
        /// Specifies which speech synthesis model variant to use for text-to-speech conversion.
        /// </summary>
        public string ModelID
        {
            set => InworldInterop.inworld_InworldSpeechSynthesisConfig_model_id_set(m_DLLPtr, value);
            get => InworldInterop.inworld_InworldSpeechSynthesisConfig_model_id_get(m_DLLPtr);
        }

        /// <summary>
        /// Gets or sets the post-processing configuration for speech synthesis.
        /// Defines how synthesized audio should be processed after generation (e.g., sample rate, silence trimming).
        /// </summary>
        public SpeechPostProcessingConfig PostProcessing
        {
            set => InworldInterop.inworld_InworldSpeechSynthesisConfig_postprocessing_set(m_DLLPtr, value.ToDLL);
            get => new SpeechPostProcessingConfig(InworldInterop.inworld_InworldSpeechSynthesisConfig_postprocessing_get(m_DLLPtr)); 
        }

        /// <summary>
        /// Gets or sets the inference configuration for speech synthesis.
        /// Defines model inference parameters such as diffusion steps, alpha/beta values, and speech tempo.
        /// </summary>
        public SpeechInferenceConfig Inference
        {
            set => InworldInterop.inworld_InworldSpeechSynthesisConfig_inference_set(m_DLLPtr, value.ToDLL);
            get => new SpeechInferenceConfig(InworldInterop.inworld_InworldSpeechSynthesisConfig_inference_get(m_DLLPtr)); 
        }
    }
}