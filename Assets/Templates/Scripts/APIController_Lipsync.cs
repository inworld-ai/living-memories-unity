/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;
using Inworld;

/// <summary>
/// API Controller for Lip Sync scene.
/// Manages API keys for Inworld AI, UseAPI.net, and Runway ML integration.
/// </summary>
public class APIController_Lipsync : SingletonBehavior<APIController_Lipsync>
{
    #region Serialized Fields

    [Header("API Configuration")]
    [Tooltip("Your Inworld AI API key")]
    [SerializeField] private string _inworldAPIKey = "YOUR_INWORLD_API_KEY";
    
    [Tooltip("Your UseAPI.net token for Runway ML proxy")]
    [SerializeField] private string _useAPIToken = "YOUR_USE_API_Token";

    [Tooltip("(Optional) If multiple Runway accounts are linked in useapi.net, specify which one to use")]
    [SerializeField] private string _runwayAccountEmail = "YOUR_RUNWAY_EMAIL";

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the Inworld AI API key.
    /// </summary>
    public string InworldAPIKey => _inworldAPIKey;
    
    /// <summary>
    /// Gets the UseAPI.net token.
    /// </summary>
    public string UseAPIToken => _useAPIToken;
    
    /// <summary>
    /// Gets the Runway account email.
    /// </summary>
    public string RunwayAccountEmail => _runwayAccountEmail;

    #endregion
}
