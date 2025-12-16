/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.Attributes;
using Inworld.Framework.LLM;
using Util = Inworld.Framework.InworldFrameworkUtil;
using UnityEngine;

namespace Inworld.Framework.Primitive
{
    /// <summary>
    /// Module for Large Language Model (LLM) integration in the Inworld framework.
    /// Provides text generation capabilities using both remote and local AI models.
    /// </summary>
    [ModelType("Remote", ExcludeTargets = new[] { "StandaloneWindows", "StandaloneWindows64" })]
    public class InworldLLMModule : InworldFrameworkModule
    {
        [Header("Remote:")] 
        [SerializeField] string m_Provider;
        [SerializeField] string m_ModelName;
        [Header("Local")]
        [SerializeField] string m_ModelPath;
        [Header("Text Config:")]
        [SerializeField] int m_MaxToken = 100;
        [SerializeField] int m_MaxPromptLength = 1000;
        [Range(0,1)][SerializeField] float m_TopP = 0.95f;
        [Range(0,1)][SerializeField] float m_Temperature = 0.5f;
        [Header("Penalties:")]
        [Range(0,1)][SerializeField] float m_Repetition = 1f;
        [Range(0,1)][SerializeField] float m_Frequency = 0;
        [Range(0,1)][SerializeField] float m_Presence = 0;
        
        TextGenerationConfig m_TextGenerationConfig;
        InworldInputStream<InworldContent> m_InputStream;
        
        /// <summary>
        /// Gets or sets the AI model provider (e.g., OpenAI, Anthropic).
        /// Used when connecting to remote LLM services.
        /// </summary>
        /// <value>The provider name as a string.</value>
        public string Provider
        {
            get => m_Provider;
            set => m_Provider = value;
        }
        
        /// <summary>
        /// Gets or sets the specific model name to use for text generation.
        /// This should correspond to models available from the configured provider.
        /// </summary>
        /// <value>The model name as a string.</value>
        public string ModelName
        {
            get => m_ModelName;
            set => m_ModelName = value;
        }

        /// <summary>
        /// Gets or sets the file path to a local LLM model.
        /// If not set, defaults to the framework's default LLM model path.
        /// </summary>
        /// <value>The full file path to the local model.</value>
        public string ModelPath
        {
            get
            {
                if (string.IsNullOrEmpty(m_ModelPath))
                    m_ModelPath = Util.LLMModelPath;
                return m_ModelPath;
            }
            set => m_ModelPath = value;
        }

        /// <summary>
        /// Gets or sets the maximum number of tokens the model can generate in a single response.
        /// Value is automatically clamped between 1 and 2500.
        /// </summary>
        /// <value>The maximum token count (1-2500).</value>
        public int MaxToken
        {
            get => m_MaxToken;
            set => m_MaxToken = Math.Clamp(value, 1, 2500);
        }

        /// <summary>
        /// Gets or sets the maximum length of the input prompt in tokens.
        /// Value is automatically clamped between 1 and 2500.
        /// </summary>
        /// <value>The maximum prompt length (1-2500).</value>
        public int MaxPromptLength
        {
            get => m_MaxPromptLength;
            set => m_MaxPromptLength = Math.Clamp(value, 1, 2500);
        }
        
        /// <summary>
        /// Gets or sets the repetition penalty factor.
        /// Higher values reduce the likelihood of repeating the same content.
        /// Value is automatically clamped between 0 and 1.
        /// </summary>
        /// <value>The repetition penalty (0.0-1.0).</value>
        public float Repetition
        {
            get => m_Repetition;
            set => m_Repetition = Mathf.Clamp(value, 0, 1);
        }
        
        /// <summary>
        /// Gets or sets the temperature for text generation.
        /// Higher values produce more creative/random output, lower values are more focused.
        /// Value is automatically clamped between 0 and 1.
        /// </summary>
        /// <value>The temperature value (0.0-1.0).</value>
        public float Temperature
        {
            get => m_Temperature;
            set => m_Temperature = Mathf.Clamp(value, 0, 1);
        }

        /// <summary>
        /// Gets or sets the top-p (nucleus sampling) parameter.
        /// Controls diversity by only considering tokens with cumulative probability up to this value.
        /// Value is automatically clamped between 0 and 1.
        /// </summary>
        /// <value>The top-p value (0.0-1.0).</value>
        public float TopP
        {
            get => m_TopP;
            set => m_TopP = Mathf.Clamp(value, 0, 1);
        }
        
