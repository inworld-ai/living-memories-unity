using System;
using Newtonsoft.Json;


namespace Inworld.Framework.Graph
{
    [Serializable]
    public class STTCreationConfigPropData : ConfigData
    {
        public STTPropertyData properties;
    }

    [Serializable]
    public class STTPropertyData : InworldPropertyData
    {
        [JsonProperty("model_path")] public string modelPath = "{{STT_MODEL_PATH}}";
        public STTDeviceData device;
        [JsonProperty("default_config")] public STTDefaultConfigPropData defaultConfig;
    }

    [Serializable]
    public class STTDeviceData
    {
        public string type = "CUDA";
        public int index = -1;
        public STTInfoData info;
    }

    [Serializable]
    public class STTInfoData
    {
        public string name;
        public int timestamp;
        [JsonProperty("free_memory_bytes")] public int freeMeoryBytes;
        [JsonProperty("total_memory_bytes")] public int totalMeoryBytes;
    }

    [Serializable]
    public class STTDefaultConfigPropData
    {
        
    }
}