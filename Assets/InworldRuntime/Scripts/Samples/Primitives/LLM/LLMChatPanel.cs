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
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Framework.Samples
{
    /// <summary>
    /// Sample UI panel for direct LLM chat interactions within the Inworld framework.
    /// Provides a clean chat interface for testing LLM responses with real-time streaming,
    /// conversation history management, and prompt-based interactions.
    /// Integrates with LLM modules to demonstrate text generation capabilities.
    /// Serves as a reference implementation for building LLM-powered chat applications.
    /// </summary>
    public class LLMChatPanel : MonoBehaviour
    {
        [SerializeField] LLMPrompt m_Prompt;
        [SerializeField] TMP_InputField m_InputField;
        [SerializeField] RectTransform m_ContentAnchor;
        [SerializeField] ChatBubble m_BubbleLeft;
        [SerializeField] ChatBubble m_BubbleRight;
        [SerializeField] bool m_ClearHistory;

        protected readonly List<ChatBubble> m_Bubbles = new List<ChatBubble>();
        List<InworldUIElement> m_UIElements = new List<InworldUIElement>();
        Utterance m_CurrentUtterance;
        ChatBubble m_CurrentBubble;
        
        void Start()
        {
            m_UIElements.AddRange(GetComponentsInChildren<InworldUIElement>(true));
            if (m_ClearHistory && m_Prompt)
                m_Prompt.ClearHistory();
        }
        
        void OnEnable()
        {
            InworldController.Instance.OnFrameworkInitialized += OnFrameworkInitialized;
            if (!InworldController.LLM) 
                return;
            InworldController.LLM.OnTask += OnLLMProcessing;
            InworldController.LLM.OnTaskFinished += OnLLMRespond;
        }


        void OnDisable()
        {
            if (InworldController.Instance && InworldController.LLM)
            {
                InworldController.LLM.OnTask -= OnLLMProcessing;
                InworldController.LLM.OnTaskFinished -= OnLLMRespond;
            }
                
            if (InworldController.Instance)
                InworldController.Instance.OnFrameworkInitialized -= OnFrameworkInitialized;
        }

        void OnLLMProcessing(string obj)
        {
            if (m_CurrentUtterance == null)
            {
                m_CurrentUtterance = new Utterance
                {
                    agentName = "Inworld",
                    utterance = obj,
                };
                InsertBubble(m_BubbleLeft, m_CurrentUtterance);
            }
            else
            {
                m_CurrentUtterance.utterance = obj;
                InsertBubble(m_BubbleLeft, m_CurrentUtterance, m_Bubbles.Count - 1);
            }
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Return) && !string.IsNullOrEmpty(m_InputField.text))
                SendText();
        }
        void OnFrameworkInitialized()
        {
            foreach (InworldUIElement element in m_UIElements)
                element.Interactable = true;
        }
        
        void OnLLMRespond(string response)
        {
            if (!m_Prompt)
            {
                Debug.LogError("Cannot find prompt field!");
                return;
            }
            m_Prompt.AddUtterance(m_CurrentUtterance);
            m_CurrentUtterance = null;
        }
        
        /// <summary>
        /// Sends the current text input to the LLM for processing.
        /// Creates a player utterance, submits the prompt to the LLM, and clears the input field.
        /// </summary>
        public async void SendText()
        {
            if (!m_Prompt)
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
            await InworldController.LLM.GenerateTextAsync(m_Prompt.ToPrompt);
        }

        /// <summary>
        /// Clears all conversation history from both the UI and the underlying prompt.
        /// Removes all chat bubbles and optionally resets the prompt history based on configuration.
        /// </summary>
        public void ClearHistory()
        {
            while (m_Bubbles.Count > 0)
            {
                ChatBubble bubble = m_Bubbles[0];
                m_Bubbles.RemoveAt(0);
                DestroyImmediate(bubble.gameObject);
            }
            if (m_ClearHistory && m_Prompt)
                m_Prompt.ClearHistory();
        }
        
        /// <summary>
        /// Forces a layout rebuild of the chat content area.
        /// Ensures proper display and positioning of chat bubbles after content changes.
        /// </summary>
        public void UpdateContent() => LayoutRebuilder.ForceRebuildLayoutImmediate(m_ContentAnchor);

        /// <summary>
        /// Processes player speech input and adds it to the conversation.
        /// Creates a player utterance, adds it to the prompt, and displays it in the chat interface.
        /// </summary>
        /// <param name="content">The text content spoken or typed by the player.</param>
        public void PlayerSpeaks(string content)
        {
            Utterance utterance = new Utterance
            {
                agentName = "Player",
                utterance = m_InputField.text
            };
            m_Prompt.AddUtterance(utterance);
            InsertBubble(m_BubbleRight, utterance);
        }

        protected virtual void InsertBubble(ChatBubble bubble, Utterance utterance, int index = -1)
        {
            if (index == -1 || index >= m_Bubbles.Count)
            {
                ChatBubble outBubble = Instantiate(bubble, m_ContentAnchor);
                outBubble.SetBubble(utterance);
                m_Bubbles.Add(outBubble);
            }
            else
            {
                ChatBubble outBubble = m_Bubbles[index];
                outBubble.SetBubble(utterance);
            }
            UpdateContent();
        }
    }
}