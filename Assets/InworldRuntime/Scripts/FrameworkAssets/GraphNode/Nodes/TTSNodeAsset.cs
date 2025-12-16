/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Framework.Node;
using Inworld.Framework.TTS;
using UnityEngine;


namespace Inworld.Framework.Graph
{
    // Input: InworldText
    // Output: InworldDataStream<TTSOutput>
    
    /// <summary>
    /// Specialized node asset for Text-to-Speech (TTS) operations within graph workflows in the Inworld framework.
    /// Extends the base node functionality to provide speech synthesis and text-to-audio conversion capabilities.
    /// This asset can be created through Unity's Create menu and used to integrate speech generation into conversation flows.
    /// Used for implementing voice output generation, text narration, and speech-based responses in AI systems.
    /// </summary>
    [CreateAssetMenu(fileName = "Node_TTS", menuName = "Inworld/Create Node/TTS", order = -2798)]
    public class TTSNodeAsset : InworldNodeAsset
    {
        [SerializeField] TTSExecutionConfigPropData m_ExeConfigData;
        
        public override string NodeTypeName => "TTSNode";

        /// <summary>
        /// The voice identifier to use for speech synthesis.
        /// Specifies which voice profile should be used for generating speech output from text input.
        /// </summary>
        public string voiceID = "Default";

        public override NodeExecutionConfig GetNodeExecutionConfig()
        {
            TTSNodeExecutionConfig nodeExecutionConfig = new TTSNodeExecutionConfig();
            nodeExecutionConfig.TTSComponentID = ComponentID;
            nodeExecutionConfig.Voice = InworldController.TTS.Voice;
            nodeExecutionConfig.SpeechSynthesisConfig = InworldController.TTS.SynthesisConfig;
            m_ExecutionConfig = nodeExecutionConfig;
            return m_ExecutionConfig;
        }

        public override ConfigData ExecutionConfigData
        {
            get
            {
                m_ExeConfigData ??= new TTSExecutionConfigPropData();
                m_ExeConfigData.type = "TTSNodeExecutionConfig";
                m_ExeConfigData.properties ??= new TTSExecutionPropertyData();
                m_ExeConfigData.properties.ttsComponentID = ComponentID;
                return m_ExeConfigData;
            }
        }

        /// <summary>
        /// Creates the runtime representation of this TTS node within the specified graph.
        /// Configures the voice settings and initializes the text-to-speech processing capabilities.
        /// Sets the voice ID for speech synthesis and creates the runtime node instance.
        /// Currently uses the old method since the DLL does not have registries implemented yet.
        /// </summary>
        /// <param name="graphAsset">The graph asset that will contain this TTS node.</param>
        /// <returns>True if runtime creation succeeded; otherwise, false.</returns>
        // YAN: Currently the dll does not have Registries. We need to use old method.
        public override bool CreateRuntime(InworldGraphAsset graphAsset)
        {
            m_Graph = graphAsset;
            InworldController.TTS.SetVoice(voiceID);
            Debug.LogWarning($"SET VOICE ID to: {voiceID}");
            TTSNodeExecutionConfig executionConfig = GetNodeExecutionConfig() as TTSNodeExecutionConfig;
            m_Graph.Runtime?.AddTTSInterface(NodeName, InworldController.TTS.Interface as TTSInterface);
            Runtime = new TTSNode(NodeName, executionConfig); 
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