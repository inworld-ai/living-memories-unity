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
    /// Provides detailed information about a specific hardware device.
    /// Contains device name, memory statistics, and other hardware characteristics.
    /// Used for device monitoring and resource management decisions.
    /// </summary>
    public class DeviceInfo : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldDeviceInfo class with default values.
        /// </summary>
        public DeviceInfo()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_DeviceInfo_new(), InworldInterop.inworld_DeviceInfo_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the InworldDeviceInfo class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the device info object.</param>
        public DeviceInfo(IntPtr rhs) => m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_DeviceInfo_delete);

        /// <summary>
        /// Gets the human-readable name of the device.
        /// Typically includes manufacturer and model information.
        /// </summary>
        public string Name => InworldInterop.inworld_DeviceInfo_name_get(m_DLLPtr);
        
        /// <summary>
        /// Gets the amount of free memory available on the device in bytes.
        /// Useful for determining if the device has sufficient memory for operations.
        /// </summary>
        public ulong FreeMemoryBytes => InworldInterop.inworld_DeviceInfo_free_memory_bytes_get(m_DLLPtr);
        
        /// <summary>
        /// Gets the total memory capacity of the device in bytes.
        /// Represents the maximum memory that can be allocated on this device.
        /// </summary>
        public ulong TotalMemoryBytes => InworldInterop.inworld_DeviceInfo_total_memory_bytes_get(m_DLLPtr);
    }
}