using System;
using Newtonsoft.Json;

namespace Inworld.Framework.Graph
{
    [Serializable]
    public class SafetyCreationConfigPropData : ConfigData
    {
        public SafetyPropertyData properties;
    }

    [Serializable]
    public class SafetyPropertyData : InworldPropertyData
    {
        [JsonProperty("embedder_component_id")]
        public string embedderComponentID;
        
        [JsonProperty("safety_config")]
        public SafetyConfigPropData safetyConfig;
    }

    [Serializable]
    public class SafetyConfigPropData
    {
        [JsonProperty("model_weights_path")]
        public string modelWeightsPath = "{{SAFETY_MODEL_PATH}}";
    }
}