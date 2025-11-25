/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Framework;
using Inworld.Framework.Graph;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Main flow controller for the Memory Companion scene.
/// Manages the user flow from setup, image upload, video generation, to AI conversation.
/// </summary>
public class FlowController_MemoryCompanion : MonoBehaviour
{
    #region Serialized Fields

    [Header("Services")]
    [Tooltip("Runway ML video generation service")]
    public RunwayImageToVideo runwayImageToVideo;
    
    [Tooltip("Inworld AI graph executor")]
    public InworldGraphExecutor graphExecutor;
    
    [Tooltip("TTS voice configuration")]
    public TTSNodeAsset ttsNodeAsset;
    
    [Tooltip("Video player component")]
    public DownloadAndPlayToRT videoplayer;

    [Header("UI - Input")]
    [Tooltip("RawImage component displaying the uploaded photo")]
    public RawImage inputTexture;
    
    [Tooltip("Toggle to enable custom prompt input")]
    public Toggle useCustomizedPrompt;
    
    [Tooltip("Input field for custom video generation prompt")]
    public TMP_InputField descriptionOfMemory;
    
    [Tooltip("Input field for AI voice ID")]
    public TMP_InputField voiceId;

    [Header("UI - Buttons")]
    public Button setup;
    public Button generate;

    [Header("UI - Pages")]
    public GameObject loadingPage;
    public GameObject videoGeneratePage;
    public GameObject chattingPage;

    [Header("Configuration")]
    [Tooltip("Default prompt for video generation")]
    [TextArea(5, 15)]
    public string defaultPrompt = "Stay faithful to the image; do not invent new objects, text, or people. Camera: static tripod, eye-level (or match the original), no pans/zooms. \n\n If a person is present: preserve identity, age, hair, skin tone, and clothing exactly. Allow only subtle micro-movements: soft blink, tiny eye saccades, slight head tilt, calm breathing; if hair is visible, a faint breeze. No makeup or wardrobe changes.\n\nIf no person is in the image: animate only gentle environmental cues: slight light shift, soft bokeh drift, leaves/curtain barely swaying, reflections flicker, shadows breathing, minimal parallax.\n\nLook: shallow depth of field with the main subject crisp. Mood: quiet memory fragment, understated, not dramatic. Grade: warm vintage/sepia (yellowed-newspaper) tint, mild fade, light film grain, soft vignette. No logos or added props.\n";

    #endregion

    #region Private Fields
    
    private bool _isFirstTime = true;

    #endregion

    #region Constants

    private const string ButtonTextSetup = "Setup";
    private const string ButtonTextConnecting = "Connecting to memory channel...";

    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Initialize UI listeners and setup default values.
    /// </summary>
    private void Start()
    {
        // Setup button listeners
        if (setup != null) setup.onClick.AddListener(LoadInworldScene);
        if (generate != null) generate.onClick.AddListener(GenerateMemory);
        
        // Setup graph executor listener
        if (graphExecutor != null)
        {
            graphExecutor.OnGraphCompiled += OnGraphCompiled;
        }
        
        // Setup Inworld controller listeners
        if (InworldController.Instance != null)
        {
            InworldController.Instance.OnInitializedFailed += (name) => UpdateSetupButtonText(ButtonTextSetup);
        }
        
        // Setup custom prompt toggle
        if (useCustomizedPrompt != null)
        {
            useCustomizedPrompt.onValueChanged.AddListener(OnCustomPromptToggled);
        }

        // Initialize prompt placeholder
        if (descriptionOfMemory != null)
        {
            TMP_Text placeholder = descriptionOfMemory.placeholder as TMP_Text;
            if (placeholder != null)
            {
                placeholder.text = defaultPrompt;
            }
            descriptionOfMemory.interactable = useCustomizedPrompt != null && useCustomizedPrompt.isOn;
        }
    }

