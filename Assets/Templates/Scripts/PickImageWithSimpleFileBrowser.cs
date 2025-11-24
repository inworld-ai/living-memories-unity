/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Provides image selection functionality using Simple File Browser.
/// Loads and displays the selected image in a RawImage component with proper aspect ratio.
/// </summary>
public class PickImageWithSimpleFileBrowser : MonoBehaviour
{
    #region Serialized Fields

    [Header("UI Configuration")]
    [Tooltip("Target raw image to display the selected picture")]
    public RawImage targetRawImage;

    #endregion

    #region Public Methods

    /// <summary>
    /// Opens file browser dialog to pick an image file.
    /// Supported formats: JPG, JPEG, PNG
    /// </summary>
    public void OnPickButton()
    {
        // Set file filters for image types
        SimpleFileBrowser.FileBrowser.SetFilters(
            true, 
            new SimpleFileBrowser.FileBrowser.Filter("Images", ".jpg", ".jpeg", ".png")
        );

        // Show file browser dialog
        SimpleFileBrowser.FileBrowser.ShowLoadDialog(
            onSuccess: (paths) => StartCoroutine(LoadImage(paths[0])),
            onCancel: () => Debug.Log("Image selection cancelled."),
            pickMode: SimpleFileBrowser.FileBrowser.PickMode.Files,
            allowMultiSelection: false,
            initialPath: null,
            title: "Select Image"
        );
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Loads image from file path and displays it in the target RawImage.
    /// </summary>
    /// <param name="path">File path to the image</param>
    private IEnumerator LoadImage(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Image path is null or empty.");
            yield break;
        }

        if (!File.Exists(path))
        {
            Debug.LogError($"Image file not found: {path}");
            yield break;
        }

        // Read image data
        byte[] imageData = File.ReadAllBytes(path);
        
        // Create texture and load image
        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        if (!texture.LoadImage(imageData, false))
        {
            Debug.LogError($"Failed to decode image: {path}");
            Destroy(texture);
            yield break;
        }

        // Display in target RawImage
        if (targetRawImage != null)
        {
            targetRawImage.texture = texture;

            // Setup aspect ratio fitter
            AspectRatioFitter aspectFitter = targetRawImage.GetComponent<AspectRatioFitter>();
            if (aspectFitter == null)
            {
                aspectFitter = targetRawImage.gameObject.AddComponent<AspectRatioFitter>();
            }

            aspectFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
            aspectFitter.aspectRatio = (float)texture.width / texture.height;

            Debug.Log($"Image loaded successfully: {path} ({texture.width}x{texture.height})");
        }
        else
        {
            Debug.LogWarning("Target RawImage is null. Image loaded but not displayed.");
        }
    }

    #endregion
}