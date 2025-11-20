using System;
using Inworld.Framework.Node;
using UnityEngine;

namespace Inworld.Framework
{
    public static class InworldConfigManager
    {
        public static bool RegisterCustomConfig(string name, CustomConfigThreadedDeserializeExecutor deserializer,
            CustomConfigThreadedSerializeExecutor serializer)
        {
            IntPtr status = InworldInterop.inworld_ConfigRegistry_RegisterCustomConfig(
                InworldInterop.inworld_ConfigRegistry_GetInstance(), name, deserializer.ToDLL, serializer.ToDLL);
            if (InworldInterop.inworld_Status_ok(status))
                return true;
            Debug.LogError(InworldInterop.inworld_Status_ToString(status));
            return false;
        }
        
        public static bool RegisterCustomExecutionConfig(string name, CustomExecutionConfigThreadedDeserializeExecutor deserializer,
            CustomExecutionConfigThreadedSerializeExecutor serializer)
        {
            IntPtr status = InworldInterop.inworld_ConfigRegistry_RegisterCustomExecutionConfig(
                InworldInterop.inworld_ConfigRegistry_GetInstance(), name, deserializer.ToDLL, serializer.ToDLL);
            if (InworldInterop.inworld_Status_ok(status))
                return true;
            Debug.LogError(InworldInterop.inworld_Status_ToString(status));
            return false;
        }

        public static CustomConfigWrapper DeserializeCustomConfig(string type, string jsonData)
        {
            IntPtr result = InworldFrameworkUtil.Execute(InworldInterop.inworld_ConfigRegistry_DeserializeCustomConfig(InworldInterop.inworld_ConfigRegistry_GetInstance(), type, jsonData),
                InworldInterop.inworld_StatusOr_CustomConfigWrapper_status,
                InworldInterop.inworld_StatusOr_CustomConfigWrapper_ok,
                InworldInterop.inworld_StatusOr_CustomConfigWrapper_value,
                InworldInterop.inworld_StatusOr_CustomConfigWrapper_delete);
            return result == IntPtr.Zero ? null : new CustomConfigWrapper(result);
        }

        public static string SerializeCustomConfig(string type, CustomConfigWrapper config)
        {
            return InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ConfigRegistry_SerializeCustomConfig(
                    InworldInterop.inworld_ConfigRegistry_GetInstance(), type, config.ToDLL),
                InworldInterop.inworld_StatusOr_string_status, 
                InworldInterop.inworld_StatusOr_string_ok,
                InworldInterop.inworld_StatusOr_string_value, 
                InworldInterop.inworld_StatusOr_string_delete);
        }

        public static CustomExecutionConfigWrapper DeserializeCustomExecutionConfig(string type, string jsonData)
        {
            IntPtr result = InworldFrameworkUtil.Execute(InworldInterop.inworld_ConfigRegistry_DeserializeCustomExecutionConfig(InworldInterop.inworld_ConfigRegistry_GetInstance(), type, jsonData),
                InworldInterop.inworld_StatusOr_CustomExecutionConfigWrapper_status,
                InworldInterop.inworld_StatusOr_CustomExecutionConfigWrapper_ok,
                InworldInterop.inworld_StatusOr_CustomExecutionConfigWrapper_value,
                InworldInterop.inworld_StatusOr_CustomExecutionConfigWrapper_delete);
            return result == IntPtr.Zero ? null : new CustomExecutionConfigWrapper(result);
        }

        public static string SerializeCustomExecutionConfig(string type, CustomExecutionConfigWrapper config)
        {
            return InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ConfigRegistry_SerializeCustomExecutionConfig(
                    InworldInterop.inworld_ConfigRegistry_GetInstance(), type, config.ToDLL),
                InworldInterop.inworld_StatusOr_string_status, 
                InworldInterop.inworld_StatusOr_string_ok,
                InworldInterop.inworld_StatusOr_string_value, 
                InworldInterop.inworld_StatusOr_string_delete);
        }
    }
}