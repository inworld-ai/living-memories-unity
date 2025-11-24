using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class ImageToVideoPayload
{
    public string model;        // e.g. "gen4_turbo"
    public string promptText;   // your multi-line text is OK now
    public string promptImage;  // data URI: data:image/jpeg;base64,...
    public string ratio;        // e.g. "1280:720" (see notes)
    public int duration;        // 5 (model-dependent)
}

// Attach to a GameObject and call StartGeneration() (e.g., from Start()).
public class RunwayImageToVideo : MonoBehaviour
{
    public bool useRunwayGenration = false;
    [Header("Runway API")]
    [Tooltip("Runway API Settings")]
    public string apiBase = "https://api.dev.runwayml.com";
    public string runwayApiVersion = "2024-11-06";

    [Header("Model & Params")]
    [Tooltip("Model id, e.g. gen4_turbo or gen3a_turbo")]
    public string model = "gen4_turbo";

    // Use Gen-4 Turbo ratios like 1280:720; for Gen-3A Turbo use 1280:768 / 768:1280.
    public string ratio = "1280:720";

    [Range(5, 10)]
    public int durationSeconds = 5;

    [TextArea(2, 6)]
    public string promptText = "Minimal motion. Hold framing. Cinematic lighting.";
/*
    [Header("Image Inputs")]
    [Tooltip("HTTPS URL or data URI for the starting image")]
    public string promptImageUrl = "https://example.com/your_image.jpg";

    [Tooltip("(Optional) HTTPS URL or data URI for the ending image; if set, the clip will end on this image")]
    public string endImageUrl = ""; // leave empty to start-only
*/
    [Header("Image (choose ONE input source)")]
    [Tooltip("If provided, we encode this Texture2D to a data URI (PNG or JPG).")]
    public Texture2D inputTexture;
    public bool encodeAsJpg = true;       // JPG is smaller; helps stay under data URI size limit.
    [Tooltip("OR: Raw base64 (no prefix). We'll add data URI prefix below.")]
    [TextArea(3,8)] public string base64WithoutPrefix;
    [Tooltip("MIME for base64WithoutPrefix, e.g. image/jpeg or image/png")]
    public string base64Mime = "image/jpeg";
    
    [Header("Output")]
    public string fileName = "runway_i2v.mp4";

    [ContextMenu("Start Generation")]
    public void StartGeneration(string userInput, Texture2D image, Action<string> onGenerationComplete)
    {
        if (!useRunwayGenration)
        {
            Debug.Log($"Runway generation skipped!");
            onGenerationComplete?.Invoke("Error: Runway generation skipped!");
            return;
        }
        
        if (string.IsNullOrEmpty(APIController_Memory.Instance.RunwayAPIKey))
        {
            Debug.LogError("Runway API key missing.");
            onGenerationComplete?.Invoke("Error: Runway API key missing.");
            return;
        }
        /*
        if (string.IsNullOrEmpty(promptImageUrl))
        {
            Debug.LogError("promptImageUrl is required (HTTPS URL or data URI).");
            return;
        }*/
        StartCoroutine(Co_GenerateAndDownload(userInput, image, onGenerationComplete));
    }

    // --- DTOs ---
    [Serializable] private class CreateTaskResponse { public string id; public string status; public string createdAt; }
    [Serializable] private class ErrorInfo { public string type; public string message; }
    [Serializable] private class TaskStatusResponse { public string id; public string status; public string createdAt; public string[] output; public ErrorInfo error; }

