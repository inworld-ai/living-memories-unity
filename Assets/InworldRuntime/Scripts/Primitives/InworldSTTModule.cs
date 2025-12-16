/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/
using Inworld.Framework.Attributes;
using Inworld.Framework.STT;
using Util = Inworld.Framework.InworldFrameworkUtil;
using UnityEngine;


namespace Inworld.Framework.Primitive
{
    /// <summary>
    /// Module for Speech-to-Text (STT) functionality within the Inworld framework.
    /// Converts audio input into text transcriptions using AI-powered speech recognition.
    /// Supports both synchronous and asynchronous speech recognition operations.
    /// </summary>
    [ModelType("Remote", ExcludeTargets = new[] { "StandaloneWindows", "StandaloneWindows64" })]
    public class InworldSTTModule : InworldFrameworkModule
    {
        SpeechRecognitionConfig m_SpeechRecognitionConfig;
        InworldInputStream<string> m_InputStream;
        
        /// <summary>
        /// Performs synchronous speech recognition on the provided audio chunk.
        /// Converts audio data to text using the configured speech recognition model.
        /// </summary>
        /// <param name="audioChunk">The audio data to transcribe into text.</param>
        /// <returns>The transcribed text result, or empty string if recognition failed.</returns>
        public string RecognizeSpeech(AudioChunk audioChunk)
        {
            string result = "";
            if (!Initialized || !(m_Interface is STTInterface sttInterface))
                return result;
            m_SpeechRecognitionConfig ??= new SpeechRecognitionConfig();
            if (m_InputStream != null)
            {
                m_InputStream.Dispose();
                m_InputStream = null;
            }
            m_InputStream ??= sttInterface.RecognizeSpeech(audioChunk, m_SpeechRecognitionConfig);
            while (m_InputStream != null && m_InputStream.HasNext)
            {
                result += m_InputStream.Read();
            }
            Debug.Log(result);
            return result;
        }
        
        /// <summary>
        /// Performs asynchronous speech recognition on the provided audio chunk.
        /// Converts audio data to text using the configured speech recognition model.
        /// Provides progress notifications through task events during processing.
        /// </summary>
        /// <param name="audioChunk">The audio data to transcribe into text.</param>
        /// <returns>A task that completes with the transcribed text result, or empty string if recognition failed.</returns>
        public async Awaitable<string> RecognizeSpeechAsync(AudioChunk audioChunk)
        {
            string result = "";
            if (!Initialized || !(m_Interface is STTInterface sttInterface))
                return result;
            m_SpeechRecognitionConfig ??= new SpeechRecognitionConfig();
            if (m_InputStream != null)
            {
                m_InputStream.Dispose();
                m_InputStream = null;
            }
            m_InputStream ??= sttInterface.RecognizeSpeech(audioChunk, m_SpeechRecognitionConfig);
            if (m_InputStream == null)
            {
                Debug.LogError($"[InworldSTTModule] Failed to create Task: RecognizeSpeech for {m_ModelType}");
                return result;
            }
            NotifyTaskStart();
            await Awaitable.BackgroundThreadAsync();
            while (m_InputStream != null && m_InputStream.HasNext)
            {
                result += m_InputStream.Read();
            }
            await Awaitable.MainThreadAsync();
            NotifyTaskEnd(result);
            Debug.Log(result);
            return result;
        }

        /// <summary>
        /// Creates and returns an STTFactory for this module.
        /// </summary>
        /// <returns>A factory instance for creating speech recognition objects.</returns>
        public override InworldFactory CreateFactory() => m_Factory ??= new STTFactory();

        /// <summary>
        /// Sets up the configuration for speech recognition operations.
        /// Configures model paths and API settings based on the selected model type (Remote or Local).
        /// </summary>
        /// <returns>An STT configuration instance for module initialization.</returns>
        public override InworldConfig SetupConfig()
        {
            if (ModelType == ModelType.Remote)
            {
                STTRemoteConfig remoteConfig = new STTRemoteConfig();
                if (!string.IsNullOrEmpty(Util.APIKey))
                    remoteConfig.APIKey = Util.APIKey;
                return remoteConfig;
            }
            STTLocalConfig localConfig = new STTLocalConfig();
            localConfig.ModelPath = $"{Application.streamingAssetsPath}/{Util.STTModelPath}";
            localConfig.Device = Util.GetDevice(ModelType);
            return localConfig;
        }
    }
}