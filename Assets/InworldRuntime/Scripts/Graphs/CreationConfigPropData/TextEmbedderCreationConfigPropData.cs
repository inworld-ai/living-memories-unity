using System;
using Newtonsoft.Json;

namespace Inworld.Framework.Graph
{
    [Serializable]
    public class TextEmbedderPropertyData : InworldPropertyData
    {
        public string provider = "inworld";
        [JsonProperty("model_name")] public string modelName = "BAAI/bge-large-en-v1.5";
        [JsonProperty("api_key")] public string apiKey = "{{INWORLD_API_KEY}}";
    }
    
    [Serializable]
    public class TextEmbedderCreationConfigPropData : ConfigData
    {
        public TextEmbedderPropertyData properties;
    }
}