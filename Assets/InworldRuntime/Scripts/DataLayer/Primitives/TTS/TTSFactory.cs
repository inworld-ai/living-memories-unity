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
    /// Factory class for creating Text-to-Speech (TTS) interfaces within the Inworld framework.
    /// Manages the creation and initialization of TTS components for speech synthesis operations.
    /// Used for instantiating TTS interfaces with proper configuration for both local and remote speech synthesis services.
    /// </summary>
    public class TTSFactory : InworldFactory
    {
        /// <summary>
        /// Initializes a new instance of the TTSFactory class with default settings.
        /// </summary>
        public TTSFactory()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_TTSFactory_new(), InworldInterop.inworld_TTSFactory_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the TTSFactory class from a native pointer.
        /// </summary>
        /// <param name="ptr">The native pointer to the TTS factory object.</param>
        public TTSFactory(IntPtr ptr) => m_DLLPtr = MemoryManager.Register(ptr, InworldInterop.inworld_TTSFactory_delete);

        /// <summary>
        /// Creates a TTS interface instance using the provided configuration.
        /// Instantiates and configures a TTS interface for speech synthesis operations.
        /// Supports both remote and local speech synthesis configurations.
        /// </summary>
        /// <param name="config">The configuration settings for the TTS interface. Must be either TTSRemoteConfig or TTSLocalConfig.</param>
        /// <returns>A TTSInterface instance if creation succeeds; otherwise, null.</returns>
        public override InworldInterface CreateInterface(InworldConfig config)
        {
            if (config is TTSRemoteConfig remoteConfig)
                return CreateInterface(remoteConfig);
            if (config is TTSLocalConfig localConfig)
                return CreateInterface(localConfig); 
            return null;
        }
        
        /// <summary>
        /// Creates a TTS interface using remote configuration settings.
        /// Configures the TTS system to use remote speech synthesis services.
        /// </summary>
        /// <param name="remoteConfig">The remote configuration for TTS processing.</param>
        /// <returns>A TTSInterface instance if creation succeeds; otherwise, null.</returns>
        public InworldInterface CreateInterface(TTSRemoteConfig remoteConfig)
        {
            
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_TTSFactory_CreateTTS_rcinworld_RemoteTTSConfig(m_DLLPtr, remoteConfig.ToDLL),
                InworldInterop.inworld_StatusOr_TTSInterface_status,
                InworldInterop.inworld_StatusOr_TTSInterface_ok,
                InworldInterop.inworld_StatusOr_TTSInterface_value,
                InworldInterop.inworld_StatusOr_TTSInterface_delete
            );
            return result != IntPtr.Zero ? new TTSInterface(result) : null;
        }
        
        /// <summary>
        /// Creates a TTS interface using local configuration settings.
        /// Configures the TTS system to use local speech synthesis models.
        /// </summary>
        /// <param name="localConfig">The local configuration for TTS processing.</param>
        /// <returns>A TTSInterface instance if creation succeeds; otherwise, null.</returns>
        public InworldInterface CreateInterface(TTSLocalConfig localConfig)
        {
            
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_TTSFactory_CreateTTS_rcinworld_LocalTTSConfig(m_DLLPtr, localConfig.ToDLL),
                InworldInterop.inworld_StatusOr_TTSInterface_status,
                InworldInterop.inworld_StatusOr_TTSInterface_ok,
                InworldInterop.inworld_StatusOr_TTSInterface_value,
                InworldInterop.inworld_StatusOr_TTSInterface_delete
            );
            return result != IntPtr.Zero ? new TTSInterface(result) : null;
        }
    }
}