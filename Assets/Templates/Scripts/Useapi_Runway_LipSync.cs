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
/// Handles lip-sync video generation using UseAPI.net as a proxy for Runway ML.
/// Manages the complete workflow: asset upload, task creation, polling, and video download.
/// </summary>
public class Useapi_Runway_LipSync : MonoBehaviour
{
    #region Serialized Fields

    [Header("Polling Configuration")]
    [Tooltip("Interval between status checks in seconds")]
    public float pollIntervalSeconds = 5f;
    
    [Tooltip("Maximum time to wait for task completion in seconds")]
    public float pollTimeoutSeconds = 600f;

    #endregion

    #region Constants

    private const string BaseUrl = "https://api.useapi.net/v1/runwayml";

    #endregion

    #region Data Transfer Objects

    [Serializable]
    private class AssetResponse
    {
        public string assetId;
    }

    [Serializable]
    private class AssetResponseAlt
    {
        public string assetId;
        public string id;
    }

    [Serializable]
    private class LipsyncCreateBody
    {
        public string image_assetId;
        public string audio_assetId;
        public string voiceId;
        public string voice_text;
        public string model_id;
        public bool exploreMode;
        public string replyUrl;
        public string replyRef;
        public int maxJobs;
    }

    [Serializable]
    private class TaskCreateResponse
    {
        public string taskId;
    }

    [Serializable]
    private class TaskStatusResponseLoose
    {
        public string status;
        public Artifact[] artifacts;

