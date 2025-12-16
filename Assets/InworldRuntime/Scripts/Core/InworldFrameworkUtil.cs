/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/


using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using Inworld.Framework.Event;
using Inworld.Framework.Goal;
using Inworld.Framework.Knowledge;
using Inworld.Framework.LLM;
using Inworld.Framework.Memory;
using Inworld.Framework.Telemetry;
using Inworld.Framework.TTS;
using UnityEngine;

namespace Inworld.Framework
{
    /// <summary>
    /// Delegate for processing base data I/O operations from the native C++ DLL.
    /// This callback is invoked by the native library to handle data processing events.
    /// </summary>
    /// <param name="contextPtr">Pointer to the native context object containing processing data.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ProcessBaseDataIODelegate(IntPtr contextPtr);
    
    /// <summary>
    /// Delegate for processing base data I/O operations that include an execution identifier.
    /// Used when native callbacks need to distinguish between different execution contexts.
    /// </summary>
    /// <param name="contextPtr">Pointer to the native context object containing processing data.</param>
    /// <param name="executionId">The execution identifier associated with this callback invocation.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ProcessBaseDataIODelegateExecutionID(IntPtr contextPtr, int executionId);
    
    /// <summary>
    /// Delegate for receiving log messages from the native C++ DLL.
    /// Allows the managed code to capture and process logging output from the native library.
    /// </summary>
    /// <param name="severity">The log level severity (0=verbose, 1=debug, 2=info, 3=warning, 4=error, 5=fatal).</param>
    /// <param name="message">The log message content as a string.</param>
    /// <param name="length">The length of the message string in characters.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void InworldExternalLogListener(int severity, string message, int length);
    
    /// <summary>
    /// Central configuration manager for the Inworld framework, stored as a ScriptableObject in the Resources folder.
    /// Contains all essential settings including model paths, API keys, timeout configurations, and client assets.
    /// This singleton provides centralized access to framework-wide configuration parameters.
    /// </summary>
    public class InworldFrameworkUtil : ScriptableObject
    {
#if UNITY_IOS && !UNITY_EDITOR
    internal const string k_Inworld = "__Internal";
#else
        internal const string k_Inworld = "inworld";
#endif
        
        [SerializeField] InworldTelemetry m_TelemetryConfig;
        [SerializeField] string m_APIKey;
        [Space(10)][Header("Local Model Path:")][Header("(Relative to StreamingAssets folder)")]
        [SerializeField] string m_LLMModelPath = "llm/Meta-Llama-3.1-8B-Instruct-Q8_0.gguf";
        [SerializeField] string m_TextEmbedderPath = "embedders/bge-large-en-v1.5_fp32.gguf";
        [SerializeField] string m_SafetyWeightPath = "safety/model_weights.json";
        [SerializeField] string m_STTModelPath = "stt/ggml-base.en.bin";
        [SerializeField] string m_VADModelPath = "vad/silero/silero_vad.onnx";
        [SerializeField] string m_TTSModelPath = "tts/model.onnx";
        [SerializeField] string m_TTSPromptPath = "tts/prompt";
        [Space(10)]
        [Range(0, 120)] [SerializeField] float m_TimeoutSpan = 30f;
        [Space(10)][Header("Client Assets:")] 
        [SerializeField] Texture2D m_InworldIcon;
        [SerializeField] Texture2D m_DefaultPlayerIcon;
        [SerializeField] string m_PlayerName = "Player";
        [SerializeField] bool m_DebugMode;
        const string k_GlobalDataPath = "InworldRuntime";
        bool m_Initialized;
        static InworldFrameworkUtil __inst;
        
        /// <summary>
        /// Gets the singleton instance of the InworldFrameworkUtil.
        /// Automatically loads the ScriptableObject from the Resources folder if not already cached.
        /// The asset must be stored in the Resources folder with the name "InworldFramework".
        /// </summary>
        /// <value>The InworldFrameworkUtil singleton instance, or null if not found in Resources.</value>
        public static InworldFrameworkUtil Instance
        {
            get
            {
                if (__inst)
                    return __inst;
                __inst = Resources.Load<InworldFrameworkUtil>(k_GlobalDataPath);
                return __inst;
            }
        }
        
        /// <summary>
        /// Gets a value indicating whether the framework utility has been properly initialized.
        /// Returns false if the instance cannot be loaded or if initialization has not completed.
        /// </summary>
        /// <value>True if the framework is initialized and ready for use; otherwise, false.</value>
        public static bool Initialized => Instance?.m_Initialized ?? false;
        
