/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;


namespace Inworld.Framework.Node
{
    /// <summary>
    /// Provides configuration settings for node execution within the Inworld framework.
    /// Serves as a base class for defining runtime parameters and execution behavior.
    /// Used for customizing how nodes behave during graph execution and data processing.
    /// </summary>
    public class NodeExecutionConfig : InworldConfig
    {
        public NodeExecutionConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_NodeExecutionConfig_new(),
                InworldInterop.inworld_NodeExecutionConfig_delete);
        }

        public NodeExecutionConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_NodeExecutionConfig_delete);
        }

        public override bool IsValid => InworldInterop.inworld_NodeExecutionConfig_is_valid(m_DLLPtr);

        public virtual bool NeedReportToClient
        {
            get => InworldInterop.inworld_NodeExecutionConfig_report_to_client_get(m_DLLPtr);
            set => InworldInterop.inworld_NodeExecutionConfig_report_to_client_set(m_DLLPtr, value);
        }

        public virtual InworldMap<string, string> Properties
        {
            get => new InworldMap<string, string>(InworldInterop.inworld_NodeExecutionConfig_properties_get(m_DLLPtr));
            set => InworldInterop.inworld_NodeExecutionConfig_properties_set(m_DLLPtr, value.ToDLL);
        }

        public virtual string ToJson
        {
            get
            {
                IntPtr optStr = InworldInterop.inworld_NodeExecutionConfig_toJsonString(m_DLLPtr);
                if (InworldInterop.inworld_optional_string_has_value(optStr))
                    return  InworldInterop.inworld_optional_string_getConst(optStr);
                return "";
            }
        }
    }
}