    private IEnumerator Co_GenerateAndDownload(string userInput, Texture2D image, Action<string> onGenerationComplete)
    {
        // Build minimal JSON by hand so we can express `promptImage` as string OR array.
        // If endImageUrl is provided, we send the array form with positions per docs.
        // Otherwise we send a single string for `promptImage`.
        // 1) Build data URI from either Texture2D or raw base64 string
        string dataUri = null;

        //if (inputTexture != null)
        if (image != null)
        {
            //byte[] bytes = encodeAsJpg ? inputTexture.EncodeToJPG(90) : inputTexture.EncodeToPNG();
            byte[] bytes = encodeAsJpg ? image.EncodeToJPG(90) : image.EncodeToPNG();
            if (bytes == null || bytes.Length == 0)
            {
                Debug.LogError("Failed to encode image.");
                onGenerationComplete?.Invoke("Error: Failed to encode image.");
                yield break;
            }
            string b64 = Convert.ToBase64String(bytes);
            string mime = encodeAsJpg ? "image/jpeg" : "image/png";
            dataUri = $"data:{mime};base64,{b64}";
        }
        else if (!string.IsNullOrEmpty(base64WithoutPrefix))
        {
            if (string.IsNullOrEmpty(base64Mime))
            {
                Debug.LogError("base64Mime must be set when using base64WithoutPrefix.");
                onGenerationComplete?.Invoke("Error: base64Mime must be set when using base64WithoutPrefix.");
                yield break;
            }
            dataUri = $"data:{base64Mime};base64,{base64WithoutPrefix}";
        }
        else
        {
            Debug.LogError("Provide either image OR base64WithoutPrefix.");
            onGenerationComplete?.Invoke("Error: Provide either image OR base64WithoutPrefix.");
            yield break;
        }

        // Optional: warn if data URI is likely over the 5 MB encoded cap for images
        var dataUriBytes = Encoding.UTF8.GetByteCount(dataUri);
        const int fiveMB = 5 * 1024 * 1024;
        if (dataUriBytes > fiveMB)
        {
            Debug.LogWarning($"Data URI is {dataUriBytes / (1024f*1024f):0.00} MB (> 5 MB limit). Consider using JPG or smaller image.");
        }
        
        var payload = new ImageToVideoPayload {
            model = model,
            // promptText = promptText,          // no manual escaping needed
            promptText = userInput,
            promptImage = dataUri,            // from your Texture2D/base64 path
            ratio = ratio,
            duration = durationSeconds
        };
        string json = JsonUtility.ToJson(payload);
        
        using (var req = new UnityWebRequest($"{apiBase}/v1/image_to_video", "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.SetRequestHeader("Authorization", $"Bearer {APIController_Memory.Instance.RunwayAPIKey}");
            req.SetRequestHeader("X-Runway-Version", runwayApiVersion);

            yield return req.SendWebRequest();
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Create failed: {req.responseCode} {req.error}\n{req.downloadHandler.text}");
                onGenerationComplete?.Invoke($"Error: Create failed: {req.responseCode}:  {req.error}\n}}.");
                yield break;
            }

            var create = JsonUtility.FromJson<CreateTaskResponse>(req.downloadHandler.text);
            if (create == null || string.IsNullOrEmpty(create.id))
            {
                Debug.LogError($"Unexpected create response: {req.downloadHandler.text}");
                onGenerationComplete?.Invoke("Error: Unexpected create response.");
                yield break;
            }

            Debug.Log($"Runway task created: {create.id}");
            yield return StartCoroutine(Co_PollTaskAndDownload(create.id, onGenerationComplete));
        }
    }

    private IEnumerator Co_PollTaskAndDownload(string taskId,Action<string> onGenerationComplete)
    {
        string statusUrl = $"{apiBase}/v1/tasks/{taskId}";
        float start = Time.realtimeSinceStartup;
        const float timeout = 300f;

        while (true)
        {
            using (var req = UnityWebRequest.Get(statusUrl))
            {
                req.downloadHandler = new DownloadHandlerBuffer();
                req.SetRequestHeader("Authorization", $"Bearer {APIController_Memory.Instance.RunwayAPIKey}");
                req.SetRequestHeader("X-Runway-Version", runwayApiVersion);

                yield return req.SendWebRequest();
                if (req.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Status error: {req.responseCode} {req.error}\n{req.downloadHandler.text}");
                    onGenerationComplete?.Invoke("Error: State error {req.responseCode}");
                    yield break;
                }

                var status = JsonUtility.FromJson<TaskStatusResponse>(req.downloadHandler.text);
                if (status == null)
                {
                    Debug.LogError($"Unexpected status response: {req.downloadHandler.text}");
                    onGenerationComplete?.Invoke("Error: State == null");
                    yield break;
                }

                if (string.Equals(status.status, "SUCCEEDED", StringComparison.OrdinalIgnoreCase))
                {
                    if (status.output != null && status.output.Length > 0)
                    {
                        yield return StartCoroutine(Co_DownloadFile(status.output[0], onGenerationComplete));
                        yield break;
                    }
                    Debug.LogError("SUCCEEDED but no output URLs.");
                    onGenerationComplete?.Invoke("Error: no url");
                    yield break;
                }

                if (string.Equals(status.status, "FAILED", StringComparison.OrdinalIgnoreCase))
                {
                    Debug.LogError($"Task FAILED: {(status.error != null ? status.error.message : "Unknown error")}");
                    onGenerationComplete?.Invoke("Error: failed.");
                    yield break;
                }

                if (Time.realtimeSinceStartup - start > timeout)
                {
                    Debug.LogError("Task timed out.");
                    onGenerationComplete?.Invoke("Error: timeout");
                    yield break;
                }

                yield return new WaitForSeconds(2f);
            }
        }
    }

    private IEnumerator Co_DownloadFile(string url, Action<string> onGenerationComplete)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        Debug.Log($"video download path: {path}");
        using (var req = UnityWebRequest.Get(url))
        {
            req.downloadHandler = new DownloadHandlerFile(path);
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Download failed: {req.responseCode} {req.error}");
                yield break;
            }
        }
        onGenerationComplete?.Invoke(path);
        Debug.Log($"Saved video: {path} (Runway output URLs are ephemeral; keep this file.)");
    }

    private static string EscapeJson(string s) => s?.Replace("\\", "\\\\").Replace("\"", "\\\"");
}
