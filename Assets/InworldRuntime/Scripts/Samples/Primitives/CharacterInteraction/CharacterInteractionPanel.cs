/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/
using System.Collections.Generic;
using Inworld.Framework.Assets;
using Inworld.Framework.Samples.UI;
using TMPro;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;


namespace Inworld.Framework.Samples
{
    /// <summary>
    /// Sample UI panel for managing character interactions within the Inworld framework.
    /// Provides a complete conversation interface with text and voice input, real-time LLM processing,
    /// speech recognition, text-to-speech synthesis, and conversation history management.
    /// Integrates multiple AI services including LLM, STT, TTS, and audio processing for full conversational AI experience.
    /// Serves as a reference implementation for building comprehensive character interaction applications.
    /// </summary>
    public class CharacterInteractionPanel : MonoBehaviour
    {
        [SerializeField] ConversationPrompt m_ConversationPrompt;
        [SerializeField] TMP_InputField m_InputField;
        [SerializeField] RectTransform m_ContentAnchor;
        [SerializeField] ChatBubble m_BubbleLeft;
        [SerializeField] ChatBubble m_BubbleRight;
        [SerializeField] bool m_ClearHistory;
        [SerializeField] Texture2D m_MaleImage;
        [SerializeField] Texture2D m_FemaleImage;
        protected readonly List<ChatBubble> m_Bubbles = new List<ChatBubble>();
        Utterance m_CurrentCharacterUtterance;
        ChatBubble m_CurrentBubble;
        string m_CurrentVoiceID;
        const string k_FemalePronoun = "She/Hers";
        const string k_MalePronoun = "He/Him";

        ConversationalCharacterData Character
        {
            get => m_ConversationPrompt.conversationData.Character;
            set => m_ConversationPrompt.conversationData.Character = value;
        }
        string PlayerName
        {
            get => m_ConversationPrompt.conversationData.Player;
            set => m_ConversationPrompt.conversationData.Player = value;
        }

        void OnEnable()
        {
            if (m_ConversationPrompt.NeedClearHistoryOnStart)
                m_ConversationPrompt.ClearHistory();
            if (!InworldController.LLM) 
                return;
            InworldController.LLM.OnTask += OnLLMProcessing;
            InworldController.LLM.OnTaskFinished += OnLLMRespond;
            if (!InworldController.STT) 
                return;
            InworldController.STT.OnTaskFinished += OnSTTFinished;
            if (!InworldController.Audio)
                return;
            InworldController.Audio.Event.onStartCalibrating.AddListener(()=>Debug.LogWarning("Start Calibration"));
            InworldController.Audio.Event.onStopCalibrating.AddListener(()=>Debug.LogWarning("Calibrated"));
            InworldController.Audio.Event.onPlayerStartSpeaking.AddListener(()=>Debug.LogWarning("Player Started Speaking"));
            InworldController.Audio.Event.onPlayerStopSpeaking.AddListener(()=>Debug.LogWarning("Player Stopped Speaking"));
            InworldController.Audio.Event.onAudioSent.AddListener(SendAudio);
        }

        void OnDisable()
        {
            if (InworldController.Instance && InworldController.LLM)
            {
                InworldController.LLM.OnTask -= OnLLMProcessing;
                InworldController.LLM.OnTaskFinished -= OnLLMRespond;
            }
            if (InworldController.Instance && InworldController.STT) 
                InworldController.STT.OnTaskFinished -= OnSTTFinished;
        }

