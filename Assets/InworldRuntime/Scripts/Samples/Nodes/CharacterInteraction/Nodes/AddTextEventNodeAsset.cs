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
    /// Custom node asset for adding text events to character conversation workflows.
    /// This is a placeholder/template node that can be extended to handle text-based events
    /// in character conversation graphs. Currently contains no implementation.
    /// This asset can be created through Unity's Create menu for character conversation graphs.
    /// </summary>
    [CreateAssetMenu(fileName = "Node_AddTextEvent", menuName = "Inworld/Create Node/Character Conversation/Add Text Event", order = -2899)]
    public class AddTextEventNodeAsset : CustomNodeAsset
    {
        public override string NodeTypeName => "AddTextEventNode";
    }
}