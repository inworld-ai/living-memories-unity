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
using Inworld.Framework.Graph;
using Inworld.Framework.Samples.UI;
using Inworld.Framework.TTS;
using TMPro;
using UnityEngine;

namespace Inworld.Framework.Samples.Node
{
    /// <summary>
    /// Sample implementation demonstrating Text-to-Speech (TTS) functionality within the Inworld framework.
    /// Provides a complete interface for text input, voice selection, and audio output generation.
    /// Supports dynamic voice switching, preview functionality, and real-time speech synthesis.
    /// Serves as a reference implementation for building TTS-enabled applications.
    /// </summary>
    public class TTSNodeTemplate : NodeTemplate
    {
        [SerializeField] TTSNodeAsset m_TTSNode; 
        [SerializeField] ModelProviders m_ModelProviders;
        [SerializeField] TMP_InputField m_InputField;
        [SerializeField] TMP_Dropdown m_VoicesDropdown;
        [SerializeField] bool m_IsStreaming;
        [SerializeField] AudioSource m_AudioSource;
        
        List<InworldUIElement> m_UIElements = new List<InworldUIElement>();
        const string k_Others = "Others";
        string m_CurrentVoiceID;

        void Awake()
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
            m_CurrentVoiceID = m_TTSNode.voiceID;
            m_VoicesDropdown.RefreshShownValue();
        }
        
        protected override void OnGraphCompiled(InworldGraphAsset obj)
        {
            foreach (InworldUIElement element in m_UIElements)
                element.Interactable = true;
        }

        bool IsAbleToSend()
        {
            if (string.IsNullOrEmpty(m_CurrentVoiceID))
            {
                Debug.LogError("Voice ID is empty!");
                return false;
            }

            if (!m_TTSNode)
            {
                Debug.LogError("TTS Node is NULL!");
                return false;
            }

            if (!m_InworldGraphExecutor)
            {
                Debug.LogError("Inworld Graph Executor is NULL!");
                return false;
            }
            if (m_TTSNode.voiceID != m_CurrentVoiceID)
            {
                Debug.LogWarning("Setting voice Not completed. Please Wait.");
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// Previews the currently selected voice by synthesizing a sample greeting message.
        /// Demonstrates the voice characteristics and quality before actual use.
        /// </summary>
        public async void Preview()
        {
            if (IsAbleToSend())
                await m_InworldGraphExecutor.ExecuteGraphAsync("STT",
                    new InworldText($"Hello, I'm {m_CurrentVoiceID}"));
        }

        void _ResetGraph()
        {
            m_TTSNode.voiceID = m_CurrentVoiceID;
            m_InworldGraphExecutor.Compile();
        }
        
        /// <summary>
        /// Converts the text input to speech using the currently selected voice.
        /// Processes the input field content and generates corresponding audio output.
        /// Clears the input field after successful processing.
        /// </summary>
        public async void TextToSpeech()
        {
            if (!m_InputField) 
                return;
            if (string.IsNullOrEmpty(m_CurrentVoiceID))
                m_CurrentVoiceID = "young_male_6";
            if (!IsAbleToSend())
                return;
            await m_InworldGraphExecutor.ExecuteGraphAsync("STT", new InworldText(m_InputField.text));
            m_InputField.text = string.Empty;
        }
        
        /// <summary>
        /// Handles voice selection from the dropdown menu.
        /// Updates the current voice ID and recompiles the graph with the new voice settings.
        /// </summary>
        public void SelectVoiceID()
        {
            string providerName = m_VoicesDropdown.options[m_VoicesDropdown.value].text;
            if (string.Equals(providerName, k_Others, StringComparison.CurrentCultureIgnoreCase))
            {
                return;
            }
            string data = m_ModelProviders.voiceIds.FirstOrDefault(voiceID => voiceID == providerName);
            if (data == null)
            {
                Debug.Log($"VoiceID {providerName} is not found");
                return;
            }
            m_CurrentVoiceID = data;
            _ResetGraph();
        }
        
        protected override async void OnGraphResult(InworldBaseData obj)
        {
            InworldDataStream<TTSOutput> outputStream = new InworldDataStream<TTSOutput>(obj);
            InworldInputStream<TTSOutput> stream = outputStream.ToInputStream();
            int sampleRate = 0;
            List<float> result = new List<float>();
            await Awaitable.BackgroundThreadAsync();
            while (stream != null && stream.HasNext)
            {
                TTSOutput ttsOutput = stream.Read();
                if (ttsOutput == null) 
                    continue;
                InworldAudio chunk = ttsOutput.Audio;
                sampleRate = chunk.SampleRate;
                List<float> data = chunk.Waveform?.ToList();
                if (data != null && data.Count > 0)
                    result.AddRange(data);
                await Awaitable.NextFrameAsync();
            }
            await Awaitable.MainThreadAsync();
            string output = $"SampleRate: {sampleRate} Sample Count: {result.Count}";
            Debug.Log(output);
            int sampleCount = result.Count;
            if (sampleRate == 0 || sampleCount == 0)
                return;
            AudioClip audioClip = AudioClip.Create("TTS", sampleCount, 1, sampleRate, false);
            audioClip.SetData(result.ToArray(), 0);
            m_AudioSource?.PlayOneShot(audioClip);
        }
    }
}