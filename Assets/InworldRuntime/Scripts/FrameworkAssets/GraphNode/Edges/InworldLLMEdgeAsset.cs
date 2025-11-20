/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Framework.LLM;
using UnityEngine;


namespace Inworld.Framework.Graph
{
    [CreateAssetMenu(fileName = "New LLM Edge", menuName = "Inworld/Create Edge/LLM", order = -2699)]
    public class InworldLLMEdgeAsset : InworldEdgeAsset
    {
        public override string EdgeTypeName => "LLMEdge";
        
        /// <summary>
        /// Evaluates whether the input data meets the safety condition for this edge.
        /// Checks the safety status of the input data and determines if the edge should allow passage.
        /// The behavior is controlled by the m_AllowedPassByDefault setting - if true, safe content passes; if false, unsafe content passes.
        /// </summary>
        /// <param name="inputData">The input data to evaluate for safety compliance.</param>
        /// <returns>True if the safety condition is met and the edge should allow passage; otherwise, false.</returns>
        protected override bool MeetsCondition(InworldBaseData inputData)
        {
            LLMChatRequest request = new LLMChatRequest(inputData);
            return m_AllowedPassByDefault ? request.IsValid : !request.IsValid;
        }
    }
}
