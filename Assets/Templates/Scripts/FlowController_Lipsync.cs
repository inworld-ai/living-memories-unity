using System.Collections.Generic;
using Inworld.Framework;
using Inworld.Framework.Audio;
using Inworld.Framework.Graph;
using Inworld.Framework.Samples.Node;
using Inworld.Framework.TTS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class FlowController_Lipsync : NodeTemplate
{
    public GameObject LoadingPage;
    public CanvasGroup ConnectGroup;
    public CanvasGroup GenerationPage;
    public GameObject resultPage;
    public Button setup;
    public Button generate;
    public Button back;
    public InworldGraphExecutor graphExecutor;
    public TTSNodeAsset ttsNodeAsset;
    // public TMP_InputField inworldAPIKey;
    public TMP_InputField voiceId;
    // public Image inputTexture;
    public RawImage inputTexture;
    public TMP_InputField dialogue;
    public DownloadAndPlayToRT videoplayer;
    public AudioSource audioSource;

    public Useapi_Runway_LipSync useapi;


    void Start()
    {
        LoadingPage.SetActive(true);
        ConnectGroup.interactable = true;
        GenerationPage.interactable = false;
        resultPage.SetActive(false);

        setup.onClick.AddListener(LoadInworldScene);
        generate.onClick.AddListener(GenerateTTS);
        back.onClick.AddListener(GoBack);
        graphExecutor.OnGraphCompiled += MoveToGenerationPage;

    }

    void LoadInworldScene()
    {
        if (InworldFrameworkUtil.APIKey != null && !string.IsNullOrEmpty(APIController_Lipsync.Instance.imworldAPIKey.Trim()))
        {
            InworldFrameworkUtil.APIKey = APIController_Lipsync.Instance.imworldAPIKey;
        }

        if (ttsNodeAsset != null && !string.IsNullOrEmpty(voiceId.text.Trim()))
        {
            ttsNodeAsset.voiceID = voiceId.text;
        }

        InworldController.Instance.InitializeAsync();
    }

    async void GenerateTTS()
    {
        if (inputTexture == null || inputTexture.texture == null ||
            dialogue == null || string.IsNullOrEmpty(dialogue.text.Trim()))
        {
            Debug.Log($"inputTexture or dialogue text is null, return.");
            return;
        }

        await graphExecutor.ExecuteGraphAsync("TTS", new InworldText(dialogue.text));
        // dialogue.text = string.Empty;
        GenerationPage.interactable = false;
        if (generate.GetComponentInChildren<TMP_Text>(true))
        {
            generate.GetComponentInChildren<TMP_Text>(true).text = "Loading...";
        }
        // runwayImageToVideo.StartGeneration(useCustomizedPrompt.isOn?descriptionOfMemory.textComponent.text : defaultPrompt, (Texture2D)inputTexture.mainTexture, OnVideoGenerated);
    }

    void GenerateLipsync(AudioClip clip)
    {
        //audioSource.clip = clip;
        byte[] bytes = WavUtility.FromAudioClip(clip);

        // Kick off lipsync generation
        StartCoroutine(useapi.GenerateLipSyncVideo(
            (Texture2D)inputTexture.texture,
            bytes,
            "runway_lipsync_result.mp4",
            // onSucceededLocalPath: path => Debug.Log("Saved to: " + path),
            OnVideoGenerated,
            onFailed: err => Debug.LogError(err)
        ));
    }

    void MoveToGenerationPage(InworldGraphAsset graphAsset)
    {
        if (InworldController.Instance != null && InworldController.TTS != null &&
            !string.IsNullOrEmpty(InworldController.TTS.Voice.SpeakerID))
        {
            // LoadingPage.SetActive(false);
            ConnectGroup.interactable = false;
            GenerationPage.interactable = true;
            
            return;
        }

        Debug.LogError($"Init failed! ");
    }

    protected override async void OnGraphResult(InworldBaseData obj)
    {
        InworldDataStream<TTSOutput> outputStream = new InworldDataStream<TTSOutput>(obj);
        InworldInputStream<TTSOutput> stream = outputStream.ToInputStream();
        int sampleRate = 0;
        List<float> result = new List<float>();
        while (stream != null && stream.HasNext)
        {
            TTSOutput ttsOutput = stream.Read();
            if (ttsOutput == null)
                continue;
            InworldAudio chunk = ttsOutput.Audio;
            sampleRate = chunk.SampleRate;
            List<float> data = chunk.Waveform?.ToList();
            if (data != null && data.Count > 0)
                result.AddRange(data);
            await Awaitable.NextFrameAsync();
        }

        await Awaitable.MainThreadAsync();
        string output = $"SampleRate: {sampleRate} Sample Count: {result.Count}";
        Debug.Log(output);


        int sampleCount = result.Count;
        if (sampleRate == 0 || sampleCount == 0)
            return;
        AudioClip audioClip = AudioClip.Create("TTS", sampleCount, 1, sampleRate, false);
        audioClip.SetData(result.ToArray(), 0);
        
        // audioSource?.PlayOneShot(audioClip);
        GenerateLipsync(audioClip);

    }

    public void OnVideoGenerated(string path)
    {
        Debug.Log($"video generated: {path}");

        // Display video
        dialogue.text = string.Empty;
        GenerationPage.interactable = true;
        if (generate.GetComponentInChildren<TMP_Text>(true))
        {
            generate.GetComponentInChildren<TMP_Text>(true).text = "Generate";
        }
        LoadingPage.SetActive(false);
        resultPage.SetActive(true);
        videoplayer?.PlayVideo(path, false);
        Debug.Log("Video generated: " + path);
    }

    public void GoBack()
    {
        LoadingPage.SetActive(true);
        resultPage.SetActive(false);
    }

}