        [Serializable]
        public class Artifact
        {
            public string url;
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Generates a lip-synced video from a portrait image and audio data.
    /// </summary>
    /// <param name="portrait">Portrait image texture</param>
    /// <param name="wavBytes">Audio data in WAV format</param>
    /// <param name="outputFileName">Output filename for the generated video</param>
    /// <param name="onSucceededLocalPath">Callback with local file path on success</param>
    /// <param name="onFailed">Callback with error message on failure</param>
    public IEnumerator GenerateLipSyncVideo(
        Texture2D portrait,
        byte[] wavBytes,
        string outputFileName = "runway_lipsync.mp4",
        Action<string> onSucceededLocalPath = null,
        Action<string> onFailed = null)
    {
        // Validate inputs
        if (portrait == null)
        {
            onFailed?.Invoke("Portrait texture is null.");
            yield break;
        }

        if (wavBytes == null || wavBytes.Length == 0)
        {
            onFailed?.Invoke("Audio data is null or empty.");
            yield break;
        }

        if (string.IsNullOrEmpty(APIController_Lipsync.Instance.UseAPIToken))
        {
            onFailed?.Invoke("UseAPI token is not configured.");
            yield break;
        }

        // Step 1: Upload image asset
        string imageAssetId = null;
        string imageError = null;
        string imageName = $"unity_portrait_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.png";
        byte[] imageBytes = portrait.EncodeToPNG();

        yield return UploadAsset(
            bytes: imageBytes,
            contentType: "image/png",
            name: imageName,
            onComplete: (assetId, err) => { imageAssetId = assetId; imageError = err; }
        );

        if (!string.IsNullOrEmpty(imageError))
        {
            onFailed?.Invoke($"Failed to upload image: {imageError}");
            yield break;
        }

        if (string.IsNullOrEmpty(imageAssetId))
        {
            onFailed?.Invoke("Image upload succeeded but asset ID is missing.");
            yield break;
        }

        Debug.Log($"Image uploaded successfully. Asset ID: {imageAssetId}");

        // Step 2: Upload audio asset
        string audioAssetId = null;
        string audioError = null;
        string audioName = $"unity_audio_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.wav";

        yield return UploadAsset(
            bytes: wavBytes,
            contentType: "audio/wav",
            name: audioName,
            onComplete: (assetId, err) => { audioAssetId = assetId; audioError = err; }
        );

        if (!string.IsNullOrEmpty(audioError))
        {
            onFailed?.Invoke($"Failed to upload audio: {audioError}");
            yield break;
        }

        if (string.IsNullOrEmpty(audioAssetId))
        {
            onFailed?.Invoke("Audio upload succeeded but asset ID is missing.");
            yield break;
        }

        Debug.Log($"Audio uploaded successfully. Asset ID: {audioAssetId}");

        // Step 3: Create lip-sync task
        string taskId = null;
        string createError = null;

        yield return CreateLipsync(
            imageAssetId: imageAssetId,
            audioAssetId: audioAssetId,
            onComplete: (tid, err) => { taskId = tid; createError = err; }
        );

        if (!string.IsNullOrEmpty(createError))
        {
            onFailed?.Invoke($"Failed to create lip-sync task: {createError}");
            yield break;
        }

        if (string.IsNullOrEmpty(taskId))
        {
            onFailed?.Invoke("Task creation succeeded but task ID is missing.");
            yield break;
        }

        Debug.Log($"Lip-sync task created successfully. Task ID: {taskId}");

        // Step 4: Poll for result
        string videoUrl = null;
        string pollError = null;

        yield return PollForResult(
            taskId: taskId,
            onComplete: (url, err) => { videoUrl = url; pollError = err; }
        );

        if (!string.IsNullOrEmpty(pollError))
        {
            onFailed?.Invoke($"Failed to poll task status: {pollError}");
            yield break;
        }

        if (string.IsNullOrEmpty(videoUrl))
        {
            onFailed?.Invoke("Task succeeded but video URL is missing.");
            yield break;
        }

        Debug.Log($"Video generation completed. URL: {videoUrl}");

        // Step 5: Download video file
        string safeName = $"{SafeTimestamp()}_{MakeSafeFileName(outputFileName)}";
        string savePath = Path.Combine(Application.persistentDataPath, safeName);
        string downloadError = null;

        yield return DownloadToFile(
            url: videoUrl,
            savePath: savePath,
            onComplete: err => downloadError = err
        );

        if (!string.IsNullOrEmpty(downloadError))
        {
            onFailed?.Invoke($"Failed to download video: {downloadError}");
            yield break;
        }

        Debug.Log($"Video downloaded successfully: {savePath}");
        onSucceededLocalPath?.Invoke(savePath);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Uploads an asset (image or audio) to UseAPI.net.
    /// </summary>
    private IEnumerator UploadAsset(byte[] bytes, string contentType, string name, Action<string, string> onComplete)
    {
        string url = $"{BaseUrl}/assets/?name={UnityWebRequest.EscapeURL(name)}";
        
        // Add optional email parameter if configured
        if (!string.IsNullOrEmpty(APIController_Lipsync.Instance.RunwayAccountEmail))
        {
            url += $"&email={UnityWebRequest.EscapeURL(APIController_Lipsync.Instance.RunwayAccountEmail)}";
        }

        using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
        {
            request.uploadHandler = new UploadHandlerRaw(bytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Authorization", $"Bearer {APIController_Lipsync.Instance.UseAPIToken}");
            request.SetRequestHeader("Content-Type", contentType);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                onComplete?.Invoke(null, $"HTTP {request.responseCode}: {request.error}\n{request.downloadHandler.text}");
                yield break;
            }

            try
            {
                // Try primary response format
                AssetResponse response = JsonUtility.FromJson<AssetResponse>(request.downloadHandler.text);
                string assetId = response?.assetId;

                // Try alternative format if primary failed
                if (string.IsNullOrEmpty(assetId))
                {
                    AssetResponseAlt altResponse = JsonUtility.FromJson<AssetResponseAlt>(request.downloadHandler.text);
                    assetId = altResponse?.assetId ?? altResponse?.id;
                }

                if (string.IsNullOrEmpty(assetId))
                {
                    onComplete?.Invoke(null, "Asset uploaded but ID is missing in response.");
                }
                else
                {
                    onComplete?.Invoke(assetId, null);
                }
            }
            catch (Exception ex)
            {
                onComplete?.Invoke(null, $"Failed to parse asset response: {ex.Message}\n{request.downloadHandler.text}");
            }
        }
    }

    /// <summary>
    /// Creates a lip-sync generation task.
    /// </summary>
    private IEnumerator CreateLipsync(string imageAssetId, string audioAssetId, Action<string, string> onComplete)
    {
        string url = $"{BaseUrl}/lipsync/create";

        LipsyncCreateBody body = new LipsyncCreateBody
        {
            image_assetId = imageAssetId,
            audio_assetId = audioAssetId
        };

        string json = JsonUtility.ToJson(body);
        
        using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
        {
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Authorization", $"Bearer {APIController_Lipsync.Instance.UseAPIToken}");
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                onComplete?.Invoke(null, $"HTTP {request.responseCode}: {request.error}\n{request.downloadHandler.text}");
                yield break;
            }

            try
            {
                TaskCreateResponse response = JsonUtility.FromJson<TaskCreateResponse>(request.downloadHandler.text);
                
                if (string.IsNullOrEmpty(response?.taskId))
                {
                    onComplete?.Invoke(null, "Lip-sync task created but task ID is missing.");
                }
                else
                {
                    onComplete?.Invoke(response.taskId, null);
                }
            }
            catch (Exception ex)
            {
                onComplete?.Invoke(null, $"Failed to parse lip-sync creation response: {ex.Message}\n{request.downloadHandler.text}");
            }
        }
    }

