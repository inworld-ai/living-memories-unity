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
    /// Specialized edge asset for text data filtering within graph workflows in the Inworld framework.
    /// Extends the base edge functionality to evaluate text data validity and control text flow.
    /// This asset can be created through Unity's Create menu and used to add text-specific routing to conversation flows.
    /// Used for implementing text processing gates and text data validation in AI conversation systems.
    /// </summary>
    [CreateAssetMenu(fileName = "New Text Edge", menuName = "Inworld/Create Edge/Text", order = -2697)]
    public class InworldTextEdgeAsset : InworldEdgeAsset
    {
        public override string EdgeTypeName => "TextEdge";
        
        /// <summary>
        /// Evaluates whether the input data meets the text condition for this edge.
        /// Checks if the input data contains valid text information and determines edge traversal.
        /// The behavior is controlled by the m_AllowedPassByDefault setting - if true, valid text passes; if false, invalid text passes.
        /// </summary>
        /// <param name="inputData">The input data to evaluate for text content validity.</param>
        /// <returns>True if the text condition is met and the edge should allow passage; otherwise, false.</returns>
        protected override bool MeetsCondition(InworldBaseData inputData)
        {
            InworldText textData = new InworldText(inputData);
            return m_AllowedPassByDefault ? textData.IsValid : !textData.IsValid;
        }
    }
}