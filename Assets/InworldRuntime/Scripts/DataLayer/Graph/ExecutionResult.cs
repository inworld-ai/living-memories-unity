using System;

namespace Inworld.Framework.Graph
{
    public class ExecutionResult : InworldFrameworkDllClass
    {
        public ExecutionResult()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_ExecutionResult_new(),
                InworldInterop.inworld_ExecutionResult_delete);
        }

        public ExecutionResult(InworldInputStream<InworldBaseData> resultStream, string variant)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_ExecutionResult_new_std_shared_ptr_Sl_inworld_InputStream_Sl_std_shared_ptr_Sl_inworld_graphs_BaseData_Sg__Sg__Sg__std_string(resultStream.ToDLL, variant),
                InworldInterop.inworld_ExecutionResult_delete);
        }
        
        public ExecutionResult(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_ExecutionResult_delete);
        }

        public InworldInputStream<InworldBaseData> ResultStream
        {
            get => new InworldInputStream<InworldBaseData>(
                InworldInterop.inworld_ExecutionResult_result_stream_get(m_DLLPtr));
            set => InworldInterop.inworld_ExecutionResult_result_stream_set(m_DLLPtr, value.ToDLL);
        }

        public string Variant
        {
            get => InworldInterop.inworld_ExecutionResult_variant_get(m_DLLPtr);
            set => InworldInterop.inworld_ExecutionResult_variant_set(m_DLLPtr, value);
        }
        
    }
}