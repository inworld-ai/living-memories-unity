using System;

namespace Inworld.Framework
{
    public class PhonemeStamp : InworldFrameworkDllClass
    {
        public PhonemeStamp()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_PhonemeTimestamp_new(),
                InworldInterop.inworld_PhonemeTimestamp_delete);
        }

        public PhonemeStamp(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_PhonemeTimestamp_delete);
        }

        float StartTime => InworldInterop.inworld_PhonemeTimestamp_start_time_sec_get(m_DLLPtr);
        
        string Phoneme => InworldInterop.inworld_PhonemeTimestamp_phoneme_get(m_DLLPtr);
    }
}