using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Inworld.Framework.Graph
{
    [Serializable]
    public class TTSCreationConfigPropData : ConfigData
    {
        public TTSPropertyData properties;
    }

    [Serializable]
    public class TTSPropertyData : InworldPropertyData
    {
        [JsonProperty("api_key")] public string apiKey = "{{INWORLD_API_KEY}}";
        [JsonProperty("synthesis_config")] public SynthesizeConfigPropData synthesizeConfig;
    }

    [Serializable]
    public class SynthesizeConfigPropData
    {
        public string type = "inworld";
        public TTSConfigData config;
    }

    [Serializable]
    public class TTSConfigData
    {
        [JsonProperty("model_id")] public string modelID = "inworld-tts-1";
        public TTSPostProcessingData postprocessing;
        public TTSInferenceData inference;
    }

    [Serializable]
    public class TTSPostProcessingData
    {
        [JsonProperty("sample_rate")] public int sampleRate = 16000;
    }

    [Serializable]
    public class TTSInferenceData
    {
        [Range(0,1)] public float temperature = 0.8f;
        [Range(0,1)] public float pitch = 0;
        [JsonProperty("speaking_rate")][Range(0,1)] public float speakingRate = 1f;
    }
}