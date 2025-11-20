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
    /// Represents a goal advancement node for tracking and progressing goals within the Inworld framework.
    /// Manages goal state changes and progression logic for conversation-driven objectives.
    /// Used for implementing goal-oriented dialogue systems and achievement tracking.
    /// </summary>
    public class GoalAdvancementNode : InworldNode
    {
        public GoalAdvancementNode(string name, GoalAdvancementNodeCreationConfig creationConfig, GoalAdvancementNodeExecutionConfig executionConfig)
        {
            IntPtr genResult = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_GoalAdvancementNode_Create(name, creationConfig.ToDLL, executionConfig.ToDLL),
                InworldInterop.inworld_StatusOr_SharedPtr_GoalAdvancementNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_GoalAdvancementNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_GoalAdvancementNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_GoalAdvancementNode_delete);
            if (genResult != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(genResult, InworldInterop.inworld_GoalAdvancementNode_delete);
        }
        /// <summary>
        /// Initializes a new instance of the GoalAdvancementNode class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the goal advancement node object.</param>
        public GoalAdvancementNode(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_GoalAdvancementNode_delete);
        }
        
        /// <summary>
        /// Gets the unique identifier of this goal advancement node.
        /// Overrides the base implementation to provide goal-specific identification.
        /// </summary>
        public override string ID => InworldInterop.inworld_GoalAdvancementNode_id(m_DLLPtr);

        /// <summary>
        /// Gets a value indicating whether this goal advancement node is in a valid state for execution.
        /// Overrides the base implementation to provide goal-specific validation logic.
        /// </summary>
        public override bool IsValid => InworldInterop.inworld_GoalAdvancementNode_is_valid(m_DLLPtr);

        // TODO(Yan): Add Process() for Goals After All branches Merged.
        // (It requires 10 param, several are not able to use.)
        // (Still unavailable in current DLL)
    }
}