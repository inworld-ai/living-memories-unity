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
    // Input: None. Previous Node's input (if exists) would be discarded.
    // Output: InworldText with the current Player's name in the graph.
    /// <summary>
    /// Custom node asset that provides the current player's name as output.
    /// Ignores any input data and returns the framework's configured player name.
    /// Useful for initializing conversation contexts or identifying the player in dialogues.
    /// This asset can be created through Unity's Create menu for character conversation graphs.
    /// </summary>
    [CreateAssetMenu(fileName = "Node_GetPlayerName", menuName = "Inworld/Create Node/Character Conversation/Get Player Name", order = -2900)]
    public class GetPlayerNameNodeAsset : CustomNodeAsset
    {
        public override string NodeTypeName => "GetPlayerNameNode";
        protected override InworldBaseData ProcessBaseData(InworldVector<InworldBaseData> inputs)
        {
            return new InworldText(InworldFrameworkUtil.PlayerName);
        }
    }
}