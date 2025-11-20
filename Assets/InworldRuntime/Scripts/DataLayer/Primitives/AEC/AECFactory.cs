/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using UnityEngine;

namespace Inworld.Framework.AEC
{
    /// <summary>
    /// Factory class for creating Acoustic Echo Cancellation (AEC) interfaces within the Inworld framework.
    /// Manages the creation and initialization of AEC filter components for audio processing.
    /// Used for instantiating AEC interfaces with proper configuration and resource management.
    /// </summary>
    public class AECFactory : InworldFactory
    {
        /// <summary>
        /// Initializes a new instance of the AECFactory class with default settings.
        /// </summary>
        public AECFactory()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_AECFilterFactory_new(),
                InworldInterop.inworld_AECFilterFactory_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the AECFactory class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the AEC factory object.</param>
        public AECFactory(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_AECFilterFactory_delete);
        }
        
        /// <summary>
        /// Creates an AEC interface instance using the provided configuration.
        /// Instantiates and configures an AEC filter interface for audio processing.
        /// </summary>
        /// <param name="config">The configuration settings for the AEC interface. Must be an AECLocalConfig instance.</param>
        /// <returns>An AECInterface instance if creation succeeds; otherwise, null.</returns>
        public override InworldInterface CreateInterface(InworldConfig config)
        {
            if (!(config is AECLocalConfig localConfig))
                return null;
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_AECFilterFactory_CreateAECFilter(m_DLLPtr, localConfig.ToDLL),
                InworldInterop.inworld_StatusOr_AECFilterInterface_status,
                InworldInterop.inworld_StatusOr_AECFilterInterface_ok,
                InworldInterop.inworld_StatusOr_AECFilterInterface_value,
                InworldInterop.inworld_StatusOr_AECFilterInterface_delete
            );
            return result != IntPtr.Zero ? new AECInterface(result) : null;
        }
    }
}