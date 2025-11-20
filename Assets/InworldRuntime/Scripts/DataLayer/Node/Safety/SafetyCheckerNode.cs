/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.Safety;

namespace Inworld.Framework.Node
{
    /// <summary>
    /// Represents a safety checker node for content moderation within the Inworld framework.
    /// Processes text input to detect potentially harmful or inappropriate content.
    /// Used for implementing content safety and moderation in AI conversation workflows.
    /// </summary>
    public class SafetyCheckerNode : InworldNode
    {
        public SafetyCheckerNode(string nodeName, InworldCreationContext inworldCreationContext,
            SafetyCheckerNodeCreationConfig creationConfig, SafetyCheckerNodeExecutionConfig executionConfig)
        {
            IntPtr genResult = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_SafetyCheckerNode_Create(nodeName, inworldCreationContext.ToDLL, creationConfig.ToDLL, executionConfig.ToDLL),
                InworldInterop.inworld_StatusOr_SharedPtr_SafetyCheckerNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_SafetyCheckerNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_SafetyCheckerNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_SafetyCheckerNode_delete);
            if (genResult != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(genResult, InworldInterop.inworld_SafetyCheckerNode_delete);
        }
        /// <summary>
        /// Initializes a new instance of the SafetyCheckerNode class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the safety checker node object.</param>
        public SafetyCheckerNode(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_SafetyCheckerNode_delete);
        }
        
        /// <summary>
        /// Gets the unique identifier of this safety checker node.
        /// Overrides the base implementation to provide safety-specific identification.
        /// </summary>
        public override string ID => InworldInterop.inworld_SafetyCheckerNode_id(m_DLLPtr);

        public override bool IsValid => InworldInterop.inworld_SafetyCheckerNode_is_valid(m_DLLPtr);

        /// <summary>
        /// Processes text input to perform safety analysis and content moderation.
        /// Analyzes the text for potentially harmful or inappropriate content.
        /// </summary>
        /// <param name="text">The text input to analyze for safety violations.</param>
        /// <returns>Safety analysis results with detected issues and severity levels, or null if processing fails.</returns>
        public SafetyResult Process(ProcessContext processContext, InworldText text)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_SafetyCheckerNode_Process(m_DLLPtr, processContext.ToDLL, text.ToDLL), 
                InworldInterop.inworld_StatusOr_SharedPtr_SafetyResult_status,
                InworldInterop.inworld_StatusOr_SharedPtr_SafetyResult_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_SafetyResult_value,
                InworldInterop.inworld_StatusOr_SharedPtr_SafetyResult_delete
            );
            return result != IntPtr.Zero ? new SafetyResult(result) : null;
        }
        
    }
}