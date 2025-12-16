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
    /// Represents a random canned text node for generating random responses from predefined text options.
    /// Selects and returns random text from a collection of pre-written responses.
    /// Used for implementing fallback responses and variety in AI conversation workflows.
    /// </summary>
    public class RandomCannedTextNode : InworldNode
    {
        /// <summary>
        /// Initializes a new instance of the RandomCannedTextNode class with predefined text options.
        /// Creates a node that can randomly select from the provided text collection.
        /// </summary>
        /// <param name="nodeName">The name identifier for this random canned text node.</param>
        /// <param name="executionConfig"></param>
        public RandomCannedTextNode(string nodeName, RandomCannedTextNodeExecutionConfig executionConfig)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_RandomCannedTextNode_Create(nodeName, executionConfig.ToDLL),
                InworldInterop.inworld_StatusOr_SharedPtr_RandomCannedTextNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_RandomCannedTextNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_RandomCannedTextNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_RandomCannedTextNode_delete
            );
            if (result != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(result, InworldInterop.inworld_RandomCannedTextNode_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the RandomCannedTextNode class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the random canned text node object.</param>
        public RandomCannedTextNode(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_RandomCannedTextNode_delete);
        }

        /// <summary>
        /// Gets a value indicating whether this random canned text node is in a valid state for execution.
        /// Overrides the base implementation to provide random text-specific validation logic.
        /// </summary>
        public override bool IsValid => InworldInterop.inworld_RandomCannedTextNode_is_valid(m_DLLPtr);

        /// <summary>
        /// Gets the unique identifier of this random canned text node.
        /// Overrides the base implementation to provide random text-specific identification.
        /// </summary>
        public override string ID => InworldInterop.inworld_RandomCannedTextNode_id(m_DLLPtr);
        
        /// <summary>
        /// Processes input data and returns a randomly selected canned text response.
        /// Hides the base Process method to provide specialized random text selection behavior.
        /// </summary>
        /// <param name="data">The input data vector (typically not used for random text selection).</param>
        /// <returns>A randomly selected text response from the predefined collection, or null if processing fails.</returns>
        public new InworldBaseData Process(ProcessContext processContext, InworldVector<InworldBaseData> data)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_RandomCannedTextNode_Process(m_DLLPtr, processContext.ToDLL, data.ToDLL), 
                InworldInterop.inworld_StatusOr_BaseData_status,
                InworldInterop.inworld_StatusOr_BaseData_ok,
                InworldInterop.inworld_StatusOr_BaseData_value,
                InworldInterop.inworld_StatusOr_BaseData_delete
            );
            return result != IntPtr.Zero ? new InworldBaseData(result) : null;
        }
    }
}