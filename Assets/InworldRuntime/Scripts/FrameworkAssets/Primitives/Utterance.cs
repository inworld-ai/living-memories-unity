/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using UnityEngine;

namespace Inworld.Framework.Assets
{
    /// <summary>
    /// Represents a single utterance in a conversation, containing both the speaker's name and their spoken text.
    /// This is used to build conversation history and format prompts for AI interactions.
    /// </summary>
    [Serializable]
    public class Utterance
    {
        /// <summary>
        /// The name of the agent or character who spoke this utterance.
        /// </summary>
        public string agentName;
        
        /// <summary>
        /// The text content of what was spoken in this utterance.
        /// </summary>
        [TextArea(5, 20)] public string utterance;

        /// <summary>
        /// Gets the formatted prompt representation of this utterance, including speaker header and content.
        /// </summary>
        /// <value>A formatted string ready for use in AI prompts.</value>
        public string ToPrompt => $"<|start_header_id|>{agentName}<|end_header_id|>\n{utterance}\n";

        /// <summary>
        /// Returns a string representation of this utterance in the format "AgentName: utterance text".
        /// </summary>
        /// <returns>A formatted string showing the speaker and their utterance.</returns>
        public override string ToString()
        {
            return $"{agentName}: {utterance}";
        }
    }
}