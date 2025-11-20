using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Inworld.Framework.Graph
{
    [Serializable]
    public class RandomCannedTextExecutionConfigPropData : ConfigData
    {
        public RandomCannedTextPropertyData properties;
    }

    [Serializable]
    public class RandomCannedTextPropertyData : InworldPropertyData
    {
        [JsonProperty("canned_phrases")] public List<string> cannedPhrases =  new List<string>
        {
            "I'm sorry, but I can't respond to that kind of content.",
            "That topic makes me uncomfortable. Let's talk about something else.",
            "I'd prefer not to discuss that. Could we change the subject?"
        };
    }
}