    /// <summary>
    /// Polls task status until completion or timeout.
    /// </summary>
    private IEnumerator PollForResult(string taskId, Action<string, string> onComplete)
    {
        float startTime = Time.realtimeSinceStartup;
        string decodedTaskId = string.IsNullOrEmpty(taskId) ? taskId : Uri.UnescapeDataString(taskId);

        while (Time.realtimeSinceStartup - startTime < pollTimeoutSeconds)
        {
            string url = $"{BaseUrl}/tasks/{decodedTaskId}";
            
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.SetRequestHeader("Authorization", $"Bearer {APIController_Lipsync.Instance.UseAPIToken}");
                request.SetRequestHeader("Accept", "application/json");
                
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    onComplete?.Invoke(null, $"HTTP {request.responseCode}: {request.error}\n{request.downloadHandler.text}");
                    yield break;
                }

                try
                {
                    TaskStatusResponseLoose status = JsonUtility.FromJson<TaskStatusResponseLoose>(request.downloadHandler.text);
                    string taskStatus = status?.status ?? "";

                    // Check if task succeeded
                    if (string.Equals(taskStatus, "SUCCEEDED", StringComparison.OrdinalIgnoreCase))
                    {
                        string videoUrl = (status?.artifacts != null && status.artifacts.Length > 0)
                            ? status.artifacts[0]?.url
                            : null;

                        if (string.IsNullOrEmpty(videoUrl))
                        {
                            onComplete?.Invoke(null, "Task succeeded but video URL is missing from artifacts.");
                        }
                        else
                        {
                            onComplete?.Invoke(videoUrl, null);
                        }
                        yield break;
                    }

                    // Check if task failed
                    if (string.Equals(taskStatus, "FAILED", StringComparison.OrdinalIgnoreCase))
                    {
                        onComplete?.Invoke(null, $"Task failed: {request.downloadHandler.text}");
                        yield break;
                    }

                    // Task still in progress
                    Debug.Log($"Task status: {taskStatus}. Polling again in {pollIntervalSeconds} seconds...");
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Failed to parse poll response: {ex.Message}");
                }
            }

            yield return new WaitForSeconds(pollIntervalSeconds);
        }

        onComplete?.Invoke(null, $"Task polling timed out after {pollTimeoutSeconds} seconds.");
    }

    /// <summary>
    /// Downloads video file from URL to local storage.
    /// </summary>
    private IEnumerator DownloadToFile(string url, string savePath, Action<string> onComplete)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Ensure directory exists
            string directory = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            request.downloadHandler = new DownloadHandlerFile(savePath)
            {
                removeFileOnAbort = true
            };

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                onComplete?.Invoke($"HTTP {request.responseCode}: {request.error}");
            }
            else
            {
                onComplete?.Invoke(null);
            }
        }
    }

    /// <summary>
    /// Generates a safe timestamp string for filenames.
    /// </summary>
    private static string SafeTimestamp()
    {
        return DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
    }

    /// <summary>
    /// Creates a safe filename by removing invalid characters.
    /// </summary>
    private static string MakeSafeFileName(string name, string defaultExtension = "mp4")
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            name = $"runway_lipsync.{defaultExtension}";
        }

        // Remove any path components
        name = Path.GetFileName(name);

        // Replace invalid characters
        char[] invalidChars = Path.GetInvalidFileNameChars();
        StringBuilder sb = new StringBuilder(name.Length);
        
        foreach (char ch in name)
        {
            sb.Append(Array.IndexOf(invalidChars, ch) >= 0 ? '_' : ch);
        }

        string cleaned = sb.ToString().Trim().TrimEnd('.');

        // Add extension if missing
        if (string.IsNullOrEmpty(Path.GetExtension(cleaned)))
        {
            cleaned += "." + defaultExtension;
        }

        return cleaned;
    }

    #endregion
}
