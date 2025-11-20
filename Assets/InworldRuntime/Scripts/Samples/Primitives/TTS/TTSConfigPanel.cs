/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Inworld.Framework.Assets;
using Inworld.Framework.Samples.UI;
using TMPro;
using UnityEngine;

namespace Inworld.Framework.Samples
{
    // TODO(Yan): Support variate SampleRates, Support Alpha/Beta
    /// <summary>
    /// Sample UI panel for configuring Text-to-Speech (TTS) settings within the Inworld framework.
    /// Provides an interface for voice selection, TTS testing, and framework connection management.
    /// Supports both predefined voice options and custom voice ID input for flexible TTS configuration.
    /// Integrates with TTS modules to demonstrate speech synthesis capabilities.
    /// Serves as a reference implementation for building TTS configuration interfaces.
    /// </summary>
    public class TTSConfigPanel : MonoBehaviour
    {
        [SerializeField] ModelProviders m_ModelProviders;
        [SerializeField] SwitchButton m_ConnectButton;
        [SerializeField] TMP_InputField m_InputField;
        [SerializeField] TMP_Dropdown m_VoicesDropdown;
        [SerializeField] TMP_InputField m_VoiceInputField;
        
        List<InworldUIElement> m_UIElements = new List<InworldUIElement>();
        const string k_Others = "Others";
        string m_CurrentVoiceID;
        void Start()
        {
            m_UIElements.AddRange(GetComponentsInChildren<InworldUIElement>(true));
            if (!m_VoicesDropdown || !m_ModelProviders)
                return;
            m_VoicesDropdown.ClearOptions();
            foreach (string voiceId in m_ModelProviders.voiceIds)
            {
                m_VoicesDropdown.options.Add(new TMP_Dropdown.OptionData(voiceId));
            }
            m_VoicesDropdown.value = 0;
            m_CurrentVoiceID = m_ModelProviders.voiceIds[0];
            m_VoicesDropdown.RefreshShownValue();
        }
        
        void OnEnable()
        {
            InworldController.Instance.OnFrameworkInitialized += OnFrameworkInitialized;
        }


        void OnDisable()
        {
            if (InworldController.Instance)
                InworldController.Instance.OnFrameworkInitialized -= OnFrameworkInitialized;
        }
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Return) && !string.IsNullOrEmpty(m_InputField.text))
                TextToSpeech();
        }
        void OnFrameworkInitialized()
        {
            if (m_ConnectButton)
                m_ConnectButton.Switch(true);
            foreach (InworldUIElement element in m_UIElements)
                element.Interactable = true;
        }
        
        /// <summary>
        /// Initiates connection to the Inworld framework and disables UI during initialization.
        /// Triggers the asynchronous initialization process for all framework components including TTS services.
        /// </summary>
        public void Connect()
        {
            foreach (InworldUIElement element in m_UIElements)
                element.Interactable = false;
            InworldController.Instance.InitializeAsync();
        }

        /// <summary>
        /// Plays a preview of the currently selected voice using a standard greeting.
        /// Demonstrates the voice characteristics and quality before actual text synthesis.
        /// </summary>
        public void Preview()
        {
            if (string.IsNullOrEmpty(m_CurrentVoiceID))
            {
                Debug.LogError("Voice ID is empty!");
                return;
            }
            InworldController.TTS.TextToSpeechAsync("Hello", m_CurrentVoiceID);
        }
        
        /// <summary>
        /// Converts the text input to speech using the currently selected voice.
        /// Processes the input field content and generates corresponding audio output,
        /// then clears the input field for the next entry.
        /// </summary>
        public void TextToSpeech()
        {
            if (!m_InputField) 
                return;
            if (string.IsNullOrEmpty(m_CurrentVoiceID))
                m_CurrentVoiceID = "young_male_6";
            InworldController.TTS.TextToSpeechAsync(m_InputField.text, m_CurrentVoiceID);
            m_InputField.text = string.Empty;
        }
        
        /// <summary>
        /// Handles voice selection from the dropdown menu.
        /// Updates the current voice ID based on selection or switches to custom input mode for "Others".
        /// </summary>
        public void SelectVoiceID()
        {
            string providerName = m_VoicesDropdown.options[m_VoicesDropdown.value].text;
            if (string.Equals(providerName, k_Others, StringComparison.CurrentCultureIgnoreCase))
            {
                m_VoicesDropdown.gameObject.SetActive(false);
                m_VoiceInputField.gameObject.SetActive(true);
                return;
            }
            string data = m_ModelProviders.voiceIds.FirstOrDefault(voiceID => voiceID == providerName);
            if (data == null)
            {
                Debug.Log($"VoiceID {providerName} is not found");
                return;
            }
            m_CurrentVoiceID = data;
        }
        
        /// <summary>
        /// Sets a custom voice ID from text input.
        /// Allows users to specify voice IDs that may not be in the predefined dropdown list.
        /// </summary>
        /// <param name="inputVoiceID">The custom voice ID to use for TTS synthesis.</param>
        public void SetVoiceID(string inputVoiceID) => m_CurrentVoiceID = inputVoiceID;
    }
}