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
using TMPro;
using UnityEngine;

namespace Inworld.Framework.Samples.Node
{
    /// <summary>
    /// Sample implementation demonstrating custom node functionality within the Inworld framework.
    /// Provides an interface for testing custom data processing with both text and audio inputs.
    /// Supports real-time audio capture and text input processing through custom node execution.
    /// Serves as a reference implementation for building applications with custom processing logic.
    /// </summary>
    public class CustomNodeTemplate : NodeTemplate
    {
        [SerializeField] InworldAudioManager m_Audio;
        [SerializeField] AudioSource m_AudioSource;
        [SerializeField] TMP_InputField m_InputField;
        [SerializeField] TMP_Text m_ResultText;
        List<InworldUIElement> m_UIElements = new List<InworldUIElement>();
        
        void Awake()
        {
            m_UIElements.AddRange(GetComponentsInChildren<InworldUIElement>(true));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Audio.Event.onAudioSent.AddListener(async audioData =>
            {
                InworldVector<float> floatArray = new InworldVector<float>();
                foreach (float data in audioData)
                {
                    floatArray.Add(data);
                }
                InworldAudio chunk = new InworldAudio(floatArray, audioData.Count);
                await m_InworldGraphExecutor.ExecuteGraphAsync("Custom", chunk);
            });
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Return) && !string.IsNullOrEmpty(m_InputField.text))
                SendText();
        }
        protected override void OnGraphCompiled(InworldGraphAsset obj)
        {
            foreach (InworldUIElement element in m_UIElements)
                element.Interactable = true;

        }
        
        protected override void OnGraphResult(InworldBaseData obj)
        {
            InworldText text = new InworldText(obj);
            if (text.IsValid)
            {
                m_ResultText.text = "Result: " + text.Text;
                return;
            }
            InworldAudio inworldAudio = new InworldAudio(obj);
            if (!inworldAudio.IsValid) 
                return;
            m_AudioSource.clip = inworldAudio.AudioClip;
            m_AudioSource.Play();
        }
        
        /// <summary>
        /// Sends the current text input to the custom node for processing.
        /// Creates a text request and submits it to the graph executor for custom node execution.
        /// Clears the input field after submission.
        /// </summary>
        public async void SendText()
        {
            InworldText txtRequest = new InworldText(m_InputField.text);
            m_InputField.text = "";
            await m_InworldGraphExecutor.ExecuteGraphAsync("Custom", txtRequest);
        }
    }
}