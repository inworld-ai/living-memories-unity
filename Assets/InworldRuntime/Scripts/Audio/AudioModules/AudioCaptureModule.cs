/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

#if !UNITY_WEBGL
using System.Collections.Generic;
using UnityEngine;


namespace Inworld.Framework.Audio
{
    /// <summary>
    /// Core module responsible for microphone capture and audio input management in the Inworld framework.
    /// This module handles the low-level microphone operations and audio thread management.
    /// Only one AudioCaptureModule should be active in the module list at any time.
    /// 
    /// The Start/Stop microphone functions control the underlying audio capture system,
    /// separate from recording state which controls actual data transmission.
    /// Use these functions sparingly and only when necessary for system management.
    /// </summary>
    public class AudioCaptureModule : InworldAudioModule, IMicrophoneHandler
    {
        /// <summary>
        /// Determines whether the microphone should automatically start when this module initializes.
        /// When true, the microphone will begin capturing and calibration will start automatically.
        /// </summary>
        [SerializeField] public bool autoStart = true;

        void Start()
        {
            if (autoStart && !IsMicRecording)
            {
                if (StartMicrophone())
                    Audio.StartCalibrate();
            }
        }

        /// <summary>
        /// Initializes and starts the microphone capture system using Unity's built-in microphone functionality.
        /// Creates an AudioClip for continuous recording and begins the audio processing thread.
        /// </summary>
        /// <returns>True if the microphone was successfully started and an AudioClip was created; otherwise, false.</returns>
        public virtual bool StartMicrophone()
        {
            Debug.LogWarning("Starting Microphone");
            Audio.RecordingClip = Microphone.Start(Audio.DeviceName, true, k_InputBufferSecond, k_InputSampleRate);
            Audio.RecordingSource.loop = true;
            Audio.RecordingSource.Play();
            Audio.ResetPointer();
            Audio.StartAudioThread();
            return Audio.RecordingClip;
        }
        
        /// <summary>
        /// Change the microphone input device name.
        /// Using 
        /// </summary>
        /// <param name="deviceName">The device name</param>
        public virtual bool ChangeInputDevice(string deviceName)
        {
            Debug.LogWarning($"Changing Microphone to {deviceName}");
            if (deviceName == Audio.DeviceName)
                return true;

            if (IsMicRecording)
                StopMicrophone();

            Audio.DeviceName = deviceName;
            if (!StartMicrophone())
                return false;
            Audio.StartCalibrate();
            return true;
        }
        /// <summary>
        /// Stop the microphone input.
        /// Using Unity's official method by default.
        /// </summary>
        /// <returns>If successfully stopped</returns>
        public virtual bool StopMicrophone()
        {
            Debug.LogWarning("Ending Microphone");
            if (!Audio)
                return false;
            Microphone.End(Audio.DeviceName);
            Audio.InputBuffer.Clear();
            Audio.ResetPointer();
            Audio.StopAudioThread();
            return true;
        }
        /// <summary>
        /// Get the microphone devices
        /// </summary>
        /// <returns></returns>
        public List<string> ListMicDevices() => new List<string>(Microphone.devices);

        /// <summary>
        /// Get if the microphone is recording.
        /// </summary>
        public virtual bool IsMicRecording => Microphone.IsRecording(Audio.DeviceName);
    }
}
#endif
