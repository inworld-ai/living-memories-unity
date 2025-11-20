/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inworld.Framework.Assets
{
    /// <summary>
    /// Represents data for a specific AI model provider, including the provider name and available models.
    /// Contains configuration information for connecting to and using different AI service providers.
    /// Used for organizing and managing the relationship between service providers and their model offerings.
    /// </summary>
    [Serializable]
    public class ModelProviderData
    {
        /// <summary>
        /// The name of the AI service provider (e.g., "OpenAI", "Anthropic", "Google").
        /// Used to identify and categorize the service provider for API connections and configuration.
        /// </summary>
        public string providerName;
        
        /// <summary>
        /// List of model names available from this provider.
        /// Contains the specific model identifiers that can be used with this provider's services.
        /// </summary>
        public List<string> models;
    }
    
    /// <summary>
    /// ScriptableObject that manages AI model provider configurations and voice settings within the Inworld framework.
    /// This asset can be created through Unity's Create menu and used to define available AI models and voices.
    /// Used for configuring and organizing AI service providers, their models, and voice synthesis options.
    /// </summary>
    [CreateAssetMenu(fileName = "ModelProviders", menuName = "Inworld/Create Model/ModelProvider", order = -1000)]
    public class ModelProviders : ScriptableObject
    {
        /// <summary>
        /// List of Large Language Model (LLM) providers and their available models.
        /// Contains configuration data for text generation services and their model offerings.
        /// </summary>
        [Header("LLM:")]
        public List<ModelProviderData> providers;

        /// <summary>
        /// List of available voice identifiers for text-to-speech synthesis.
        /// Contains voice profile names that can be used for speech generation services.
        /// </summary>
        [Header("TTS:")] 
        public List<string> voiceIds;
    }
}