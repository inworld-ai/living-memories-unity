using System;

namespace Inworld.Framework
{
    public class SafetyResult : InworldBaseData
    {
        public SafetyResult(string input, bool isSafe)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_SafetyResult_new(input, isSafe),
                InworldInterop.inworld_SafetyResult_delete);
        }

        public SafetyResult(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_SafetyResult_delete);
        }

        public SafetyResult(InworldBaseData parent)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_SafetyResult(parent.ToDLL),
                InworldInterop.inworld_SafetyResult_delete);
        }
        
        public override bool IsValid => InworldInterop.inworld_SafetyResult_is_valid(m_DLLPtr);

        public override string ToString() => InworldInterop.inworld_SafetyResult_ToString(m_DLLPtr);

        public bool IsSafe => InworldInterop.inworld_SafetyResult_is_safe(m_DLLPtr);

        public string Content => InworldInterop.inworld_SafetyResult_text(m_DLLPtr);
        
        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}