/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inworld.Framework.Audio
{
    /// <summary>
    /// Abstract base class for all audio processing modules in the Inworld framework.
    /// Provides common functionality for audio modules including coroutine management and audio manager access.
    /// All audio modules should inherit from this class to integrate with the audio processing pipeline.
    /// </summary>
    public abstract class InworldAudioModule : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets the execution priority of this audio module.
        /// Lower values indicate higher priority and will be processed first.
        /// </summary>
        /// <value>The priority value for module execution ordering.</value>
        public int Priority {get; set;}
        
        protected const int k_InputSampleRate = 16000;
        protected const int k_InputChannels = 1;
        protected const int k_InputBufferSecond = 1;
        protected const int k_SizeofInt16 = sizeof(short);
        InworldAudioManager m_Manager;
        IEnumerator m_ModuleCoroutine;

        /// <summary>
        /// Gets the InworldAudioManager instance that coordinates audio processing.
        /// Automatically finds and caches the audio manager in the scene if not already set.
        /// </summary>
        /// <value>The InworldAudioManager instance responsible for audio coordination.</value>
        public InworldAudioManager Audio
        {
            get
            {
                if (m_Manager != null)
                    return m_Manager;
                m_Manager = FindFirstObjectByType<InworldAudioManager>();
                return m_Manager;
            }
        }
        
        /// <summary>
        /// Starts the module's processing coroutine.
        /// Only one coroutine can be active per module at a time.
        /// </summary>
        /// <param name="moduleCycle">The coroutine to start for this module's processing cycle.</param>
        public virtual void StartModule(IEnumerator moduleCycle)
        {
            if (moduleCycle == null || m_ModuleCoroutine != null) 
                return;
            m_ModuleCoroutine = moduleCycle;
            StartCoroutine(m_ModuleCoroutine);
        }

        /// <summary>
        /// Stops the currently running module coroutine and cleans up resources.
        /// Safe to call even if no coroutine is currently running.
        /// </summary>
        public virtual void StopModule()
        {
            if (m_ModuleCoroutine == null) 
                return;
            StopCoroutine(m_ModuleCoroutine);
            m_ModuleCoroutine = null;
        }
    }
    
    public class ModuleNotFoundException : Exception
    {
        public ModuleNotFoundException(string moduleName) : base($"Module {moduleName} not found")
        {
        }
    }

    /// <summary>
    /// The Interface for the AudioCapture module.
    /// </summary>
    public interface IMicrophoneHandler
    {
        List<string> ListMicDevices();
        bool IsMicRecording {get;}
        bool StartMicrophone();
        bool ChangeInputDevice(string deviceName);
        bool StopMicrophone();
    }

    /// <summary>
    /// The Interface for the Audio collector
    /// </summary>
    public interface ICollectAudioHandler
    {
        int OnCollectAudio();
        void ResetPointer();
    }

    /// <summary>
    /// The interface for the Audio calibrator.
    /// </summary>
    public interface ICalibrateAudioHandler
    {
        void OnStartCalibration();
        void OnStopCalibration();
        void OnCalibrate();
    }

    /// <summary>
    /// The Interface for handling player's command. 
    /// </summary>
    public interface IPlayerAudioEventHandler
    {
        IEnumerator OnPlayerUpdate();
        void StartVoiceDetecting();
        void StopVoiceDetecting();
    }

    /// <summary>
    /// The interface for the Audio Processor.
    /// </summary>
    public interface IProcessAudioHandler
    {
        bool OnPreProcessAudio();
        bool OnPostProcessAudio();
        CircularBuffer<float> ProcessedBuffer { get; set; }
    }

    /// <summary>
    /// The interface of the Audio Sender.
    /// </summary>
    public interface ISendAudioHandler
    {
        void OnStartSendAudio();
        void OnStopSendAudio();
        // void OnSendAudio(List<float> samples);
        List<float> Samples { get; }
    }
}