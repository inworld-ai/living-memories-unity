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
    /// Represents a collection of available devices for computation within the Inworld framework.
    /// Provides vector-like operations for managing and accessing device objects.
    /// Used for device enumeration, selection, and management in resource allocation.
    /// </summary>
    public class Devices : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldDevices class with devices from the system registry.
        /// Automatically populates the collection with all available devices.
        /// </summary>
        public Devices()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_DeviceRegistry_GetAvailableDevices
                (InworldInterop.inworld_DeviceRegistry_GetInstance()),InworldInterop.inworld_vector_Device_delete); 
        }
        
        /// <summary>
        /// Initializes a new instance of the InworldDevices class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the device collection.</param>
        public Devices(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_vector_Device_delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldDevices class by copying another device collection.
        /// </summary>
        /// <param name="rhs">The source device collection to copy from.</param>
        public Devices(Devices rhs)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_vector_Device_copy(rhs.ToDLL), 
                InworldInterop.inworld_vector_Device_delete);
        }
        
        /// <summary>
        /// Gets the number of devices in the collection.
        /// </summary>
        public int Size => InworldInterop.inworld_vector_Device_size(m_DLLPtr);
        
        /// <summary>
        /// Gets the capacity of the device collection (total allocated space).
        /// </summary>
        public int Capacity => InworldInterop.inworld_vector_Device_capacity(m_DLLPtr);

        /// <summary>
        /// Reserves memory capacity for the specified number of devices.
        /// </summary>
        /// <param name="capacity">The number of devices to reserve capacity for.</param>
        public void Reserve(int capacity) => InworldInterop.inworld_vector_Device_reserve(m_DLLPtr, capacity);

        /// <summary>
        /// Gets a value indicating whether the device collection is empty.
        /// </summary>
        public bool IsEmpty => InworldInterop.inworld_vector_Device_empty(m_DLLPtr);
        
        /// <summary>
        /// Removes all devices from the collection.
        /// </summary>
        public void Clear() => InworldInterop.inworld_vector_Device_clear(m_DLLPtr);

        /// <summary>
        /// Adds a device to the end of the collection.
        /// </summary>
        /// <param name="device">The device to add to the collection.</param>
        public void Add(InworldDevice device) => InworldInterop.inworld_vector_Device_push_back(m_DLLPtr, device.ToDLL);

        /// <summary>
        /// Gets or sets the device at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the device to get or set.</param>
        /// <returns>The device at the specified index.</returns>
        public InworldDevice this[int index]
        {
            get => new(InworldInterop.inworld_vector_Device_get(m_DLLPtr, index));
            set => InworldInterop.inworld_vector_Device_set(m_DLLPtr, index, value.ToDLL);
        }
    }
}