    /// <summary>
    /// Cleanup event listeners to prevent memory leaks.
    /// </summary>
    private void OnDestroy()
    {
        // Remove button listeners
        if (setup != null) setup.onClick.RemoveListener(LoadInworldScene);
        if (generate != null) generate.onClick.RemoveListener(GenerateMemory);
        
        // Remove graph executor listener
        if (graphExecutor != null)
        {
            graphExecutor.OnGraphCompiled -= OnGraphCompiled;
        }
        
        // Remove Inworld controller listeners
        if (InworldController.Instance != null)
        {
            InworldController.Instance.OnInitializedFailed -= (name) => UpdateSetupButtonText(ButtonTextSetup);
        }
        
        // Remove custom prompt toggle listener
        if (useCustomizedPrompt != null)
        {
            useCustomizedPrompt.onValueChanged.RemoveListener(OnCustomPromptToggled);
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Initiates memory video generation from uploaded image.
    /// </summary>
    public void GenerateMemory()
    {
        // Prevent multiple generation attempts
        if (!_isFirstTime)
        {
            Debug.LogWarning("Video generation already in progress.");
            return;
        }
        
        _isFirstTime = false;

        // Validate input texture
        if (inputTexture == null || inputTexture.texture == null)
        {
            Debug.LogError("Input texture is null. Please upload an image first.");
            _isFirstTime = true;
            return;
        }

        // Get the prompt text
        string prompt = (useCustomizedPrompt != null && useCustomizedPrompt.isOn) 
            ? descriptionOfMemory.textComponent.text 
            : defaultPrompt;

        // Start video generation
        if (runwayImageToVideo != null && runwayImageToVideo.useRunwayGeneration)
        {
            runwayImageToVideo.StartGeneration(prompt, (Texture2D)inputTexture.texture, OnVideoGenerated);
        }
        else
        {
            // Skip video generation if not enabled
            OnVideoGenerated(Application.persistentDataPath + "/placeholder_video.mp4");
        }
    }

    /// <summary>
    /// Loads and initializes the Inworld AI scene.
    /// </summary>
    public void LoadInworldScene()
    {
        // Validate API controller
        if (APIController_Memory.Instance == null)
        {
            Debug.LogError("APIController_Memory instance not found!");
            return;
        }

        // Update button text to connecting state
        UpdateSetupButtonText(ButtonTextConnecting);

        // Set API key
        if (!string.IsNullOrEmpty(APIController_Memory.Instance.InworldAPIKey))
        {
            InworldFrameworkUtil.APIKey = APIController_Memory.Instance.InworldAPIKey;
        }
        
        // Set voice ID
        if (ttsNodeAsset != null && voiceId != null && !string.IsNullOrEmpty(voiceId.text))
        {
            ttsNodeAsset.voiceID = voiceId.text;
        }
        
        // Initialize Inworld
        InworldController.Instance.InitializeAsync();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Called when custom prompt toggle value changes.
    /// </summary>
    private void OnCustomPromptToggled(bool isOn)
    {
        if (descriptionOfMemory != null)
        {
            descriptionOfMemory.interactable = isOn;
        }
    }

    /// <summary>
    /// Updates the setup button text.
    /// </summary>
    private void UpdateSetupButtonText(string text)
    {
        if (setup != null)
        {
            TMP_Text buttonText = setup.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = text;
            }
        }
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Called when Inworld graph compilation is complete.
    /// </summary>
    private void OnGraphCompiled(InworldGraphAsset graphAsset)
    {
        // Verify initialization
        if (InworldController.Instance != null && 
            InworldController.TTS != null &&
            !string.IsNullOrEmpty(InworldController.TTS.Voice.SpeakerID))
        {
            // Hide loading page
            if (loadingPage != null)
            {
                loadingPage.SetActive(false);
            }

            // Show appropriate next page
            if (runwayImageToVideo != null && runwayImageToVideo.useRunwayGeneration)
            {
                if (videoGeneratePage != null)
                {
                    videoGeneratePage.SetActive(true);
                }
            }
            else
            {
                if (chattingPage != null)
                {
                    chattingPage.SetActive(true);
                }
            }

            Debug.Log("Inworld AI initialized successfully.");
            return;
        }
        
        // Reset button text 
        UpdateSetupButtonText(ButtonTextSetup);
        Debug.LogError("Inworld AI initialization failed. Please check your API key and voice configuration.");
    }
    
    /// <summary>
    /// Called when video generation is complete or fails.
    /// </summary>
    private void OnVideoGenerated(string result)
    {
        if (result.Contains("Error: "))
        {
            // Handle error
            Debug.LogError($"Video generation failed: {result}");
            _isFirstTime = true;
            return;
        }

        // Video generated successfully
        Debug.Log($"Video generated successfully: {result}");
        
        // Switch to chat page
        if (videoGeneratePage != null)
        {
            videoGeneratePage.SetActive(false);
        }
        
        if (chattingPage != null)
        {
            chattingPage.SetActive(true);
        }

        // Play the video
        if (videoplayer != null)
        {
            videoplayer.PlayVideo(result);
        }
    }

    #endregion
}
