/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

#if !UNITY_WEBGL
using System.Linq;
using UnityEngine;

namespace Inworld.Framework.Audio
{
    /// <summary>
    /// Audio sampling module responsible for collecting audio data from the microphone input buffer.
    /// This module interfaces with Unity's microphone system to continuously capture audio frames
    /// and populate the input buffer for further processing by other audio modules.
    /// </summary>
    public class AudioCollectModule : InworldAudioModule, ICollectAudioHandler
    {
        /// <summary>
        /// Determines whether the module should automatically attempt to reconnect the microphone if it becomes disconnected.
        /// When true, the module will automatically restart the microphone if recording stops unexpectedly.
        /// </summary>
        [SerializeField] protected bool m_AutoReconnect = true;
        
        protected int m_LastPosition;
        protected int m_CurrPosition;

        /// <summary>
        /// Collects audio data from the microphone for the current frame and updates the input buffer.
        /// Called by the AudioManager's audio processing coroutine to capture approximately 0.1 seconds of audio data.
        /// Uses Unity's microphone API to retrieve the latest audio samples.
        /// </summary>
        /// <returns>The number of audio samples collected, or -1 if collection failed.</returns>
        public virtual int OnCollectAudio()
        {
            string deviceName = Audio.DeviceName;
            if (m_AutoReconnect && !Audio.IsMicRecording)
                Audio.StartMicrophone();
            AudioClip recClip = Audio.RecordingClip;
            if (!recClip)
                return -1;
            m_CurrPosition = Microphone.GetPosition(deviceName);
            if (m_CurrPosition < m_LastPosition)
                m_CurrPosition = recClip.samples;
            if (m_CurrPosition <= m_LastPosition)
                return -1;
            int nSize = m_CurrPosition - m_LastPosition;
            float[] rawInput = new float[nSize];
            if (!Audio.RecordingClip.GetData(rawInput, m_LastPosition))
                return -1;
            Audio.InputBuffer.Enqueue(rawInput.ToList());
            m_LastPosition = m_CurrPosition % recClip.samples;
            return nSize;
        }
        /// <summary>
        /// Reset the pointer of the Audio Buffer.
        /// </summary>
        public void ResetPointer() => m_LastPosition = m_CurrPosition = 0;
    }
}
#endif