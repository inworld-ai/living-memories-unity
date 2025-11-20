/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using Inworld.Framework.Assets;

namespace Inworld.Framework.Samples
{
    /// <summary>
    /// Data structure representing a conversational AI character's basic information.
    /// Contains essential character attributes needed for personality-driven conversations
    /// including identity, role, and behavioral context.
    /// </summary>
    [Serializable]
    public class ConversationalCharacterData
    {
        /// <summary>
        /// The character's display name used in conversations.
        /// </summary>
        public string name;
        
        /// <summary>
        /// The character's role or occupation (e.g., "teacher", "shopkeeper", "friend").
        /// </summary>
        public string role;
        
        /// <summary>
        /// The character's pronouns for proper gender representation (e.g., "he/him", "she/her", "they/them").
        /// </summary>
        public string pronouns;
        
        /// <summary>
        /// Detailed description of the character's appearance, personality, and background.
        /// </summary>
        public string description;
        
        /// <summary>
        /// The character's underlying motivations and goals that drive their behavior.
        /// </summary>
        public string motivation;
    }

    /// <summary>
    /// Data structure for managing knowledge information in conversations.
    /// Contains a collection of knowledge records that can be referenced during interactions.
    /// </summary>
    [Serializable]
    public class ConversationalKnowledgeData
    {
        /// <summary>
        /// List of knowledge records available for the conversation context.
        /// </summary>
        public List<string> records;
    }

    /// <summary>
    /// Data structure for storing conversation event history.
    /// Maintains a chronological record of speech events that occurred during the conversation.
    /// </summary>
    [Serializable]
    public class ConversationalEventHistory
    {
        /// <summary>
        /// Chronological list of speech utterances from all participants in the conversation.
        /// </summary>
        public List<Utterance> speechEvents;
    }
    
    /// <summary>
    /// Base data structure for conversation information.
    /// Contains the event history that tracks what has been said during the conversation.
    /// </summary>
    [Serializable]
    public class ConversationData
    {
        // ReSharper disable once InconsistentNaming as it's serialized data name.
        /// <summary>
        /// The complete history of events that have occurred during this conversation.
        /// </summary>
        public ConversationalEventHistory EventHistory;
    }
    
    /// <summary>
    /// Extended conversation data structure specifically for character-based interactions.
    /// Includes character information, player identity, knowledge context, and conversation history.
    /// </summary>
    [Serializable]
    public class CharacterConversationData : ConversationData
    {
        // ReSharper disable once InconsistentNaming as it's serialized data name.
        /// <summary>
        /// The AI character participating in this conversation.
        /// </summary>
        public ConversationalCharacterData Character;
        
        // ReSharper disable once InconsistentNaming as it's serialized data name.
        /// <summary>
        /// The name or identifier of the human player in the conversation.
        /// </summary>
        public string Player;
        
        // ReSharper disable once InconsistentNaming as it's serialized data name.
        /// <summary>
        /// Knowledge data available for reference during the conversation.
        /// </summary>
        public ConversationalKnowledgeData Knowledge;
    }
}