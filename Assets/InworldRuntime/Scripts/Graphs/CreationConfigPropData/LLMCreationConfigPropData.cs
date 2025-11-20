using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Inworld.Framework.Graph
{
            
    [Serializable]
    public class LLMCreationConfigPropData : ConfigData
    {
        public LLMPropertyData properties;
    }
    
    [Serializable]
    public class LLMPropertyData : InworldPropertyData
    {
        public string provider;
        [JsonProperty("model_name")] public string modelName;
        [JsonProperty("api_key")] public string apiKey = "{{INWORLD_API_KEY}}";
        [JsonProperty("default_config")] public LLMDefaultConfigPropData defaultConfig;
    }

    [Serializable]
    public class LLMDefaultConfigPropData
    {
        [JsonProperty("max_new_tokens")] public int maxNewTokens;
        [JsonProperty("max_prompt_length")] public int maxPromptLength;
        public float temperature;
        [JsonProperty("top_p")][Range(0,1)] public float topP;
        [JsonProperty("repetition_penalty")][Range(0,1)] public float repetitionPenalty = 1.0f;
        [JsonProperty("frequency_penalty")][Range(0,1)] public float frequencyPenalty = 0.0f;
        [JsonProperty("presence_penalty")][Range(0,1)] public float presencePenalty = 0.0f;

        [JsonProperty("stop_sequences")] public List<string> stopSequences = new List<string>()
        {
            "\n\n"
        };
    }

}