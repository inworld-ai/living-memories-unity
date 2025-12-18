/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using Inworld.Framework;
using Inworld.Framework.Audio;
using Inworld.Framework.Graph;
using Inworld.Framework.Samples.Node;
using Inworld.Framework.TTS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Main flow controller for the Lip Sync scene.
/// Manages the workflow from setup, image upload, TTS generation, to lip-synced video output.
/// </summary>
public class FlowController_Lipsync : NodeTemplate
{
    #region Serialized Fields

    [Header("Services")]
    [Tooltip("Inworld AI graph executor")]
    public InworldGraphExecutor graphExecutor;
    
    [Tooltip("TTS voice configuration")]
    public TTSNodeAsset ttsNodeAsset;
    
    [Tooltip("UseAPI Runway lip sync service")]
    public Useapi_Runway_LipSync useapi;
    
    [Tooltip("Video player component")]
    public DownloadAndPlayToRT videoplayer;
    
    [Tooltip("Audio source for playback")]
    public AudioSource audioSource;

    [Header("UI - Pages")]
    [Tooltip("Loading page container")]
    public GameObject LoadingPage;
    
    [Tooltip("Connection setup canvas group")]
    public CanvasGroup ConnectGroup;
    
    [Tooltip("Video generation page canvas group")]
    public CanvasGroup GenerationPage;
    
    [Tooltip("Result display page")]
    public GameObject resultPage;

    [Header("UI - Buttons")]
    [Tooltip("Setup button to initialize Inworld")]
    public Button setup;
    
    [Tooltip("Generate button to create lip-sync video")]
    public Button generate;
    
    [Tooltip("Back button to return from result")]
    public Button back;

    [Header("UI - Input")]
    [Tooltip("Voice ID input field")]
    public TMP_InputField voiceId;
    
    [Tooltip("Portrait image for lip-sync")]
    public RawImage inputTexture;
    
    [Tooltip("Dialogue text input")]
    public TMP_InputField dialogue;

    #endregion

    #region Unity Lifecycle


    /// <summary>
    /// Initialize UI and setup event listeners.
    /// </summary>
    private void Start()
    {
        // Initialize page states
        if (LoadingPage != null) LoadingPage.SetActive(true);
        if (ConnectGroup != null)
        {
            ConnectGroup.interactable = true; 
            if(setup != null && setup.GetComponentInChildren<TMP_Text>() != null)
                setup.GetComponentInChildren<TMP_Text>().text = "Connect";
        }
        if (GenerationPage != null) GenerationPage.interactable = false;
        if (resultPage != null) resultPage.SetActive(false);

        // Setup button listeners
        if (setup != null) setup.onClick.AddListener(LoadInworldScene);
        if (generate != null) generate.onClick.AddListener(GenerateTTS);
        if (back != null) back.onClick.AddListener(GoBack);

        // Setup graph executor listener
        if (graphExecutor != null)
        {
            graphExecutor.OnGraphCompiled += MoveToGenerationPage;
        }
    }

