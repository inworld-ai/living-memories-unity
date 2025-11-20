/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;


namespace Inworld.Framework.Memory
{
    /// <summary>
    /// Factory class for creating memory interfaces within the Inworld framework.
    /// Manages the creation and initialization of memory components for conversation state management.
    /// Currently serves as a placeholder for future memory interface creation functionality.
    /// </summary>
    public class MemoryFactory : InworldFactory
    {
        /// <summary>
        /// Initializes a new instance of the MemoryFactory class with default settings.
        /// </summary>
        public MemoryFactory()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_MemoryFactory_new(), InworldInterop.inworld_MemoryFactory_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the MemoryFactory class from a native pointer.
        /// </summary>
        /// <param name="ptr">The native pointer to the memory factory object.</param>
        public MemoryFactory(IntPtr ptr) => m_DLLPtr = MemoryManager.Register(ptr, InworldInterop.inworld_MemoryFactory_delete);

        /// <summary>
        /// Creates a memory interface instance using the provided configuration.
        /// Currently not implemented and throws NotImplementedException.
        /// </summary>
        /// <param name="config">The configuration settings for the memory interface.</param>
        /// <returns>A memory interface instance (currently throws NotImplementedException).</returns>
        /// <exception cref="NotImplementedException">This method is not yet implemented.</exception>
        public override InworldInterface CreateInterface(InworldConfig config)
        {
            throw new NotImplementedException();
        }
    }
}