        /// <summary>
        /// Gets the external log listener delegate for receiving log messages from the native C++ DLL.
        /// This listener captures and processes log output from the underlying Inworld native library.
        /// </summary>
        /// <value>The delegate function that handles external log messages.</value>
        public static readonly InworldExternalLogListener LogListener = OnLogReceived;
        
        /// <summary>
        /// Gets or sets the API key used for authenticating with Inworld remote services.
        /// Required for accessing cloud-based AI models and services.
        /// </summary>
        /// <value>The API key string for Inworld service authentication.</value>
        public static string APIKey
        {
            get => Instance.m_APIKey; 
            set => Instance.m_APIKey = value;
        }

        /// <summary>
        /// Gets or sets the timeout duration in seconds for network operations and API calls.
        /// Determines how long the system will wait for responses before timing out.
        /// </summary>
        /// <value>The timeout duration in seconds (0-120 seconds range).</value>
        public static float TimeoutSpan
        {
            get => Instance.m_TimeoutSpan; 
            set => Instance.m_TimeoutSpan = value;
        }
        
        /// <summary>
        /// Gets or sets the display name for the player/user in conversations and interactions.
        /// This name is used to identify the human participant in AI conversations.
        /// </summary>
        /// <value>The player's display name as a string.</value>
        public static string PlayerName
        {
            get => Instance.m_PlayerName;
            set => Instance.m_PlayerName = value;
        }

        public static bool IsDebugMode => Instance.m_DebugMode;
        /// <summary>
        /// Gets the telemetry configuration settings for monitoring and analytics.
        /// Contains settings for data collection, logging, and performance monitoring.
        /// </summary>
        /// <value>The InworldTelemetry configuration object.</value>
        public static InworldTelemetry TelemetryConfig => Instance.m_TelemetryConfig;
        
        /// <summary>
        /// Gets the file path to the Large Language Model (LLM) used for text generation.
        /// Path is relative to the StreamingAssets folder.
        /// </summary>
        /// <value>The relative path to the LLM model file.</value>
        public static string LLMModelPath => Instance.m_LLMModelPath;
        
        /// <summary>
        /// Gets the file path to the text embedding model used for semantic analysis.
        /// Path is relative to the StreamingAssets folder.
        /// </summary>
        /// <value>The relative path to the text embedder model file.</value>
        public static string TextEmbedderPath => Instance.m_TextEmbedderPath;
        
        /// <summary>
        /// Gets the file path to the Speech-to-Text (STT) model for voice recognition.
        /// Path is relative to the StreamingAssets folder.
        /// </summary>
        /// <value>The relative path to the STT model file.</value>
        public static string STTModelPath => Instance.m_STTModelPath;
        
        /// <summary>
        /// Gets the file path to the Voice Activity Detection (VAD) model for speech detection.
        /// Path is relative to the StreamingAssets folder.
        /// </summary>
        /// <value>The relative path to the VAD model file.</value>
        public static string VADModelPath => Instance.m_VADModelPath;
        
        /// <summary>
        /// Gets the file path to the Text-to-Speech (TTS) model for voice synthesis.
        /// Path is relative to the StreamingAssets folder.
        /// </summary>
        /// <value>The relative path to the TTS model file.</value>
        public static string TTSModelPath => Instance.m_TTSModelPath;
        
        /// <summary>
        /// Gets the file path to the safety checker model weights for content filtering.
        /// Path is relative to the StreamingAssets folder.
        /// </summary>
        /// <value>The relative path to the safety model weights file.</value>
        public static string SafetyWeightPath => Instance.m_SafetyWeightPath;
        
        /// <summary>
        /// Gets the directory path containing TTS prompt templates and configurations.
        /// Path is relative to the StreamingAssets folder.
        /// </summary>
        /// <value>The relative path to the TTS prompt directory.</value>
        public static string TTSPromptPath => Instance.m_TTSPromptPath;
        
        /// <summary>
        /// Get the Inworld Icon.
        /// </summary>
        public static Texture2D InworldIcon => Instance.m_InworldIcon;
        
        /// <summary>
        /// Get the Player Icon.
        /// </summary>
        public static Texture2D PlayerIcon => Instance.m_DefaultPlayerIcon;
        
