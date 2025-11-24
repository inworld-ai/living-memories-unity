using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

public class DownloadAndPlayToRT : MonoBehaviour
{
    [Header("UI Target")]
    public RawImage rawImage;                 // Assign in Inspector
    public AspectRatioFitter aspectFitter;    // Optional: keep aspect

    // private string localPath;
    private VideoPlayer vp;
    private RenderTexture rt;


    public void PlayVideo(string localPath, bool noAudio = true)
    {
        // 2) Create VideoPlayer and RenderTexture
        if(!gameObject.GetComponent<VideoPlayer>())
            vp = gameObject.AddComponent<VideoPlayer>();
        else
            vp = gameObject.GetComponent<VideoPlayer>();    
    
        vp.source = VideoSource.Url;
        vp.url = localPath;
        if(noAudio)
            vp.audioOutputMode = VideoAudioOutputMode.None;

        vp.renderMode = VideoRenderMode.RenderTexture;
        vp.isLooping = true;

        // Defer RT creation until we know the real size
        vp.prepareCompleted += OnPrepared;
        vp.Prepare();
    }

    private void OnPrepared(VideoPlayer p)
    {
        // Allocate a RenderTexture matching the video’s native size
        int w = (int)Mathf.Max(1, p.width);
        int h = (int)Mathf.Max(1, p.height);

        rt = new RenderTexture(w, h, 0, RenderTextureFormat.ARGB32)
        {
            useMipMap = false,
            autoGenerateMips = false
        };
        rt.Create();

        p.targetTexture = rt;

        // 3) Hook RT to UI RawImage (or to a MeshRenderer’s material.mainTexture)
        if (rawImage != null)
        {
            rawImage.texture = rt;
            if (aspectFitter != null && w > 0 && h > 0)
            {
                aspectFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
                aspectFitter.aspectRatio = (float)w / h;
            }
        }

        // Optional: wait for the first frame before showing (avoid black flash)
        p.sendFrameReadyEvents = true;
        p.frameReady += OnFirstFrameReady;

        p.Play();
    }

    private bool firstFrameShown = false;
    private void OnFirstFrameReady(VideoPlayer source, long frameIdx)
    {
        if (firstFrameShown) return;
        firstFrameShown = true;
        // e.g., enable a container GameObject now that we have a frame
        // container.SetActive(true);
        source.frameReady -= OnFirstFrameReady;
    }

    private void OnDestroy()
    {
        if (vp != null)
        {
            vp.Stop();
            vp.targetTexture = null;
        }
        if (rt != null)
        {
            rt.Release();
            Destroy(rt);
        }
    }
}
