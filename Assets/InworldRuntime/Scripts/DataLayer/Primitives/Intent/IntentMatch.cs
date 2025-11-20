/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework
{
    /// <summary>
    /// Represents a matched intent with confidence score within the Inworld framework.
    /// Contains information about recognized user intentions and their matching confidence levels.
    /// Used for intent recognition and natural language understanding in conversation systems.
    /// </summary>
    public class IntentMatch : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the IntentMatch class with default settings.
        /// </summary>
        public IntentMatch()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_IntentMatch_new(),
                InworldInterop.inworld_IntentMatch_delete);
        }

        /// <summary>
        /// Initializes a new instance of the IntentMatch class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the intent match object.</param>
        public IntentMatch(IntPtr rhs)
        {            
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_IntentMatch_delete);
        }

        /// <summary>
        /// Gets or sets the name of the matched intent.
        /// Identifies the specific intent that was recognized from user input.
        /// </summary>
        public string IntentName
        {
            get => InworldInterop.inworld_IntentMatch_intent_name_get(m_DLLPtr);
            set => InworldInterop.inworld_IntentMatch_intent_name_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the confidence score of the intent match.
        /// Represents how confident the system is that this intent was correctly identified, typically ranging from 0.0 to 1.0.
        /// </summary>
        public float Score
        {
            get => InworldInterop.inworld_IntentMatch_score_get(m_DLLPtr);
            set => InworldInterop.inworld_IntentMatch_score_set(m_DLLPtr, value);
        }
    }
}