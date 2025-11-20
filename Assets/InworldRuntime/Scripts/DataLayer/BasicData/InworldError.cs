using System;

namespace Inworld.Framework
{
    public class InworldError : InworldBaseData
    {
        public InworldError(string errorMessage, StatusCode statusCode = StatusCode.Aborted)
        {
            InworldStatus status = new InworldStatus(statusCode, errorMessage);
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_Error_new(status.ToDLL),
                InworldInterop.inworld_Error_delete);
        }
        public InworldError(InworldStatus status)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_Error_new(status.ToDLL),
                InworldInterop.inworld_Error_delete);
        }

        public InworldError(InworldBaseData baseData)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_Error(baseData.ToDLL),
                InworldInterop.inworld_Error_delete);
        }

        public InworldError(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_Error_delete);
        }

        public InworldStatus Status => new InworldStatus(InworldInterop.inworld_Error_status(m_DLLPtr));

        public override string ToString() => InworldInterop.inworld_Error_ToString(m_DLLPtr);

        public override bool IsValid => InworldInterop.inworld_Error_is_valid(m_DLLPtr);

        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}