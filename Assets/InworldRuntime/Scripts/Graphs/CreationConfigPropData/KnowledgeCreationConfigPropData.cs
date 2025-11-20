using System;
using Newtonsoft.Json;

namespace Inworld.Framework.Graph
{
    [Serializable]
    public class KnowledgeCreationConfigPropData : ConfigData
    {
        public KnowledgePropertyData properties;
    }

    [Serializable]
    public class KnowledgePropertyData : InworldPropertyData
    {
        [JsonProperty("knowledge_compile_config")] public KnowledgeCompileConfigPropData knowledgeCompileConfig;
        [JsonProperty("api_key")] public string apiKey = "{{INWORLD_API_KEY}}";
    }

    [Serializable]
    public class KnowledgeCompileConfigPropData
    {
        [JsonProperty("parsing_config")] public ParsingConfig parsingConfig;
    }

    [Serializable]
    public class ParsingConfig
    {
        [JsonProperty("max_chars_per_chunk")] public int maxCharsPerChunk = 200;
        [JsonProperty("max_chunks_per_document")] public int maxChunksPerDocument = 100;
    }
}