/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using Inworld.Framework.Goal;
using Inworld.Framework.Intents;
using Inworld.Framework.Knowledge;
using UnityEngine;

namespace Inworld.Framework.Samples
{
    /// <summary>
    /// ScriptableObject that defines comprehensive character data for AI conversational agents.
    /// Contains all the essential information needed to create and configure an AI character including
    /// personality traits, knowledge bases, intents, goals, and safety configurations.
    /// This asset can be created through Unity's Create menu for easy character management.
    /// </summary>
    [CreateAssetMenu(fileName = "New Character", menuName = "Inworld/Create Data/Character Data", order = -2500)]
    public class CharacterData : ScriptableObject
    {
        /// <summary>
        /// The display name of the AI character.
        /// Used for identification and presentation in conversations.
        /// </summary>
        public string characterName;
        
        /// <summary>
        /// The role or occupation of the character (e.g., "teacher", "shopkeeper", "friend").
        /// Defines the character's social function and behavioral context.
        /// </summary>
        public string role;
        
        /// <summary>
        /// The pronouns to use when referring to the character (e.g., "he/him", "she/her", "they/them").
        /// Ensures appropriate gender representation in conversations.
        /// </summary>
        public string pronouns;
        
        /// <summary>
        /// Detailed description of the character's appearance, personality, and background.
        /// Provides context for how the character should behave and respond in conversations.
        /// </summary>
        [TextArea(5, 20)] public string description;
        
        /// <summary>
        /// The character's underlying motivations, goals, and driving forces.
        /// Influences the character's conversational style and decision-making patterns.
        /// </summary>
        [TextArea(5, 20)] public string motivation;
        
        /// <summary>
        /// List of knowledge bases that the character has access to.
        /// Defines what information the character can reference and discuss.
        /// </summary>
        public List<KnowledgeData> knowledges;
        
        /// <summary>
        /// List of intents that the character can recognize and respond to.
        /// Defines the types of user requests or conversational patterns the character understands.
        /// </summary>
        public List<IntentData> intents;
        
        /// <summary>
        /// List of goals that the character is trying to achieve in conversations.
        /// Provides direction and purpose for the character's interactions.
        /// </summary>
        public List<GoalData> goals;
        
        /// <summary>
        /// List of predefined rejection messages for unsafe or inappropriate content.
        /// Used by the safety system to provide character-appropriate responses to harmful inputs.
        /// </summary>
        public List<string> safetyRejections;

        // Convert temporarily.
        // TODO(Yan): Use the new version of prompt. The previous field may not be needed in future.
        /// <summary>
        /// Converts this character data into a conversational character format.
        /// Provides compatibility with the conversation system by mapping core character properties.
        /// Note: This is a temporary conversion method and may be deprecated in future versions.
        /// </summary>
        public ConversationalCharacterData ToConversation => new ConversationalCharacterData
        {
            name = characterName,
            role = role,
            pronouns = pronouns,
            description = description,
            motivation = motivation
        };
    }
}