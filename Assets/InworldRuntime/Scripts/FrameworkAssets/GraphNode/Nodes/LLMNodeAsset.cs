/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;
using Inworld.Framework.LLM;
using Inworld.Framework.Node;
using Inworld.Framework.Primitive;

namespace Inworld.Framework.Graph
{
    // Input: LLMChatRequest
    // Output: LLMChatResponse
    
    /// <summary>
    /// Specialized node asset for Large Language Model (LLM) operations within graph workflows in the Inworld framework.
    /// Extends the base node functionality to provide AI text generation and language processing capabilities.
    /// This asset can be created through Unity's Create menu and used to integrate LLM processing into conversation flows.
    /// Used for implementing AI-powered text generation, conversation, and natural language understanding in AI systems.
    /// </summary>
    [CreateAssetMenu(fileName = "Node_LLM", menuName = "Inworld/Create Node/LLM", order = -2800)]
    public class LLMNodeAsset : InworldNodeAsset
    {
        [SerializeField] LLMExecutionConfigPropData m_ExeConfigData;
        
        public override string NodeTypeName => "LLMChatNode";

        /// <summary>
        /// Whether to enable streaming mode for LLM responses.
        /// When true, the LLM will provide responses in real-time chunks rather than waiting for complete generation.
        /// </summary>
        [Header("LLM Configuration")]
        [SerializeField] bool m_Stream = false;
        
        /// <summary>
        /// The maximum number of tokens the LLM should generate in its response.
        /// Controls the length limit for generated text to manage response size and processing time.
        /// </summary>
        [SerializeField] int m_MaxTokens = 100;
        
        /// <summary>
        /// The temperature parameter for controlling randomness in LLM generation.
        /// Higher values (closer to 1.0) produce more creative/random outputs, lower values produce more focused outputs.
        /// </summary>
        [SerializeField] float m_Temperature = 0.7f;
        
        /// <summary>
        /// The top-p parameter for nucleus sampling in LLM generation.
        /// Controls the diversity of token selection by limiting to the top cumulative probability mass.
        /// </summary>
        [SerializeField] float m_TopP = 0.9f;
        
        public override NodeExecutionConfig GetNodeExecutionConfig()
        {
            LLMChatNodeExecutionConfig executionConfig = new LLMChatNodeExecutionConfig();
            executionConfig.TextGenerationConfig = InworldController.LLM.SetupTextGenerationConfig();
            executionConfig.LLMComponentID = ComponentID;
            executionConfig.IsStream = m_Stream;
            m_ExecutionConfig = executionConfig;
            return m_ExecutionConfig;
        }

        public override ConfigData ExecutionConfigData
        {
            get
            {
                m_ExeConfigData ??= new LLMExecutionConfigPropData();
                m_ExeConfigData.type = "LLMChatNodeExecutionConfig";
                m_ExeConfigData.properties ??= new LLMExecutionPropertyData();
                m_ExeConfigData.properties.llmComponentID = ComponentID;
                m_ExeConfigData.properties.stream = m_Stream;
                return m_ExeConfigData;
            }
        }
        /// <summary>
        /// Creates the runtime representation of this LLM node within the specified graph.
        /// Configures the LLM module with the specified parameters and creates the runtime node instance.
        /// Currently uses the old method since the DLL does not have registries implemented yet.
        /// </summary>
        /// <param name="graphAsset">The graph asset that will contain this LLM node.</param>
        /// <returns>True if runtime creation succeeded; otherwise, false.</returns>
        // YAN: Currently the dll does not have Registries. We need to use old method.
        public override bool CreateRuntime(InworldGraphAsset graphAsset)
        {
            m_Graph = graphAsset;
            InworldLLMModule llm = InworldController.LLM;
            if (!llm)
                return false;
            llm.TopP = m_TopP;
            llm.Temperature = m_Temperature;
            llm.MaxToken = m_MaxTokens;
            LLMChatNodeExecutionConfig llmExecutionCfg = GetNodeExecutionConfig() as LLMChatNodeExecutionConfig;
            m_Graph.Runtime?.AddLLMInterface(NodeName, InworldController.LLM.Interface as LLMInterface);
            Runtime = new LLMChatNode(NodeName, llmExecutionCfg); 
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