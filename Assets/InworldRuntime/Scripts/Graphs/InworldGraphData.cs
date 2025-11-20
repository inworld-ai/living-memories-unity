using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace Inworld.Framework.Graph
{
    [Serializable]
    public class ComponentData 
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string id;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string type;
        [JsonProperty("creation_config", NullValueHandling = NullValueHandling.Ignore)]
        public ConfigData creationConfig;
        [JsonProperty("execution_config", NullValueHandling = NullValueHandling.Ignore)]
        public ConfigData executionConfig;
        [JsonProperty("subgraph_id", NullValueHandling = NullValueHandling.Ignore)]
        public string subgraphID;
    }

    [Serializable]
    public class EdgeData
    {
        [JsonProperty("from_node")]
        public string fromNode;
        [JsonProperty("to_node")]
        public string toNode;
        [JsonProperty("condition_id", NullValueHandling = NullValueHandling.Ignore)]
        public string conditionID;
        [JsonProperty("condition_expression", NullValueHandling = NullValueHandling.Ignore)]
        public string conditionExpression;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool optional;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool loop;
    }
    
    [Serializable]
    public class ConfigData 
    {
        public string type;
    }

    [Serializable]
    public class InworldGraphData
    {
        public string id;
        public List<ComponentData> nodes;
        public List<EdgeData> edges;
        [JsonProperty("start_nodes")]
        public List<string> startNodes;
        [JsonProperty("end_nodes")]
        public List<string> endNodes;
    }
    
    [Serializable]
    public class GraphConfigData
    {
        [JsonProperty("schema_version")]
        public string schemaVersion; 
        public List<ComponentData> components; 
        public InworldGraphData main;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<InworldGraphData> subgraphs;
    }
}