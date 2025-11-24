using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class RunwayLipsync : MonoBehaviour
{
    [Header("Runway Runtime API")]
    [Tooltip("Your Runway Runtime API key (Runtime_API)")]
    public string runtimeApiKey = "YOUR_RUNTIME_API_KEY";

    [Tooltip("Base URL for Runway Runtime API")]
    public string baseUrl = "https://api.dev.runwayml.com/v1";

    [Header("Options")]
    [Tooltip("Run lightweight exploratory jobs (testing/debug).")]
    public bool exploreMode = false;

    [Tooltip("Output file name (without extension).")]
    public string outputFileName = "runway_lipsync";

    [Tooltip("Encode the image as JPEG (smaller) or PNG (lossless).")]
    public bool encodeImageAsJpg = true;

    [Tooltip("Optionally downscale the input image to keep data URI small.")]
    public bool autoResizeImage = true;

    [Tooltip("If auto-resize is ON, longest side will be clamped to this.")]
    public int maxLongSide = 1280;

    [Tooltip("API version header required by Runway.")]
    public string runwayApiVersion = "2024-11-06";

    /// <summary>
    /// Start a lip-sync job using an image Texture2D and audio bytes.
    /// audioMime examples: "audio/wav", "audio/mpeg" (mp3), "audio/ogg"
    /// </summary>
    public void StartLipsync(Texture2D image, byte[] audioBytes, string audioMime, Action<string> onComplete)
    {
        if (image == null) { onComplete?.Invoke("Error: image Texture2D is null."); return; }
        if (audioBytes == null || audioBytes.Length == 0) { onComplete?.Invoke("Error: audio bytes missing."); return; }
        if (string.IsNullOrEmpty(audioMime)) { onComplete?.Invoke("Error: audio MIME required."); return; }
        if (string.IsNullOrEmpty(runtimeApiKey)) { onComplete?.Invoke("Error: Runtime API key missing."); return; }

        StartCoroutine(Co_Run(image, audioBytes, audioMime, onComplete));
    }

    private IEnumerator Co_Run(Texture2D srcImage, byte[] audioBytes, string audioMime, Action<string> done)
    {
        // 1) Optionally resize (works for portrait or landscape)
        Texture2D imageForUpload = srcImage;
        if (autoResizeImage)
        {
            imageForUpload = ResizeIfNeeded(srcImage, maxLongSide);
        }

        // 2) Encode image
        string imageMime = encodeImageAsJpg ? "image/jpeg" : "image/png";
        byte[] imgBytes = encodeImageAsJpg ? imageForUpload.EncodeToJPG(90) : imageForUpload.EncodeToPNG();
        if (imgBytes == null || imgBytes.Length == 0)
        {
            done?.Invoke("Error: failed to encode image.");
            yield break;
        }

        // 3) Build data URIs
        string imageDataUri = ToDataUri(imgBytes, imageMime);
        string audioDataUri = ToDataUri(audioBytes, audioMime);

        // 4) Create lipsync task with data-URIs
        string taskId = null;
        yield return StartCoroutine(Co_CreateLipsync_DataUris(
            imageDataUri,
            audioDataUri,
            exploreMode,
            id => taskId = id,
            err => { done?.Invoke(err); }
        ));
        if (string.IsNullOrEmpty(taskId)) yield break;

        // 5) Poll + download
        yield return StartCoroutine(Co_PollTask(taskId, statusJson =>
        {
            var m = Regex.Match(statusJson, "(https:\\/\\/[^\\\"\\s]+\\.(mp4|mov))");
            if (!m.Success)
            {
                done?.Invoke("Error: no video url in result.");
                return;
            }
            StartCoroutine(Co_DownloadFile(m.Value, done));
        },
        err => { done?.Invoke(err); }));
    }

    // ---------- HTTP: Create lipsync with data URIs ----------
    private IEnumerator Co_CreateLipsync_DataUris(string imageDataUri, string audioDataUri, bool explore,
        Action<string> onTaskId, Action<string> onError)
    {
        var payload = new LipsyncDataUriBody
        {
            image = imageDataUri,
            audio = audioDataUri,
            exploreMode = explore
        };
        string json = JsonUtility.ToJson(payload);

        using (var req = new UnityWebRequest($"{baseUrl}/lipsync/create", "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Authorization", $"Bearer {runtimeApiKey}");
            req.SetRequestHeader("X-Runway-Version", runwayApiVersion);
            req.SetRequestHeader("Content-Type", "application/json");

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Create failed: {req.responseCode} {req.error}\n{req.downloadHandler.text}");
                onError?.Invoke("Error: lipsync create failed.");
                yield break;
            }

            // Extract "id"
            var m = Regex.Match(req.downloadHandler.text, "\"id\"\\s*:\\s*\"([^\"]+)\"");
            if (!m.Success)
            {
                Debug.LogError($"Unexpected create response: {req.downloadHandler.text}");
                onError?.Invoke("Error: could not extract task id.");
                yield break;
            }
            onTaskId?.Invoke(m.Groups[1].Value);
        }
    }

    // ---------- HTTP: Poll tasks ----------
    private IEnumerator Co_PollTask(string taskId, Action<string> onSucceededJson, Action<string> onError)
    {
        string url = $"{baseUrl}/tasks/{taskId}";
        float start = Time.realtimeSinceStartup;
        const float timeout = 300f;

        while (true)
        {
            using (var req = UnityWebRequest.Get(url))
            {
                req.downloadHandler = new DownloadHandlerBuffer();
                req.SetRequestHeader("Authorization", $"Bearer {runtimeApiKey}");
                req.SetRequestHeader("X-Runway-Version", runwayApiVersion);
                yield return req.SendWebRequest();

                if (req.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Status error: {req.responseCode} {req.error}\n{req.downloadHandler.text}");
                    onError?.Invoke("Error: polling failed.");
                    yield break;
                }

                string txt = req.downloadHandler.text;
                var m = Regex.Match(txt, "\"status\"\\s*:\\s*\"([A-Za-z_]+)\"");
                string status = m.Success ? m.Groups[1].Value.ToUpperInvariant() : "";

                if (status == "SUCCEEDED" || status == "COMPLETED")
                {
                    onSucceededJson?.Invoke(txt);
                    yield break;
                }
                if (status == "FAILED" || status == "ERROR")
                {
                    Debug.LogError($"Task FAILED: {txt}");
                    onError?.Invoke("Error: task failed.");
                    yield break;
                }
                if (Time.realtimeSinceStartup - start > timeout)
                {
                    onError?.Invoke("Error: task timed out.");
                    yield break;
                }

                yield return new WaitForSeconds(2f);
            }
        }
    }

    // ---------- HTTP: Download ----------
    private IEnumerator Co_DownloadFile(string url, Action<string> onDone)
    {
        string stamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
        string path = Path.Combine(Application.persistentDataPath, $"{outputFileName}_{stamp}.mp4");

        using (var req = UnityWebRequest.Get(url))
        {
            req.downloadHandler = new DownloadHandlerFile(path);
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Download failed: {req.responseCode} {req.error}");
                onDone?.Invoke("Error: download failed.");
                yield break;
            }
        }
        Debug.Log($"Saved video: {path}");
        onDone?.Invoke(path);
    }

    // ---------- Helpers ----------
    [Serializable]
    private class LipsyncDataUriBody
    {
        public string image;       // data:image/jpeg;base64,...
        public string audio;       // data:audio/wav;base64,...
        public bool exploreMode;   // optional
    }

    private static string ToDataUri(byte[] bytes, string mime)
    {
        var b64 = Convert.ToBase64String(bytes);
        return $"data:{mime};base64,{b64}";
    }

    // Optional: downscale keeping aspect ratio if longest side > maxLongSide
    private Texture2D ResizeIfNeeded(Texture2D src, int maxSide)
    {
        int w = src.width;
        int h = src.height;
        int longest = Mathf.Max(w, h);
        if (longest <= maxSide) return src;

        float scale = (float)maxSide / longest;
        int nw = Mathf.Max(1, Mathf.RoundToInt(w * scale));
        int nh = Mathf.Max(1, Mathf.RoundToInt(h * scale));

        RenderTexture rt = RenderTexture.GetTemporary(nw, nh, 0, RenderTextureFormat.ARGB32);
        Graphics.Blit(src, rt);

        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D dst = new Texture2D(nw, nh, TextureFormat.RGBA32, false);
        dst.ReadPixels(new Rect(0, 0, nw, nh), 0, 0);
        dst.Apply();

        RenderTexture.active = prev;
        RenderTexture.ReleaseTemporary(rt);
        return dst;
    }
}
