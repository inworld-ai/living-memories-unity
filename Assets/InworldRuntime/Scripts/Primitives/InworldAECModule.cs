/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Framework.AEC;
using Inworld.Framework.Attributes;


namespace Inworld.Framework.Primitive
{
    /// <summary>
    /// Module for Acoustic Echo Cancellation (AEC) within the Inworld framework.
    /// Processes audio streams to remove echo and feedback from microphone input.
    /// Uses CPU-based local processing only and does not support remote operation.
    /// Essential for clear audio communication in applications with both input and output audio.
    /// </summary>
    [ModelType("LocalCPU", LockAlways = true)]
    public class InworldAECModule : InworldFrameworkModule
    {
        void Awake()
        {
            // YAN: Has to be localCPU.
            ModelType = ModelType.LocalCPU;
        }

        /// <summary>
        /// Creates and returns an AECFactory for this module.
        /// </summary>
        /// <returns>A factory instance for creating acoustic echo cancellation objects.</returns>
        public override InworldFactory CreateFactory()
        {
            m_Factory ??= new AECFactory();
            return m_Factory;
        }

        /// <summary>
        /// Sets up the configuration for acoustic echo cancellation operations.
        /// Only supports local CPU processing; remote operation is not available.
        /// </summary>
        /// <returns>An AEC configuration instance for module initialization, or null if remote mode is attempted.</returns>
        public override InworldConfig SetupConfig()
        {
            if (ModelType == ModelType.Remote)
            {
                return null;
            }
            AECLocalConfig localConfig = new AECLocalConfig();
            localConfig.Device = InworldFrameworkUtil.GetDevice(ModelType);
            return localConfig;
        }
        
        /// <summary>
        /// Filters audio to remove echo and feedback using acoustic echo cancellation.
        /// Processes both near-end (microphone) and far-end (speaker) audio to produce clean output.
        /// </summary>
        /// <param name="nearend">The microphone audio input that may contain echo.</param>
        /// <param name="farend">The speaker audio output that creates potential echo.</param>
        /// <returns>The filtered audio chunk with echo removed, or null if processing failed.</returns>
        public AudioChunk FilterAudio(AudioChunk nearend, AudioChunk farend)
        {
            if (!Initialized || !(m_Interface is AECInterface aecInterface))
                return null;
            
            return aecInterface.FilterAudio(nearend, farend);
        }
    }
}