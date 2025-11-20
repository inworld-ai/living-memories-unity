/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.Goal
{
    public class GoalAdvancement : InworldBaseData
    {
        public GoalAdvancement(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_GoalAdvancement_delete);
        }

        public GoalAdvancement(InworldBaseData parent)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_GoalAdvancement(parent.ToDLL),
                InworldInterop.inworld_GoalAdvancement_delete);
        }

        public InworldVector<string> AdvancedGoals 
            => new InworldVector<string>(InworldInterop.inworld_GoalAdvancement_activated_goals(m_DLLPtr));

        public InworldVector<string> CompletedGoals 
            => new InworldVector<string>(InworldInterop.inworld_GoalAdvancement_completed_goals(m_DLLPtr));

        public InworldVector<string> CurrentGoals 
            => new InworldVector<string>(InworldInterop.inworld_GoalAdvancement_current_goals(m_DLLPtr));

        public override bool IsValid => InworldInterop.inworld_GoalAdvancement_is_valid(m_DLLPtr);
        
        public override string ToString() => InworldInterop.inworld_GoalAdvancement_ToString(m_DLLPtr);

        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}