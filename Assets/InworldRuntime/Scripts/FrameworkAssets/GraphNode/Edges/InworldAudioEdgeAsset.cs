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
    /// Specialized edge asset for audio data filtering within graph workflows in the Inworld framework.
    /// Extends the base edge functionality to evaluate audio data validity and control audio flow.
    /// This asset can be created through Unity's Create menu and used to add audio-specific routing to conversation flows.
    /// Used for implementing audio processing gates and audio data validation in AI conversation systems.
    /// </summary>
    [CreateAssetMenu(fileName = "New Audio Edge", menuName = "Inworld/Create Edge/Audio", order = -2698)]
    public class InworldAudioEdgeAsset : InworldEdgeAsset
    {
        public override string EdgeTypeName => "AudioEdge";
        
        /// <summary>
        /// Evaluates whether the input data meets the audio condition for this edge.
        /// Checks if the input data contains valid audio information and determines edge traversal.
        /// The behavior is controlled by the m_AllowedPassByDefault setting - if true, valid audio passes; if false, invalid audio passes.
        /// </summary>
        /// <param name="inputData">The input data to evaluate for audio content validity.</param>
        /// <returns>True if the audio condition is met and the edge should allow passage; otherwise, false.</returns>
        protected override bool MeetsCondition(InworldBaseData inputData)
        {
            InworldAudio audioData = new InworldAudio(inputData);
            return m_AllowedPassByDefault ? audioData.IsValid : !audioData.IsValid;
        }
    }
}