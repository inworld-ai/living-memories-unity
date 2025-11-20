/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using Inworld.Framework.Graph;
using Inworld.Framework.Samples.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Framework.Samples.Node
{
    /// <summary>
    /// Sample implementation demonstrating content safety checking functionality within the Inworld framework.
    /// Provides a chat interface for testing text content against safety filters and moderation systems.
    /// Displays safety analysis results including content safety status and explanations.
    /// Serves as a reference implementation for building content moderation applications.
    /// </summary>
    public class IntentNodeTemplate : NodeTemplate
    {
        [SerializeField] TMP_InputField m_InputField;
        [SerializeField] RectTransform m_ContentAnchor;
        [SerializeField] ChatBubble m_BubbleLeft;
        [SerializeField] ChatBubble m_BubbleRight;
        
        protected readonly List<ChatBubble> m_Bubbles = new List<ChatBubble>();
        List<InworldUIElement> m_UIElements = new List<InworldUIElement>();
        
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
        
        protected override void OnGraphResult(InworldBaseData output)
        {
            MatchedIntents matched = new MatchedIntents(output);
            if (matched.IsValid)
                DisplayMatchedIntents(matched);
            else
            {
                Debug.LogError(output);
            }
        }
        
        /// <summary>
        /// Sends the current text input for safety analysis.
        /// Creates a player chat bubble and submits the text to the safety checking system.
        /// Clears the input field after submission.
        /// </summary>
        public async void SendText()
        {
            InsertBubble(m_BubbleLeft, "Player", m_InputField.text);
            await m_InworldGraphExecutor.ExecuteGraphAsync("Safety", new InworldText(m_InputField.text));
            m_InputField.text = "";
        }

        void DisplayMatchedIntents(MatchedIntents result)
        {
            InworldVector<IntentMatch> data = result.IntentMatches;
            List<IntentMatch> list = data.ToList();
            foreach (IntentMatch match in list)
            {
                InsertBubble(m_BubbleRight, "IntentMatcher", $"{match.IntentName}: {match.Score}");
            }
        }
        
        protected virtual void InsertBubble(ChatBubble bubble, string player, string message, int index = -1)
        {
            if (index == -1 || index >= m_Bubbles.Count)
            {
                ChatBubble outBubble = Instantiate(bubble, m_ContentAnchor);
                outBubble.SetBubble(player, InworldFrameworkUtil.PlayerIcon, message);
                m_Bubbles.Add(outBubble);
            }
            else
            {
                ChatBubble outBubble = m_Bubbles[index];
                outBubble.SetBubble(player, InworldFrameworkUtil.InworldIcon, message);
            }
            UpdateContent();
        }
        
        /// <summary>
        /// Forces a layout rebuild of the chat content area.
        /// Ensures proper display and positioning of chat bubbles after content changes.
        /// </summary>
        public void UpdateContent() => LayoutRebuilder.ForceRebuildLayoutImmediate(m_ContentAnchor);
    }
}