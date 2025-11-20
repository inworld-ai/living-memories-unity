using System;

namespace Inworld.Framework
{
    public class SpeechChunkDataStreamStrategy : IDataStreamStrategy<SpeechChunk>
    {
        public IntPtr CreateFromInputStream(InworldInputStream<SpeechChunk> inputs,
            CancellationContext cancellationContext)
            => InworldInterop
                .inworld_DataStream_SpeechChunk_new_std_shared_ptr_Sl_inworld_InputStream_Sl_inworld_SpeechChunk_Sg__Sg__std_shared_ptr_Sl_inworld_graphs_CancellationContext_Sg_(
                    inputs.ToDLL, cancellationContext.ToDLL);

        public IntPtr CreateFromBaseData(InworldBaseData source)
            => InworldInterop.inworld_BaseDataAs_DataStream_SpeechChunk(source.ToDLL);

        public IntPtr CreateCopy(IntPtr source)
            => InworldInterop.inworld_DataStream_SpeechChunk_copy(source);

        public string ToString(IntPtr ptr)
            => InworldInterop.inworld_DataStream_SpeechChunk_ToString(ptr);

        public void Delete(IntPtr ptr)
            => InworldInterop.inworld_DataStream_SpeechChunk_delete(ptr);

        public InworldInputStream<SpeechChunk> ToInputStream(IntPtr ptr)
            => new InworldInputStream<SpeechChunk>(InworldInterop.inworld_DataStream_SpeechChunk_stream(ptr));

        public bool IsValid(IntPtr ptr)
            => InworldInterop.inworld_DataStream_SpeechChunk_is_valid(ptr);

        public CancellationContext CancellationContext(IntPtr ptr)
            => new CancellationContext(InworldInterop.inworld_DataStream_SpeechChunk_cancellation_context(ptr));

        public void Accept(IBaseDataVisitor visitor, InworldDataStream<SpeechChunk> stream)
            => visitor.Visit(stream);
    }
}