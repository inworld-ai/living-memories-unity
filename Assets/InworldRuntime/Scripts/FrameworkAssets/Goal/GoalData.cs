/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using UnityEngine;
using System;

namespace Inworld.Framework.Goal
{
    /// <summary>
    /// Defines the types of responses that can be associated with a goal.
    /// </summary>
    public enum ResponseType
    {
        /// <summary>
        /// Response that provides instructions or guidance.
        /// </summary>
        Instruction,
        
        /// <summary>
        /// Response that should be delivered exactly as written.
        /// </summary>
        Verbatim
    }

    /// <summary>
    /// Represents a response associated with a goal, including its type and content.
    /// </summary>
    [Serializable]
    public class GoalResponse
    {
        /// <summary>
        /// The type of response (Instruction or Verbatim).
        /// </summary>
        public ResponseType type;
        
        /// <summary>
        /// The text content of the response.
        /// </summary>
        public string responseText;
    }
    
    /// <summary>
    /// Represents a condition that must be met for a goal to be activated.
    /// </summary>
    [Serializable]
    public class Condition
    {
        /// <summary>
        /// Description of what this condition represents.
        /// </summary>
        public string description;
        
        /// <summary>
        /// List of activation triggers or keywords that can activate this condition.
        /// </summary>
        public List<string> activations;
    }
    
    /// <summary>
    /// Represents a complete goal definition with its conditions, responses, and behavior settings.
    /// </summary>
    [Serializable]
    public class Goals
    {
        /// <summary>
        /// The name identifier for this goal.
        /// </summary>
        public string goalName;
        
        /// <summary>
        /// The motivation or purpose behind this goal.
        /// </summary>
        public string motivation;
        
        /// <summary>
        /// Whether this goal can be activated multiple times or only once.
        /// </summary>
        public bool repeatable = true;
        
        /// <summary>
        /// The condition that must be met for this goal to be activated.
        /// </summary>
        public Condition condition;
        
        /// <summary>
        /// The response that should be given when this goal is achieved.
        /// </summary>
        public GoalResponse response;
    }
    
    /// <summary>
    /// ScriptableObject that holds a collection of goal definitions for AI character behavior.
    /// This can be created through Unity's Create menu and assigned to characters.
    /// </summary>
    [CreateAssetMenu(fileName = "New Goal", menuName = "Inworld/Create Data/Goals", order = -2497)]
    public class GoalData : ScriptableObject
    {
        /// <summary>
        /// The collection of goals defined in this asset.
        /// </summary>
        public List<Goals> goals;
    }
}