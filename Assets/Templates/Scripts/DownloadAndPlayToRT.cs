/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// Downloads and plays video to a RenderTexture displayed in a RawImage UI element.
/// Automatically handles aspect ratio and video preparation.
/// </summary>
public class DownloadAndPlayToRT : MonoBehaviour
{
    #region Serialized Fields 

    [Header("UI Configuration")]
    [Tooltip("Raw image component to display the video")]
    public RawImage rawImage;
    
    [Tooltip("Optional aspect ratio fitter to maintain video aspect")]
    public AspectRatioFitter aspectFitter;

    #endregion

    #region Private Fields

    private VideoPlayer _videoPlayer;
    private RenderTexture _renderTexture;
    private bool _firstFrameShown = false;

    #endregion

    #region Public Methods

    /// <summary>
    /// Plays a video from local path to the render texture.
    /// </summary>
    /// <param name="localPath">Path to the video file</param>
    /// <param name="noAudio">Whether to disable audio playback</param>
    public void PlayVideo(string localPath, bool noAudio = true)
    {
        // Stop any existing video first
        StopVideo();

        // Get or create VideoPlayer component
        _videoPlayer = GetComponent<VideoPlayer>();
        if (_videoPlayer == null)
        {
            _videoPlayer = gameObject.AddComponent<VideoPlayer>();
        }

        // Configure video player
        _videoPlayer.source = VideoSource.Url;
        _videoPlayer.url = localPath;
        _videoPlayer.audioOutputMode = noAudio ? VideoAudioOutputMode.None : VideoAudioOutputMode.Direct;
        _videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        _videoPlayer.isLooping = true;

        // Prepare video (render texture created when size is known)
        _videoPlayer.prepareCompleted += OnVideoPrepared;
        _videoPlayer.Prepare();
    }

    /// <summary>
    /// Pauses the current video playback.
    /// </summary>
    public void PauseVideo()
    {
        if (_videoPlayer != null && _videoPlayer.isPlaying)
        {
            _videoPlayer.Pause();
            Debug.Log("Video paused.");
        }
    }

    /// <summary>
    /// Resumes paused video playback.
    /// </summary>
    public void ResumeVideo()
    {
        if (_videoPlayer != null && _videoPlayer.isPaused)
        {
            _videoPlayer.Play();
            Debug.Log("Video resumed.");
        }
    }

    /// <summary>
    /// Stops video playback completely.
    /// </summary>
    public void StopVideo()
    {
        if (_videoPlayer != null && (_videoPlayer.isPlaying || _videoPlayer.isPaused))
        {
            _videoPlayer.Stop();
            Debug.Log("Video stopped.");
        }
    }

    /// <summary>
    /// Checks if video is currently playing.
    /// </summary>
    /// <returns>True if playing, false otherwise</returns>
    public bool IsPlaying()
    {
        return _videoPlayer != null && _videoPlayer.isPlaying;
    }

    /// <summary>
    /// Checks if video is currently paused.
    /// </summary>
    /// <returns>True if paused, false otherwise</returns>
    public bool IsPaused()
    {
        return _videoPlayer != null && _videoPlayer.isPaused;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Called when video preparation is complete and size is known.
    /// </summary>
    private void OnVideoPrepared(VideoPlayer player)
    {
        // Get video dimensions
        int width = (int)Mathf.Max(1, player.width);
        int height = (int)Mathf.Max(1, player.height);

        // Create render texture matching video size
        _renderTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32)
        {
            useMipMap = false,
            autoGenerateMips = false
        };
        _renderTexture.Create();

        // Assign render texture to video player
        player.targetTexture = _renderTexture;

        // Update UI
        if (rawImage != null)
        {
            rawImage.texture = _renderTexture;
            
            // Set aspect ratio
            if (aspectFitter != null && width > 0 && height > 0)
            {
                aspectFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
                aspectFitter.aspectRatio = (float)width / height;
            }
        }

        // Setup first frame callback to avoid black flash
        player.sendFrameReadyEvents = true;
        player.frameReady += OnFirstFrameReady;

        // Start playback
        player.Play();
    }

    /// <summary>
    /// Called when first frame is ready for display.
    /// </summary>
    private void OnFirstFrameReady(VideoPlayer source, long frameIdx)
    {
        if (_firstFrameShown)
        {
            return;
        }

        _firstFrameShown = true;
        source.frameReady -= OnFirstFrameReady;
        
        Debug.Log("Video playback started.");
    }

    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Cleanup resources on destroy.
    /// </summary>
    private void OnDestroy()
    {
        // Stop and cleanup video player
        if (_videoPlayer != null)
        {
            _videoPlayer.Stop();
            _videoPlayer.targetTexture = null;
        }

        // Release render texture
        if (_renderTexture != null)
        {
            _renderTexture.Release();
            Destroy(_renderTexture);
        }
    }

    #endregion
}
