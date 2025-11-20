/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using UnityEngine;

namespace Inworld.Framework.VAD
{
    /// <summary>
    /// Factory class for creating Voice Activity Detection (VAD) interfaces within the Inworld framework.
    /// Manages the creation and initialization of VAD components for speech detection operations.
    /// Used for instantiating VAD interfaces with proper configuration for local voice activity detection.
    /// </summary>
    public class VADFactory : InworldFactory
    { 
        /// <summary>
        /// Initializes a new instance of the VADFactory class with default settings.
        /// </summary>
        public VADFactory()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_VADFactory_new(), InworldInterop.inworld_VADFactory_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the VADFactory class from a native pointer.
        /// </summary>
        /// <param name="ptr">The native pointer to the VAD factory object.</param>
        public VADFactory(IntPtr ptr) => m_DLLPtr = MemoryManager.Register(ptr, InworldInterop.inworld_VADFactory_delete);

        /// <summary>
        /// Creates a VAD interface instance using the provided configuration.
        /// Instantiates and configures a VAD interface for voice activity detection operations.
        /// Currently only supports local VAD configurations; remote VAD is not supported.
        /// </summary>
        /// <param name="config">The configuration settings for the VAD interface. Must be a VADLocalConfig instance.</param>
        /// <returns>A VADInterface instance if creation succeeds; otherwise, null.</returns>
        public override InworldInterface CreateInterface(InworldConfig config)
        {
            if (config is not VADLocalConfig localConfig)
            {
                Debug.LogError("Remote VAD Not supported.\nVADFactory::CreateInterface: config is not a VADLocalConfig");
                return null;
            }
            Debug.Log("VAD ModelPath: " + localConfig.ModelPath);
            Debug.Log("VAD Device: " + localConfig.Device.Info.Name);
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_VADFactory_CreateVAD(m_DLLPtr, config.ToDLL),
                InworldInterop.inworld_StatusOr_VADInterface_status,
                InworldInterop.inworld_StatusOr_VADInterface_ok,
                InworldInterop.inworld_StatusOr_VADInterface_value,
                InworldInterop.inworld_StatusOr_VADInterface_delete
            );
            return result != IntPtr.Zero ? new VADInterface(result) : null;
        }
    }
}