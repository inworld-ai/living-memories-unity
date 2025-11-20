/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Inworld.Framework.Audio;
using Inworld.Framework.Primitive;
using UnityEngine;


namespace Inworld.Framework
{
    /// <summary>
    /// Represents a group of framework modules that should be initialized together as a single process.
    /// Used to organize module initialization into sequential phases for proper dependency management.
    /// </summary>
    [Serializable]
    public class LoadingProcess
    {
        /// <summary>
        /// The list of framework modules that belong to this initialization process.
        /// All modules in this list will be initialized concurrently within the same process phase.
        /// </summary>
        public List<InworldFrameworkModule> modules;
    }
    
    /// <summary>
    /// The singleton class of the controller. It's the primitive handler
    /// </summary>
    public class InworldController : SingletonBehavior<InworldController>
    {
        [SerializeField] List<LoadingProcess> m_LoadingProcesses = new List<LoadingProcess>();
        [SerializeField] TelemetryModule m_Telemetry;
        [SerializeField] bool m_AutoStart;
        [Space(10)]
        [SerializeField] bool m_VerboseLog;
        
        InworldLLMModule m_LLMInstance;
        InworldTTSModule m_TTSInstance;
        InworldSTTModule m_STTInstance;
        InworldVADModule m_VADInstance;
        InworldSafetyModule m_SafetyInstance;
        InworldAECModule m_AECInstance;
        
        TextEmbedderModule m_TextEmbedderModuleInstance;
        KnowledgeModule m_KnowledgeModuleInstance;
        InworldAudioManager m_AudioManager;
        bool m_Initialized;
        int m_CurrentProcess = 0;
        
        public Action<int> OnProcessInitialized;
        public Action OnFrameworkInitialized;

        /// <summary>
        /// Set/Get DebugMode
        /// </summary>
        public static bool IsDebugMode
        {
            get => Instance && Instance.m_VerboseLog;
            set
            {
                if (!Instance)
                    return;
                Instance.m_VerboseLog = value;
                MemoryManager.DebugMode = value;
            }
        }

        /// <summary>
        /// Get the LLM Module. (Need manually add the module under Controller)
        /// </summary>
        public static InworldLLMModule LLM
        {
            get
            {
                if (!Instance)
                    return null;
                if (Instance.m_LLMInstance)
                    return Instance.m_LLMInstance;
                Instance.m_LLMInstance = Instance.GetComponentInChildren<InworldLLMModule>();
                return Instance.m_LLMInstance;
            }
        }

        /// <summary>
        /// Get the TTS Module. (Need manually add the module under Controller)
        /// </summary>
        public static InworldTTSModule TTS
        {
            get
            {
                if (!Instance)
                    return null;
                if (Instance.m_TTSInstance)
                    return Instance.m_TTSInstance;
                Instance.m_TTSInstance = Instance.GetComponentInChildren<InworldTTSModule>();
                return Instance.m_TTSInstance;
            }
        }
        
        /// <summary>
        /// Get the STT Module. (Need manually add the module under Controller)
        /// </summary>
        public static InworldSTTModule STT
        {
            get
            {
                if (!Instance)
                    return null;
                if (Instance.m_STTInstance)
                    return Instance.m_STTInstance;
                Instance.m_STTInstance = Instance.GetComponentInChildren<InworldSTTModule>();
                return Instance.m_STTInstance;
            }
        }
        
        /// <summary>
        /// Get the AEC Module. (Need manually add the module under Controller)
        /// </summary>
        public static InworldAECModule AEC
        {
            get
            {
                if (!Instance)
                    return null;
                if (Instance.m_AECInstance)
                    return Instance.m_AECInstance;
                Instance.m_AECInstance = Instance.GetComponentInChildren<InworldAECModule>();
                return Instance.m_AECInstance;
            }
        }
        
        /// <summary>
        /// Get the VAD Module. (Need manually add the module under Controller)
        /// </summary>
        public static InworldVADModule VAD
        {
            get
            {
                if (!Instance)
                    return null;
                if (Instance.m_VADInstance)
                    return Instance.m_VADInstance;
                Instance.m_VADInstance = Instance.GetComponentInChildren<InworldVADModule>();
                return Instance.m_VADInstance;
            }
        }
        