        static readonly Dictionary<Type, Func<IntPtr, IntPtr>> s_BaseDataTypeConverter = 
            new Dictionary<Type, Func<IntPtr, IntPtr>>
            {
                { typeof(ClassificationResult), InworldInterop.inworld_BaseDataAs_ClassificationResult },
                { typeof(InworldText), InworldInterop.inworld_BaseDataAs_Text },
                { typeof(InworldAudio), InworldInterop.inworld_BaseDataAs_Audio },
                { typeof(EventHistory), InworldInterop.inworld_BaseDataAs_EventHistory },
                { typeof(InworldCustomDataWrapper), InworldInterop.inworld_BaseDataAs_CustomDataWrapper },
                { typeof(GoalAdvancement), InworldInterop.inworld_BaseDataAs_GoalAdvancement },
                { typeof(KnowledgeRecords), InworldInterop.inworld_BaseDataAs_KnowledgeRecords },
                { typeof(InworldDataStream<TTSOutput>), InworldInterop.inworld_BaseDataAs_DataStream_TTSOutput },
                { typeof(InworldDataStream<string>), InworldInterop.inworld_BaseDataAs_DataStream_string },
                { typeof(InworldDataStream<InworldContent>), InworldInterop.inworld_BaseDataAs_DataStream_Content },
                { typeof(MemoryState), InworldInterop.inworld_BaseDataAs_MemoryState },
                { typeof(InworldError), InworldInterop.inworld_BaseDataAs_Error },
                { typeof(InworldJson), InworldInterop.inworld_BaseDataAs_Json },
                { typeof(LLMChatRequest), InworldInterop.inworld_BaseDataAs_LLMChatRequest },
                { typeof(LLMChatResponse), InworldInterop.inworld_BaseDataAs_LLMChatResponse },
                { typeof(LLMCompletionResponse), InworldInterop.inworld_BaseDataAs_LLMCompletionResponse },
                { typeof(MatchedIntents), InworldInterop.inworld_BaseDataAs_MatchedIntents },
                { typeof(MatchedKeywords), InworldInterop.inworld_BaseDataAs_MatchedKeywords },
                { typeof(SafetyResult), InworldInterop.inworld_BaseDataAs_SafetyResult }
            };
        
        /// <summary>
        /// Initialize the framework util. Register logger to c++ dll.
        /// </summary>
        public static void Initialize()
        {
            InitLogger();
            Instance.m_Initialized = true;
        }

        /// <summary>
        /// Unregister logger, and shut down telemetry.
        /// </summary>
        public static void Shutdown()
        {
            if (Instance.m_Initialized)
                InworldInterop.inworld_ShutdownTelemetry();
            UnregisterLogger();
        }

        static void InitLogger() => RegisterExternalLogListener(LogListener);
        
        static void UnregisterLogger() => UnregisterExternalLogListener(LogListener);
        
        [DllImport(k_Inworld, CallingConvention = CallingConvention.Cdecl)]
        static extern void RegisterExternalLogListener(InworldExternalLogListener log_listener);
        
        [DllImport(k_Inworld, CallingConvention = CallingConvention.Cdecl)]
        static extern void UnregisterExternalLogListener(InworldExternalLogListener log_listener);
        
        [MonoPInvokeCallback(typeof(InworldExternalLogListener))]
        internal static void OnLogReceived(int severity, string message, int length)
        {
            switch (severity)
            {
                case 1:
                    Debug.LogWarning($"[InworldFramework DLL]: {message}");
                    break;
                case 2:
                    Debug.LogError($"[InworldFramework DLL]: {message}");
                    break;
                case 3:
                    // YAN: We leave base exception here. Might add more details later.
                    // throw new Exception($"[InworldFramework DLL]: {message}"); 
                    Debug.LogError($"[InworldFramework DLL Exception]: {message}");
                    break;
                default:
                    Debug.Log($"[InworldFramework DLL]: {message}");
                    break;
            }
        }

        /// <summary>
        /// Use the param (key value pair) to replace jinja template.
        /// </summary>
        /// <param name="jinja">target jinja template.</param>
        /// <param name="param">input parameter.</param>
        /// <returns>the output string updated by jinja.</returns>
        public static string RenderJinja(string jinja, string param)
        {
            IntPtr statusString = InworldInterop.inworld_RenderJinja(jinja, param);
            IntPtr status = InworldInterop.inworld_StatusOr_string_status(statusString);
            string strResult = InworldInterop.inworld_Status_ToString(status);
            if (InworldInterop.inworld_StatusOr_string_ok(statusString))
                return InworldInterop.inworld_StatusOr_string_value(statusString);
            Debug.LogError(strResult);
            InworldInterop.inworld_StatusOr_string_delete(statusString);
            return "";
        }
        
