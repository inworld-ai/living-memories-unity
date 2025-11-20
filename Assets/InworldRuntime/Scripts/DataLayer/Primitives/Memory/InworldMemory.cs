/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

namespace Inworld.Framework.Memory
{
    /// <summary>
    /// Abstract base class for memory components within the Inworld framework.
    /// Provides the foundation for different types of memory storage and retrieval systems.
    /// Used as a base for implementing specific memory types like flash memory and long-term memory.
    /// </summary>
    public abstract class InworldMemory : InworldFrameworkDllClass
    {
        /// <summary>
        /// Gets or sets the knowledge collection associated with this memory instance.
        /// Contains the knowledge records that are stored and managed by this memory component.
        /// </summary>
        public abstract KnowledgeCollection KnowledgeCollection { get; set; }
    }
}