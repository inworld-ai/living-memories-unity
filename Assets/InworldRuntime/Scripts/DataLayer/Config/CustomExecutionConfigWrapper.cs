using System;
using System.Runtime.InteropServices;
using Inworld.Framework.Node;

namespace Inworld.Framework
{
    public class CustomExecutionConfigWrapper : NodeExecutionConfig
    {
        public CustomExecutionConfigWrapper(string typeID, bool needReportToClient, ConfigWrapperDestructor destructor)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_CustomExecutionConfigWrapper_new(typeID,
                GCHandle.ToIntPtr(GCHandle.Alloc(this)), needReportToClient, destructor.ToDLL), InworldInterop.inworld_CustomExecutionConfigWrapper_delete);
        }

        public CustomExecutionConfigWrapper(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_CustomExecutionConfigWrapper_delete);
        }
        
        public string TypeID => InworldInterop.inworld_CustomExecutionConfigWrapper_type_id(m_DLLPtr);

        public IntPtr Value => InworldInterop.inworld_CustomExecutionConfigWrapper_value(m_DLLPtr);

        public override bool NeedReportToClient
        {
            get => InworldInterop.inworld_CustomExecutionConfigWrapper_report_to_client_get(m_DLLPtr);
            set => InworldInterop.inworld_CustomExecutionConfigWrapper_report_to_client_set(m_DLLPtr, value);
        }

        public override InworldMap<string, string> Properties
        {
            get => new InworldMap<string, string>(
                InworldInterop.inworld_CustomExecutionConfigWrapper_properties_get(m_DLLPtr));
            set => InworldInterop.inworld_CustomExecutionConfigWrapper_properties_set(m_DLLPtr, value.ToDLL);
        }
    }
}