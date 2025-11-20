using System;

namespace Inworld.Framework
{
    public class CompiledIntent : InworldFrameworkDllClass
    {
        public CompiledIntent()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_CompiledIntent_new(),
                InworldInterop.inworld_CompiledIntent_delete);
        }
        
        public CompiledIntent(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_CompiledIntent_delete);
        }

        public string Name
        {
            get => InworldInterop.inworld_CompiledIntent_name_get(m_DLLPtr);
            set => InworldInterop.inworld_CompiledIntent_name_set(m_DLLPtr, value);
        }

        public InworldVector<string> NormalizedPhrases
        {
            get => new InworldVector<string>(InworldInterop.inworld_CompiledIntent_normalized_phrases_get(m_DLLPtr));
            set => InworldInterop.inworld_CompiledIntent_normalized_phrases_set(m_DLLPtr, value.ToDLL);
        }

        public InworldVector<InworldVector<float>> PhraseEmbeddings
        {
            get => new InworldVector<InworldVector<float>>(
                InworldInterop.inworld_CompiledIntent_phrase_embeddings_get(m_DLLPtr));
            
            set => InworldInterop.inworld_CompiledIntent_phrase_embeddings_set(m_DLLPtr, value.ToDLL);
        }
    }
}