using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class HeygenLipsyncPipeline : MonoBehaviour
{
    [Header("HeyGen API")]
    [Tooltip("Your HeyGen API token (Settings > Subscriptions > HeyGen API > API Token)")]
    public string heygenApiKey = "YOUR_HEYGEN_API_KEY";

    // Core endpoints (documented)
    private const string UploadAssetUrl = "https://upload.heygen.com/v1/asset";                   // raw bytes upload (image/audio)
    private const string CreateVideoV2Url = "https://api.heygen.com/v2/video/generate";           // talking_photo (V2) create
    private const string CreateAvatarIVUrl = "https://api.heygen.com/v2/video/av4/generate";      // Avatar IV create
    private const string VideoStatusUrl = "https://api.heygen.com/v1/video_status.get?video_id="; // status + signed URLs

    [Header("Mode")]
    [Tooltip("If true, use Avatar IV (single photo -> video). If false, use Talking Photo (requires talkingPhotoId below).")]
    public bool useAvatarIV = false;

    [Tooltip("Required for Talking Photo mode. Find it via API or your HeyGen dashboard.")]
    public string talkingPhotoId = ""; // e.g., "tp_xxx"

    [Header("Image options")]
    [Tooltip("Encode Texture2D as JPG (smaller) or PNG (lossless) when uploading for Avatar IV.")]
    public bool encodeImageAsJpg = true;

    [Tooltip("Optionally clamp the longest side when uploading image for Avatar IV (keeps data small). Set 0 to disable.")]
    public int maxLongSide = 1280;

    [Header("Output")]
    [Tooltip("Base file name for the saved .mp4")]
    public string outputBaseName = "heygen_lipsync";

    // -------------- Public entry points --------------

    /// <summary>
    /// Full pipeline entry:
    /// - Talking Photo mode: needs talkingPhotoId, uploads audio only, then generate + poll + download.
    /// - Avatar IV mode: uploads image + audio, then generate + poll + download.
    /// audioMime:
    ///   Prefer 'audio/mpeg' (MP3). If using WAV bytes, try 'audio/wav' (if 415, convert to MP3 and retry).
    /// </summary>
    public void GenerateLipsync(Texture2D image, byte[] audioBytes, string audioMime, Action<string> onComplete)
    {
        if (string.IsNullOrEmpty(heygenApiKey)) { onComplete?.Invoke("Error: HeyGen API key missing."); return; }
        if (!useAvatarIV && string.IsNullOrEmpty(talkingPhotoId))
        {
            onComplete?.Invoke("Error: talkingPhotoId is required in Talking Photo mode.");
            return;
        }
        if (audioBytes == null || audioBytes.Length == 0) { onComplete?.Invoke("Error: audio bytes empty."); return; }
        if (useAvatarIV && image == null) { onComplete?.Invoke("Error: image Texture2D is required for Avatar IV."); return; }

        StartCoroutine(Co_Run(image, audioBytes, audioMime, onComplete));
    }

    // -------------- Main flow --------------

    private IEnumerator Co_Run(Texture2D image, byte[] audioBytes, string audioMime, Action<string> done)
    {
        // Upload audio as asset
        string audioAssetId = null;
        yield return StartCoroutine(Co_UploadAssetBytes(audioBytes, audioMime, id => audioAssetId = id));
        if (string.IsNullOrEmpty(audioAssetId)) { done?.Invoke("Error: audio upload failed."); yield break; }

        if (useAvatarIV)
        {
            // Upload the image (as JPG/PNG) to get an image URL
            string imageUrl = null;
            yield return StartCoroutine(Co_UploadImageGetUrl(image, url => imageUrl = url));
            if (string.IsNullOrEmpty(imageUrl)) { done?.Invoke("Error: image upload failed."); yield break; }

            // Create Avatar IV video (image_url + voice audio_asset_id)
            string videoId = null;
            yield return StartCoroutine(Co_CreateAvatarIV(imageUrl, audioAssetId, id => videoId = id));
            if (string.IsNullOrEmpty(videoId)) { done?.Invoke("Error: Avatar IV create failed."); yield break; }

            // Poll + download
            yield return StartCoroutine(Co_PollAndDownload(videoId, path => done?.Invoke(path), err => done?.Invoke(err)));
        }
        else
        {
            // Talking Photo mode: you already have talkingPhotoId
            string videoId = null;
            yield return StartCoroutine(Co_CreateVideoV2_TalkingPhoto(talkingPhotoId, audioAssetId, id => videoId = id));
            if (string.IsNullOrEmpty(videoId)) { done?.Invoke("Error: Talking Photo create failed."); yield break; }

            // Poll + download
            yield return StartCoroutine(Co_PollAndDownload(videoId, path => done?.Invoke(path), err => done?.Invoke(err)));
        }
    }

    // -------------- Uploads --------------
    // Upload audio/image as asset → callback returns assetId or URL
    private IEnumerator Co_UploadAssetBytes(byte[] bytes, string mime, Action<string> onResult)
    {
        if (mime == "audio/wav") mime = "audio/x-wav";
        
        using (var req = new UnityWebRequest(UploadAssetUrl, "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(bytes);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("X-Api-Key", heygenApiKey);
            req.SetRequestHeader("Content-Type", mime);

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Upload asset failed: {req.responseCode} {req.error}\n{req.downloadHandler.text}");
                onResult?.Invoke(null);
                yield break;
            }

            var m = Regex.Match(req.downloadHandler.text, "\"id\"\\s*:\\s*\"([^\"]+)\"");
            onResult?.Invoke(m.Success ? m.Groups[1].Value : null);
        }
    }

    private IEnumerator Co_UploadImageGetUrl(Texture2D image, Action<string> onResult)
    {
        Texture2D img = image;
        if (maxLongSide > 0) img = ResizeIfNeeded(image, maxLongSide);

        byte[] bytes = encodeImageAsJpg ? img.EncodeToJPG(90) : img.EncodeToPNG();
        string mime = encodeImageAsJpg ? "image/jpeg" : "image/png";

        using (var req = new UnityWebRequest(UploadAssetUrl, "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(bytes);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("X-Api-Key", heygenApiKey);
            req.SetRequestHeader("Content-Type", mime);

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Upload image failed: {req.responseCode} {req.error}\n{req.downloadHandler.text}");
                onResult?.Invoke(null);
                yield break;
            }

            var m = Regex.Match(req.downloadHandler.text, "\"url\"\\s*:\\s*\"([^\"]+)\"");
            onResult?.Invoke(m.Success ? m.Groups[1].Value.Replace("\\/", "/") : null);
        }
    }

    // Talking Photo create → callback returns videoId
    private IEnumerator Co_CreateVideoV2_TalkingPhoto(string talkingPhotoId, string audioAssetId, Action<string> onResult)
    {
        var body = new
        {
            video_inputs = new object[] {
                new {
                    character = new { type = "talking_photo", talking_photo_id = talkingPhotoId, scale = 1.0f },
                    voice = new { type = "audio", audio_asset_id = audioAssetId },
                    background = new { type = "color", value = "#000000" }
                }
            },
            dimension = new { width = 1280, height = 720 }
        };

        string json = JsonUtility.ToJson(body);
        using (var req = new UnityWebRequest(CreateVideoV2Url, "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("X-Api-Key", heygenApiKey);
            req.SetRequestHeader("Content-Type", "application/json");
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Create video (V2) failed: {req.responseCode} {req.error}\n{req.downloadHandler.text}");
                onResult?.Invoke(null);
                yield break;
            }

            var m = Regex.Match(req.downloadHandler.text, "\"video_id\"\\s*:\\s*\"([^\"]+)\"");
            onResult?.Invoke(m.Success ? m.Groups[1].Value : null);
        }
    }

    // Avatar IV (single photo → video)
    private IEnumerator Co_CreateAvatarIV(string imageUrl, string audioAssetId, Action<string> onResult)
    {
        var body = new
        {
            image_url = imageUrl,
            voice = new { type = "audio", audio_asset_id = audioAssetId },
            dimension = new { width = 1280, height = 720 }
        };

        string json = JsonUtility.ToJson(body);
        using (var req = new UnityWebRequest(CreateAvatarIVUrl, "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("X-Api-Key", heygenApiKey);
            req.SetRequestHeader("Content-Type", "application/json");
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Create Avatar IV failed: {req.responseCode} {req.error}\n{req.downloadHandler.text}");
                onResult?.Invoke(null);
                yield break;
            }

            var m = Regex.Match(req.downloadHandler.text, "\"video_id\"\\s*:\\s*\"([^\"]+)\"");
            onResult?.Invoke(m.Success ? m.Groups[1].Value : null);
        }
    }

    // -------------- Poll + Download --------------

    private IEnumerator Co_PollAndDownload(string videoId, Action<string> onSuccessPath, Action<string> onError)
    {
        string statusUrl = VideoStatusUrl + UnityWebRequest.EscapeURL(videoId);
        float start = Time.realtimeSinceStartup;
        const float timeout = 300f;

        while (true)
        {
            using (var req = UnityWebRequest.Get(statusUrl))
            {
                req.downloadHandler = new DownloadHandlerBuffer();
                req.SetRequestHeader("X-Api-Key", heygenApiKey);
                yield return req.SendWebRequest();

                if (req.result != UnityWebRequest.Result.Success)
                {
                    onError?.Invoke($"Error: status {req.responseCode} {req.error}");
                    yield break;
                }

                string txt = req.downloadHandler.text;
                var status = Regex.Match(txt, "\"status\"\\s*:\\s*\"([^\"]+)\"").Groups[1].Value;
                if (status == "completed")
                {
                    var m = Regex.Match(txt, "\"video_url\"\\s*:\\s*\"([^\"]+)\"");
                    if (!m.Success) { onError?.Invoke("Error: completed but no video_url"); yield break; }
                    string url = m.Groups[1].Value.Replace("\\/", "/");
                    StartCoroutine(Co_DownloadFile(url, onSuccessPath, onError));
                    yield break;
                }
                if (status == "failed") { onError?.Invoke($"Video failed: {txt}"); yield break; }
                if (Time.realtimeSinceStartup - start > timeout) { onError?.Invoke("Error: task timed out."); yield break; }
            }
            yield return new WaitForSeconds(2f);
        }
    }

    private IEnumerator Co_DownloadFile(string url, Action<string> onSuccessPath, Action<string> onError)
    {
        string path = Path.Combine(Application.persistentDataPath, $"{outputBaseName}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.mp4");
        using (var req = UnityWebRequest.Get(url))
        {
            req.downloadHandler = new DownloadHandlerFile(path);
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke($"Error: download failed {req.responseCode} {req.error}");
                yield break;
            }
        }
        onSuccessPath?.Invoke(path);
    }

    // -------------- Utils --------------

    private Texture2D ResizeIfNeeded(Texture2D src, int maxSide)
    {
        if (maxSide <= 0) return src;
        int w = src.width, h = src.height;
        int longest = Mathf.Max(w, h);
        if (longest <= maxSide) return src;

        float scale = (float)maxSide / longest;
        int nw = Mathf.Max(1, Mathf.RoundToInt(w * scale));
        int nh = Mathf.Max(1, Mathf.RoundToInt(h * scale));

        RenderTexture rt = RenderTexture.GetTemporary(nw, nh, 0, RenderTextureFormat.ARGB32);
        Graphics.Blit(src, rt);
        var prev = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D dst = new Texture2D(nw, nh, TextureFormat.RGBA32, false);
        dst.ReadPixels(new Rect(0, 0, nw, nh), 0, 0);
        dst.Apply();

        RenderTexture.active = prev;
        RenderTexture.ReleaseTemporary(rt);
        return dst;
    }
    
    
    
    
    IEnumerator HG_UploadImage_GetImageKey(Texture2D tex, bool jpg, string apiKey, Action<string> onKey)
    {
        byte[] bytes = jpg ? tex.EncodeToJPG(90) : tex.EncodeToPNG();
        string mime = jpg ? "image/jpeg" : "image/png";
        using (var req = new UnityWebRequest("https://upload.heygen.com/v1/asset", "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(bytes);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("X-Api-Key", apiKey);
            req.SetRequestHeader("Content-Type", mime);
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success) { Debug.LogError(req.downloadHandler.text); onKey?.Invoke(null); yield break; }

            // Prefer image_key; fall back to id if needed
            var m = System.Text.RegularExpressions.Regex.Match(req.downloadHandler.text, "\"image_key\"\\s*:\\s*\"([^\"]+)\"");
            if (!m.Success) m = System.Text.RegularExpressions.Regex.Match(req.downloadHandler.text, "\"id\"\\s*:\\s*\"([^\"]+)\"");
            onKey?.Invoke(m.Success ? m.Groups[1].Value.Replace("\\/", "/") : null);
        }
    }

    [Serializable] class CreateGroupBody { public string name; public string image_key; }

    IEnumerator HG_CreatePhotoAvatarGroup(string apiKey, string imageKey, string name, Action<string> onGroupId)
    {
        var body = new CreateGroupBody { name = string.IsNullOrEmpty(name) ? "runtime_avatar" : name, image_key = imageKey };
        string json = JsonUtility.ToJson(body);

        using (var req = new UnityWebRequest("https://api.heygen.com/v2/photo_avatar/avatar_group/create", "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("X-Api-Key", apiKey);
            req.SetRequestHeader("Content-Type", "application/json");
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success) { Debug.LogError(req.downloadHandler.text); onGroupId?.Invoke(null); yield break; }

            var m = System.Text.RegularExpressions.Regex.Match(req.downloadHandler.text, "\"group_id\"\\s*:\\s*\"([^\"]+)\"");
            if (!m.Success) m = System.Text.RegularExpressions.Regex.Match(req.downloadHandler.text, "\"id\"\\s*:\\s*\"([^\"]+)\"");
            onGroupId?.Invoke(m.Success ? m.Groups[1].Value : null);
        }
    }

    [Serializable] class TrainBody { public string group_id; }

    IEnumerator HG_TrainPhotoAvatarGroup(string apiKey, string groupId, Action<bool> onStarted)
    {
        string url = "https://api.heygen.com/v2/photo_avatar/train";
        string json = JsonUtility.ToJson(new TrainBody { group_id = groupId });
        using (var req = new UnityWebRequest(url, "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("X-Api-Key", apiKey);
            req.SetRequestHeader("Content-Type", "application/json");
            yield return req.SendWebRequest();
            onStarted?.Invoke(req.result == UnityWebRequest.Result.Success);
        }
    }

    IEnumerator HG_PollTrainStatus(string apiKey, string jobIdOrGroupId, Action<string> onStatus)
    {
        // The guide shows a separate training status endpoint that returns status: pending|training|ready
        // If your response included a training job id, use it in the URL; some accounts allow querying by id in the path.
        string url = $"https://api.heygen.com/v2/photo_avatar/train/status/{UnityWebRequest.EscapeURL(jobIdOrGroupId)}";
        while (true)
        {
            using (var req = UnityWebRequest.Get(url))
            {
                req.downloadHandler = new DownloadHandlerBuffer();
                req.SetRequestHeader("X-Api-Key", apiKey);
                yield return req.SendWebRequest();

                if (req.result != UnityWebRequest.Result.Success) { onStatus?.Invoke("error"); yield break; }

                var m = System.Text.RegularExpressions.Regex.Match(req.downloadHandler.text, "\"status\"\\s*:\\s*\"([^\"]+)\"");
                string st = m.Success ? m.Groups[1].Value : "unknown";
                onStatus?.Invoke(st);
                if (st == "ready") yield break;
            }
            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator HG_GetTalkingPhotoIdFromGroup(string apiKey, string groupId, Action<string> onTalkingPhotoId)
    {
        string url = $"https://api.heygen.com/v2/avatar_group/{UnityWebRequest.EscapeURL(groupId)}/avatars";
        using (var req = UnityWebRequest.Get(url))
        {
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("X-Api-Key", apiKey);
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success) { Debug.LogError(req.downloadHandler.text); onTalkingPhotoId?.Invoke(null); yield break; }

            // Pick the first completed look
            var m = System.Text.RegularExpressions.Regex.Match(req.downloadHandler.text, "\"id\"\\s*:\\s*\"([^\"]+)\"[\\s\\S]*?\"status\"\\s*:\\s*\"completed\"");
            onTalkingPhotoId?.Invoke(m.Success ? m.Groups[1].Value : null);
        }
    }

    IEnumerator HG_UploadAudio_GetAssetId(byte[] audioBytes, string mime, string apiKey, Action<string> onId)
    {
        // Normalize WAV MIME if needed
        if (mime == "audio/wav") mime = "audio/x-wav";
        using (var req = new UnityWebRequest("https://upload.heygen.com/v1/asset", "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(audioBytes);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("X-Api-Key", apiKey);
            req.SetRequestHeader("Content-Type", mime);  // prefer audio/mpeg (MP3) or audio/x-wav
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success) { Debug.LogError(req.downloadHandler.text); onId?.Invoke(null); yield break; }
            var m = System.Text.RegularExpressions.Regex.Match(req.downloadHandler.text, "\"id\"\\s*:\\s*\"([^\"]+)\"");
            onId?.Invoke(m.Success ? m.Groups[1].Value : null);
        }
    }

    IEnumerator HG_CreateVideo_TalkingPhoto(string apiKey, string talkingPhotoId, string audioAssetId, Action<string> onVideoId)
    {
        var body = new
        {
            video_inputs = new object[] {
                new {
                    character = new { type = "talking_photo", talking_photo_id = talkingPhotoId, scale = 1.0f },
                    voice = new { type = "audio", audio_asset_id = audioAssetId },
                    background = new { type = "color", value = "#000000" }
                }
            },
            dimension = new { width = 1280, height = 720 }
        };
        string json = JsonUtility.ToJson(body);

        using (var req = new UnityWebRequest("https://api.heygen.com/v2/video/generate", "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(System.Text.UTF8Encoding.UTF8.GetBytes(json));
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("X-Api-Key", apiKey);
            req.SetRequestHeader("Content-Type", "application/json");
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success) { Debug.LogError(req.downloadHandler.text); onVideoId?.Invoke(null); yield break; }
            var m = System.Text.RegularExpressions.Regex.Match(req.downloadHandler.text, "\"video_id\"\\s*:\\s*\"([^\"]+)\"");
            onVideoId?.Invoke(m.Success ? m.Groups[1].Value : null);
        }
    }

    IEnumerator HG_PollAndDownloadVideo(string apiKey, string videoId, Action<string> onSavedPath, Action<string> onErr)
    {
        string statusUrl = $"https://api.heygen.com/v1/video_status.get?video_id={UnityWebRequest.EscapeURL(videoId)}";
        Debug.Log("url: "+ statusUrl);
        float start = Time.realtimeSinceStartup; const float timeout = 300f;

        while (true)
        {
            using (var req = UnityWebRequest.Get(statusUrl))
            {
                req.downloadHandler = new DownloadHandlerBuffer();
                req.SetRequestHeader("X-Api-Key", apiKey);
                yield return req.SendWebRequest();

                if (req.result != UnityWebRequest.Result.Success) { onErr?.Invoke(req.error); yield break; }

                string txt = req.downloadHandler.text;
                var st = System.Text.RegularExpressions.Regex.Match(txt, "\"status\"\\s*:\\s*\"([^\"]+)\"").Groups[1].Value;
                if (st == "completed")
                {
                    var m = System.Text.RegularExpressions.Regex.Match(txt, "\"video_url\"\\s*:\\s*\"([^\"]+)\"");
                    if (!m.Success) { onErr?.Invoke("completed but no video_url"); yield break; }
                    string url = m.Groups[1].Value.Replace("\\/", "/");
                    string path = Path.Combine(Application.persistentDataPath, $"heygen_talkingphoto_{DateTime.UtcNow:yyyyMMdd_HHmmss}.mp4");
                    using (var d = UnityWebRequest.Get(url)) { d.downloadHandler = new DownloadHandlerFile(path); yield return d.SendWebRequest(); }
                    onSavedPath?.Invoke(path);
                    yield break;
                }
                if (st == "failed") { onErr?.Invoke("video failed"); yield break; }
                if (Time.realtimeSinceStartup - start > timeout) { onErr?.Invoke("timeout"); yield break; }
            }
            yield return new WaitForSeconds(2f);
        }
    }

    public void CreateTalkingPhotoFromTexture(Texture2D img, Action<string> onTalkingPhotoId, Action<string> onErr)
    {
        StartCoroutine(HG_UploadImage_GetImageKey(img, jpg:true, heygenApiKey, imageKey =>
        {
            if (string.IsNullOrEmpty(imageKey)) { onErr?.Invoke("Upload image failed"); return; }

            StartCoroutine(HG_CreatePhotoAvatarGroup(heygenApiKey, imageKey, "runtime_avatar", groupId =>
            {
                if (string.IsNullOrEmpty(groupId)) { onErr?.Invoke("Create group failed"); return; }

                // Kick training
                StartCoroutine(HG_TrainPhotoAvatarGroup(heygenApiKey, groupId, started =>
                {
                    if (!started) { onErr?.Invoke("Train start failed"); return; }

                    // Poll until ready (some tenants return a job id; if you receive one, pass it below)
                    StartCoroutine(HG_PollTrainStatus(heygenApiKey, groupId, status =>
                    {
                        if (status == "ready")
                        {
                            // Retrieve a completed look id (talking_photo_id)
                            StartCoroutine(HG_GetTalkingPhotoIdFromGroup(heygenApiKey, groupId, tpId =>
                            {
                                if (string.IsNullOrEmpty(tpId)) onErr?.Invoke("No completed talking photo look found");
                                else onTalkingPhotoId?.Invoke(tpId);
                            }));
                        }
                    }));
                }));
            }));
        }));
    }

    public void CreateVideoFromTalkingPhoto(string talkingPhotoId, byte[] audio, string audioMime, Action<string> onSaved, Action<string> onErr)
    {
        StartCoroutine(HG_UploadAudio_GetAssetId(audio, audioMime, heygenApiKey, audioId =>
        {
            if (string.IsNullOrEmpty(audioId)) { onErr?.Invoke("Audio upload failed"); return; }

            StartCoroutine(HG_CreateVideo_TalkingPhoto(heygenApiKey, talkingPhotoId, audioId, videoId =>
            {
                if (string.IsNullOrEmpty(videoId)) { onErr?.Invoke("Create video failed"); return; }

                StartCoroutine(HG_PollAndDownloadVideo(heygenApiKey, videoId, onSaved, onErr));
            }));
        }));
    }
}
