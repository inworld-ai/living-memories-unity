/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using Inworld.Framework.Audio;
using Inworld.Framework.Graph;
using Inworld.Framework.Samples.UI;
using Inworld.Framework.TTS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Framework.Samples.Node
{
    /// <summary>
    /// Sample implementation demonstrating Speech-to-Text (STT) functionality within the Inworld framework.
    /// Provides a complete interface for microphone input, audio device selection, and speech recognition.
    /// Supports voice activity detection, real-time calibration, and audio processing.
    /// Serves as a reference implementation for building voice-enabled applications.
    /// </summary>
    public class STTNodeTemplate : NodeTemplate
    {
        [SerializeField] InworldAudioManager m_Audio;
        [SerializeField] PlayerVoiceDetector m_VolumeDetector;
        [SerializeField] TMP_Dropdown m_Dropdown;
        [SerializeField] TMP_Text m_Text;
        [SerializeField] TMP_Text m_STTResult;
        [SerializeField] Image m_Volume;
        [SerializeField] Button m_MicButton;
        [SerializeField] Sprite m_MicOn;
        [SerializeField] Sprite m_MicOff;
        
        IMicrophoneHandler m_AudioCapturer;
        bool m_ModuleInitialized = false;
        List<string> m_Devices = new List<string>();
        List<InworldUIElement> m_UIElements = new List<InworldUIElement>();
        
        /// <summary>
        /// Change the current input device from the selection of drop down field.
        /// </summary>
        /// <param name="nIndex">the index of the audio input devices.</param>
        public void UpdateAudioInput(int nIndex)
        {
            int nDeviceIndex = nIndex - 1;
            if (nDeviceIndex < 0)
            {
                if (m_Text)
                    m_Text.text = "Please Choose Input Device!";
                return;
            }
            m_AudioCapturer?.ChangeInputDevice(m_Devices[nIndex]);
            if (!m_MicButton)
                return;
            m_MicButton.interactable = true;
            m_MicButton.image.sprite = m_MicOff;
        }
        /// <summary>
        /// Mute/Unmute microphone.
        /// </summary>
        public void SwitchMicrophone()
        {
            if (!m_MicButton || !m_MicButton.interactable)
                return;
            if (m_MicButton.image.sprite == m_MicOff)
            {
                m_MicButton.image.sprite = m_MicOn;
                m_AudioCapturer?.StopMicrophone();
            }
            else
            {
                m_MicButton.image.sprite = m_MicOff;
                m_AudioCapturer?.StartMicrophone();
            }
        }

        /// <summary>
        /// Initiates connection to the Inworld framework and disables UI during initialization.
        /// Triggers the asynchronous initialization process for the Inworld controller.
        /// </summary>
        public void Connect()
        {
            foreach (InworldUIElement element in m_UIElements)
                element.Interactable = false;
            InworldController.Instance.InitializeAsync();
        }

        /// <summary>
        /// Starts the audio calibration process for microphone input.
        /// Adjusts microphone sensitivity and prepares the audio system for speech recognition.
        /// </summary>
        public void Calibrate()
        {
            if (m_Audio)
                m_Audio.StartCalibrate();
        }
        
        protected void Awake()
        {
            if (m_Audio)
                m_AudioCapturer = m_Audio.GetModule<IMicrophoneHandler>();
            _InitUI();
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            if (!m_Audio)
                return;
            m_Audio.Event.onStartCalibrating.AddListener(()=>Title("Calibrating"));
            m_Audio.Event.onStopCalibrating.AddListener(Calibrated);
            m_Audio.Event.onPlayerStartSpeaking.AddListener(()=>Title("PlayerSpeaking"));
            m_Audio.Event.onPlayerStopSpeaking.AddListener(()=>
            {
                Title("");
                if (m_STTResult)
                    m_STTResult.text = "";
            });
            m_Audio.Event.onAudioSent.AddListener(SendAudio);
        }
        
        void Calibrated()
        {
            Title("Calibrated");
        }
        void Title(string newText)
        {
            if (m_Text)
                m_Text.text = newText;
        }
        void _InitUI()
        {
            m_UIElements.AddRange(GetComponentsInChildren<InworldUIElement>(true));
            if (m_AudioCapturer != null)
                m_Devices = m_AudioCapturer.ListMicDevices();
            if (!m_Dropdown)
                return;
            if (m_Dropdown.options == null)
                m_Dropdown.options = new List<TMP_Dropdown.OptionData>();
            m_Dropdown.options.Clear();
            foreach (string device in m_Devices)
            {
                m_Dropdown.options.Add(new TMP_Dropdown.OptionData(device));
            }
        }
        void SendAudio(List<float> audioData)
        {
            if (!m_ModuleInitialized)
                return;
            InworldVector<float> wave = new InworldVector<float>();
            wave.AddRange(audioData);
            
            _ = m_InworldGraphExecutor.ExecuteGraphAsync("STT", new InworldAudio(wave, wave.Size));
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (InworldController.Audio)
                InworldController.Audio.Event.onAudioSent.RemoveListener(SendAudio);
        }

        protected override void OnGraphCompiled(InworldGraphAsset obj)
        {
            m_ModuleInitialized = true;
            foreach (InworldUIElement element in m_UIElements)
                element.Interactable = true;
        }
        
        protected override void OnGraphResult(InworldBaseData obj)
        {
            InworldText outputStream = new InworldText(obj);
            if (outputStream.IsValid && m_STTResult)
                m_STTResult.text += outputStream;
        }
    }
}