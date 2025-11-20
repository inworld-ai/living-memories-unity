/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Framework.Samples.UI
{
    /// <summary>
    /// Base UI component for managing interactable state of Unity UI elements within the Inworld framework.
    /// Provides a unified interface for controlling the interactable property of any Selectable UI component.
    /// Enables bulk UI state management during framework initialization and loading states.
    /// Serves as a reference implementation for building framework-aware UI elements.
    /// </summary>
    public class InworldUIElement : MonoBehaviour
    {
        [SerializeField] Selectable m_Selectable;

        /// <summary>
        /// Gets or sets the interactable state of the underlying UI element.
        /// Controls whether the UI element can respond to user interactions.
        /// Returns false if no selectable component is assigned.
        /// </summary>
        public bool Interactable
        {
            get => m_Selectable && m_Selectable.interactable;
            set
            {
                if (m_Selectable)
                    m_Selectable.interactable = value;
            }
        }
    }
}