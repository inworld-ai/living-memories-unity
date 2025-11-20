/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Framework.Assets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Framework.Samples.UI
{
    /// <summary>
    /// Sample UI component representing a chat bubble for displaying conversation messages.
    /// Provides a flexible interface for showing chat content with user icons, titles, and text.
    /// Supports multiple data formats including InworldMessage, Utterance, and custom string data.
    /// Serves as a reference implementation for building conversational UI elements.
    /// </summary>
    public class ChatBubble : MonoBehaviour
    {
        [SerializeField] protected RawImage m_Icon;
        [SerializeField] protected TMP_Text m_Title;
        [SerializeField] protected TMP_Text m_Content;
        
        /// <summary>
        /// Gets or sets the unique identifier for this chat bubble.
        /// </summary>
        public int ID {get; set;}
        
        /// <summary>
        /// Gets or sets the utterance data associated with this chat bubble.
        /// </summary>
        public Utterance Utterance {get; set;}

        /// <summary>
        /// Sets the chat bubble content using an InworldMessage object.
        /// Configures the title, content, and icon based on the message role and provided icon.
        /// </summary>
        /// <param name="message">The InworldMessage containing role and content information.</param>
        /// <param name="icon">Optional custom icon texture; uses default role-based icons if null.</param>
        public virtual void SetBubble(InworldMessage message, Texture2D icon = null)
        {
            if (message == null || !message.IsValid)
                return;

            m_Title.text = message.Role.ToString();
            m_Content.text = message.Content;
            if (!icon)
            {
                m_Icon.texture = message.Role == Role.User
                    ? InworldFrameworkUtil.PlayerIcon
                    : InworldFrameworkUtil.InworldIcon;
            }
            else
                m_Icon.texture = icon;
        }
        
        /// <summary>
        /// Sets the chat bubble content using an Utterance object.
        /// Configures the title, content, and icon based on the utterance data and provided icon.
        /// </summary>
        /// <param name="utterance">The Utterance containing agent name and speech content.</param>
        /// <param name="icon">Optional custom icon texture; uses default agent-based icons if null.</param>
        public virtual void SetBubble(Utterance utterance, Texture2D icon = null)
        {
            if (utterance == null || string.IsNullOrEmpty(utterance.agentName) || string.IsNullOrEmpty(utterance.utterance))
                return;
            Utterance = utterance;
            m_Title.text = utterance.agentName;
            m_Content.text = utterance.utterance;
            if (!icon)
            {
                m_Icon.texture = utterance.agentName == InworldFrameworkUtil.PlayerName
                    ? InworldFrameworkUtil.PlayerIcon
                    : InworldFrameworkUtil.InworldIcon;
            }
            else
                m_Icon.texture = icon;
        }
        
        /// <summary>
        /// Sets the chat bubble content using individual string parameters.
        /// Provides flexible configuration for title, icon, and content text independently.
        /// </summary>
        /// <param name="charName">The character or agent name to display as the title.</param>
        /// <param name="thumbnail">Optional thumbnail image to display as the icon.</param>
        /// <param name="text">Optional text content to display in the bubble.</param>
        public virtual void SetBubble(string charName, Texture2D thumbnail = null, string text = null)
        {
            if (m_Title)
                m_Title.text = charName;
            if (m_Icon && thumbnail)
                m_Icon.texture = thumbnail;
            if (m_Content && !string.IsNullOrEmpty(text))
                m_Content.text = text;
        }
        
        /// <summary>
        /// Appends additional text to the existing bubble content.
        /// Useful for streaming text updates or multi-part messages.
        /// </summary>
        /// <param name="text">The text to append to the current content.</param>
        public void AttachBubble(string text) => m_Content.text = $"{m_Content.text.Trim()} {text.Trim()}";
    }
}