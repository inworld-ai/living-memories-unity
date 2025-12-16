using System;
using Inworld.Framework.Graph;
using UnityEngine;

namespace Inworld.Framework.Samples.Node
{
    [CreateAssetMenu(fileName = "New Loop Edge", menuName = "Inworld/Create Edge/Loop", order = -2699)]
    public class LoopEdgeAsset : InworldEdgeAsset
    {
        public int echoTimes = 3;
        int m_CurrentLoop = 0;
        public override string EdgeTypeName => "LoopEdge";
        
        /// <summary>
        /// Evaluates whether the input data meets the safety condition for this edge.
        /// Checks the safety status of the input data and determines if the edge should allow passage.
        /// The behavior is controlled by the m_AllowedPassByDefault setting - if true, safe content passes; if false, unsafe content passes.
        /// </summary>
        /// <param name="inputData">The input data to evaluate for safety compliance.</param>
        /// <returns>True if the safety condition is met and the edge should allow passage; otherwise, false.</returns>
        protected override bool MeetsCondition(InworldBaseData inputData)
        {
            Debug.Log($"Current Loop: {m_CurrentLoop} -> {echoTimes}");
            m_CurrentLoop++;
            if (m_CurrentLoop < echoTimes) 
                return m_AllowedPassByDefault;
            m_CurrentLoop = 0;
            return !m_AllowedPassByDefault;
        }

        void OnEnable()
        {
            m_CurrentLoop = 0;
        }
    }
}