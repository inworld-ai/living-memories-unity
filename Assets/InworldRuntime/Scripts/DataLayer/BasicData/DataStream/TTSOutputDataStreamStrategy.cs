/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.TTS;

namespace Inworld.Framework
{
    /// <summary>
    /// The implementation of DataStream of TTSOutput
    /// </summary>
    public class TTSOutputDataStreamStrategy : IDataStreamStrategy<TTSOutput>
    {
        public IntPtr CreateFromInputStream(InworldInputStream<TTSOutput> inputs, CancellationContext cancellationContext)
            => InworldInterop.inworld_DataStream_TTSOutput_new_std_shared_ptr_Sl_inworld_InputStream_Sl_inworld_graphs_TTSOutput_Sg__Sg__std_shared_ptr_Sl_inworld_graphs_CancellationContext_Sg_(inputs.ToDLL, cancellationContext.ToDLL);

        public IntPtr CreateFromBaseData(InworldBaseData source) 
            => InworldInterop.inworld_BaseDataAs_DataStream_TTSOutput(source.ToDLL);

        public IntPtr CreateCopy(IntPtr source)
            => InworldInterop.inworld_DataStream_TTSOutput_copy(source);

        public string ToString(IntPtr ptr)
            => InworldInterop.inworld_DataStream_TTSOutput_ToString(ptr);

        public void Delete(IntPtr ptr)
            => InworldInterop.inworld_DataStream_TTSOutput_delete(ptr);
        
        public InworldInputStream<TTSOutput> ToInputStream(IntPtr ptr)
            => new InworldInputStream<TTSOutput>(InworldInterop.inworld_DataStream_TTSOutput_stream(ptr));

        public CancellationContext CancellationContext(IntPtr ptr)
            => new CancellationContext(InworldInterop.inworld_DataStream_TTSOutput_cancellation_context(ptr));

        public bool IsValid(IntPtr ptr) => InworldInterop.inworld_DataStream_TTSOutput_is_valid(ptr);
        
        public void Accept(IBaseDataVisitor visitor, InworldDataStream<TTSOutput> stream)
            => visitor.Visit(stream);
    }
}