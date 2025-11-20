/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework
{
    /// <summary>
    /// Represents a hardware device available for computation within the Inworld framework.
    /// Wraps a native device object that provides information about device type, index, and detailed info.
    /// Used for device management and selection in resource allocation.
    /// </summary>
    public class InworldDevice : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldDevice class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the device object.</param>
        public InworldDevice(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_Device_delete);
        }

        /// <summary>
        /// Gets the type of this device (CPU, GPU, etc.).
        /// </summary>
        public DeviceType Type => (DeviceType)InworldInterop.inworld_Device_type(m_DLLPtr);
        
        /// <summary>
        /// Gets the index of this device within devices of the same type.
        /// </summary>
        public short Index => InworldInterop.inworld_Device_index(m_DLLPtr);
        
        /// <summary>
        /// Gets detailed information about this device such as name and memory usage.
        /// </summary>
        public DeviceInfo Info => new(InworldInterop.inworld_Device_info(m_DLLPtr));
    }
}