/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;
using Inworld;

/// <summary>
/// API Controller for Memory Companion scene.
/// Manages API keys for Inworld AI and Runway ML services.
/// </summary>
public class APIController_Memory : SingletonBehavior<APIController_Memory>
{
    #region Serialized Fields

    [Header("API Configuration")]
    [Tooltip("Get your Inworld API key from: https://docs.inworld.ai/api-reference/introduction")]
    [SerializeField] private string _inworldAPIKey = "YOUR_INWORLD_API_KEY";
    
    [Tooltip("Get your Runway API key from: https://docs.dev.runwayml.com/guides/setup/")]
    [SerializeField] private string _runwayAPIKey = "YOUR_RUNWAY_API_KEY";

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the Inworld AI API key.
    /// </summary>
    public string InworldAPIKey => _inworldAPIKey;
    
    /// <summary>
    /// Gets the Runway ML API key.
    /// </summary>
    public string RunwayAPIKey => _runwayAPIKey;

    #endregion
}
