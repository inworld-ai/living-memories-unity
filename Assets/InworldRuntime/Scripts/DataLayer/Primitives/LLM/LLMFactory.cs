/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using UnityEngine;

namespace Inworld.Framework.LLM
{
    /// <summary>
    /// Factory class for creating Large Language Model (LLM) interfaces within the Inworld framework.
    /// Manages the creation and initialization of LLM components for text generation and conversation processing.
    /// Used for instantiating LLM interfaces with proper configuration for both local and remote LLM services.
    /// </summary>
    public class LLMFactory : InworldFactory
    { 
        /// <summary>
        /// Initializes a new instance of the LLMFactory class with default settings.
        /// </summary>
        public LLMFactory()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LLMFactory_new(), InworldInterop.inworld_LLMFactory_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the LLMFactory class from a native pointer.
        /// </summary>
        /// <param name="ptr">The native pointer to the LLM factory object.</param>
        public LLMFactory(IntPtr ptr) => m_DLLPtr = MemoryManager.Register(ptr, InworldInterop.inworld_LLMFactory_delete);

        /// <summary>
        /// Creates an LLM interface instance using the provided configuration.
        /// Instantiates and configures an LLM interface for text generation operations.
        /// Supports both remote and local LLM configurations.
        /// </summary>
        /// <param name="config">The configuration settings for the LLM interface. Must be either LLMRemoteConfig or LLMLocalConfig.</param>
        /// <returns>An LLMInterface instance if creation succeeds; otherwise, null.</returns>
        public override InworldInterface CreateInterface(InworldConfig config)
        {
            if (config is LLMRemoteConfig remoteConfig)
                return CreateInterface(remoteConfig);
            if (config is LLMLocalConfig localConfig)
                return CreateInterface(localConfig);
            return null;
        }
        
        /// <summary>
        /// Creates an LLM interface using remote configuration settings.
        /// Configures the LLM system to use remote LLM services for content generation.
        /// </summary>
        /// <param name="config">The remote configuration for LLM processing.</param>
        /// <returns>An LLMInterface instance if creation succeeds; otherwise, null.</returns>
        public InworldInterface CreateInterface(LLMRemoteConfig config)
        {            
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_LLMFactory_CreateLLM_rcinworld_RemoteLLMConfig(m_DLLPtr, config.ToDLL),
                InworldInterop.inworld_StatusOr_LLMInterface_status,
                InworldInterop.inworld_StatusOr_LLMInterface_ok,
                InworldInterop.inworld_StatusOr_LLMInterface_value,
                InworldInterop.inworld_StatusOr_LLMInterface_delete
            );
            return result != IntPtr.Zero ? new LLMInterface(result) : null;
        }
        
        /// <summary>
        /// Creates an LLM interface using local configuration settings.
        /// Configures the LLM system to use local LLM models for content generation.
        /// Logs model path and device information for debugging purposes.
        /// </summary>
        /// <param name="config">The local configuration for LLM processing.</param>
        /// <returns>An LLMInterface instance if creation succeeds; otherwise, null.</returns>
        public InworldInterface CreateInterface(LLMLocalConfig config)
        {
            Debug.Log("ModelPath: " + config.ModelPath);
            Debug.Log("Device: " + config.Device.Info.Name);
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_LLMFactory_CreateLLM_rcinworld_LocalLLMConfig(m_DLLPtr, config.ToDLL),
                InworldInterop.inworld_StatusOr_LLMInterface_status,
                InworldInterop.inworld_StatusOr_LLMInterface_ok,
                InworldInterop.inworld_StatusOr_LLMInterface_value,
                InworldInterop.inworld_StatusOr_LLMInterface_delete
            );
            return result != IntPtr.Zero ? new LLMInterface(result) : null;
        }
    }
}