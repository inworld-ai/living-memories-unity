using System;

namespace Inworld.Framework.Memory
{
    public class MemoryState : InworldBaseData
    {
        public MemoryState(MemorySnapshot snapshot)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_MemoryState_new(snapshot.ToDLL),
                InworldInterop.inworld_MemoryState_delete);
        }

        public MemoryState(InworldBaseData baseData)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_MemoryState(baseData.ToDLL),
                InworldInterop.inworld_MemoryState_delete);
        }
        
        public MemoryState(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_MemoryState_delete);
        }

        public override string ToString() => InworldInterop.inworld_MemoryState_ToString(m_DLLPtr);

        public string ToJson => InworldInterop.inworld_MemoryState_GetJson(m_DLLPtr);

        public override bool IsValid => InworldInterop.inworld_MemoryState_is_valid(m_DLLPtr);
        
        public MemorySnapshot Snapshot => new MemorySnapshot(InworldInterop.inworld_MemoryState_memory_snapshot(m_DLLPtr));

        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}