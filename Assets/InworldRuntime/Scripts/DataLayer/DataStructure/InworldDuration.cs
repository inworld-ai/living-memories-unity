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
    /// Represents duration or time span information within the Inworld framework.
    /// Used for tracking elapsed time, timeouts, and temporal measurements
    /// in various operations and processes.
    /// </summary>
    public class InworldDuration : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldDuration class with default settings.
        /// Creates a new native duration object and registers it with the memory manager.
        /// </summary>
        public InworldDuration()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_Duration_new(), InworldInterop.inworld_Duration_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the InworldDuration class from an existing native pointer.
        /// Used for wrapping existing native duration objects created by the C++ library.
        /// </summary>
        /// <param name="rhs">Native pointer to an existing duration instance.</param>
        public InworldDuration(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_Duration_delete);;
        }
    }
}