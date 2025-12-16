/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Framework.Node;
using Inworld.Framework.STT;
using UnityEngine;

namespace Inworld.Framework.Graph
{
    // Input: InworldAudio
    // Output: InworldText
    
    /// <summary>
    /// Specialized node asset for Speech-to-Text (STT) operations within graph workflows in the Inworld framework.
    /// Extends the base node functionality to provide speech recognition and audio-to-text conversion capabilities.
    /// This asset can be created through Unity's Create menu and used to integrate speech recognition into conversation flows.
    /// Used for implementing voice input processing, audio transcription, and speech-based interaction in AI systems.
    /// </summary>
    [CreateAssetMenu(fileName = "Node_STT", menuName = "Inworld/Create Node/STT", order = -2799)]
    public class STTNodeAsset : InworldNodeAsset
    {
        [SerializeField] STTExecutionConfigPropData m_ExeConfigData;
        
        public override string NodeTypeName => "STTNode";
        
        public override NodeExecutionConfig GetNodeExecutionConfig()
        {
            STTNodeExecutionConfig sttNodeExecutionConfig = new STTNodeExecutionConfig();
            sttNodeExecutionConfig.STTComponentID = ComponentID;
            m_ExecutionConfig = sttNodeExecutionConfig;
            return m_ExecutionConfig;
        }

        public override ConfigData ExecutionConfigData
        {
            get
            {
                m_ExeConfigData ??= new STTExecutionConfigPropData();
                m_ExeConfigData.type = "STTNodeExecutionConfig";
                m_ExeConfigData.properties ??= new STTExecutionPropertyData();
                m_ExeConfigData.properties.sttComponentID = ComponentID;
                return m_ExeConfigData;
            }
        }

        /// <summary>
        /// Creates the runtime representation of this STT node within the specified graph.
        /// Initializes the speech-to-text processing capabilities and creates the runtime node instance.
        /// Currently uses the old method since the DLL does not have registries implemented yet.
        /// </summary>
        /// <param name="graphAsset">The graph asset that will contain this STT node.</param>
        /// <returns>True if runtime creation succeeded; otherwise, false.</returns>
        // YAN: Currently the dll does not have Registries. We need to use old method.
        public override bool CreateRuntime(InworldGraphAsset graphAsset)
        {
            m_Graph = graphAsset;
            STTNodeExecutionConfig executionCfg = GetNodeExecutionConfig() as STTNodeExecutionConfig;
            m_Graph.Runtime?.AddSTTInterface(NodeName, InworldController.STT.Interface as STTInterface);
            Runtime = new STTNode(NodeName, executionCfg);
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