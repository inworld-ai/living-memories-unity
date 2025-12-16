/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using Inworld.Framework.Graph;
using Inworld.Framework.LLM;
using Inworld.Framework.Samples.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Framework.Samples.Node
{
    /// <summary>
    /// Sample implementation demonstrating node connection patterns within the Inworld framework.
    /// Provides a chat interface that shows how different nodes connect and pass data through graph execution.
    /// Handles LLM interactions with conversation management and UI updates for testing node connections.
    /// Serves as a reference implementation for understanding graph node communication patterns.
    /// </summary>
    public class NodeConnectionTemplate : NodeTemplate
    {
        [SerializeField] TMP_InputField m_InputField;
        [SerializeField] RectTransform m_ContentAnchor;
        [SerializeField] ChatBubble m_BubbleLeft;
        [SerializeField] ChatBubble m_BubbleRight;
        
        protected readonly List<ChatBubble> m_Bubbles = new List<ChatBubble>();
        List<InworldUIElement> m_UIElements = new List<InworldUIElement>();
        InworldVector<InworldMessage> m_Messages = new InworldVector<InworldMessage>();
        
        void Awake()
        {
            m_UIElements.AddRange(GetComponentsInChildren<InworldUIElement>(true));
        }
        
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Return) && !string.IsNullOrEmpty(m_InputField.text))
                SendText();
        }
        
        protected override void OnGraphCompiled(InworldGraphAsset obj)
        {
            foreach (InworldUIElement element in m_UIElements)
                element.Interactable = true;

        }
        
        protected override void OnGraphResult(InworldBaseData obj)
        {
            LLMChatResponse response = new LLMChatResponse(obj);
            Debug.Log(obj);
            if (response.IsValid && response.Content != null && response.Content.IsValid)
            {
                string message = response.Content.ToString();
                InsertBubble(m_BubbleLeft, Role.Assistant.ToString(), message);
            }
        }
        
        /// <summary>
        /// Forces a layout rebuild of the chat content area.
        /// Ensures proper display and positioning of chat bubbles after content changes.
        /// </summary>
        public void UpdateContent() => LayoutRebuilder.ForceRebuildLayoutImmediate(m_ContentAnchor);
        
        /// <summary>
        /// Sends the current text input through the node connection chain for processing.
        /// Creates a user message bubble and submits the text to the graph executor for node processing.
        /// Clears the input field after submission.
        /// </summary>
        public async void SendText()
        {
            if (!InworldController.LLM)
            {
                Debug.LogError("Cannot find LLM Module!");
                return;
            }

            InworldText text = new InworldText(m_InputField.text); 
            InsertBubble(m_BubbleRight, Role.User.ToString(), m_InputField.text);
            m_InputField.text = "";
            await m_InworldGraphExecutor.ExecuteGraphAsync("LLM",text);
        }
        
        protected virtual void InsertBubble(ChatBubble bubble, string speaker, string content, int index = -1)
        {
            if (index == -1 || index >= m_Bubbles.Count)
            {
                ChatBubble outBubble = Instantiate(bubble, m_ContentAnchor);
                outBubble.SetBubble(speaker, InworldFrameworkUtil.PlayerIcon, content);
                m_Bubbles.Add(outBubble);
            }
            else
            {
                ChatBubble outBubble = m_Bubbles[index];
                outBubble.SetBubble(speaker, InworldFrameworkUtil.InworldIcon, content);
            }
            UpdateContent();
        }
        
        /// <summary>
        /// Clears all conversation history and removes all chat bubbles.
        /// Resets both the visual chat interface and the internal message history for a fresh start.
        /// </summary>
        public void ClearHistory()
        {
            while (m_Bubbles.Count > 0)
            {
                ChatBubble bubble = m_Bubbles[0];
                m_Bubbles.RemoveAt(0);
                DestroyImmediate(bubble.gameObject);
            }
            m_Messages.Clear();
        }
    }
}