        /// <summary>
        /// Get the using CPU or GPU. Depending on the type.
        /// </summary>
        /// <param name="modelType">CPU or GPU</param>
        /// <returns>the device that converted into InworldDevice (C++ dll object)</returns>
        public static InworldDevice GetDevice(ModelType modelType)
        {
            if (modelType == ModelType.Remote)
                return null;
            Devices devices = new Devices();
            int nSize = devices.Size;
            for (int i = 0; i < nSize; i++)
            {
                InworldDevice device = devices[i];
                DeviceInfo info = device.Info;
                if (IsDebugMode)
                    Debug.Log($"{device.Type}: {info.Name}");
                if ((modelType == ModelType.LocalCPU && device.Type == DeviceType.CPU) || (modelType == ModelType.LocalGPU && device.Type == DeviceType.CUDA))
                {
                    if (IsDebugMode)
                        Debug.Log($"Select {device.Type}: {info.Name}");
                    return device;
                }
            }
            return null;
        }

        /// <summary>
        /// Executing the dll functions that returns StatusOrString, and get the result if status is OK.
        /// </summary>
        /// <param name="executeFuncResult">The function with parameter. Get result (StatusOrString) </param>
        /// <param name="statusFunction">The status function of StatusOrString</param>
        /// <param name="checkerFunction">The ok function of StatusOrString</param>
        /// <param name="valueFunction">The value function of StatusOrString</param>
        /// <param name="deleteFunction">The delete function of StatusOrString</param>
        /// <returns>The string of the value</returns>
        public static string Execute(IntPtr executeFuncResult,
            Func<IntPtr, IntPtr> statusFunction,
            Func<IntPtr, bool> checkerFunction,
            Func<IntPtr, string> valueFunction,
            Action<IntPtr> deleteFunction)
        {
            IntPtr status = statusFunction(executeFuncResult);
            string strResult = InworldInterop.inworld_Status_ToString(status);
            if (checkerFunction(executeFuncResult))
            {
                return valueFunction(executeFuncResult);
            }
            Debug.LogError(strResult);
            deleteFunction(executeFuncResult);
            return "";
        }
        
        /// <summary>
        /// Executing the dll functions that returns StatusOr, and get the data if status is OK.
        /// </summary>
        /// <param name="executeFuncResult">The function with parameter. Get result (StatusOr) </param>
        /// <param name="statusFunction">The status function of StatusOr</param>
        /// <param name="checkerFunction">The ok function of StatusOr</param>
        /// <param name="valueFunction">The value function of StatusOr</param>
        /// <param name="deleteFunction">The delete function of statusOr</param>
        /// <returns>The actual value (Not StatusOr)</returns>
        public static IntPtr Execute(IntPtr executeFuncResult,
            Func<IntPtr, IntPtr> statusFunction,
            Func<IntPtr, bool> checkerFunction,
            Func<IntPtr, IntPtr> valueFunction,
            Action<IntPtr> deleteFunction)
        {
            IntPtr status = statusFunction(executeFuncResult);
            string strResult = InworldInterop.inworld_Status_ToString(status);
            if (checkerFunction(executeFuncResult))
                return valueFunction(executeFuncResult);
            Debug.LogError(strResult);
            deleteFunction(executeFuncResult);
            return IntPtr.Zero;
        }
        
        /// <summary>
        /// Executing the dll functions that returns StatusOrInt, and get the data if status is OK.
        /// </summary>
        /// <param name="executeFuncResult">The function with parameter. Get result (StatusOrInt) </param>
        /// <param name="statusFunction">The status function of StatusOrInt</param>
        /// <param name="checkerFunction">The ok function of StatusOrInt</param>
        /// <param name="valueFunction">The value function of StatusOrInt</param>
        /// <param name="deleteFunction">The delete function of StatusOrInt</param>
        /// <returns>The actual int value</returns>
        public static int Execute(IntPtr executeFuncResult,
            Func<IntPtr, IntPtr> statusFunction,
            Func<IntPtr, bool> checkerFunction,
            Func<IntPtr, int> valueFunction,
            Action<IntPtr> deleteFunction)
        {
            IntPtr status = statusFunction(executeFuncResult);
            string strResult = InworldInterop.inworld_Status_ToString(status);
            if (checkerFunction(executeFuncResult))
            {
                return valueFunction(executeFuncResult);
            }
            Debug.LogError(strResult);
            deleteFunction(executeFuncResult);
            return -2;
        }
    }
}