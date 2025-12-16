/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Framework.Assets;
using Inworld.Framework.Graph;
using Inworld.Framework.LLM;
using Inworld.Framework.TTS;
using UnityEngine;

namespace Inworld.Framework.Samples.Node
{
    // Input: InworldText or SafetyResult or LLMChatResponse or InworldDataStream<TTSOutput>
    // Output: InworldText or InworldError
    
    /// <summary>
    /// Custom node asset for adding speech events to character conversation history.
    /// Processes various input data types (safety results, TTS output, LLM responses, text) and
    /// converts them to speech utterances that are added to the conversation log.
    /// This asset can be created through Unity's Create menu for character conversation graphs.
    /// </summary>
    [CreateAssetMenu(fileName = "Node_AddSpeechNode", menuName = "Inworld/Create Node/Character Conversation/Add Speech Node", order = -2899)]

    public class AddSpeechEventNodeAsset : CustomNodeAsset
    {
        [SerializeField] bool m_IsPlayer;
        [SerializeField] string m_SpeakerName;

        public override string NodeTypeName => "AddSpeechEventNode";

        /// <summary>
        /// Creates the runtime instance of this node for the specified graph asset.
        /// Automatically sets the speaker name based on whether this is a player or character node.
        /// </summary>
        /// <param name="graphAsset">The graph asset this node belongs to.</param>
        /// <returns>True if runtime creation succeeded, false otherwise.</returns>
        public override bool CreateRuntime(InworldGraphAsset graphAsset)
        {
            if (graphAsset is CharacterInteractionGraphAsset charGraph && charGraph.characters.Count > 0)
                m_SpeakerName = m_IsPlayer ? InworldFrameworkUtil.PlayerName : charGraph.characters[0].characterName;
            return base.CreateRuntime(graphAsset);
        }
        
        protected override InworldBaseData ProcessBaseData(InworldVector<InworldBaseData> inputs)
        {
            if (!(m_Graph is CharacterInteractionGraphAsset charGraph))
            {
                return new InworldError("AddSpeechEvent Node only be used on Character Interaction Graph.", StatusCode.FailedPrecondition);
            }
            InworldBaseData inputData = inputs[0];
            string outResult = TryProcessSafetyResult(inputData);
            if (string.IsNullOrEmpty(outResult))
                outResult = TryProcessTTSOutput(inputData);
            if (string.IsNullOrEmpty(outResult))
                outResult = TryProcessLLMResponse(inputData);
            if (string.IsNullOrEmpty(outResult))
                outResult = TryProcessText(inputData);
            if (string.IsNullOrEmpty(outResult))
                return new InworldError($"Unsupported data type {inputData.GetType()}.", StatusCode.Unimplemented);
            AddUtterance(m_SpeakerName, outResult);
            return new InworldText(outResult);
        }

        /// <summary>
        /// Adds a new utterance to the conversation history.
        /// Records the speaker and their text content in the graph's speech events log.
        /// </summary>
        /// <param name="speaker">The name of the speaker (player or character).</param>
        /// <param name="text">The text content of the utterance.</param>
        public void AddUtterance(string speaker, string text)
        {
            if (m_Graph is not CharacterInteractionGraphAsset charGraph) 
                return;
            Utterance utterance = new Utterance();
            utterance.agentName = speaker;
            utterance.utterance = text;
            charGraph.prompt.conversationData.EventHistory.speechEvents.Add(utterance);
        }

        string TryProcessText(InworldBaseData input)
        {
            InworldText text = new InworldText(input);
            return text.IsValid ? text.Text : "";
        }

        string TryProcessLLMResponse(InworldBaseData input)
        {
            LLMChatResponse response = new LLMChatResponse(input);
            return response.IsValid ? response.Content.ToString() : "";
        }
        string TryProcessSafetyResult(InworldBaseData input)
        {
            SafetyResult safetyResult = new SafetyResult(input);
            return safetyResult.IsValid ? safetyResult.Content : "";
        }

        string TryProcessTTSOutput(InworldBaseData inputData)
        {
            string outputText = "";
            InworldDataStream<TTSOutput> ttsOutputStream = new InworldDataStream<TTSOutput>(inputData);
            if (!ttsOutputStream.IsValid) 
                return outputText;
            InworldInputStream<TTSOutput> ttsInputStream = ttsOutputStream.ToInputStream();
            while (ttsInputStream.IsValid && ttsInputStream.HasNext)
            {
                TTSOutput ttsData = ttsInputStream.Read();
                outputText += ttsData.Text;
            }
            return outputText;
        }

        public override bool RegisterJson(InworldGraphAsset graphAsset)
        {
            if (graphAsset is CharacterInteractionGraphAsset charGraph && charGraph.characters.Count > 0)
                m_SpeakerName = m_IsPlayer ? InworldFrameworkUtil.PlayerName : charGraph.characters[0].characterName;
            return base.RegisterJson(graphAsset);
        }
    }
}