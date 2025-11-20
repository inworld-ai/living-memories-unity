/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inworld.Framework.Audio
{
    /// <summary>
    /// Audio dispatch module responsible for sending processed audio data to the Inworld service.
    /// Manages the transmission of audio samples and handles queuing, debugging, and test mode functionality.
    /// This module serves as the final stage in the audio processing pipeline before data reaches the server.
    /// </summary>
    public class AudioDispatchModule: InworldAudioModule, ISendAudioHandler
    {
        [SerializeField] bool m_IsAudioDebugging;
        [SerializeField] bool m_TestMode;
        
        readonly ConcurrentQueue<float> m_AudioToSend = new ConcurrentQueue<float>();
        List<float> m_DebugInput = new List<float>();
        int m_CurrPosition;

        /// <summary>
        /// Gets the audio buffer to use for sending data to the Inworld service.
        /// Returns either the processed buffer from an audio processor module or the raw input buffer.
        /// </summary>
        /// <value>The circular buffer containing audio data ready for transmission.</value>
        public CircularBuffer<float> ShortBufferToSend
        {
            get
            {
                IProcessAudioHandler processor = Audio.GetModule<IProcessAudioHandler>();
                return processor == null ? Audio.InputBuffer : processor.ProcessedBuffer;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the audio data is ready to be sent to the Inworld service.
        /// Checks if the Inworld controller is available and properly initialized.
        /// </summary>
        /// <value>True if audio data can be sent; otherwise, false.</value>
        public bool IsReadyToSend => InworldController.Instance
                                     && InworldController.STT 
                                     && m_AudioToSend != null
                                     && m_AudioToSend.Count > 0;
        
        void OnEnable()
        {
            StartModule(AudioDispatchingCoroutine());
        }
        void OnDisable()
        {
            StopModule();
        }

        AudioChunk GetAudioChunk(List<float> audioData)
        {
            AudioChunk chunk = new AudioChunk();
            InworldVector<float> floatArray = new InworldVector<float>();
            foreach (float data in audioData)
            {
                floatArray.Add(data);
            }
            chunk.SampleRate = 16000;
            chunk.Data = floatArray;
            return chunk;
        }
        IEnumerator AudioDispatchingCoroutine()
        {
            while (isActiveAndEnabled)
            {
                if (Audio.IsPlayerSpeaking)
                {
                    ConverBufferToAudioChunk();
                }
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        protected virtual void ConverBufferToAudioChunk()
        {
            CircularBuffer<float> buffer = ShortBufferToSend;
            if (m_CurrPosition == buffer.currPos) 
                return;
            List<float> data = buffer.GetRange(m_CurrPosition, buffer.currPos);
            foreach (float wavPoint in data)
            {
                m_AudioToSend?.Enqueue(wavPoint);
            }
                
            m_CurrPosition = buffer.currPos;
            if (m_IsAudioDebugging)
                m_DebugInput.AddRange(data);
        }

        /// <summary>
        /// It would be called when the player start allowed to send audio into the buffer.
        /// Implemented in the child class.
        /// </summary>
        public void OnStartSendAudio()
        {

        }
        /// <summary>
        /// It would be called when the player stop allowed to send audio into the buffer.
        /// </summary>
        public void OnStopSendAudio()
        {
            if (m_AudioToSend.Count > 0)
                Audio.OnSendAudio();
            m_AudioToSend.Clear();
        }

        /// <summary>
        /// Get all the current audio samples.
        /// </summary>
        public List<float> Samples => m_AudioToSend.ToList();
    }
}