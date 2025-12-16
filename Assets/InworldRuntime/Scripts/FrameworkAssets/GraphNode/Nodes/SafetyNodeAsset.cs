/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using Inworld.Framework.Graph;
using Inworld.Framework.Node;
using Inworld.Framework.Primitive;
using Inworld.Framework.Safety;
using Inworld.Framework.TextEmbedder;
using UnityEngine;

namespace Inworld.Framework.Samples.Node
{
    // Input: InworldText
    // Output: SafetyResult
    
    /// <summary>
    /// Specialized node asset for content safety checking within graph workflows in the Inworld framework.
    /// Extends the base node functionality to provide content moderation and safety validation capabilities.
    /// This asset can be created through Unity's Create menu and used to implement safety gates in conversation flows.
    /// Used for implementing content filtering, harmful content detection, and safety compliance in AI conversation systems.
    /// </summary>
    [CreateAssetMenu(fileName = "Node_Safety", menuName = "Inworld/Create Node/Safety", order = -2797)]
    public class SafetyNodeAsset : InworldNodeAsset
    {
        [SerializeField] SafetyCreationConfigPropData m_CreationConfigData;
        [SerializeField] SafetyExecutionConfigPropData m_ExeConfigData;
        
        /// <summary>
        /// List of safety threshold configurations for content moderation.
        /// Defines the safety criteria and thresholds used for evaluating content safety.
        /// Contains various safety parameters for different types of harmful content detection.
        /// </summary>
        [SerializeField] List<SafetyThreshold> m_SafetyData;
        
        public override string NodeTypeName => "SafetyCheckerNode";
        /// <summary>
        /// Creates and returns the configuration object for this safety node.
        /// Retrieves the safety configuration from the Inworld controller for content moderation setup.
        /// </summary>
        /// <returns>The safety configuration object containing all necessary safety checking parameters.</returns>
        public override NodeCreationConfig GetNodeCreationConfig()
        {
            SafetyCheckerNodeCreationConfig nodeCreationCfg = new SafetyCheckerNodeCreationConfig();
            nodeCreationCfg.SafetyConfig = InworldController.Safety.CreationConfig;
            nodeCreationCfg.EmbedderComponentID = ComponentID;
            m_CreationConfig = nodeCreationCfg;
            return m_CreationConfig;
        }

        public override ConfigData CreationConfigData
        {
            get
            {
                m_CreationConfigData ??= new SafetyCreationConfigPropData();
                m_CreationConfigData.type = "SafetyCheckerNodeCreationConfig";
                m_CreationConfigData.properties ??= new SafetyPropertyData();
                m_CreationConfigData.properties.embedderComponentID = ComponentID;
                m_CreationConfigData.properties.safetyConfig ??= new SafetyConfigPropData();
                return m_CreationConfigData;
            }
        }

        public override NodeExecutionConfig GetNodeExecutionConfig()
        {
            SafetyCheckerNodeExecutionConfig executionConfig = new SafetyCheckerNodeExecutionConfig();
            SafetyConfig safetyConfig = new SafetyConfig();
            InworldVector<TopicThreshold> forbiddenTopics = new InworldVector<TopicThreshold>();
            foreach (SafetyThreshold safetyThreshold in m_SafetyData)
            {
                TopicThreshold topicThreshold = new TopicThreshold();
                topicThreshold.TopicName = safetyThreshold.topic;
                topicThreshold.Threshold = safetyThreshold.threshold;
                forbiddenTopics.Add(topicThreshold);
            }
            safetyConfig.ForbiddenTopics = forbiddenTopics;
            executionConfig.SafetyConfig = safetyConfig;
            m_ExecutionConfig = executionConfig;
            return m_ExecutionConfig;
        }
        
        public override ConfigData ExecutionConfigData
        {
            get
            {
                m_ExeConfigData ??= new SafetyExecutionConfigPropData();
                m_ExeConfigData.type = "SafetyCheckerNodeExecutionConfig";
                m_ExeConfigData.properties ??= new SafetyExecutionPropertyData();
                return m_ExeConfigData;
            }
        }

        /// <summary>
        /// Creates the runtime representation of this safety node within the specified graph.
        /// Configures the safety module with the specified threshold data and creates the runtime node instance.
        /// Sets up content moderation parameters and initializes the safety checking capabilities.
        /// </summary>
        /// <param name="graphAsset">The graph asset that will contain this safety node.</param>
        /// <returns>True if runtime creation succeeded; otherwise, false.</returns>
        public override bool CreateRuntime(InworldGraphAsset graphAsset)
        {
            m_Graph = graphAsset;
            InworldSafetyModule safety = InworldController.Safety;
            if (!safety)
                return false;
            safety.SetupSafetyThreshold(m_SafetyData);
            
            ComponentStore componentStore = new ComponentStore();
            componentStore.AddTextEmbedderInterface(NodeName, InworldController.TextEmbedder.Interface as TextEmbedderInterface);
            InworldCreationContext creationContext = new InworldCreationContext(componentStore);
            SafetyCheckerNodeCreationConfig creationCfg = GetNodeCreationConfig() as SafetyCheckerNodeCreationConfig;
            SafetyCheckerNodeExecutionConfig executionCfg = GetNodeExecutionConfig() as SafetyCheckerNodeExecutionConfig;
            Runtime = new SafetyCheckerNode(NodeName, creationContext, creationCfg, executionCfg);
            return Runtime?.IsValid ?? false;
        }
        
        public override ComponentData ComponentData => new ComponentData
        {
            id = NodeName,
            type = NodeTypeName,
            creationConfig = CreationConfigData,
            executionConfig = ExecutionConfigData
        };
    }
}