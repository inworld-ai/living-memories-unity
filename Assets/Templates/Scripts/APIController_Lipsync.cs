using UnityEngine;
using Inworld;

public class APIController_Lipsync : SingletonBehavior<APIController_Lipsync>
{
    [SerializeField] private string m_InworldAPIKey = "YOUR_INWORLD_API_KEY";
    public string imworldAPIKey {get {return m_InworldAPIKey;}}
    
    [SerializeField] private string m_UseAPIToken = "YOUR_USE_API_Token";
    public string useAPIToken { get {return m_InworldAPIKey;}}

    [Tooltip("(Optional) If multiple Runway accounts are linked in useapi.net, choose which one gets the job.")]
    [SerializeField] private string m_RunwayAccountEmail = "YOUR_RUNWAY_EMAIL";
    public string  runwayAccountEmail { get {return m_RunwayAccountEmail;}}
}
