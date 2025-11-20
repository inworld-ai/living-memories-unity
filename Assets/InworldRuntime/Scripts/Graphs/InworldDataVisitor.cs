/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using Inworld.Framework.Event;
using Inworld.Framework.Goal;
using Inworld.Framework.Knowledge;
using Inworld.Framework.LLM;
using Inworld.Framework.Memory;
using Inworld.Framework.TTS;
using UnityEngine;

namespace Inworld.Framework.Graph
{
    /// <summary>
    /// [OBSOLETE] Component that implements the visitor pattern for processing different types of Inworld data.
    /// This class has been deprecated and should not be used in new implementations.
    /// Provides default handling and logging for various data types produced by Inworld graph execution.
    /// Requires an InworldGraphExecutor component to function properly.
    /// </summary>
    [Obsolete]
    [RequireComponent(typeof(InworldGraphExecutor))]
    public class InworldDataVisitor : MonoBehaviour, IBaseDataVisitor
    {
        [SerializeField] InworldGraphExecutor m_InworldGraphExecutor;
        
        /// <summary>
        /// Event triggered when an audio clip is generated from TTS output stream processing.
        /// </summary>
        public event Action<AudioClip> OnAudioClipGenerated;
        
        void Awake()
        {
            if (!m_InworldGraphExecutor)
                m_InworldGraphExecutor = GetComponent<InworldGraphExecutor>();
            if (!m_InworldGraphExecutor)
            {
                Debug.LogError("[InworldFramework] Cannot Find Inworld Graph Executor");
                return;
            }
            m_InworldGraphExecutor.OnGraphCompiled += OnGraphCompiled;
            m_InworldGraphExecutor.OnGraphStarted += OnGraphStarted;
            m_InworldGraphExecutor.OnGraphResult += OnGraphResult;
            m_InworldGraphExecutor.OnGraphFinished += OnGraphFinished;
            m_InworldGraphExecutor.OnGraphError += OnGraphError;
        }

        void OnDisable()
        {
            if (!m_InworldGraphExecutor) 
                return;
            m_InworldGraphExecutor.OnGraphCompiled -= OnGraphCompiled;
            m_InworldGraphExecutor.OnGraphStarted -= OnGraphStarted;
            m_InworldGraphExecutor.OnGraphResult -= OnGraphResult;
            m_InworldGraphExecutor.OnGraphFinished -= OnGraphFinished;
            m_InworldGraphExecutor.OnGraphError -= OnGraphError;
        }

        /// <summary>
        /// Called when a graph finishes execution. Override to implement custom completion handling.
        /// </summary>
        /// <param name="asset">The graph asset that finished execution.</param>
        public virtual void OnGraphFinished(InworldGraphAsset asset)
        {
            Debug.LogWarning($"Graph {asset.Name} Finished");
        }

        void OnGraphStarted(InworldGraphAsset asset)
        {
            Debug.LogWarning($"Graph {asset.Name} Started");
        }

        /// <summary>
        /// Called when a graph encounters an error during execution. Override to implement custom error handling.
        /// </summary>
        /// <param name="asset">The graph asset that encountered an error.</param>
        /// <param name="errorMessage">The error message describing what went wrong.</param>
        public virtual void OnGraphError(InworldGraphAsset asset, string errorMessage)
        {
            Debug.LogError($"Graph {asset.Name} Error: {errorMessage}");
        }

        /// <summary>
        /// Called when a graph is successfully compiled. Override to implement custom compilation handling.
        /// </summary>
        /// <param name="asset">The graph asset that was compiled.</param>
        public virtual void OnGraphCompiled(InworldGraphAsset asset)
        {
            Debug.LogWarning($"Graph {asset.Name} Compiled");
        }

        /// <summary>
        /// Called when a graph produces a result. Automatically dispatches to appropriate visitor methods.
        /// </summary>
        /// <param name="data">The data result from graph execution.</param>
        public virtual void OnGraphResult(InworldBaseData data)
        {
            
            data.Accept(this);
        }
        
