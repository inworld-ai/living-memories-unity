using System;

namespace Inworld.Framework
{
    public class NodeExecutionTelemetryContext : InworldContext
    {
        public NodeExecutionTelemetryContext(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_NodeExecutionTelemetryContext_delete);
        }

        public string ExecutionID => InworldInterop.inworld_NodeExecutionTelemetryContext_execution_id(m_DLLPtr);

        public override bool IsValid => InworldInterop.inworld_NodeExecutionTelemetryContext_is_valid(m_DLLPtr);
    }
}