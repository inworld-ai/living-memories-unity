/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Drives visual effects for memory companion loading animation.
/// Controls particle emission, attraction, and texture mapping for VFX graph.
/// </summary>
public class MemoryVFXDriver : MonoBehaviour
{
    #region Serialized Fields

    [Header("VFX Configuration")]
    [Tooltip("Visual effect graph component")]
    public VisualEffect vfx;
    
    [Tooltip("UI frame rect transform for target position")]
    public RectTransform frameRect;
    
    [Tooltip("Camera rendering the UI (usually main camera)")]
    public Camera uiCamera;
    
    [Tooltip("Source texture for particle color sampling (the memory image)")]
    public Texture2D sourceTex;
    
    [Tooltip("Optional spawn mask texture")]
    public Texture2D spawnMask;

    [Header("Effect Parameters")]
    [Tooltip("Initial particle burst rate (particles per second)")]
    public float startBurstRate = 500000f;
    
    [Tooltip("Duration of the burst effect in seconds")]
    public float burstSeconds = 1.5f;
    
    [Tooltip("Particle attraction force strength")]
    public float attraction = 28f;

    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Initialize VFX parameters and start the effect.
    /// </summary>
    private void Start()
    {
        if (vfx == null)
        {
            Debug.LogError("Visual Effect component is not assigned.");
            return;
        }

        // Calculate and set target position in world space
        Vector3 worldTarget = GetFrameWorldCenter();
        vfx.SetVector3("TargetPosition", worldTarget);

        // Set textures
        if (sourceTex != null)
        {
            vfx.SetTexture("SourceTex", sourceTex);
        }
        
        if (spawnMask != null)
        {
            vfx.SetTexture("SpawnMask", spawnMask);
        }

        // Set effect parameters
        vfx.SetFloat("AttractionForce", attraction);
        vfx.SetFloat("BurstSeconds", burstSeconds);

        // Start the effect
        vfx.SendEvent("OnPlay");
        
        Debug.Log("Memory VFX effect started.");
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Calculates the world center position of the UI frame.
    /// </summary>
    /// <returns>World space center position</returns>
    private Vector3 GetFrameWorldCenter()
    {
        if (frameRect == null)
        {
            Debug.LogWarning("Frame RectTransform is null. Using zero vector.");
            return Vector3.zero;
        }

        Vector3[] corners = new Vector3[4];
        frameRect.GetWorldCorners(corners);
        Vector3 worldCenter = (corners[0] + corners[2]) * 0.5f;
        return worldCenter;
    }

    #endregion
}