    /// <summary>
    /// Cleanup event listeners to prevent memory leaks.
    /// </summary>
    private void OnDestroy()
    {
        // Remove button listeners
        if (setup != null) setup.onClick.RemoveListener(LoadInworldScene);
        if (generate != null) generate.onClick.RemoveListener(GenerateTTS);
        if (back != null) back.onClick.RemoveListener(GoBack);

        // Remove graph executor listener
        if (graphExecutor != null)
        {
            graphExecutor.OnGraphCompiled -= MoveToGenerationPage;
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Loads and initializes the Inworld AI scene.
    /// </summary>
    public void LoadInworldScene()
    {
        // Validate API controller
        if (APIController_Lipsync.Instance == null)
        {
            Debug.LogError("APIController_Lipsync instance not found!");
            return;
        }

        // Set API key
        if (!string.IsNullOrEmpty(APIController_Lipsync.Instance.InworldAPIKey))
        {
            InworldFrameworkUtil.APIKey = APIController_Lipsync.Instance.InworldAPIKey;
        }

        // Set voice ID
        if (ttsNodeAsset != null && voiceId != null && !string.IsNullOrEmpty(voiceId.text.Trim()))
        {
            ttsNodeAsset.voiceID = voiceId.text;
        }

        // Initialize Inworld
        InworldController.Instance.InitializeAsync();
    }

    /// <summary>
    /// Generates TTS audio from input dialogue text.
    /// </summary>
    public async void GenerateTTS()
    {
        // Validate inputs
        if (inputTexture == null || inputTexture.texture == null)
        {
            Debug.LogError("Input texture is missing. Please upload a portrait image.");
            return;
        }

        if (dialogue == null || string.IsNullOrEmpty(dialogue.text.Trim()))
        {
            Debug.LogError("Dialogue text is empty. Please enter text to speak.");
            return;
        }

        // Execute TTS generation
        await graphExecutor.ExecuteGraphAsync("TTS", new InworldText(dialogue.text));

        // Update UI state
        if (GenerationPage != null)
        {
            GenerationPage.interactable = false;
        }

        // Update button text
        if (generate != null)
        {
            TMP_Text buttonText = generate.GetComponentInChildren<TMP_Text>(true);
            if (buttonText != null)
            {
                buttonText.text = "Loading...";
            }
        }
    }

    /// <summary>
    /// Navigates back to the generation page from result page.
    /// </summary>
    public void GoBack()
    {
        if (LoadingPage != null) LoadingPage.SetActive(true);
        if (resultPage != null) resultPage.SetActive(false);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Generates lip-synced video from audio clip.
    /// </summary>
    private void GenerateLipsync(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("Audio clip is null. Cannot generate lip-sync.");
            return;
        }

        // Convert audio clip to WAV bytes
        byte[] audioBytes = WavUtility.FromAudioClip(clip);

        // Start lip-sync generation
        StartCoroutine(useapi.GenerateLipSyncVideo(
            (Texture2D)inputTexture.texture,
            audioBytes,
            "runway_lipsync_result.mp4",
            onSucceededLocalPath: OnVideoGenerated,
            onFailed: error => Debug.LogError($"Lip-sync generation failed: {error}")
        ));
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Called when Inworld graph compilation is complete.
    /// </summary>
    private void MoveToGenerationPage(InworldGraphAsset graphAsset)
    {
        // Verify initialization
        if (InworldController.Instance != null && 
            InworldController.TTS != null &&
            !string.IsNullOrEmpty(InworldController.TTS.Voice.SpeakerID))
        {
            // Move to generation page
            if (ConnectGroup != null)
            {
                ConnectGroup.interactable = false; 
                if(setup != null && setup.GetComponentInChildren<TMP_Text>() != null)
                    setup.GetComponentInChildren<TMP_Text>().text = "Connected";
            }
            if (GenerationPage != null) GenerationPage.interactable = true;

            Debug.Log("Inworld AI initialized successfully.");
            return;
        }

        Debug.LogError("Inworld AI initialization failed. Please check your API key and voice configuration.");
    }

    /// <summary>
    /// Called when TTS audio generation is complete.
    /// Processes the audio stream and initiates lip-sync generation.
    /// </summary>
    protected override async void OnGraphResult(InworldBaseData obj)
    {
        // Create audio data stream
        InworldDataStream<TTSOutput> outputStream = new InworldDataStream<TTSOutput>(obj);
        InworldInputStream<TTSOutput> stream = outputStream.ToInputStream();
        
        int sampleRate = 0;
        List<float> audioSamples = new List<float>();

        // Read all audio chunks from stream
        while (stream != null && stream.HasNext)
        {
            TTSOutput ttsOutput = stream.Read();
            if (ttsOutput == null)
            {
                continue;
            }

            InworldAudio chunk = ttsOutput.Audio;
            sampleRate = chunk.SampleRate;
            
            List<float> waveformData = chunk.Waveform?.ToList();
            if (waveformData != null && waveformData.Count > 0)
            {
                audioSamples.AddRange(waveformData);
            }

            await Awaitable.NextFrameAsync();
        }

        // Ensure we're on main thread
        await Awaitable.MainThreadAsync();

        Debug.Log($"TTS audio generated - Sample Rate: {sampleRate}, Sample Count: {audioSamples.Count}");

        // Validate audio data
        if (sampleRate == 0 || audioSamples.Count == 0)
        {
            Debug.LogError("Invalid TTS audio data. Cannot generate lip-sync.");
            return;
        }

        // Create audio clip
        AudioClip audioClip = AudioClip.Create("TTS", audioSamples.Count, 1, sampleRate, false);
        audioClip.SetData(audioSamples.ToArray(), 0);

        // Generate lip-sync video
        GenerateLipsync(audioClip);
    }

    /// <summary>
    /// Called when lip-sync video generation is complete.
    /// </summary>
    private void OnVideoGenerated(string path)
    {
        Debug.Log($"Lip-sync video generated successfully: {path}");

        // Clear dialogue input
        if (dialogue != null)
        {
            dialogue.text = string.Empty;
        }

        // Re-enable generation page
        if (GenerationPage != null)
        {
            GenerationPage.interactable = true;
        }

        // Reset button text
        if (generate != null)
        {
            TMP_Text buttonText = generate.GetComponentInChildren<TMP_Text>(true);
            if (buttonText != null)
            {
                buttonText.text = "Generate";
            }
        }

        // Show result page
        if (LoadingPage != null) LoadingPage.SetActive(false);
        if (resultPage != null) resultPage.SetActive(true);

        // Play video
        if (videoplayer != null)
        {
            videoplayer.PlayVideo(path, false);
        }
    }

    #endregion
}
