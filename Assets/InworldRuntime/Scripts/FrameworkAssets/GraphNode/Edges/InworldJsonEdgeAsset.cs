/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;

namespace Inworld.Framework.Graph
{
    /// <summary>
    /// Specialized edge asset for safety checking within graph workflows in the Inworld framework.
    /// Extends the base edge functionality to evaluate content safety and filter harmful content.
    /// This asset can be created through Unity's Create menu and used to add safety gates to conversation flows.
    /// Used for implementing content moderation and safety checks in AI conversation systems.
    /// </summary>
    [CreateAssetMenu(fileName = "New Json Edge", menuName = "Inworld/Create Edge/Json", order = -2695)]
    public class InworldJsonEdgeAsset : InworldEdgeAsset
    {
        public override string EdgeTypeName => "JsonEdge";
        
        /// <summary>
        /// Evaluates whether the input data meets the safety condition for this edge.
        /// Checks the safety status of the input data and determines if the edge should allow passage.
        /// The behavior is controlled by the m_AllowedPassByDefault setting - if true, safe content passes; if false, unsafe content passes.
        /// </summary>
        /// <param name="inputData">The input data to evaluate for safety compliance.</param>
        /// <returns>True if the safety condition is met and the edge should allow passage; otherwise, false.</returns>
        protected override bool MeetsCondition(InworldBaseData inputData)
        {
            InworldJson json = new InworldJson(inputData);
            return m_AllowedPassByDefault ? json.IsValid : !json.IsValid;
        }
    }
}