/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Framework.Graph;
using UnityEngine;

namespace Inworld.Framework.Samples.Node
{
    /// <summary>
    /// Custom node asset that serves as an endpoint for character conversation flows.
    /// Formats final output by prefixing text with the appropriate speaker name.
    /// Acts as the terminal node in conversation graphs to provide properly formatted output.
    /// This asset can be created through Unity's Create menu for character conversation graphs.
    /// </summary>
    [CreateAssetMenu(fileName = "Node_Final", menuName = "Inworld/Create Node/Character Conversation/Endpoint", order = -2899)]

    public class ConversationEndpointNodeAsset : CustomNodeAsset
    {
        [SerializeField] bool m_IsPlayer;
        string m_SpeakerName;
        public override string NodeTypeName => "ConversationEndpointNode";
        /// <summary>
        /// Creates the runtime instance of this node for the specified graph asset.
        /// Automatically sets the speaker name based on whether this is a player or character endpoint.
        /// </summary>
        /// <param name="graphAsset">The graph asset this node belongs to.</param>
        /// <returns>True if runtime creation succeeded, false otherwise.</returns>
        public override bool CreateRuntime(InworldGraphAsset graphAsset)
        {
            if (graphAsset is CharacterInteractionGraphAsset charGraph)
                m_SpeakerName = m_IsPlayer ? InworldFrameworkUtil.PlayerName : charGraph.characters[0].characterName;
            return base.CreateRuntime(graphAsset);
        }

        public override bool RegisterJson(InworldGraphAsset graphAsset)
        {
            if (graphAsset is CharacterInteractionGraphAsset charGraph)
                m_SpeakerName = m_IsPlayer ? InworldFrameworkUtil.PlayerName : charGraph.characters[0].characterName;
            return base.RegisterJson(graphAsset);
        }

        protected override InworldBaseData ProcessBaseData(InworldVector<InworldBaseData> inputs)
        {
            if (inputs.Size <= 0)
                return inputs[0];
            
            InworldText text = new InworldText(inputs[0]);
            if (text.IsValid)
            {
                return new InworldText($"{m_SpeakerName}: {text.Text}");
            }

            return inputs[0];
        }
    }
}