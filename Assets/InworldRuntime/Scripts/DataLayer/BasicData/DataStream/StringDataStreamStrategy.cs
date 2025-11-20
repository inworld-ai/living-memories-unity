/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework
{
    /// <summary>
    /// The implementation of DataStream of String
    /// </summary>
    public class StringDataStreamStrategy : IDataStreamStrategy<string>
    {
        public IntPtr CreateFromInputStream(InworldInputStream<string> inputs, CancellationContext cancellationContext)
            => InworldInterop.inworld_DataStream_string_new_std_shared_ptr_Sl_inworld_InputStream_Sl_std_string_Sg__Sg__std_shared_ptr_Sl_inworld_graphs_CancellationContext_Sg_(inputs.ToDLL, cancellationContext.ToDLL);

        public IntPtr CreateFromBaseData(InworldBaseData source) 
            => InworldInterop.inworld_BaseDataAs_DataStream_string(source.ToDLL);

        public IntPtr CreateCopy(IntPtr source)
            => InworldInterop.inworld_DataStream_string_copy(source);

        public string ToString(IntPtr ptr)
            => InworldInterop.inworld_DataStream_string_ToString(ptr);

        public void Delete(IntPtr ptr)
            => InworldInterop.inworld_DataStream_string_delete(ptr);

        public InworldInputStream<string> ToInputStream(IntPtr ptr)
            => new InworldInputStream<string>(InworldInterop.inworld_DataStream_string_stream(ptr));

        public CancellationContext CancellationContext(IntPtr ptr) =>
            new CancellationContext(InworldInterop.inworld_DataStream_string_cancellation_context(ptr));
        public bool IsValid(IntPtr ptr) => InworldInterop.inworld_DataStream_string_is_valid(ptr);

        public void Accept(IBaseDataVisitor visitor, InworldDataStream<string> stream)
            => visitor.Visit(stream);
    }
}