using Inworld.Framework;
using Inworld.Framework.Graph;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlowController_MemoryCompanion : MonoBehaviour
{
    public RunwayImageToVideo runwayImageToVideo;
    
    // public Texture2D inputTexture;
    public Image inputTexture;
    
    public string defaultPrompt = "Stay faithful to the image; do not invent new objects, text, or people. Camera: static tripod, eye-level (or match the original), no pans/zooms. \n\n If a person is present: preserve identity, age, hair, skin tone, and clothing exactly. Allow only subtle micro-movements: soft blink, tiny eye saccades, slight head tilt, calm breathing; if hair is visible, a faint breeze. No makeup or wardrobe changes.\n\nIf no person is in the image: animate only gentle environmental cues: slight light shift, soft bokeh drift, leaves/curtain barely swaying, reflections flicker, shadows breathing, minimal parallax.\n\nLook: shallow depth of field with the main subject crisp. Mood: quiet memory fragment, understated, not dramatic. Grade: warm vintage/sepia (yellowed-newspaper) tint, mild fade, light film grain, soft vignette. No logos or added props.\n";
    // public bool useCustomizedPrompt = false;
    public Toggle useCustomizedPrompt;
    public TMP_InputField descriptionOfMemory;
    // public string voiceId = "William";
    public TMP_InputField voiceId;

    public Button setup;
    public Button generate;
    
    public TTSNodeAsset ttsNodeAsset;
    public InworldGraphExecutor graphExecutor;

    public GameObject loadingPage;
    public GameObject videoGeneratePage;
    public GameObject chattingPage;

    public DownloadAndPlayToRT videoplayer;

    public bool isDebugging = false;
    
    bool videoGenrated = false;
    bool CharacterGenerated = false;
    
    bool isFirstTime = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setup.onClick.AddListener(LoadInworldScene);
        generate.onClick.AddListener(GenerateMemory);
        // InworldController.Instance.OnFrameworkInitialized += OnFrameworkInit;
        graphExecutor.OnGraphCompiled += OnGraphCompiled;
        
        if (useCustomizedPrompt)
            useCustomizedPrompt.onValueChanged.AddListener((res) => {descriptionOfMemory.interactable = res;});

        if (descriptionOfMemory)
        {
            var ph = descriptionOfMemory.placeholder as TMP_Text;
            if (ph != null)
                ph.text = defaultPrompt;
            descriptionOfMemory.interactable = useCustomizedPrompt;
        }
    }

    void GenerateMemory()
    {
        if (!isFirstTime)
            return;
        isFirstTime = false;

        if (inputTexture == null || inputTexture.mainTexture == null)
        {
            Debug.Log($"inputTexture is null, return.");
            return;
        }

        if(!isDebugging)
            runwayImageToVideo.StartGeneration(useCustomizedPrompt.isOn?descriptionOfMemory.textComponent.text : defaultPrompt, (Texture2D)inputTexture.mainTexture, OnVideoGenerated);
        else
            OnVideoGenerated("C:/Users/Shuang/AppData/LocalLow/DefaultCompany/TestFramework/runway_gen_video.mp4");
    }

    void LoadInworldScene()
    {
        if(setup != null && setup.GetComponentInChildren<TMP_Text>()!= null)
            setup.GetComponentInChildren<TMP_Text>().text = "Connecting to memory channel...";
        if (InworldFrameworkUtil.APIKey != null && !string.IsNullOrEmpty(APIController_Memory.Instance.InworldAPIKey))
        {
            InworldFrameworkUtil.APIKey = APIController_Memory.Instance.InworldAPIKey;
        }
        
        if (ttsNodeAsset != null && voiceId != null && !string.IsNullOrEmpty(voiceId.text))
        {
            ttsNodeAsset.voiceID = voiceId.text;
        } 
        
        InworldController.Instance.InitializeAsync();
    }

    void OnGraphCompiled(InworldGraphAsset graphAsset)
    {
        if (InworldController.Instance != null && InworldController.TTS != null &&
            !string.IsNullOrEmpty(InworldController.TTS.Voice.SpeakerID))
        {
            // Load Next page
            loadingPage.SetActive(false);
            if(runwayImageToVideo != null && runwayImageToVideo.useRunwayGenration)
                videoGeneratePage.SetActive(true);
            else
            {
                chattingPage.SetActive(true);
            }

            return;
        }
        
        Debug.LogError($"Init failed! ");
    }
    
    void OnVideoGenerated(string result)
    {
        if (result.Contains("Error: "))
        {
            // Report error
            Debug.Log($"Error: {result}");
            isFirstTime = true;
        }
        else
        {
            // Display video
            videoGeneratePage.SetActive(false);
            chattingPage.SetActive(true);
            videoplayer?.PlayVideo(result);
            Debug.Log("Video generated: " + result);
        }
    }
}
