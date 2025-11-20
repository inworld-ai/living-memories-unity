using System;


namespace Inworld.Framework.Graph
{
    [Serializable]
    public class SafetyExecutionConfigPropData : ConfigData
    {
        public SafetyExecutionPropertyData properties;
    }
    
    [Serializable]
    public class SafetyExecutionPropertyData : InworldPropertyData
    {

    }
}