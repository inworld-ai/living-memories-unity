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
    /// Factory class for creating Speech-to-Text (STT) interfaces within the Inworld framework.
    /// Manages the creation and initialization of STT components for speech recognition operations.
    /// Used for instantiating STT interfaces with proper configuration for both local and remote speech recognition services.
    /// </summary>
    public class STTFactory : InworldFactory
    {
        /// <summary>
        /// Initializes a new instance of the STTFactory class with default settings.
        /// </summary>
        public STTFactory()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_STTFactory_new(), InworldInterop.inworld_STTFactory_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the STTFactory class from a native pointer.
        /// </summary>
        /// <param name="ptr">The native pointer to the STT factory object.</param>
        public STTFactory(IntPtr ptr) => m_DLLPtr = MemoryManager.Register(ptr, InworldInterop.inworld_STTFactory_delete);
        
        /// <summary>
        /// Creates an STT interface instance using the provided configuration.
        /// Instantiates and configures an STT interface for speech recognition operations.
        /// Supports both remote and local speech recognition configurations.
        /// </summary>
        /// <param name="config">The configuration settings for the STT interface. Must be either STTRemoteConfig or STTLocalConfig.</param>
        /// <returns>An STTInterface instance if creation succeeds; otherwise, null.</returns>
        public override InworldInterface CreateInterface(InworldConfig config)
        {
            if (config is STTRemoteConfig remoteConfig)
                return CreateInterface(remoteConfig);
            if (config is STTLocalConfig localConfig)
                return CreateInterface(localConfig); 
            return null;
        }
        
        /// <summary>
        /// Creates an STT interface using remote configuration settings.
        /// Configures the STT system to use remote speech recognition services.
        /// </summary>
        /// <param name="remoteConfig">The remote configuration for STT processing.</param>
        /// <returns>An STTInterface instance if creation succeeds; otherwise, null.</returns>
        public InworldInterface CreateInterface(STTRemoteConfig remoteConfig)
        {
            
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_STTFactory_CreateSTT_rcinworld_RemoteSTTConfig(m_DLLPtr, remoteConfig.ToDLL),
                InworldInterop.inworld_StatusOr_STTInterface_status,
                InworldInterop.inworld_StatusOr_STTInterface_ok,
                InworldInterop.inworld_StatusOr_STTInterface_value,
                InworldInterop.inworld_StatusOr_STTInterface_delete
            );
            return result != IntPtr.Zero ? new STTInterface(result) : null;
        }
        
        /// <summary>
        /// Creates an STT interface using local configuration settings.
        /// Configures the STT system to use local speech recognition models.
        /// </summary>
        /// <param name="localConfig">The local configuration for STT processing.</param>
        /// <returns>An STTInterface instance if creation succeeds; otherwise, null.</returns>
        public InworldInterface CreateInterface(STTLocalConfig localConfig)
        {
            
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_STTFactory_CreateSTT_rcinworld_LocalSTTConfig(m_DLLPtr, localConfig.ToDLL),
                InworldInterop.inworld_StatusOr_STTInterface_status,
                InworldInterop.inworld_StatusOr_STTInterface_ok,
                InworldInterop.inworld_StatusOr_STTInterface_value,
                InworldInterop.inworld_StatusOr_STTInterface_delete
            );
            return result != IntPtr.Zero ? new STTInterface(result) : null;
        }
    }
}