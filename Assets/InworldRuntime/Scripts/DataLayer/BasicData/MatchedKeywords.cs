using System;

namespace Inworld.Framework
{
    public class MatchedKeywords : InworldBaseData
    {
        public MatchedKeywords(InworldVector<KeywordMatch> keywordMatches)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_MatchedKeywords_new(keywordMatches.ToDLL),
                InworldInterop.inworld_MatchedKeywords_delete);
        }

        public MatchedKeywords(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_MatchedKeywords_delete);
        }

        public MatchedKeywords(InworldBaseData parent)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_MatchedKeywords(parent.ToDLL), 
                InworldInterop.inworld_MatchedKeywords_delete);
        }

        public override bool IsValid => InworldInterop.inworld_MatchedKeywords_is_valid(m_DLLPtr);

        public override string ToString() => InworldInterop.inworld_MatchedKeywords_ToString(m_DLLPtr);

        public InworldVector<KeywordMatch> Matches => new InworldVector<KeywordMatch>(InworldInterop.inworld_MatchedKeywords_matches(m_DLLPtr));
        
        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}