        void OnSTTFinished(string sttData)
        {
            PlayerSpeaks(sttData);
            RequestResponse();
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                Submit();
            }
        }
        void OnLLMProcessing(string llmData)
        {
            if (m_CurrentCharacterUtterance == null)
            {
                m_CurrentCharacterUtterance = new Utterance
                {
                    agentName = Character.name,
                    utterance = llmData,
                };
                InsertBubble(m_BubbleLeft, m_CurrentCharacterUtterance);
            }
            else
            {
                m_CurrentCharacterUtterance.utterance = llmData;
                InsertBubble(m_BubbleLeft, m_CurrentCharacterUtterance, m_Bubbles.Count - 1);
            }
        }
        void OnLLMRespond(string response)
        {
            if (!m_ConversationPrompt)
            {
                Debug.LogError("Cannot find prompt field!");
                return;
            }
            if (!string.IsNullOrEmpty(m_CurrentVoiceID))
                InworldController.TTS.TextToSpeechAsync(m_CurrentCharacterUtterance.utterance, m_CurrentVoiceID);
            m_ConversationPrompt.AddUtterance(m_CurrentCharacterUtterance);
            m_CurrentCharacterUtterance = null;
        }
        public void OnCharacterCreated(ConversationalCharacterData characterData, string voiceID)
        {
            PlayerName = InworldFrameworkUtil.PlayerName;
            Character = characterData;
            m_CurrentVoiceID = voiceID;
            string json = JsonConvert.SerializeObject(m_ConversationPrompt.conversationData);
            string data = InworldFrameworkUtil.RenderJinja(m_ConversationPrompt.prompt, json);
            if (!string.IsNullOrEmpty(data))
            {
                Debug.Log("Write data completed!");
                m_ConversationPrompt.jinjaPrompt = data;
            }
        }

        async void SendAudio(List<float> audioData)
        {
            if (InworldController.STT)
            {
                AudioChunk chunk = new AudioChunk();
                InworldVector<float> floatArray = new InworldVector<float>();
                foreach (float data in audioData)
                {
                    floatArray.Add(data);
                }
                chunk.SampleRate = 16000;
                chunk.Data = floatArray;
                await InworldController.STT.RecognizeSpeechAsync(chunk);
            }
        }
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
        
        /// <summary>
        /// Forces a layout rebuild of the conversation content area.
        /// Ensures proper display and positioning of chat bubbles after content changes.
        /// </summary>
        public void UpdateContent() => LayoutRebuilder.ForceRebuildLayoutImmediate(m_ContentAnchor);

        Texture2D GetAgentIcon(Utterance utterance)
        {
            if (utterance.agentName == PlayerName)
                return InworldFrameworkUtil.PlayerIcon;
            if (utterance.agentName != Character.name)
                return InworldFrameworkUtil.InworldIcon;
            
                
            if (Character.pronouns == k_FemalePronoun)
                return m_FemaleImage;
            

            if (Character.pronouns == k_MalePronoun)
                return m_MaleImage;
            return InworldFrameworkUtil.InworldIcon;
        }
        
        /// <summary>
        /// Submits the current text input for character interaction.
        /// Processes the input field content as player speech and requests an AI response.
        /// Clears the input field after submission and triggers the conversation flow.
        /// </summary>
        public void Submit()
        {
            if (!m_ConversationPrompt)
            {
                Debug.LogError("Cannot find prompt field!");
                return;
            }
            if (!InworldController.LLM)
            {
                Debug.LogError("Cannot find LLM Module!");
                return;
            }
            PlayerSpeaks(m_InputField.text);
            if (m_InputField)
                m_InputField.text = string.Empty;
            RequestResponse();
        }

        /// <summary>
        /// Requests an AI response based on the current conversation context.
        /// Renders the conversation prompt with current data and submits it to the LLM for processing.
        /// Updates the conversation prompt with the latest context before generating response.
        /// </summary>
        public async void RequestResponse()
        {
            string json = JsonConvert.SerializeObject(m_ConversationPrompt.conversationData);
            string data = InworldFrameworkUtil.RenderJinja(m_ConversationPrompt.prompt, json);
            if (!string.IsNullOrEmpty(data))
            {
                Debug.Log("Write data completed!");
                m_ConversationPrompt.jinjaPrompt = data;
            }
            await InworldController.LLM.GenerateTextAsync(m_ConversationPrompt.jinjaPrompt);
        }
        
        /// <summary>
        /// Processes player speech input and adds it to the conversation history.
        /// Creates a player utterance, adds it to the conversation prompt, and displays it in the chat interface.
        /// </summary>
        /// <param name="content">The text content spoken or typed by the player.</param>
        public void PlayerSpeaks(string content)
        {
            Utterance utterance = new Utterance
            {
                agentName = PlayerName,
                utterance = content
            };
            m_ConversationPrompt.AddUtterance(utterance);
            InsertBubble(m_BubbleRight, utterance);
        }
    }
}