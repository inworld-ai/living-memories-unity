using System;

namespace Inworld.Framework.Graph
{
    public class ConfigParser : InworldFrameworkDllClass
    {
        public ConfigParser()
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ConfigParser_CreateConfigParser(),
                InworldInterop.inworld_StatusOr_ConfigParserPtr_status,
                InworldInterop.inworld_StatusOr_ConfigParserPtr_ok,
                InworldInterop.inworld_StatusOr_ConfigParserPtr_value,
                InworldInterop.inworld_StatusOr_ConfigParserPtr_delete);
            if (result != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(result, InworldInterop.inworld_ConfigParser_delete);
        }

        public ConfigParser(InworldHashMap<string, string> defaultResults)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ConfigParser_CreateConfigParserWithDefaults(defaultResults.ToDLL),
                InworldInterop.inworld_StatusOr_ConfigParserPtr_status,
                InworldInterop.inworld_StatusOr_ConfigParserPtr_ok,
                InworldInterop.inworld_StatusOr_ConfigParserPtr_value,
                InworldInterop.inworld_StatusOr_ConfigParserPtr_delete);
            if (result != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(result, InworldInterop.inworld_ConfigParser_delete);
        }

        public CompiledGraphInterface ParseGraph(string json)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ConfigParser_ParseGraphFromJsonString(m_DLLPtr, json),
                InworldInterop.inworld_StatusOr_CompiledGraphInterfacePtr_status,
                InworldInterop.inworld_StatusOr_CompiledGraphInterfacePtr_ok,
                InworldInterop.inworld_StatusOr_CompiledGraphInterfacePtr_value,
                InworldInterop.inworld_StatusOr_CompiledGraphInterfacePtr_delete);
            return result == IntPtr.Zero ? null : new CompiledGraphInterface(result);
        }
        
        public CompiledGraphInterface ParseGraph(string json, InworldHashMap<string, string> substitutions)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ConfigParser_ParseGraphFromJsonStringWithSubstitutions(m_DLLPtr, json, substitutions.ToDLL),
                InworldInterop.inworld_StatusOr_CompiledGraphInterfacePtr_status,
                InworldInterop.inworld_StatusOr_CompiledGraphInterfacePtr_ok,
                InworldInterop.inworld_StatusOr_CompiledGraphInterfacePtr_value,
                InworldInterop.inworld_StatusOr_CompiledGraphInterfacePtr_delete);
            return result == IntPtr.Zero ? null : new CompiledGraphInterface(result);
        }
    }
}