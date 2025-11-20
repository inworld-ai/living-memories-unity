/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.Node
{
    /// <summary>
    /// Represents a Text-to-Speech (TTS) node for converting text data to audio within the Inworld framework.
    /// Processes text input and generates corresponding audio output using TTS capabilities.
    /// Used for implementing voice synthesis functionality in AI conversation workflows.
    /// </summary>
    public class TTSNode : InworldNode
    {
        public TTSNode(string nodeName, TTSNodeExecutionConfig executionConfig)
        {
            IntPtr genResult = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_TTSNode_Create(nodeName, executionConfig.ToDLL),
                InworldInterop.inworld_StatusOr_SharedPtr_TTSNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_TTSNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_TTSNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_TTSNode_delete);
            if (genResult != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(genResult, InworldInterop.inworld_TTSNode_delete);
        }
        /// <summary>
        /// Initializes a new instance of the TTSNode class from a native pointer.
        /// </summary>
        /// <param name="dllPtr">The native pointer to the TTS node object.</param>
        public TTSNode(IntPtr dllPtr) 
        {
            m_DLLPtr = MemoryManager.Register(dllPtr, InworldInterop.inworld_TTSNode_delete);
        }
                
        /// <summary>
        /// Gets the unique identifier of this TTS node.
        /// Overrides the base implementation to provide TTS-specific identification.
        /// </summary>
        public override string ID => InworldInterop.inworld_TTSNode_id(m_DLLPtr);

        /// <summary>
        /// Gets a value indicating whether this TTS node is in a valid state for execution.
        /// Overrides the base implementation to provide TTS-specific validation logic.
        /// </summary>
        public override bool IsValid => InworldInterop.inworld_TTSNode_is_valid(m_DLLPtr);
        
        // TODO(Yan): The process is unavailable in the current DLL (Aug 15)
    }
}