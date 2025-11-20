/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Globalization;
using Inworld.Framework.Primitive;
using TMPro;
using UnityEngine;

namespace Inworld.Framework.Samples
{
    /// <summary>
    /// Sample UI panel for configuring LLM text generation parameters within the Inworld framework.
    /// Provides an interface for adjusting text generation settings such as token limits, temperature,
    /// top-p sampling, and penalty values. Supports real-time parameter updates and default value restoration.
    /// Serves as a reference implementation for building LLM parameter configuration interfaces.
    /// </summary>
    public class TextConfigPanel : MonoBehaviour
    {
        const int k_DefaultMaxToken = 100;
        const int k_DefaultMaxPrompt = 1000;
        const float k_DefaultTemperature = 0.5f;
        const float k_DefaultTopP = 0.95f;
        const float k_DefaultRepetition = 1f;
        const float k_DefaultFrequency = 0f;
        const float k_DefaultPresence = 0f;
        
        [SerializeField] TMP_InputField m_MaxTokenInputField;
        [SerializeField] TMP_InputField m_MaxPromptInputField;
        [SerializeField] TMP_InputField m_TemperatureInputField;
        [SerializeField] TMP_InputField m_TopPInputField;
        [SerializeField] TMP_InputField m_RepetitionInputField;
        [SerializeField] TMP_InputField m_FrequencyInputField;
        [SerializeField] TMP_InputField m_PresenceInputField;
        
        /// <summary>
        /// Sets the maximum number of tokens the LLM can generate in a response.
        /// Controls the length limit for generated text output.
        /// </summary>
        /// <param name="maxToken">String representation of the maximum token count.</param>
        public void SetMaxToken(string maxToken)
        {
            if (InworldController.LLM && int.TryParse(maxToken, out int outMaxToken))
                InworldController.LLM.MaxToken = outMaxToken;
        }
        
        /// <summary>
        /// Sets the maximum length of the input prompt that can be processed.
        /// Limits how much context can be provided to the LLM for processing.
        /// </summary>
        /// <param name="maxPromptLength">String representation of the maximum prompt length.</param>
        public void SetMaxPromptLength(string maxPromptLength)
        {
            if (InworldController.LLM && int.TryParse(maxPromptLength, out int outMaxPromptLength))
                InworldController.LLM.MaxPromptLength = outMaxPromptLength;
        }
        
        /// <summary>
        /// Sets the temperature parameter for text generation randomness.
        /// Higher values (closer to 1.0) produce more creative/random text, lower values (closer to 0.0) produce more deterministic text.
        /// </summary>
        /// <param name="temperature">String representation of the temperature value (0.0 to 1.0).</param>
        public void SetTemperature(string temperature)
        {
            if (InworldController.LLM && float.TryParse(temperature, out float outTemperature))
                InworldController.LLM.Temperature = outTemperature;
        }
        
        /// <summary>
        /// Sets the top-p (nucleus) sampling parameter for text generation.
        /// Controls the cumulative probability threshold for token selection during generation.
        /// </summary>
        /// <param name="topP">String representation of the top-p value (0.0 to 1.0).</param>
        public void SetTopP(string topP)
        {
            if (InworldController.LLM && float.TryParse(topP, out float outTopP))
                InworldController.LLM.TopP = outTopP;
        }
        
        /// <summary>
        /// Sets the repetition penalty to discourage the model from repeating tokens.
        /// Values greater than 1.0 penalize repetition, values less than 1.0 encourage it.
        /// </summary>
        /// <param name="repetitionPenalty">String representation of the repetition penalty value.</param>
        public void SetRepetitionPenalty(string repetitionPenalty)
        {
            if (InworldController.LLM && float.TryParse(repetitionPenalty, out float outRepetitionPenalty))
                InworldController.LLM.Repetition = outRepetitionPenalty;
        }
        
        /// <summary>
        /// Sets the frequency penalty to reduce the likelihood of frequently used tokens.
        /// Positive values discourage frequent tokens, negative values encourage them.
        /// </summary>
        /// <param name="frequencyPenalty">String representation of the frequency penalty value.</param>
        public void SetFrequencyPenalty(string frequencyPenalty)
        {
            if (InworldController.LLM && float.TryParse(frequencyPenalty, out float outFrequencyPenalty))
                InworldController.LLM.Frequency = outFrequencyPenalty;
        }
        
        /// <summary>
        /// Sets the presence penalty to encourage the use of new topics and concepts.
        /// Positive values encourage new content, negative values encourage staying on topic.
        /// </summary>
        /// <param name="presencePenalty">String representation of the presence penalty value.</param>
        public void SetPresencePenalty(string presencePenalty)
        {
            if (InworldController.LLM && float.TryParse(presencePenalty, out float outPresencePenalty))
                InworldController.LLM.Presence = outPresencePenalty;
        }
        
        /// <summary>
        /// Resets all LLM parameters to their default values.
        /// Restores both the UI input fields and the LLM module settings to predefined defaults.
        /// </summary>
        public void ResetDefault()
        {
            InworldLLMModule llm = InworldController.LLM;
            if (!llm) 
                return;
            
            if (m_MaxTokenInputField)
                m_MaxTokenInputField.text = k_DefaultMaxToken.ToString();
            llm.MaxToken = k_DefaultMaxToken;
            
            if (m_MaxPromptInputField)
                m_MaxPromptInputField.text = k_DefaultMaxPrompt.ToString();
            llm.MaxPromptLength = k_DefaultMaxPrompt;
            
            if (m_TemperatureInputField)
                m_TemperatureInputField.text = k_DefaultTemperature.ToString(CultureInfo.CurrentCulture);
            llm.Temperature = k_DefaultTemperature;
            
            if (m_TopPInputField)
                m_TopPInputField.text = k_DefaultTopP.ToString(CultureInfo.CurrentCulture);
            llm.TopP = k_DefaultTopP;
            
            if (m_FrequencyInputField)
                m_FrequencyInputField.text = k_DefaultFrequency.ToString(CultureInfo.CurrentCulture);
            llm.Frequency = k_DefaultFrequency;
            
            if (m_RepetitionInputField)
                m_RepetitionInputField.text = k_DefaultRepetition.ToString(CultureInfo.CurrentCulture);
            llm.Repetition = k_DefaultRepetition;
            
            if (m_PresenceInputField)
                m_PresenceInputField.text = k_DefaultPresence.ToString(CultureInfo.CurrentCulture);
            llm.Presence = k_DefaultPresence;
        }

        void Start()
        {
            ResetDefault();
        }
    }
}