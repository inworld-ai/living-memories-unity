/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using Inworld.Framework.Assets;
using Inworld.Framework.Samples;
using UnityEngine;

namespace Inworld.Framework.Graph
{
    /// <summary>
    /// Specialized graph asset for character interaction workflows within the Inworld framework.
    /// Extends the base graph functionality with character-specific data and conversation prompts.
    /// This asset can be created through Unity's Create menu and used to define character-based conversations.
    /// Used for configuring AI character interactions with specific voice settings and conversation data.
    /// </summary>
    [CreateAssetMenu(fileName = "New CharacterInteractionGraph", menuName = "Inworld/Create Graph/Character Interaction", order = -2999)]
    public class CharacterInteractionGraphAsset : InworldGraphAsset
    {
        /// <summary>
        /// The collection of character data associated with this interaction graph.
        /// Contains the character definitions that will participate in the conversation flow.
        /// </summary>
        [Header("Character Interaction Data:")]
        public List<CharacterData> characters;

        /// <summary>
        /// The voice identifier to use for speech synthesis in character interactions.
        /// Specifies which voice profile should be used for generating character speech.
        /// </summary>
        public string voiceID;
        
        /// <summary>
        /// The conversation prompt configuration for this character interaction.
        /// Defines the conversation context, prompts, and behavioral settings for the interaction.
        /// </summary>
        public ConversationPrompt prompt;

        public override bool NeedClearHistory => prompt.NeedClearHistoryOnStart;

        /// <summary>
        /// Loads character data into the conversation prompt and returns the character name.
        /// Configures the conversation prompt with the first character's data and sets the player name.
        /// </summary>
        /// <returns>The name of the character loaded into the conversation data.</returns>
        public void LoadCharacterData()
        {
            prompt.conversationData.Character = characters[0].ToConversation;
            prompt.conversationData.Player = PlayerName;
        }

        /// <summary>
        /// Set the first voice ID in TTS (if there are multiple TTS nodes)
        /// Use the `voiceID` as the default one in the graph.
        /// </summary>
        public void SetFirstVoiceID()
        {
            if (string.IsNullOrEmpty(voiceID))
                return;
            InworldNodeAsset firstTTSNode = m_Nodes.FirstOrDefault(node => node is TTSNodeAsset);
            if (firstTTSNode != null)
            {
                (firstTTSNode as TTSNodeAsset)!.voiceID = voiceID;
            }
        }

        public override bool LoadGameData()
        {
            SetFirstVoiceID();
            LoadCharacterData();
            return true;
        }

        public override void ClearHistory() => prompt.ClearHistory();

    }
}