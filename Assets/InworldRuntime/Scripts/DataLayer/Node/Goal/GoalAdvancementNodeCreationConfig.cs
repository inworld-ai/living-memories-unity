using System;

namespace Inworld.Framework.Node
{
    public class GoalAdvancementNodeCreationConfig : NodeCreationConfig
    {
        public GoalAdvancementNodeCreationConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_GoalAdvancementNodeCreationConfig_new(),
                InworldInterop.inworld_GoalAdvancementNodeCreationConfig_delete);
        }
        
        public GoalAdvancementNodeCreationConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_GoalAdvancementNodeCreationConfig_delete);
        }
        
        public InworldVector<InworldGoal> Goals
        {
            get => new InworldVector<InworldGoal>(InworldInterop.inworld_GoalAdvancementNodeCreationConfig_goals_get(m_DLLPtr));
            set => InworldInterop.inworld_GoalAdvancementNodeCreationConfig_goals_set(m_DLLPtr, value.ToDLL);
        }
    }
}