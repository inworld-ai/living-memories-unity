/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using Inworld.Framework.TTS;
using Inworld.Framework.Node;
using UnityEngine;
using Util = Inworld.Framework.InworldFrameworkUtil;

namespace Inworld.Framework.Primitive
{
    /// <summary>
    /// Module for Text-to-Speech (TTS) functionality within the Inworld framework.
    /// Converts text input into synthesized speech audio using AI-powered voice synthesis.
    /// Supports both synchronous and asynchronous speech synthesis operations with customizable voice settings.
    /// </summary>
    public class InworldTTSModule : InworldFrameworkModule
    {
        [SerializeField] AudioSource m_AudioSource;

        InworldVoice m_InworldVoice;
        InworldInputStream<SpeechChunk> m_InputStream;
        SpeechSynthesisConfig m_SpeechSynthesisConfig;

        public SpeechSynthesisConfig SynthesisConfig => m_SpeechSynthesisConfig;
        public InworldVoice Voice => m_InworldVoice;
        /// <summary>
        /// Performs synchronous text-to-speech conversion and immediately plays the result.
        /// Converts the provided text to speech audio using the specified speaker voice.
        /// </summary>
        /// <param name="text">The text content to convert to speech.</param>
        /// <param name="speakerID">The voice identifier to use for speech synthesis.</param>
        public void TextToSpeech(string text, string speakerID)
        {
            SetVoice(speakerID);
            if (!Initialized || !(m_Interface is TTSInterface ttsInterface))
                return;
            
            m_InputStream ??= ttsInterface.SynthesizeSpeech(text, m_InworldVoice);
            List<float> result = new List<float>();
            int sampleRate = 0;
            while (m_InputStream != null && m_InputStream.HasNext)
            {
                SpeechChunk chunk = m_InputStream.Read();
                if (chunk == null) 
                    continue;
                sampleRate = chunk.SampleRate;
                List<float> data = chunk.WaveForm?.ToList();
                if (data != null && data.Count > 0)
                    result.AddRange(data);
            }
            Debug.Log($"SampleRate: {sampleRate}");
            
            int sampleCount = result.Count;
            if (sampleRate == 0 || sampleCount == 0)
                return;
            AudioClip audioClip = AudioClip.Create("TTS", sampleCount, 1, sampleRate, false);
            audioClip.SetData(result.ToArray(), 0);
            m_AudioSource.clip = audioClip;
            m_AudioSource.Play();
        }

        /// <summary>
        /// Sets the voice profile to use for speech synthesis.
        /// Configures the speaker identity for subsequent TTS operations.
        /// </summary>
        /// <param name="voiceID">The voice identifier to set as the current speaker.</param>
        public void SetVoice(string voiceID)
        {
            m_InworldVoice ??= new InworldVoice();
            m_InworldVoice.SpeakerID = voiceID;
        }
        
        /// <summary>
        /// Performs asynchronous text-to-speech conversion and plays the result when complete.
        /// Converts the provided text to speech audio using the specified speaker voice.
        /// Provides progress notifications through task events during processing.
        /// </summary>
        /// <param name="text">The text content to convert to speech.</param>
        /// <param name="speakerID">The voice identifier to use for speech synthesis.</param>
        public async void TextToSpeechAsync(string text, string speakerID)
        {
            SetVoice(speakerID);
            if (!Initialized || !(m_Interface is TTSInterface ttsInterface))
                return;
            if (m_InputStream != null)
            {
                m_InputStream.Dispose();
                m_InputStream = null;
            }

            await Awaitable.BackgroundThreadAsync();
            m_InputStream = ttsInterface.SynthesizeSpeech(text, m_InworldVoice);
            List<float> result = new List<float>();
            int sampleRate = 0;
            NotifyTaskStart();
            while (m_InputStream != null && m_InputStream.HasNext)
            {
                SpeechChunk chunk = m_InputStream.Read();
                if (chunk == null) 
                    continue;
                sampleRate = chunk.SampleRate;
                List<float> data = chunk.WaveForm?.ToList();
                if (data != null && data.Count > 0)
                    result.AddRange(data);
                await Awaitable.NextFrameAsync();
            }
            await Awaitable.MainThreadAsync();
            string output = $"SampleRate: {sampleRate} Sample Count: {result.Count}";
            NotifyTaskEnd(output);
            Debug.Log(output);
            int sampleCount = result.Count;
            if (sampleRate == 0 || sampleCount == 0)
                return;
            AudioClip audioClip = AudioClip.Create("TTS", sampleCount, 1, sampleRate, false);
            audioClip.SetData(result.ToArray(), 0);
            m_AudioSource.clip = audioClip;
            m_AudioSource.Play();
        }
        
        /// <summary>
        /// Creates and returns a TTSFactory for this module.
        /// </summary>
        /// <returns>A factory instance for creating text-to-speech synthesis objects.</returns>
        public override InworldFactory CreateFactory() => m_Factory ??= new TTSFactory();

        /// <summary>
        /// Sets up the configuration for text-to-speech synthesis operations.
        /// Configures model paths, API settings, and speech processing parameters based on the selected model type.
        /// </summary>
        /// <returns>A TTS configuration instance for module initialization.</returns>
        public override InworldConfig SetupConfig()
        {
            if (ModelType == ModelType.Remote)
            {
                TTSRemoteConfig remoteConfig = new TTSRemoteConfig();
                if (!string.IsNullOrEmpty(Util.APIKey))
                    remoteConfig.APIKey = Util.APIKey;
                m_SpeechSynthesisConfig ??= new SpeechSynthesisConfig();
                m_SpeechSynthesisConfig.Inworld.PostProcessing.SampleRate = 16000;
                remoteConfig.Config = m_SpeechSynthesisConfig;
                return remoteConfig;
            }
            TTSLocalConfig localConfig = new TTSLocalConfig();
            localConfig.ModelPath = $"{Application.streamingAssetsPath}/{Util.TTSModelPath}";
            localConfig.PromptPath = $"{Application.streamingAssetsPath}/{Util.TTSPromptPath}";
            m_SpeechSynthesisConfig ??= new SpeechSynthesisConfig();
            m_SpeechSynthesisConfig.Inworld.PostProcessing.SampleRate = 16000;
            localConfig.Config ??= m_SpeechSynthesisConfig;
            localConfig.Device = Util.GetDevice(ModelType);
            return localConfig;
        }
    }
}