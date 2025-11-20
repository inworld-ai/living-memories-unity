using System;
using Newtonsoft.Json;

namespace Inworld.Framework.Graph
{
    [Serializable]
    public class STTExecutionConfigPropData : ConfigData
    {
        public STTExecutionPropertyData properties;
    }

    [Serializable]
    public class STTExecutionPropertyData : InworldPropertyData
    {
        [JsonProperty("stt_component_id")] public string sttComponentID;
    }
}