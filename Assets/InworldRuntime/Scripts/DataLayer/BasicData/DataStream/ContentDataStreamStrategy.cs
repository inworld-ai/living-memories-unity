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
    /// The implementation of DataStream of InworldContent
    /// </summary>
    public class ContentDataStreamStrategy: IDataStreamStrategy<InworldContent>
    {
        public IntPtr CreateFromInputStream(InworldInputStream<InworldContent> inputs, CancellationContext cancelContext)
            => InworldInterop.inworld_DataStream_Content_new_std_shared_ptr_Sl_inworld_InputStream_Sl_inworld_Content_Sg__Sg__std_shared_ptr_Sl_inworld_graphs_CancellationContext_Sg_(inputs.ToDLL, cancelContext.ToDLL);

        public IntPtr CreateFromBaseData(InworldBaseData source) 
            => InworldInterop.inworld_BaseDataAs_DataStream_Content(source.ToDLL);

        public IntPtr CreateCopy(IntPtr source)
            => InworldInterop.inworld_DataStream_Content_copy(source);

        public string ToString(IntPtr ptr)
            => InworldInterop.inworld_DataStream_Content_ToString(ptr);

        public void Delete(IntPtr ptr)
            => InworldInterop.inworld_DataStream_Content_delete(ptr);

        public InworldInputStream<InworldContent> ToInputStream(IntPtr ptr)
            => new InworldInputStream<InworldContent>(InworldInterop.inworld_DataStream_Content_stream(ptr));
        
        public CancellationContext CancellationContext(IntPtr ptr)
            => new CancellationContext(InworldInterop.inworld_DataStream_Content_cancellation_context(ptr));

        public bool IsValid(IntPtr ptr) => InworldInterop.inworld_DataStream_Content_is_valid(ptr);
        
        public void Accept(IBaseDataVisitor visitor, InworldDataStream<InworldContent> stream)
            => visitor.Visit(stream);

    }
}