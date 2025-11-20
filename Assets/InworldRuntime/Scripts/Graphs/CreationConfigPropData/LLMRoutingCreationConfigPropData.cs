using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Inworld.Framework.Graph
{
    [Serializable]
    public class LLMRoutingCreationConfigPropData : ConfigData
    {
        public LLMRoutingPropertyData properties;
    }
    
    [Serializable]
    public class LLMRoutingPropertyData : InworldPropertyData
    {
        [JsonProperty("routing_configs")]
        public List<RoutingConfigData> routingConfigs;
    }


    [Serializable]
    public class RoutingConfigData
    {
        [JsonProperty("llm_component_id")] public string llmComponentID;
        public int priority;
        [JsonProperty("error_cooldown_seconds")] public float errorCooldownSeconds;
        [JsonProperty("min_errors")] public int minErrors;
    }
}