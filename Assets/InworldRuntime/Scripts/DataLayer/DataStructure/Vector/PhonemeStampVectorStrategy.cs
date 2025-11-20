using System;

namespace Inworld.Framework
{
    public class PhonemeStampVectorStrategy : IVectorStrategy<PhonemeStamp>
    {
        public IntPtr CreateNew() => InworldInterop.inworld_vector_SynthesizedSpeech_PhonemeTimestamp_new();

        public IntPtr CreateCopy(IntPtr source) => InworldInterop.inworld_vector_SynthesizedSpeech_PhonemeTimestamp_copy(source);

        public void Delete(IntPtr ptr) => InworldInterop.inworld_vector_SynthesizedSpeech_PhonemeTimestamp_delete(ptr);

        public void Reserve(IntPtr ptr, int nSize) => InworldInterop.inworld_vector_SynthesizedSpeech_PhonemeTimestamp_reserve(ptr, nSize);

        public void Clear(IntPtr ptr) => InworldInterop.inworld_vector_SynthesizedSpeech_PhonemeTimestamp_clear(ptr);

        public void PushBack(IntPtr ptr, PhonemeStamp value) =>
            InworldInterop.inworld_vector_SynthesizedSpeech_PhonemeTimestamp_push_back(ptr, value.ToDLL);

        public int GetSize(IntPtr ptr) => InworldInterop.inworld_vector_SynthesizedSpeech_PhonemeTimestamp_size(ptr);

        public int GetCapacity(IntPtr ptr) =>
            InworldInterop.inworld_vector_SynthesizedSpeech_PhonemeTimestamp_capacity(ptr);

        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_vector_SynthesizedSpeech_PhonemeTimestamp_empty(ptr);

        public PhonemeStamp GetItem(IntPtr ptr, int index) => new PhonemeStamp(InworldInterop.inworld_vector_SynthesizedSpeech_PhonemeTimestamp_get(ptr, index));

        public void SetItem(IntPtr ptr, int index, PhonemeStamp value) =>
            InworldInterop.inworld_vector_SynthesizedSpeech_PhonemeTimestamp_set(ptr, index, value.ToDLL);

    }
}