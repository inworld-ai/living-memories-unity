using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PickImageWithSimpleFileBrowser : MonoBehaviour
{
    public RawImage targetRawImage;

    public void OnPickButton()
    {
        // Filters (jpg/png)
        SimpleFileBrowser.FileBrowser.SetFilters(true, 
            new SimpleFileBrowser.FileBrowser.Filter("Images", ".jpg", ".jpeg", ".png"));

        // Open dialog (blocking UI until selected/cancelled)
        SimpleFileBrowser.FileBrowser.ShowLoadDialog(
            onSuccess: (paths) => StartCoroutine(Load(paths[0])),
            onCancel:  () => Debug.Log("Canceled"),
            pickMode:  SimpleFileBrowser.FileBrowser.PickMode.Files,
            initialPath: null,
            allowMultiSelection: false
        );
    }

    private IEnumerator Load(string path)
    {
        byte[] data = File.ReadAllBytes(path);
        var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        if (!tex.LoadImage(data, false))
        {
            Debug.LogError("Decode failed: " + path);
            yield break;
        }
        if (targetRawImage)
        {
            targetRawImage.texture = tex;

            // Ensure there is an AspectRatioFitter and set it to height-controls-width
            var arf = targetRawImage.GetComponent<AspectRatioFitter>();
            if (!arf) arf = targetRawImage.gameObject.AddComponent<AspectRatioFitter>();

            arf.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
            arf.aspectRatio = (float)tex.width / tex.height; // width / height
            // Keep the current height as-is; ARF will adjust width to match the ratio.
            // (If wrapped in layout groups, this plays nicely.)
        }
    }
}