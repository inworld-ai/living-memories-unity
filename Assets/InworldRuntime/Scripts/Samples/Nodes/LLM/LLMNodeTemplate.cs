
/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using Inworld.Framework.Assets;
using Inworld.Framework.Graph;
using Inworld.Framework.LLM;
using Inworld.Framework.Node;
using Inworld.Framework.Samples.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Framework.Samples.Node
{
    /// <summary>
    /// Sample implementation demonstrating Large Language Model (LLM) interaction functionality within the Inworld framework.
    /// Provides a complete chat interface for conversing with AI language models, managing conversation history,
    /// and handling both regular chat responses and streaming data. Supports message management and UI updates.
    /// Serves as a reference implementation for building conversational AI applications.
    /// </summary>
    public class LLMNodeTemplate : NodeTemplate
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
            if (response.IsValid && response.Content != null && response.Content.IsValid)
                LLMSpeaks(response.Content.ToString());
            else
            {
                // YAN: Currently the Streaming Mode is not working as the swig binding issues.
                InworldDataStream<string> stream = new InworldDataStream<string>(obj);
                if (stream.IsValid)
                {
                    InworldInputStream<string> inputStream = stream.ToInputStream();
                    while (inputStream.HasNext)
                    {
                        Debug.Log(inputStream.Read());
                    }

                }
            }
        }

        /// <summary>
        /// Forces a layout rebuild of the chat content area.
        /// Ensures proper display and positioning of chat bubbles after content changes.
        /// </summary>
        public void UpdateContent() => LayoutRebuilder.ForceRebuildLayoutImmediate(m_ContentAnchor);
        
        /// <summary>
        /// Creates a player message and adds it to the conversation.
        /// Generates a user message bubble and updates the message history for LLM context.
        /// </summary>
        /// <param name="content">The text content of the player's message.</param>
        /// <returns>The created InworldMessage representing the player's input.</returns>
        public InworldMessage PlayerSpeaks(string content)
        {
            InworldMessage message = new InworldMessage();
            message.Role = Role.User;
            message.Content = m_InputField.text;
            InsertBubble(m_BubbleRight, message);
            m_Messages.Add(message);
            return message;
        }

        /// <summary>
        /// Creates an AI assistant message and adds it to the conversation.
        /// Generates an AI response bubble and updates the message history for future context.
        /// </summary>
        /// <param name="content">The text content of the AI's response.</param>
        /// <returns>The created InworldMessage representing the AI's response.</returns>
        public InworldMessage LLMSpeaks(string content)
        {
            InworldMessage message = new InworldMessage();
            message.Role = Role.Assistant;
            message.Content = content;
            InsertBubble(m_BubbleRight, message);
            m_Messages.Add(message);
            return message;
        }
        
        /// <summary>
        /// Sends the current text input to the LLM for processing.
        /// Creates a player message, clears the input field, and submits the conversation to the LLM system.
        /// </summary>
        public async void SendText()
        {
            if (!InworldController.LLM)
            {
                Debug.LogError("Cannot find LLM Module!");
                return;
            }
            InworldMessage message = PlayerSpeaks(m_InputField.text);
            m_InputField.text = "";
            LLMChatRequest llmChatRequest = new LLMChatRequest(m_Messages);
            await m_InworldGraphExecutor.ExecuteGraphAsync("LLM", llmChatRequest);
        }

        protected virtual void InsertBubble(ChatBubble bubble, InworldMessage message, int index = -1)
        {
            if (index == -1 || index >= m_Bubbles.Count)
            {
                ChatBubble outBubble = Instantiate(bubble, m_ContentAnchor);
                outBubble.SetBubble(message);
                m_Bubbles.Add(outBubble);
            }
            else
            {
                ChatBubble outBubble = m_Bubbles[index];
                outBubble.SetBubble(message);
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
