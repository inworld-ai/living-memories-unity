/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Framework.Assets;
using UnityEngine;

namespace Inworld.Framework.Samples
{
    /// <summary>
    /// ScriptableObject that extends LLMPrompt specifically for character conversation scenarios.
    /// Manages conversation prompts and maintains dialogue history for AI character interactions.
    /// This asset can be created through Unity's Create menu and integrates with the character conversation system.
    /// </summary>
    [CreateAssetMenu(fileName = "ConversationPrompt", menuName = "Inworld/Create Prompt/Conversation Prompt", order = -2399)]
    public class ConversationPrompt : LLMPrompt
    {
        [SerializeField] bool m_ClearHistoryOnStart = false;
        
        public bool NeedClearHistoryOnStart => m_ClearHistoryOnStart;
        /// <summary>
        /// The conversation data containing character information and interaction history.
        /// Stores all relevant context needed for maintaining coherent character-based conversations.
        /// </summary>
        public CharacterConversationData conversationData;

        /// <summary>
        /// Adds a new utterance to the conversation history.
        /// Updates the speech events log to maintain context for future AI responses.
        /// This method is called when new dialogue occurs in the conversation.
        /// </summary>
        /// <param name="currentUtterance">The utterance to add to the conversation history.</param>
        public override void AddUtterance(Utterance currentUtterance)
        {
            conversationData.EventHistory.speechEvents.Add(currentUtterance);
        }
        
        public override void ClearHistory()
        {
            conversationData.EventHistory.speechEvents.Clear();
        }
    }
}