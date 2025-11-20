using System;

namespace Inworld.Framework.Knowledge
{
    public class KnowledgeRecords : InworldBaseData
    {
        public KnowledgeRecords(InworldVector<string> records)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_KnowledgeRecords_new(records.ToDLL),
                InworldInterop.inworld_KnowledgeRecords_delete);
        }

        public KnowledgeRecords(InworldBaseData rhs)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_KnowledgeRecords(rhs.ToDLL),
                InworldInterop.inworld_KnowledgeRecords_delete);
        }
        
        public KnowledgeRecords(IntPtr rhs) 
            => MemoryManager.Register(rhs, InworldInterop.inworld_KnowledgeRecords_delete);

        public InworldVector<string> Records
            => new InworldVector<string>(InworldInterop.inworld_KnowledgeRecords_records(m_DLLPtr));

        public override bool IsValid => InworldInterop.inworld_KnowledgeRecords_is_valid(m_DLLPtr);

        public override string ToString()
        {
            return InworldInterop.inworld_KnowledgeRecords_ToString(m_DLLPtr);
        }

        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}