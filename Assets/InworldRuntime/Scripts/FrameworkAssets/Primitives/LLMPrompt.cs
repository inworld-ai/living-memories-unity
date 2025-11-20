/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inworld.Framework.Assets
{
    /// <summary>
    /// Represents a Large Language Model (LLM) prompt configuration with conversation history.
    /// This ScriptableObject manages prompt templates and conversation context for AI interactions.
    /// </summary>
    [CreateAssetMenu(fileName = "LLM Prompt", menuName = "Inworld/Create Prompt/LLM Prompt", order = -2400)]
    public class LLMPrompt : ScriptableObject
    {
        /// <summary>
        /// The base prompt template used for LLM interactions.
        /// </summary>
        [TextArea(5, 20)] public string prompt;
        
        //TODO(Yan): Replace with Jinja.
        /// <summary>
        /// The Jinja template version of the prompt (future implementation).
        /// </summary>
        [Space(20)][TextArea(5, 20)] public string jinjaPrompt;
        
        /// <summary>
        /// The conversation history containing previous utterances.
        /// </summary>
        [HideInInspector] public List<Utterance> conversation;
        
        /// <summary>
        /// Maximum number of utterances to keep in conversation history.
        /// </summary>
        public int maxHistoryLength = 100;
        
        const string k_SpeakerInworld = "<|start_header_id|>Inworld.AI<|end_header_id|>";

        /// <summary>
        /// Gets the complete prompt string including base prompt, conversation history, and speaker header.
        /// </summary>
        /// <value>The formatted prompt ready for LLM processing.</value>
        public string ToPrompt
        {
            get
            {
                string result = conversation.Aggregate(prompt, (current, utterance) => current + utterance.ToPrompt);
                result += k_SpeakerInworld;
                return result;
            }
        }

        /// <summary>
        /// Adds a new utterance to the conversation history.
        /// Automatically manages history length by removing oldest entries when exceeding the limit.
        /// </summary>
        /// <param name="utterance">The utterance to add to the conversation history.</param>
        public virtual void AddUtterance(Utterance utterance)
        {
            conversation.Add(utterance);
            if (conversation.Count > maxHistoryLength)
                conversation.RemoveAt(0);
        }

        /// <summary>
        /// Clears all conversation history, resetting the prompt to its base state.
        /// </summary>
        public virtual void ClearHistory()
        {
            conversation.Clear();
        }
    }
}