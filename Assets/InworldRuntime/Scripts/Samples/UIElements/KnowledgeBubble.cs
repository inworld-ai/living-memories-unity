/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Framework.Samples.UI
{
    /// <summary>
    /// Sample UI component representing a knowledge bubble for displaying knowledge entries.
    /// Provides an interface for showing knowledge content with title and description text.
    /// Supports knowledge removal functionality through integration with the knowledge module.
    /// Serves as a reference implementation for building knowledge management UI elements.
    /// </summary>
    public class KnowledgeBubble : MonoBehaviour
    {
        [SerializeField] protected TMP_Text m_Title;
        [SerializeField] protected TMP_Text m_Content;

        /// <summary>
        /// Gets the title text of the knowledge bubble.
        /// Returns the current title displayed in the bubble, typically the knowledge ID.
        /// </summary>
        public string Title => m_Title.text;
        
        /// <summary>
        /// Sets the knowledge bubble content with the specified knowledge ID and content.
        /// Configures both the title and content text fields with the provided information.
        /// </summary>
        /// <param name="knowledgeID">The identifier for the knowledge entry to display as title.</param>
        /// <param name="knowledge">Optional knowledge content text to display in the bubble.</param>
        public virtual void SetBubble(string knowledgeID, string knowledge = null)
        {
            if (m_Title)
                m_Title.text = knowledgeID;
            if (m_Content)
                m_Content.text = knowledge;
        }

        /// <summary>
        /// Removes the knowledge entry associated with this bubble from the knowledge module.
        /// Uses the bubble's title text as the knowledge ID for removal operation.
        /// Only executes if the knowledge module is available and the title is not empty.
        /// </summary>
        public void RemoveKnowledge()
        {
            if (InworldController.Knowledge && !string.IsNullOrEmpty(m_Title.text))
                InworldController.Knowledge.RemoveKnowledge(m_Title.text);
        }
    }
}