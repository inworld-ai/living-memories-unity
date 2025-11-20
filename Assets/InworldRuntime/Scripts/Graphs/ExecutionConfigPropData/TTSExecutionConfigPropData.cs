using System;
using Newtonsoft.Json;


namespace Inworld.Framework.Graph
{
    [Serializable]
    public class TTSExecutionConfigPropData : ConfigData
    {
        public TTSExecutionPropertyData properties;
    }

    [Serializable]
    public class TTSExecutionPropertyData : InworldPropertyData
    {
        [JsonProperty("tts_component_id")] public string ttsComponentID;
        public InworldVoicePropData voice;
        [JsonProperty("synthesis_config")] public SynthesizeConfigPropData synthesizeConfig;
        [JsonProperty("report_to_client")] public bool reportToClient = true;
    }

    [Serializable]
    public class InworldVoicePropData
    {
        [JsonProperty("speaker_id")] public string speakerID = "Dennis";
    }
}