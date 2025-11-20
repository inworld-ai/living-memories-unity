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
    /// Represents a Speech-to-Text (STT) node for converting audio data to text within the Inworld framework.
    /// Processes audio input and generates corresponding text output using speech recognition capabilities.
    /// Used for implementing voice recognition functionality in AI conversation workflows.
    /// </summary>
    public class STTNode : InworldNode
    {
        public STTNode(string nodeName, STTNodeExecutionConfig executionConfig)
        {
            IntPtr genResult = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_STTNode_Create(nodeName, executionConfig.ToDLL),
                InworldInterop.inworld_StatusOr_SharedPtr_STTNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_STTNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_STTNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_STTNode_delete);
            if (genResult != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(genResult, InworldInterop.inworld_STTNode_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the STTNode class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the STT node object.</param>
        public STTNode(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_STTNode_delete);
        }

        public override bool IsValid => InworldInterop.inworld_STTNode_is_valid(m_DLLPtr);
        
        /// <summary>
        /// Gets the unique identifier of this STT node.
        /// Overrides the base implementation to provide STT-specific identification.
        /// </summary>
        public override string ID => InworldInterop.inworld_STTNode_id(m_DLLPtr);
        
        // TODO(Yan): Add back Process function as the current dll (Aug 15) is unavailable.
        // public InworldText Process(ProcessContext processContext, InworldAudio audio)
        // {
        //     IntPtr result = InworldFrameworkUtil.Execute(
        //         InworldInterop.inworld_STTNode_Process(m_DLLPtr, processContext.ToDLL, audio.ToDLL), 
        //         InworldInterop.inworld_StatusOr_SharedPtr_Text_status,
        //         InworldInterop.inworld_StatusOr_SharedPtr_Text_ok,
        //         InworldInterop.inworld_StatusOr_SharedPtr_Text_value,
        //         InworldInterop.inworld_StatusOr_SharedPtr_Text_delete
        //     );
        //     return result != IntPtr.Zero ? new InworldText(result) : null;
        // }
        
    }
}