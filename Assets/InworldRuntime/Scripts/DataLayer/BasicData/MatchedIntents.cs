using System;

namespace Inworld.Framework
{
    public class MatchedIntents : InworldBaseData
    {
        public MatchedIntents()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_MatchedIntents_new(),
                InworldInterop.inworld_MatchedIntents_delete);
        }

        public MatchedIntents(InworldVector<IntentMatch> intentMatches)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_MatchedIntents_new_std_vector_Sl_inworld_IntentMatch_Sg_(intentMatches.ToDLL),
                InworldInterop.inworld_MatchedIntents_delete);
        }

        public MatchedIntents(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_MatchedIntents_delete);
        }

        public MatchedIntents(InworldBaseData parent)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_MatchedIntents(parent.ToDLL),
                InworldInterop.inworld_MatchedIntents_delete);
        }

        public InworldVector<IntentMatch> IntentMatches 
            => new InworldVector<IntentMatch>(InworldInterop.inworld_MatchedIntents_intent_matches(m_DLLPtr));

        public override bool IsValid => InworldInterop.inworld_MatchedIntents_is_valid(m_DLLPtr);

        public override string ToString() => InworldInterop.inworld_MatchedIntents_ToString(m_DLLPtr);
        
        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}