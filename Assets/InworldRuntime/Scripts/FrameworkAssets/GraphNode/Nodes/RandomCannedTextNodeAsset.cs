/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using Inworld.Framework.Node;
using UnityEngine;

namespace Inworld.Framework.Graph
{
    /// <summary>
    /// Specialized node asset for random canned text generation within graph workflows in the Inworld framework.
    /// Extends the base node functionality to provide predefined text selection and randomization capabilities.
    /// This asset can be created through Unity's Create menu and used to generate random responses from predefined text sets.
    /// Used for implementing variety in AI responses, fallback messages, and random content generation in conversation systems.
    /// </summary>
    [CreateAssetMenu(fileName = "Node_RandomCannedText", menuName = "Inworld/Create Node/Text Operation/Random Canned Text", order = -2497)]

    public class RandomCannedTextNodeAsset : InworldNodeAsset
    {
        [SerializeField] RandomCannedTextExecutionConfigPropData m_ExeConfigData;
        public override string NodeTypeName => "RandomCannedTextNode";

        public override NodeExecutionConfig GetNodeExecutionConfig()
        {
            InworldVector<string> cannedTexts = new InworldVector<string>();
            cannedTexts.FromList(m_ExeConfigData.properties.cannedPhrases);
            RandomCannedTextNodeExecutionConfig executionConfig  = new RandomCannedTextNodeExecutionConfig();
            executionConfig.CannedPhrases = cannedTexts;
            m_ExecutionConfig = executionConfig;
            return m_ExecutionConfig;
        }

        public override ConfigData ExecutionConfigData
        {
            get
            {
                m_ExeConfigData ??= new RandomCannedTextExecutionConfigPropData();
                m_ExeConfigData.type = "RandomCannedTextNodeExecutionConfig";
                m_ExeConfigData.properties ??= new RandomCannedTextPropertyData();
                return m_ExeConfigData;
            }
        }

        /// <summary>
        /// Creates the runtime representation of this random canned text node within the specified graph.
        /// Converts the random text list to an Inworld vector and initializes the random text generation capabilities.
        /// Sets up the logic for randomly selecting from predefined text responses.
        /// </summary>
        /// <param name="graphAsset">The graph asset that will contain this random canned text node.</param>
        /// <returns>True if runtime creation succeeded; otherwise, false.</returns>
        public override bool CreateRuntime(InworldGraphAsset graphAsset)
        {
            m_Graph = graphAsset;
            RandomCannedTextNodeExecutionConfig executionConfig = GetNodeExecutionConfig() 
                as RandomCannedTextNodeExecutionConfig;
            Runtime = new RandomCannedTextNode(NodeName, executionConfig);
            return Runtime?.IsValid ?? false;
        }
        
        public override ComponentData ComponentData => new ComponentData
        {
            id = NodeName,
            type = NodeTypeName,
            executionConfig = ExecutionConfigData
        };
    }
}