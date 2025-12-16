/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using Inworld.Framework.Assets;
using Inworld.Framework.Graph;
using Inworld.Framework.Samples.UI;
using Inworld.Framework.TTS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Framework.Samples.Node
{
    /// <summary>
    /// Sample implementation demonstrating character interaction functionality within the Inworld framework.
    /// Provides a complete conversation interface with text input, chat bubbles, audio playback, and voice interaction.
    /// Handles both text and speech-based communication with AI characters, including TTS audio output.
    /// Serves as a reference implementation for building conversational AI applications.
    /// </summary>
    public class CharacterInteractionNodeTemplate : NodeTemplate
    {        
        [SerializeField] TMP_InputField m_InputField;
        [SerializeField] RectTransform m_ContentAnchor;
        [SerializeField] ChatBubble m_BubbleLeft;
        [SerializeField] ChatBubble m_BubbleRight;
        [SerializeField] bool m_ClearHistory;
        [SerializeField] Texture2D m_CharImage;
        [SerializeField] AudioSource m_AudioSource;
        protected readonly List<ChatBubble> m_Bubbles = new List<ChatBubble>();
        Utterance m_CurrentCharacterUtterance;
        ChatBubble m_CurrentBubble;
        string m_CurrentVoiceID;
        const string k_FemalePronoun = "She/Hers";
        const string k_MalePronoun = "He/Him";

        string m_CharacterName;
        protected override void OnEnable()
        {
            base.OnEnable();
            if (!InworldController.Audio)
                return;
            InworldController.Audio.Event.onStartCalibrating.AddListener(()=>Debug.LogWarning("Start Calibration"));
            InworldController.Audio.Event.onStopCalibrating.AddListener(()=>Debug.LogWarning("Calibrated"));
            InworldController.Audio.Event.onPlayerStartSpeaking.AddListener(()=>Debug.LogWarning("Player Started Speaking"));
            InworldController.Audio.Event.onPlayerStopSpeaking.AddListener(()=>Debug.LogWarning("Player Stopped Speaking"));
            InworldController.Audio.Event.onAudioSent.AddListener(SendAudio);
        }
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                Submit();
            }
        }
        protected override void OnGraphCompiled(InworldGraphAsset obj)
        {
            if (!(obj is CharacterInteractionGraphAsset charGraph))
                return;
            m_CharacterName = charGraph.prompt.conversationData.Character.name;
        }

        protected override async void OnGraphResult(InworldBaseData obj)
        {
            InworldText text = new InworldText(obj);
            if (text.IsValid)
            {
                string speech = text.Text;
                string[] speechData = speech.Split(':', 2);
                if (speechData.Length <= 1) 
                    return;
                if (speechData[0] == InworldFrameworkUtil.PlayerName) 
                    PlayerSpeaks(speechData[1]);
                else 
                    LLMSpeaks(speechData[1]);
                return;
            }

            InworldDataStream<TTSOutput> outputStream = new InworldDataStream<TTSOutput>(obj);
            if (!outputStream.IsValid) return;

            InworldInputStream<TTSOutput> stream = outputStream.ToInputStream();

            int sampleRate = 0;
            float[] finalData = null;
            List<float> buffer = new List<float>(64 * 1024);
            await Awaitable.BackgroundThreadAsync();
            while (stream != null && stream.HasNext)
            {
                TTSOutput ttsOutput = stream.Read();
                if (ttsOutput == null) continue;
                InworldAudio ttsOutputAudio = ttsOutput.Audio;
                sampleRate = ttsOutputAudio.SampleRate;
                List<float> wf = ttsOutputAudio.Waveform?.ToList();
                if (wf != null && wf.Count > 0)
                    buffer.AddRange(wf);
            }
            await Awaitable.MainThreadAsync();
            finalData = buffer.Count > 0 ? buffer.ToArray() : null;
            if (sampleRate <= 0 || finalData == null || finalData.Length == 0) 
                return;
            AudioClip clip = AudioClip.Create("TTS", finalData.Length, 1, sampleRate, false);
            clip.SetData(finalData, 0);
            m_AudioSource?.PlayOneShot(clip);
        }
        
        /// <summary>
        /// Handles AI character speech by creating and displaying a speech bubble.
        /// Processes the character's response and adds it to the conversation interface.
        /// </summary>
        /// <param name="content">The text content spoken by the AI character.</param>
        public void LLMSpeaks(string content)
        {
            Utterance utterance = new Utterance
            {
                agentName = m_CharacterName,
                utterance = content
            };
            if (m_BubbleLeft)
                InsertBubble(m_BubbleLeft, utterance);
        }
        
        /// <summary>
        /// Inserts a chat bubble into the conversation interface.
        /// Creates or updates a chat bubble with the provided utterance and appropriate avatar.
        /// </summary>
        /// <param name="bubble">The bubble template to use for display.</param>
        /// <param name="utterance">The utterance data to display in the bubble.</param>
        /// <param name="index">Optional index for bubble placement. If -1, appends to end.</param>
        protected virtual void InsertBubble(ChatBubble bubble, Utterance utterance, int index = -1)
        {
            if (index == -1 || index >= m_Bubbles.Count)
            {
                ChatBubble outBubble = Instantiate(bubble, m_ContentAnchor);
                outBubble.SetBubble(utterance, GetAgentIcon(utterance));
                m_Bubbles.Add(outBubble);
            }
            else
            {
                ChatBubble outBubble = m_Bubbles[index];
                outBubble.SetBubble(utterance, GetAgentIcon(utterance));
            }
            UpdateContent();
        }
        Texture2D GetAgentIcon(Utterance utterance)
        {
            if (utterance.agentName == InworldFrameworkUtil.PlayerName)
                return InworldFrameworkUtil.PlayerIcon;
            if (utterance.agentName != m_CharacterName)
                return InworldFrameworkUtil.InworldIcon;
            return m_CharImage;
        }
        
        /// <summary>
        /// Forces a layout rebuild of the conversation content area.
        /// Ensures proper display and positioning of chat bubbles after content changes.
        /// </summary>
        public void UpdateContent() => LayoutRebuilder.ForceRebuildLayoutImmediate(m_ContentAnchor);
        
        async void SendAudio(List<float> audioData)
        {
            if (m_InworldGraphExecutor.Graph.IsJsonInitialized || InworldController.STT)
            {
                InworldVector<float> floatArray = new InworldVector<float>();
                foreach (float data in audioData)
                {
                    floatArray.Add(data);
                }

                InworldAudio audio = new InworldAudio(floatArray, 16000);
                await m_InworldGraphExecutor.ExecuteGraphAsync("Audio", audio);
            }
        }
        
        /// <summary>
        /// Submits the current text input to the conversation system.
        /// Sends the input field text to the graph executor and clears the input field.
        /// Triggered by the Return key or can be called programmatically.
        /// </summary>
        public async void Submit()
        {
            string input = m_InputField.text;
            if (m_InputField)
                m_InputField.text = string.Empty;
            await m_InworldGraphExecutor.ExecuteGraphAsync("Text", new InworldText(input));
        }

        /// <summary>
        /// Handles player speech by creating and displaying a player speech bubble.
        /// Processes player input and adds it to the conversation interface.
        /// </summary>
        /// <param name="content">The text content spoken by the player.</param>
        public void PlayerSpeaks(string content)
        {
            Utterance utterance = new Utterance
            {
                agentName = InworldFrameworkUtil.PlayerName,
                utterance = content
            };
            if(m_BubbleRight)
                InsertBubble(m_BubbleRight, utterance);
        }
    }
}