        /// <summary>
        /// Gets or sets the frequency penalty factor.
        /// Reduces the likelihood of repeating frequently used tokens.
        /// Value is automatically clamped between 0 and 1.
        /// </summary>
        /// <value>The frequency penalty (0.0-1.0).</value>
        public float Frequency
        {
            get => m_Frequency;
            set => m_Frequency = Mathf.Clamp(value, 0, 1);
        }
        
        /// <summary>
        /// Gets or sets the presence penalty factor.
        /// Encourages the model to talk about new topics by penalizing tokens that have already appeared.
        /// Value is automatically clamped between 0 and 1.
        /// </summary>
        /// <value>The presence penalty (0.0-1.0).</value>
        public float Presence
        {
            get => m_Presence;
            set => m_Presence = Mathf.Clamp(value, 0, 1);
        }

        /// <summary>
        /// Generates text synchronously using the configured LLM.
        /// This method blocks until the generation is complete.
        /// </summary>
        /// <param name="text">The input text prompt for generation.</param>
        /// <returns>The generated text response, or empty string if generation fails.</returns>
        public string GenerateText(string text)
        {
            string result = "";
            if (!Initialized || !(m_Interface is LLMInterface llmInterface))
                return result;
            SetupTextGenerationConfig();
            m_InputStream ??= llmInterface.GenerateContent(text, m_TextGenerationConfig);

            while (m_InputStream != null && m_InputStream.HasNext)
            {
                InworldContent content = m_InputStream.Read();
                if (content != null && content.IsValid)
                    result += content.Content;
            }
            Debug.Log(result);
            return result;
        }

        public async Awaitable<string> GenerateTextAsync(string text)
        {
            Debug.Log($"Prompt: {text}");
            string result = "";
            if (!Initialized || !(m_Interface is LLMInterface llmInterface))
                return result;
            SetupTextGenerationConfig();
            if (m_InputStream != null)
            {
                m_InputStream.Dispose();
                m_InputStream = null;
            }
            m_InputStream ??= llmInterface.GenerateContent(text, m_TextGenerationConfig);
            if (m_InputStream == null || !m_InputStream.IsValid)
            {
                Debug.LogError($"[InworldLLMModule] Failed to create Task: GenerateText for {m_ModelType}:{m_ModelName}");
                return result;
            }
            NotifyTaskStart();
            await Awaitable.BackgroundThreadAsync();
            while (m_InputStream != null && m_InputStream.HasNext)
            {
                InworldContent content = m_InputStream.Read();
                if (content != null && content.IsValid)
                    result += content.Content;
            }
            await Awaitable.MainThreadAsync();
            NotifyTask(result);
            NotifyTaskEnd(text);
            Debug.Log(result);
            return result;
        }

        public override InworldFactory CreateFactory() => m_Factory ??= new LLMFactory();

        public override InworldConfig SetupConfig()
        {
            if (ModelType == ModelType.Remote)
            {
                LLMRemoteConfig remoteConfig = new LLMRemoteConfig();
                if (!string.IsNullOrEmpty(m_Provider))
                    remoteConfig.Provider = m_Provider;
                if (!string.IsNullOrEmpty(m_ModelName))
                    remoteConfig.ModelName = m_ModelName;
                if (!string.IsNullOrEmpty(Util.APIKey))
                    remoteConfig.APIKey = Util.APIKey;
                return remoteConfig;
            }
            LLMLocalConfig localConfig = new LLMLocalConfig();
            localConfig.ModelPath = $"{Application.streamingAssetsPath}/{ModelPath}";
            localConfig.Device = Util.GetDevice(ModelType);
            return localConfig;
        }

        public TextGenerationConfig SetupTextGenerationConfig()
        {
            m_TextGenerationConfig ??= new TextGenerationConfig();
            m_TextGenerationConfig.MaxToken = m_MaxToken;
            m_TextGenerationConfig.RepetitionPenalty = m_Repetition;
            if (ModelName != "gpt-5") // YAN: Current GPT-5 hack.
            {
                m_TextGenerationConfig.TopP = m_TopP;
                m_TextGenerationConfig.Temperature = m_Temperature;
            }
            m_TextGenerationConfig.MaxPromptLength = m_MaxPromptLength;
            m_TextGenerationConfig.FrequencyPenalty = m_Frequency;
            m_TextGenerationConfig.PresencePenalty = m_Presence;
            return m_TextGenerationConfig;
        }
    }
}