/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Inworld.Framework.Intents
{
    /// <summary>
    /// Represents an intent definition with its name and sample phrases within the Inworld framework.
    /// Contains the intent identifier and training examples for natural language understanding.
    /// Used for defining user intents that the AI system should recognize and respond to appropriately.
    /// </summary>
    [System.Serializable]
    public class Intent
    {
        /// <summary>
        /// The unique name identifier for this intent.
        /// Used to reference and categorize the specific intent type.
        /// </summary>
        [JsonProperty("name")]
        public string intentName;
        
        /// <summary>
        /// A collection of sample phrases or utterances that represent this intent.
        /// These examples help train the system to recognize variations of how users might express this intent.
        /// </summary>
        [JsonProperty("phrases")]
        public List<string> intentSample;
    }
    
    /// <summary>
    /// ScriptableObject that holds a collection of intent definitions for AI natural language understanding.
    /// This asset can be created through Unity's Create menu and used to configure intent recognition.
    /// Used for defining and managing the various intents that the AI system should understand and process.
    /// </summary>
    [CreateAssetMenu(fileName = "New Intent", menuName = "Inworld/Create Data/Intents", order = -2498)]
    public class IntentData : ScriptableObject
    {
        /// <summary>
        /// The collection of intent definitions managed by this asset.
        /// Contains all the intents with their associated sample phrases for training and recognition.
        /// </summary>
        public List<Intent> intents = new List<Intent>();

        public InworldVector<InworldIntent> CreateRuntime()
        {
            InworldVector<InworldIntent> result = new InworldVector<InworldIntent>();
            foreach (Intent intent in intents)
            {
                InworldIntent inworldIntent = new InworldIntent();
                inworldIntent.Name = intent.intentName;
                inworldIntent.Phrases = new InworldVector<string>();
                inworldIntent.Phrases.AddRange(intent.intentSample);
                result.Add(inworldIntent);
            }
            return result;
        }
    }
}