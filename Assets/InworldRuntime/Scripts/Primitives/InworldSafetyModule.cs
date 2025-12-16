/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/
using System;
using System.Collections.Generic;
using Inworld.Framework.Attributes;
using Inworld.Framework.Node;
using Inworld.Framework.Safety;
using Inworld.Framework.TextEmbedder;
using UnityEngine;


namespace Inworld.Framework.Primitive
{
    /// <summary>
    /// Represents a safety threshold configuration for a specific unsafe topic.
    /// Used to define the sensitivity level for content moderation across different topic categories.
    /// </summary>
    [Serializable]
    public class SafetyThreshold
    {
        /// <summary>
        /// The type of unsafe topic to monitor (e.g., violence, hate speech, etc.).
        /// </summary>
        public UnsafeTopic topic;
        
        /// <summary>
        /// The threshold value for this topic (typically between 0.0 and 1.0).
        /// Lower values are more restrictive, higher values are more permissive.
        /// </summary>
        public float threshold;
    }
    
    /// <summary>
    /// Module for content safety checking and moderation within the Inworld framework.
    /// Provides real-time content analysis to detect potentially harmful or inappropriate content.
    /// Integrates with text embedding services for advanced semantic analysis of user inputs and AI responses.
    /// </summary>
    [ModelType("LocalCPU", LockAlways = true)]
    public class InworldSafetyModule : InworldFrameworkModule
    {
        [SerializeField] List<SafetyThreshold> m_SafetyData = new List<SafetyThreshold>();
        
        SafetyConfig m_SafetyConfig;
        
        /// <summary>
        /// Creates and returns a SafetyCheckerFactory for this module.
        /// </summary>
        /// <returns>A factory instance for creating safety checker objects.</returns>
        public override InworldFactory CreateFactory() => m_Factory ??= new SafetyCheckerFactory();

        public SafetyConfig SafetyConfig => m_SafetyConfig;
        public SafetyCheckerCreationConfig CreationConfig => m_Config as SafetyCheckerCreationConfig;
        /// <summary>
        /// Sets up the configuration for safety checking operations.
        /// Initializes safety thresholds and text embedder integration.
        /// </summary>
        /// <returns>A SafetyCheckerCreationConfig instance for module initialization.</returns>
        public override InworldConfig SetupConfig()
        {
            SetupSafetyConfig(m_SafetyData);
            if (!InworldController.TextEmbedder || !InworldController.TextEmbedder.Initialized) 
                return null;
            InworldController.Safety.SetupEmbedder(InworldController.TextEmbedder.Interface as TextEmbedderInterface);

            SafetyCheckerCreationConfig config = new SafetyCheckerCreationConfig();
#if UNITY_ANDROID
            config.ModelPath = $"{Application.persistentDataPath}/{InworldFrameworkUtil.SafetyWeightPath}";
#else
            config.ModelPath = $"{Application.streamingAssetsPath}/{InworldFrameworkUtil.SafetyWeightPath}";
#endif
            return config;
        }

        /// <summary>
        /// Sets up safety thresholds for content moderation.
        /// Replaces current threshold configuration with the provided data.
        /// </summary>
        /// <param name="safetyData">List of safety thresholds to configure for different topic types.</param>
        public void SetupSafetyThreshold(List<SafetyThreshold> safetyData)
        {
            m_SafetyData ??= new List<SafetyThreshold>();
            m_SafetyData.Clear();
            m_SafetyData.AddRange(safetyData);
        }
        
        /// <summary>
        /// Sets up the text embedder interface for semantic analysis.
        /// Required for advanced content understanding and safety classification.
        /// </summary>
        /// <param name="textEmbedderInterface">The text embedder interface to use for content analysis.</param>
        public void SetupEmbedder(TextEmbedderInterface textEmbedderInterface)
        {
            m_Factory ??= new SafetyCheckerFactory();
            if (m_Factory is SafetyCheckerFactory safetyCheckerFactory)
                safetyCheckerFactory.TextEmbedder = textEmbedderInterface;
        }
        
        /// <summary>
        /// Configures safety settings based on provided threshold data.
        /// Converts SafetyThreshold objects to internal TopicThreshold format.
        /// </summary>
        /// <param name="safetyThresholds">List of safety thresholds to configure.</param>
        public void SetupSafetyConfig(List<SafetyThreshold> safetyThresholds)
        {
            m_SafetyConfig ??= new SafetyConfig();
            InworldVector<TopicThreshold> topicThresholds = new InworldVector<TopicThreshold>();
            if (safetyThresholds == null || safetyThresholds.Count == 0) 
                return;
            foreach (SafetyThreshold threshold in safetyThresholds)
            {
                TopicThreshold topicThreshold = new TopicThreshold();
                topicThreshold.TopicName = threshold.topic;
                topicThreshold.Threshold = threshold.threshold;
                topicThresholds.Add(topicThreshold);
            }
            m_SafetyConfig.ForbiddenTopics = topicThresholds;
        }


        /// <summary>
        /// Checks if the provided sentence is safe according to current safety thresholds.
        /// Performs real-time content analysis using the configured safety parameters.
        /// </summary>
        /// <param name="sentence">The text content to analyze for safety violations.</param>
        /// <returns>True if the content is considered safe, false if it violates safety thresholds.</returns>
        public bool IsSafe(string sentence)
        {
            SetupSafetyConfig(m_SafetyData);
            if (!Initialized || !(m_Interface is SafetyCheckerInterface safetyInterface))
                return false;
            return safetyInterface.IsSafe(sentence, m_SafetyConfig);
        }
    }
}