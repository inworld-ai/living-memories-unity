/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Inworld.Framework.Audio
{
    /// <summary>
    /// Defines audio-related events that can be triggered during voice recording and processing.
    /// This class contains Unity Events for various audio states and operations in the Inworld framework.
    /// </summary>
    [Serializable]
    public class AudioEvent
    {
        /// <summary>
        /// Event triggered when audio calibration begins.
        /// Used to set up microphone sensitivity and audio levels.
        /// </summary>
        public UnityEvent onStartCalibrating;
        
        /// <summary>
        /// Event triggered when audio calibration ends.
        /// Indicates that microphone calibration is complete.
        /// </summary>
        public UnityEvent onStopCalibrating;
        
        /// <summary>
        /// Event triggered when audio recording starts.
        /// Fired when the system begins capturing audio input.
        /// </summary>
        public UnityEvent onRecordingStart;
        
        /// <summary>
        /// Event triggered when audio recording ends.
        /// Fired when the system stops capturing audio input.
        /// </summary>
        public UnityEvent onRecordingEnd;
        
        /// <summary>
        /// Event triggered when the player starts speaking.
        /// Detected through voice activity detection algorithms.
        /// </summary>
        public UnityEvent onPlayerStartSpeaking;
        
        /// <summary>
        /// Event triggered when the player stops speaking.
        /// Detected when voice activity falls below threshold.
        /// </summary>
        public UnityEvent onPlayerStopSpeaking;
        
        /// <summary>
        /// Event triggered when audio data is sent for processing.
        /// Provides the audio samples as a list of float values.
        /// </summary>
        public UnityEvent<List<float>> onAudioSent;
        
        /// <summary>
        /// Removes all listeners from all audio events.
        /// This is useful for cleanup when the audio system is being destroyed or reset.
        /// </summary>
        public void RemoveAllEvents()
        {
            onStartCalibrating.RemoveAllListeners();
            onStopCalibrating.RemoveAllListeners();
            onRecordingStart.RemoveAllListeners();
            onRecordingEnd.RemoveAllListeners();
            onPlayerStartSpeaking.RemoveAllListeners();
            onPlayerStopSpeaking.RemoveAllListeners();
            onAudioSent.RemoveAllListeners();
        }
    }
}