        /// <summary>
        /// Get the Text Embedder Module. (Need manually add the module under Controller)
        /// </summary>
        public static TextEmbedderModule TextEmbedder
        {
            get
            {
                if (!Instance)
                    return null;
                if (Instance.m_TextEmbedderModuleInstance)
                    return Instance.m_TextEmbedderModuleInstance;
                Instance.m_TextEmbedderModuleInstance = Instance.GetComponentInChildren<TextEmbedderModule>();
                return Instance.m_TextEmbedderModuleInstance;
            }
        }
        
        /// <summary>
        /// Get the Safety Module. (Need manually add the module under Controller)
        /// This module should be established after text embedder initialize.
        /// </summary>
        public static InworldSafetyModule Safety
        {
            get
            {
                if (!Instance)
                    return null;
                if (Instance.m_SafetyInstance)
                    return Instance.m_SafetyInstance;
                Instance.m_SafetyInstance = Instance.GetComponentInChildren<InworldSafetyModule>();
                return Instance.m_SafetyInstance;
            }
        }
        
        /// <summary>
        /// Get the Knowledge Module. (Need manually add the module under Controller)
        /// This module should be established after text embedder initialize.
        /// </summary>
        public static KnowledgeModule Knowledge
        {
            get
            {
                if (!Instance)
                    return null;
                if (Instance.m_KnowledgeModuleInstance)
                    return Instance.m_KnowledgeModuleInstance;
                Instance.m_KnowledgeModuleInstance = Instance.GetComponentInChildren<KnowledgeModule>();
                return Instance.m_KnowledgeModuleInstance;
            }
        }

        /// <summary>
        /// Gets the InworldAudioManager component responsible for audio input, processing, and output.
        /// Automatically finds and caches the audio manager in the scene if not already set.
        /// Handles microphone capture, voice detection, and audio streaming to the Inworld service.
        /// </summary>
        /// <value>The InworldAudioManager instance, or null if not found or if no instance exists.</value>
        public static InworldAudioManager Audio
        {
            get
            {
                if (!Instance) 
                    return null;
                if (Instance.m_AudioManager)
                    return Instance.m_AudioManager;
                Instance.m_AudioManager = Instance.GetComponentInChildren<InworldAudioManager>();
                return Instance.m_AudioManager;
            }
        }
        
        /// <summary>
        /// Asynchronously initializes all framework modules and establishes connections to required services.
        /// Validates the API key, builds telemetry configuration, and sequentially initializes each module group.
        /// This method should be called before using any Inworld functionality.
        /// </summary>
        public void InitializeAsync()
        {
            if (m_Telemetry)
                m_Telemetry.Build();
            if (string.IsNullOrEmpty(InworldFrameworkUtil.APIKey))
            {
                Debug.LogError("Please input API key in Resources/InworldFramework");
                return;
            }
            InworldFrameworkUtil.Initialize();
            m_CurrentProcess = 0;
            m_Initialized = false;
            InitializeProcess(m_CurrentProcess);
        }

        async void InitializeProcess(int currentProcess)
        {
            float startTime = Time.time;
            List<bool> results = new List<bool>();
            if (currentProcess >= m_LoadingProcesses.Count)
            {
                m_Initialized = true;
                OnFrameworkInitialized?.Invoke();
                return;
            }
            foreach (InworldFrameworkModule module in m_LoadingProcesses[currentProcess].modules)
            {
                if (Time.time - startTime > InworldFrameworkUtil.TimeoutSpan)
                {
                    Debug.LogError("Module initialization timed out!");
                    return;
                }

                bool result = await module.InitializeAsync();
                results.Add(result);

                if (!result)
                {
                    Debug.LogError($"Module {module.GetType().Name} failed to initialize!");
                }
            }
            if (results.All(r => r == true))
            {
                m_CurrentProcess++;
                OnProcessInitialized?.Invoke(m_CurrentProcess);
            }
        }

        void OnEnable()
        {
            OnProcessInitialized += InitializeProcess;
        }

        void OnDisable()
        {
            OnProcessInitialized -= InitializeProcess;
        }

        void Start()
        {
            if (m_AutoStart && !m_Initialized)
                InitializeAsync();
        }

        void OnDestroy()
        {
            InworldFrameworkUtil.Shutdown();
        }
    }
}