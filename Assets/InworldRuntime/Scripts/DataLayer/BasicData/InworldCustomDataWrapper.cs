/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework
{
    public class InworldCustomDataWrapper : InworldBaseData
    {
        public InworldCustomDataWrapper(string id, IntPtr function, IntPtr callback, IntPtr destructor)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_CustomDataWrapper_new(id, function, callback, destructor),
                InworldInterop.inworld_CustomNodeWrapper_delete);
        }

        public InworldCustomDataWrapper(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_CustomNodeWrapper_delete);
        }

        public InworldCustomDataWrapper(InworldBaseData parent)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_CustomDataWrapper(parent.ToDLL), 
                InworldInterop.inworld_CustomNodeWrapper_delete);
        }

        public string ID => InworldInterop.inworld_CustomDataWrapper_type_id(m_DLLPtr);

        public override string ToString() => InworldInterop.inworld_CustomDataWrapper_ToString(m_DLLPtr);

        public override bool IsValid => InworldInterop.inworld_CustomDataWrapper_is_valid(m_DLLPtr);
        public IntPtr Value => InworldInterop.inworld_CustomDataWrapper_value(m_DLLPtr);

        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}