        /// <summary>
        /// Visits generic base data. Override to implement custom handling for unknown data types.
        /// </summary>
        /// <param name="data">The base data to process.</param>
        public virtual void Visit(InworldBaseData data)
        {
            Debug.LogWarning($"Visited BaseData. {data}");
        }

        /// <summary>
        /// Visits text data from the Inworld system. Override to implement custom text processing.
        /// </summary>
        /// <param name="text">The text data to process.</param>
        public virtual void Visit(InworldText text)
        {
            Debug.LogWarning($"Visited Text: {text}");
        }

        /// <summary>
        /// Visits audio data from the Inworld system. Override to implement custom audio processing.
        /// </summary>
        /// <param name="inworldAudio">The audio data to process.</param>
        public virtual void Visit(InworldAudio inworldAudio)
        {
            Debug.Log($"Visited Audio: {inworldAudio}");
        }

        /// <summary>
        /// Visits error data from the Inworld system. Override to implement custom error handling.
        /// </summary>
        /// <param name="error">The error data to process.</param>
        public virtual void Visit(InworldError error)
        {
            Debug.LogError($"Visited Error: {error}");
        }

        /// <summary>
        /// Visits matched keywords data from intent recognition. Override to implement custom keyword handling.
        /// </summary>
        /// <param name="keyword">The matched keywords data to process.</param>
        public virtual void Visit(MatchedKeywords keyword)
        {
            Debug.LogError($"Matched Keywords: {keyword}");
        }

        /// <summary>
        /// Visits classification result data from content analysis. Override to implement custom classification handling.
        /// </summary>
        /// <param name="result">The classification result to process.</param>
        public virtual void Visit(ClassificationResult result)
        {
            Debug.LogError($"ClassificationResult: {result}");
        }

        /// <summary>
        /// Visits JSON data from the Inworld system. Override to implement custom JSON processing.
        /// </summary>
        /// <param name="json">The JSON data to process.</param>
        public virtual void Visit(InworldJson json)
        {
            Debug.Log($"Visited Json: {json}");
        }

        /// <summary>
        /// Visits string data stream from the Inworld system. Override to implement custom string stream processing.
        /// </summary>
        /// <param name="stringStream">The string data stream to process.</param>
        public virtual void Visit(InworldDataStream<string> stringStream)
        {
            Debug.Log($"Visited StringStream: {stringStream}");
        }

        /// <summary>
        /// Visits TTS output stream and converts it to an AudioClip.
        /// Processes the stream asynchronously and triggers OnAudioClipGenerated when complete.
        /// Override to implement custom TTS output handling.
        /// </summary>
        /// <param name="ttsOutputStream">The TTS output stream to process.</param>
        public virtual async void Visit(InworldDataStream<TTSOutput> ttsOutputStream)
        {
            InworldInputStream<TTSOutput> stream = ttsOutputStream.ToInputStream();
            int sampleRate = 0;
            List<float> result = new List<float>();
            await Awaitable.BackgroundThreadAsync();
            while (stream != null && stream.HasNext)
            {
                TTSOutput ttsOutput = stream.Read();
                if (ttsOutput == null) 
                    continue;
                InworldAudio chunk = ttsOutput.Audio;
                sampleRate = chunk.SampleRate;
                List<float> data = chunk.Waveform?.ToList();
                if (data != null && data.Count > 0)
                    result.AddRange(data);
                await Awaitable.NextFrameAsync();
            }
            await Awaitable.MainThreadAsync();
            string output = $"SampleRate: {sampleRate} Sample Count: {result.Count}";
            Debug.Log(output);
            int sampleCount = result.Count;
            if (sampleRate == 0 || sampleCount == 0)
                return;
            AudioClip audioClip = AudioClip.Create("TTS", sampleCount, 1, sampleRate, false);
            audioClip.SetData(result.ToArray(), 0);
            Debug.Log($"Visited TTSOutputStream: {ttsOutputStream}");
            OnAudioClipGenerated?.Invoke(audioClip);
        }

