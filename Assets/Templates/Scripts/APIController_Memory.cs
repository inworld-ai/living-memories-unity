using UnityEngine;
using Inworld;
public class APIController_Memory : SingletonBehavior<APIController_Memory>
{
    // Get your Inworld API following the steps here: https://docs.inworld.ai/api-reference/introduction
    [SerializeField] private string inworldAPIKey = "YOUR_INWORLD_API_KEY";
    public string InworldAPIKey { get => inworldAPIKey; }
    
    // Get your Runway API following the steps here: https://docs.dev.runwayml.com/guides/setup/ 
    [SerializeField] private string runwayAPIKey = "YOUR_RUNWAY_API_KEY";
    public string RunwayAPIKey { get => runwayAPIKey; }
}
