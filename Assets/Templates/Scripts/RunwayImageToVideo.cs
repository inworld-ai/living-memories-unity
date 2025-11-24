/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Payload structure for Runway ML image-to-video API request.
/// </summary>
[Serializable]
public class ImageToVideoPayload
{
    public string model;        // Model name (e.g. "gen4_turbo")
    public string promptText;   // Generation prompt text
    public string promptImage;  // Data URI: data:image/jpeg;base64,...
    public string ratio;        // Video ratio (e.g. "1280:720")
    public int duration;        // Duration in seconds (model-dependent)
}

/// <summary>
/// Handles image-to-video generation using Runway ML API.
/// Converts static images into animated videos with customizable prompts and parameters.
/// </summary>
public class RunwayImageToVideo : MonoBehaviour
{
    #region Serialized Fields

    [Header("General Settings")]
    [Tooltip("Enable Runway ML video generation")]
    public bool useRunwayGeneration = false;

    [Header("Runway API Configuration")]
    [Tooltip("Runway API base URL")]
    public string apiBase = "https://api.dev.runwayml.com";
    
    [Tooltip("Runway API version")]
    public string runwayApiVersion = "2024-11-06";

    [Header("Model Parameters")]
    [Tooltip("Model ID (e.g., gen4_turbo or gen3a_turbo)")]
    public string model = "gen4_turbo";

    [Tooltip("Video ratio - Gen-4 Turbo: 1280:720, Gen-3A Turbo: 1280:768 or 768:1280")]
    public string ratio = "1280:720";

    [Tooltip("Video duration in seconds (5-10)")]
    [Range(5, 10)]
    public int durationSeconds = 5;

    [Tooltip("Default generation prompt")]
    [TextArea(2, 6)]
    public string promptText = "Minimal motion. Hold framing. Cinematic lighting.";

    [Header("Image Input")]
    [Tooltip("Input texture to animate (optional, can be provided via StartGeneration)")]
    public Texture2D inputTexture;
    
    [Tooltip("Encode as JPG (smaller file size, stays under 5MB limit)")]
    public bool encodeAsJpg = true;

    [Tooltip("Alternative: Raw base64 string without data URI prefix")]
    [TextArea(3, 8)]
    public string base64WithoutPrefix;
    
    [Tooltip("MIME type for base64 input (e.g., image/jpeg or image/png)")]
    public string base64Mime = "image/jpeg";
    
    [Header("Output Settings")]
    [Tooltip("Output video filename")]
    public string fileName = "runway_i2v.mp4";

    #endregion

    #region Constants

    private const int MaxDataUriSize = 5 * 1024 * 1024; // 5 MB limit for image data URIs
    private const float PollingInterval = 2f; // Seconds between status checks
    private const float TimeoutSeconds = 300f; // 5 minutes timeout

    #endregion

    #region Data Transfer Objects

    [Serializable]
    private class CreateTaskResponse
    {
        public string id;
        public string status;
        public string createdAt;
    }

    [Serializable]
    private class ErrorInfo
    {
        public string type;
        public string message;
    }

    [Serializable]
    private class TaskStatusResponse
    {
        public string id;
        public string status;
        public string createdAt;
        public string[] output;
        public ErrorInfo error;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Starts video generation from an image with a custom prompt.
    /// </summary>
    /// <param name="userInput">Custom generation prompt text</param>
    /// <param name="image">Source image to animate</param>
    /// <param name="onGenerationComplete">Callback with result path or error message</param>
    [ContextMenu("Start Generation")]
    public void StartGeneration(string userInput, Texture2D image, Action<string> onGenerationComplete)
    {
        // Check if generation is enabled
        if (!useRunwayGeneration)
        {
            Debug.LogWarning("Runway generation is disabled.");
            onGenerationComplete?.Invoke("Error: Runway generation is disabled.");
            return;
        }
        
        // Validate API key
        if (string.IsNullOrEmpty(APIController_Memory.Instance.RunwayAPIKey))
        {
            Debug.LogError("Runway API key is missing. Please set it in APIController_Memory.");
            onGenerationComplete?.Invoke("Error: Runway API key missing.");
            return;
        }

        // Start generation coroutine
        StartCoroutine(GenerateAndDownload(userInput, image, onGenerationComplete));
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Coroutine to generate video and download the result.
    /// </summary>
    private IEnumerator GenerateAndDownload(string userInput, Texture2D image, Action<string> onGenerationComplete)
    {
        // Build data URI from texture or base64 string
        string dataUri = null;

        if (image != null)
        {
            // Encode texture to bytes
            byte[] imageBytes = encodeAsJpg ? image.EncodeToJPG(90) : image.EncodeToPNG();
            
            if (imageBytes == null || imageBytes.Length == 0)
            {
                Debug.LogError("Failed to encode image.");
                onGenerationComplete?.Invoke("Error: Failed to encode image.");
                yield break;
            }

            // Convert to base64 data URI
            string base64 = Convert.ToBase64String(imageBytes);
            string mimeType = encodeAsJpg ? "image/jpeg" : "image/png";
            dataUri = $"data:{mimeType};base64,{base64}";
        }
        else if (!string.IsNullOrEmpty(base64WithoutPrefix))
        {
            // Use provided base64 string
            if (string.IsNullOrEmpty(base64Mime))
            {
                Debug.LogError("base64Mime must be set when using base64WithoutPrefix.");
                onGenerationComplete?.Invoke("Error: base64Mime must be specified.");
                yield break;
            }
            dataUri = $"data:{base64Mime};base64,{base64WithoutPrefix}";
        }
        else
        {
            Debug.LogError("No image provided. Please provide either a Texture2D or base64 string.");
            onGenerationComplete?.Invoke("Error: No image provided.");
            yield break;
        }

        // Check data URI size
        int dataUriSize = Encoding.UTF8.GetByteCount(dataUri);
        if (dataUriSize > MaxDataUriSize)
        {
            Debug.LogWarning($"Data URI size is {dataUriSize / (1024f * 1024f):0.00} MB (exceeds {MaxDataUriSize / (1024 * 1024)} MB limit). Consider using JPG or smaller image.");
        }
        
        // Create payload
        ImageToVideoPayload payload = new ImageToVideoPayload
        {
            model = model,
            promptText = userInput,
            promptImage = dataUri,
            ratio = ratio,
            duration = durationSeconds
        };
        string json = JsonUtility.ToJson(payload);
        
        // Send create request
        using (UnityWebRequest request = new UnityWebRequest($"{apiBase}/v1/image_to_video", "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {APIController_Memory.Instance.RunwayAPIKey}");
            request.SetRequestHeader("X-Runway-Version", runwayApiVersion);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Task creation failed: {request.responseCode} {request.error}\n{request.downloadHandler.text}");
                onGenerationComplete?.Invoke($"Error: Task creation failed with code {request.responseCode}.");
                yield break;
            }

            CreateTaskResponse response = JsonUtility.FromJson<CreateTaskResponse>(request.downloadHandler.text);
            if (response == null || string.IsNullOrEmpty(response.id))
            {
                Debug.LogError($"Unexpected create response: {request.downloadHandler.text}");
                onGenerationComplete?.Invoke("Error: Invalid task creation response.");
                yield break;
            }

            Debug.Log($"Runway task created successfully: {response.id}");
            yield return StartCoroutine(PollTaskAndDownload(response.id, onGenerationComplete));
        }
    }

