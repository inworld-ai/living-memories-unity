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
    /// Represents the properties and configuration of an AI model within the Inworld framework.
    /// Provides access to model identification and settings for model management and execution.
    /// Used for configuring and tracking AI model instances.
    /// </summary>
    public class ModelProperties : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldModelProperties class with default settings.
        /// </summary>
        public ModelProperties()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_ModelProperties_new(),
                InworldInterop.inworld_ModelProperties_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the InworldModelProperties class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the model properties object.</param>
        public ModelProperties(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_ModelProperties_delete);
        }
        
        /// <summary>
        /// Gets or sets the unique identifier of the AI model.
        /// Used to specify which model variant should be loaded and executed.
        /// </summary>
        public string ModelID
        {
            get => InworldInterop.inworld_ModelProperties_model_id_get(m_DLLPtr);
            set => InworldInterop.inworld_ModelProperties_model_id_set(m_DLLPtr, value);
        }
    }
}