        /// <summary>
        /// Visits content data stream from the Inworld system. Override to implement custom content stream processing.
        /// </summary>
        /// <param name="contentStream">The content data stream to process.</param>
        public virtual void Visit(InworldDataStream<InworldContent> contentStream)
        {
            Debug.Log($"Visited InworldContent DataStream: {contentStream}");
        }

        /// <summary>
        /// Visits memory state data from the Inworld system. Override to implement custom memory state handling.
        /// </summary>
        /// <param name="data">The memory state data to process.</param>
        public virtual void Visit(MemoryState data)
        {
            Debug.Log($"Visited MemoryState: {data}");
        }

        /// <summary>
        /// Visits custom data wrapper from the Inworld system. Override to implement custom data handling.
        /// </summary>
        /// <param name="customDataWrapper">The custom data wrapper to process.</param>
        public virtual void Visit(InworldCustomDataWrapper customDataWrapper)
        {
            Debug.Log($"Visited CustomData: {customDataWrapper}");
        }

        /// <summary>
        /// Visits event history data from the Inworld system. Override to implement custom event history handling.
        /// </summary>
        /// <param name="eventHistory">The event history data to process.</param>
        public virtual void Visit(EventHistory eventHistory)
        {
            Debug.Log($"Visited EventHistory: {eventHistory}");
        }

        /// <summary>
        /// Visits knowledge records data from the Inworld system. Override to implement custom knowledge processing.
        /// </summary>
        /// <param name="knowledgeRecords">The knowledge records data to process.</param>
        public virtual void Visit(KnowledgeRecords knowledgeRecords)
        {
            Debug.Log($"Visited KnowledgeRecords: {knowledgeRecords}");
        }

        /// <summary>
        /// Visits goal advancement data from the Inworld system. Override to implement custom goal handling.
        /// </summary>
        /// <param name="goal">The goal advancement data to process.</param>
        public virtual void Visit(GoalAdvancement goal)
        {
            Debug.Log($"Visited GoalAdvancement: {goal}");
        }

        /// <summary>
        /// Visits LLM chat request data from the Inworld system. Override to implement custom chat request handling.
        /// </summary>
        /// <param name="llmChatRequest">The LLM chat request data to process.</param>
        public virtual void Visit(LLMChatRequest llmChatRequest)
        {
            Debug.Log($"Visited LLMChatRequest: {llmChatRequest}");
        }

        /// <summary>
        /// Visits LLM chat response data from the Inworld system. Override to implement custom chat response handling.
        /// </summary>
        /// <param name="llmChatResponse">The LLM chat response data to process.</param>
        public virtual void Visit(LLMChatResponse llmChatResponse)
        {
            InworldContent llmContent = llmChatResponse.Content;
            Debug.Log($"LLM Content: {llmContent}");
        }

        /// <summary>
        /// Visits LLM completion response data from the Inworld system. Override to implement custom completion handling.
        /// </summary>
        /// <param name="llmCompletionResponse">The LLM completion response data to process.</param>
        public virtual void Visit(LLMCompletionResponse llmCompletionResponse)
        {
            Debug.Log($"LLMCompletionResponse: {llmCompletionResponse}");
        }

        /// <summary>
        /// Visits safety result data from content moderation. Override to implement custom safety handling.
        /// </summary>
        /// <param name="safetyResult">The safety result data to process.</param>
        public virtual void Visit(SafetyResult safetyResult)
        {
            Debug.Log($"SafetyResult: {safetyResult}");
        }

        /// <summary>
        /// Visits matched intents data from intent recognition. Override to implement custom intent handling.
        /// </summary>
        /// <param name="intents">The matched intents data to process.</param>
        public virtual void Visit(MatchedIntents intents)
        {
            Debug.Log($"MatchedIntents: {intents}");
        }
    }
}