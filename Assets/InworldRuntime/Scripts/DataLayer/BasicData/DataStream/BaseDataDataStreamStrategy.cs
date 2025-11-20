using System;

namespace Inworld.Framework
{
    public class BaseDataDataStreamStrategy : IDataStreamStrategy<InworldBaseData>
    {
        public IntPtr CreateFromInputStream(InworldInputStream<InworldBaseData> inputs,
            CancellationContext cancellationContext)
            => InworldInterop
                .inworld_DataStream_BaseData_new_std_shared_ptr_Sl_inworld_InputStream_Sl_std_shared_ptr_Sl_inworld_graphs_BaseData_Sg__Sg__Sg__std_shared_ptr_Sl_inworld_graphs_CancellationContext_Sg_(inputs.ToDLL, cancellationContext.ToDLL);

        public IntPtr CreateFromBaseData(InworldBaseData source)
            => InworldInterop.inworld_BaseDataAs_DataStream_BaseData(source.ToDLL);

        public IntPtr CreateCopy(IntPtr source)
            => InworldInterop.inworld_DataStream_BaseData_copy(source);

        public string ToString(IntPtr ptr)
            => InworldInterop.inworld_DataStream_BaseData_ToString(ptr);

        public void Delete(IntPtr ptr)
            => InworldInterop.inworld_DataStream_BaseData_delete(ptr);

        public InworldInputStream<InworldBaseData> ToInputStream(IntPtr ptr)
            => new InworldInputStream<InworldBaseData>(InworldInterop.inworld_DataStream_BaseData_stream(ptr));

        public bool IsValid(IntPtr ptr)
            => InworldInterop.inworld_DataStream_BaseData_is_valid(ptr);

        public CancellationContext CancellationContext(IntPtr ptr)
            => new CancellationContext(InworldInterop.inworld_DataStream_BaseData_cancellation_context(ptr));

        public void Accept(IBaseDataVisitor visitor, InworldDataStream<InworldBaseData> stream)
            => visitor.Visit(stream);
    }
}