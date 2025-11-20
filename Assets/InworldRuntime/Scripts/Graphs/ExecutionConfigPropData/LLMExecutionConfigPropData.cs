using System;
using Newtonsoft.Json;

namespace Inworld.Framework.Graph
{
    [Serializable]
    public class LLMExecutionConfigPropData : ConfigData
    {
        public LLMExecutionPropertyData properties;
    }

    [Serializable]
    public class LLMExecutionPropertyData : InworldPropertyData
    {
        [JsonProperty("llm_component_id")] public string llmComponentID;
        public bool stream;
    }
}