    /// <summary>
    /// Polls task status and downloads video when complete.
    /// </summary>
    private IEnumerator PollTaskAndDownload(string taskId, Action<string> onGenerationComplete)
    {
        string statusUrl = $"{apiBase}/v1/tasks/{taskId}";
        float startTime = Time.realtimeSinceStartup;

        while (true)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(statusUrl))
            {
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Authorization", $"Bearer {APIController_Memory.Instance.RunwayAPIKey}");
                request.SetRequestHeader("X-Runway-Version", runwayApiVersion);

                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Status check failed: {request.responseCode} {request.error}\n{request.downloadHandler.text}");
                    onGenerationComplete?.Invoke($"Error: Status check failed with code {request.responseCode}.");
                    yield break;
                }

                TaskStatusResponse status = JsonUtility.FromJson<TaskStatusResponse>(request.downloadHandler.text);
                if (status == null)
                {
                    Debug.LogError($"Invalid status response: {request.downloadHandler.text}");
                    onGenerationComplete?.Invoke("Error: Invalid status response.");
                    yield break;
                }

                // Check if task succeeded
                if (string.Equals(status.status, "SUCCEEDED", StringComparison.OrdinalIgnoreCase))
                {
                    if (status.output != null && status.output.Length > 0)
                    {
                        Debug.Log($"Task completed successfully. Downloading video...");
                        yield return StartCoroutine(DownloadFile(status.output[0], onGenerationComplete));
                        yield break;
                    }
                    
                    Debug.LogError("Task succeeded but no output URLs were provided.");
                    onGenerationComplete?.Invoke("Error: No output URL.");
                    yield break;
                }

                // Check if task failed
                if (string.Equals(status.status, "FAILED", StringComparison.OrdinalIgnoreCase))
                {
                    string errorMessage = status.error != null ? status.error.message : "Unknown error";
                    Debug.LogError($"Task failed: {errorMessage}");
                    onGenerationComplete?.Invoke($"Error: Task failed - {errorMessage}");
                    yield break;
                }

                // Check timeout
                if (Time.realtimeSinceStartup - startTime > TimeoutSeconds)
                {
                    Debug.LogError($"Task timed out after {TimeoutSeconds} seconds.");
                    onGenerationComplete?.Invoke("Error: Task timed out.");
                    yield break;
                }

                // Wait before next poll
                yield return new WaitForSeconds(PollingInterval);
            }
        }
    }

    /// <summary>
    /// Downloads video file from URL to local storage.
    /// </summary>
    private IEnumerator DownloadFile(string url, Action<string> onGenerationComplete)
    {
        string localPath = Path.Combine(Application.persistentDataPath, fileName);
        Debug.Log($"Downloading video to: {localPath}");

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.downloadHandler = new DownloadHandlerFile(localPath);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Video download failed: {request.responseCode} {request.error}");
                onGenerationComplete?.Invoke($"Error: Download failed with code {request.responseCode}.");
                yield break;
            }
        }

        Debug.Log($"Video saved successfully: {localPath}");
        Debug.Log("Note: Runway output URLs are ephemeral. The local file should be kept.");
        onGenerationComplete?.Invoke(localPath);
    }

    #endregion
}
