/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/


using System.Collections.Generic;
using Inworld.Framework.Audio;
using Inworld.Framework.Samples.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Framework.Samples
{
    /// <summary>
    /// Sample UI canvas for Speech-to-Text (STT) functionality within the Inworld framework.
    /// Provides a comprehensive interface for microphone selection, voice activity detection,
    /// real-time audio calibration, and speech recognition testing. Includes volume visualization
    /// and device management capabilities for complete STT workflow demonstration.
    /// Serves as a reference implementation for building STT-enabled applications.
    /// </summary>
    public class STTCanvas : MonoBehaviour
    {
        [SerializeField] InworldAudioManager m_Audio;
        [SerializeField] PlayerVoiceDetector m_VolumeDetector;
        [SerializeField] TMP_Dropdown m_Dropdown;
        [SerializeField] TMP_Text m_Text;
        [SerializeField] TMP_Text m_STTResult;
        [SerializeField] Image m_Volume;
        [SerializeField] Button m_MicButton;
        [SerializeField] SwitchButton m_ConnectButton;
        [SerializeField] Sprite m_MicOn;
        [SerializeField] Sprite m_MicOff;
        
        IMicrophoneHandler m_AudioCapturer;
        bool m_IsFrameworkInitialized;
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
        /// Triggers the asynchronous initialization process for all framework components including STT services.
        /// </summary>
        public void Connect()
        {
            foreach (InworldUIElement element in m_UIElements)
                element.Interactable = false;
            InworldController.Instance.InitializeAsync();
        }

        /// <summary>
        /// Starts the audio calibration process for microphone input optimization.
        /// Adjusts microphone sensitivity and noise levels to ensure optimal speech recognition accuracy.
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
        
        void OnEnable()
        {
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
            m_Audio.Event.onAudioSent.AddListener((audioData) =>
            {
                AudioChunk chunk = new AudioChunk();
                InworldVector<float> floatArray = new InworldVector<float>();
                foreach (float data in audioData)
                {
                    floatArray.Add(data);
                }
                chunk.SampleRate = 16000;
                chunk.Data = floatArray;
                _ = InworldController.STT.RecognizeSpeechAsync(chunk);
            });
            InworldController.Instance.OnFrameworkInitialized += OnFrameworkInitialized;
            InworldController.STT.OnTaskFinished += OnSpeechRecognized;
        }

        void OnDisable()
        {
            if (InworldController.Instance && InworldController.STT)
                InworldController.STT.OnTaskFinished -= OnSpeechRecognized;
            if (InworldController.Instance)
                InworldController.Instance.OnFrameworkInitialized -= OnFrameworkInitialized;
        }

        void OnSpeechRecognized(string data)
        {
            if (m_STTResult)
                m_STTResult.text += data;
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
        void OnFrameworkInitialized()
        {
            if (m_ConnectButton)
                m_ConnectButton.Switch(true);
            foreach (InworldUIElement element in m_UIElements)
                element.Interactable = true;
            m_IsFrameworkInitialized = true;
        }
        void Update()
        {
            if (m_IsFrameworkInitialized && m_Volume && m_VolumeDetector)
                m_Volume.fillAmount = m_VolumeDetector.CalculateSNR() * 0.05f;
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
    }
}

