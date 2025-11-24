// Useapi_Runway_LipSync.cs
// Unity 2020+.
// Fix: no ref/out in IEnumerators. Each step returns via callbacks.

using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Useapi_Runway_LipSync : MonoBehaviour
{
    [Header("Polling")]
    public float pollIntervalSeconds = 5f;
    public float pollTimeoutSeconds  = 600f;

    const string BaseUrl = "https://api.useapi.net/v1/runwayml";

    // Public entry point
    public IEnumerator GenerateLipSyncVideo(
        Texture2D portrait,
        byte[] wavBytes,
        string outputFileName = "runway_lipsync.mp4",
        Action<string> onSucceededLocalPath = null,
        Action<string> onFailed = null)
    {
        if (portrait == null) { onFailed?.Invoke("portrait Texture2D is null"); yield break; }
        if (wavBytes == null || wavBytes.Length == 0) { onFailed?.Invoke("wavBytes is null/empty"); yield break; }
        if (string.IsNullOrEmpty(APIController_Lipsync.Instance.useAPIToken)) { onFailed?.Invoke("apiToken is empty"); yield break; }

        // 1) Upload image asset
        string imageAssetId = null;
        string imageErr     = null;
        string imageName    = $"unity_portrait_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.png";
        byte[] imageBytes   = portrait.EncodeToPNG();

        yield return UploadAsset(
            bytes: imageBytes,
            contentType: "image/png",
            name: imageName,
            onComplete: (assetId, err) => { imageAssetId = assetId; imageErr = err; }
        );

        if (!string.IsNullOrEmpty(imageErr)) { onFailed?.Invoke($"Upload image failed: {imageErr}"); yield break; }
        if (string.IsNullOrEmpty(imageAssetId)) { onFailed?.Invoke("Upload image succeeded but assetId is empty."); yield break; }

        // 2) Upload audio asset
        string audioAssetId = null;
        string audioErr     = null;
        string audioName    = $"unity_audio_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.wav";

        yield return UploadAsset(
            bytes: wavBytes,
            contentType: "audio/wav", // IMPORTANT: not audio/x-wav
            name: audioName,
            onComplete: (assetId, err) => { audioAssetId = assetId; audioErr = err; }
        );

        if (!string.IsNullOrEmpty(audioErr)) { onFailed?.Invoke($"Upload audio failed: {audioErr}"); yield break; }
        if (string.IsNullOrEmpty(audioAssetId)) { onFailed?.Invoke("Upload audio succeeded but assetId is empty."); yield break; }

        // 3) Create lipsync task
        string taskId  = null;
        string createErr = null;

        yield return CreateLipsync(
            imageAssetId: imageAssetId,
            audioAssetId: audioAssetId,
            onComplete: (tid, err) => { taskId = tid; createErr = err; }
        );

        if (!string.IsNullOrEmpty(createErr)) { onFailed?.Invoke($"Create lipsync failed: {createErr}"); yield break; }
        if (string.IsNullOrEmpty(taskId)) { onFailed?.Invoke("Create lipsync succeeded but taskId is empty."); yield break; }

        // 4) Poll for result
        string videoUrl = null;
        string pollErr  = null;

        yield return PollForResult(
            taskId: taskId,
            onComplete: (url, err) => { videoUrl = url; pollErr = err; }
        );

        if (!string.IsNullOrEmpty(pollErr)) { onFailed?.Invoke($"Poll failed: {pollErr}"); yield break; }
        if (string.IsNullOrEmpty(videoUrl)) { onFailed?.Invoke("Task succeeded but video URL is missing."); yield break; }

        // 5) Download MP4
        //string savePath = Path.Combine(Application.persistentDataPath, $"{DateTime.Today.Date.TimeOfDay.ToString()}_{outputFileName}");
        var safeName = $"{SafeTimestamp()}_{MakeSafeFileName(outputFileName)}";
        string savePath = System.IO.Path.Combine(Application.persistentDataPath, safeName);

        string dlErr    = null;

        yield return DownloadToFile(
            url: videoUrl,
            savePath: savePath,
            onComplete: err => dlErr = err
        );

        if (!string.IsNullOrEmpty(dlErr)) { onFailed?.Invoke($"Download failed: {dlErr}"); yield break; }

        onSucceededLocalPath?.Invoke(savePath);
    }

    // --- Helpers (callback style, no out/ref) ---

    private IEnumerator UploadAsset(byte[] bytes, string contentType, string name, Action<string,string> onComplete)
    {
        string url = $"{BaseUrl}/assets/?name={UnityWebRequest.EscapeURL(name)}";
        if (!string.IsNullOrEmpty(APIController_Lipsync.Instance.runwayAccountEmail))
            url += $"&email={UnityWebRequest.EscapeURL(APIController_Lipsync.Instance.runwayAccountEmail)}";

        using (var req = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
        {
            req.uploadHandler   = new UploadHandlerRaw(bytes);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Authorization", $"Bearer {APIController_Lipsync.Instance.useAPIToken}");
            req.SetRequestHeader("Content-Type", contentType);

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                onComplete?.Invoke(null, $"HTTP {req.responseCode}: {req.error}\n{req.downloadHandler.text}");
                yield break;
            }

            try
            {
                var json = JsonUtility.FromJson<AssetResponse>(req.downloadHandler.text);
                var assetId = json != null ? json.assetId : null;

                if (string.IsNullOrEmpty(assetId))
                {
                    var alt = JsonUtility.FromJson<AssetResponseAlt>(req.downloadHandler.text);
                    assetId = alt?.assetId ?? alt?.id;
                }

                if (string.IsNullOrEmpty(assetId))
                    onComplete?.Invoke(null, "Upload succeeded but assetId missing in response.");
                else
                    onComplete?.Invoke(assetId, null);
            }
            catch (Exception ex)
            {
                onComplete?.Invoke(null, "Failed to parse /assets response: " + ex.Message + "\n" + req.downloadHandler.text);
            }
        }
    }

    private IEnumerator CreateLipsync(string imageAssetId, string audioAssetId, Action<string,string> onComplete)
    {
        string url = $"{BaseUrl}/lipsync/create";

        var body = new LipsyncCreateBody
        {
            image_assetId = imageAssetId,
            audio_assetId = audioAssetId,
            // Optional fields:
            // model_id   = "eleven_multilingual_v2",
            // exploreMode = false,
            // replyUrl, replyRef, maxJobs, voiceId, voice_text...
        };

        var json = JsonUtility.ToJson(body);
        using (var req = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
        {
            req.uploadHandler   = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Authorization", $"Bearer {APIController_Lipsync.Instance.useAPIToken}");
            req.SetRequestHeader("Content-Type", "application/json");

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                onComplete?.Invoke(null, $"HTTP {req.responseCode}: {req.error}\n{req.downloadHandler.text}");
                yield break;
            }

            try
            {
                var res = JsonUtility.FromJson<TaskCreateResponse>(req.downloadHandler.text);
                if (string.IsNullOrEmpty(res?.taskId))
                    onComplete?.Invoke(null, "Create lipsync succeeded but taskId missing.");
                else
                    onComplete?.Invoke(res.taskId, null);
            }
            catch (Exception ex)
            {
                onComplete?.Invoke(null, "Failed to parse lipsync/create response: " + ex.Message + "\n" + req.downloadHandler.text);
            }
        }
    }

    private IEnumerator PollForResult(string taskId, Action<string,string> onComplete)
    {
        float start = Time.realtimeSinceStartup;
        // If taskId arrived URL-encoded, decode it
        string tid = string.IsNullOrEmpty(taskId) ? taskId : Uri.UnescapeDataString(taskId);

        while (Time.realtimeSinceStartup - start < pollTimeoutSeconds)
        {
            string url = $"{BaseUrl}/tasks/{tid}";
            using (var req = UnityWebRequest.Get(url))
            {
                req.SetRequestHeader("Authorization", $"Bearer {APIController_Lipsync.Instance.useAPIToken}");
                req.SetRequestHeader("Accept", "application/json");
                yield return req.SendWebRequest();

                if (req.result != UnityWebRequest.Result.Success)
                {
                    onComplete?.Invoke(null, $"HTTP {req.responseCode}: {req.error}\n{req.downloadHandler.text}");
                    yield break;
                }

                try
                {
                    var loose = JsonUtility.FromJson<TaskStatusResponseLoose>(req.downloadHandler.text);
                    var status = loose?.status ?? "";
                    if (string.Equals(status, "SUCCEEDED", StringComparison.OrdinalIgnoreCase))
                    {
                        var videoUrl = (loose?.artifacts != null && loose.artifacts.Length > 0)
                            ? loose.artifacts[0]?.url : null;
                        onComplete?.Invoke(videoUrl, string.IsNullOrEmpty(videoUrl) ? "Task succeeded but artifacts URL missing." : null);
                        yield break;
                    }
                    if (string.Equals(status, "FAILED", StringComparison.OrdinalIgnoreCase))
                    {
                        onComplete?.Invoke(null, "Task failed: " + req.downloadHandler.text);
                        yield break;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("Poll parse warning: " + ex.Message);
                }
            }
            yield return new WaitForSeconds(pollIntervalSeconds);
        }
        onComplete?.Invoke(null, $"Polling timed out after {pollTimeoutSeconds} seconds.");
    }


    private IEnumerator DownloadToFile(string url, string savePath, Action<string> onComplete)
    {
        using (var req = UnityWebRequest.Get(url))
        {
            string dir = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            req.downloadHandler = new DownloadHandlerFile(savePath) { removeFileOnAbort = true };
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
                onComplete?.Invoke($"HTTP {req.responseCode}: {req.error}");
            else
                onComplete?.Invoke(null);
        }
    }

    // ---- DTOs ----
    [Serializable] private class AssetResponse   { public string assetId; }
    [Serializable] private class AssetResponseAlt{ public string assetId; public string id; }

    [Serializable]
    private class LipsyncCreateBody
    {
        public string image_assetId;
        public string audio_assetId;
        public string voiceId;
        public string voice_text;
        public string model_id;
        public bool   exploreMode;
        public string replyUrl;
        public string replyRef;
        public int    maxJobs;
    }

    [Serializable] private class TaskCreateResponse { public string taskId; }

    [Serializable]
    private class TaskStatusResponseLoose
    {
        public string status;
        public Artifact[] artifacts;
        [Serializable] public class Artifact { public string url; }
    }
    
    
    private static string SafeTimestamp() => DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");

    private static string MakeSafeFileName(string name, string defaultExt = "mp4")
    {
        if (string.IsNullOrWhiteSpace(name)) name = $"runway_lipsync.{defaultExt}";
        name = System.IO.Path.GetFileName(name); // strip any accidental path
        char[] invalid = System.IO.Path.GetInvalidFileNameChars();
        var sb = new System.Text.StringBuilder(name.Length);
        foreach (var ch in name) sb.Append(Array.IndexOf(invalid, ch) >= 0 ? '_' : ch);
        var cleaned = sb.ToString().Trim().TrimEnd('.');
        if (string.IsNullOrEmpty(System.IO.Path.GetExtension(cleaned)))
            cleaned += "." + defaultExt;
        return cleaned;
    }

}

/*
Usage:

public class Demo : MonoBehaviour
{
    public Useapi_Runway_LipSync client;
    public Texture2D portrait;

    void Start()
    {
        var wavPath = Path.Combine(Application.streamingAssetsPath, "speech.wav");
        byte[] wavBytes = File.ReadAllBytes(wavPath);

        StartCoroutine(client.GenerateLipSyncVideo(
            portrait,
            wavBytes,
            "runway_lipsync_result.mp4",
            onSucceededLocalPath: path => Debug.Log("Saved to: " + path),
            onFailed: err => Debug.LogError(err)
        ));
    }
}
*/
