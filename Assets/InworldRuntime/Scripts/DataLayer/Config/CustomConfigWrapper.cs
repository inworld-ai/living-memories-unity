using System;

namespace Inworld.Framework.Node
{
    public class CustomConfigWrapper : InworldFrameworkDllClass
    {
        public CustomConfigWrapper(string typeID, IntPtr customDataPtr, ConfigWrapperDestructor destructor)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_CustomConfigWrapper_new(typeID, customDataPtr, destructor.ToDLL),
                InworldInterop.inworld_CustomConfigWrapper_delete);
        }

        public CustomConfigWrapper(IntPtr rhs) =>
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_CustomConfigWrapper_delete);

        string TypeID => InworldInterop.inworld_CustomConfigWrapper_type_id(m_DLLPtr);
        
        IntPtr Value => InworldInterop.inworld_CustomConfigWrapper_value(m_DLLPtr);
    }
}