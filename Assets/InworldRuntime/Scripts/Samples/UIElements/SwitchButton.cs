/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using TMPro;
using UnityEngine;

namespace Inworld.Framework.Samples.UI
{
    /// <summary>
    /// Sample UI component representing a toggle switch button with customizable on/off states.
    /// Provides text-based visual feedback for binary state changes and supports multiple switch methods.
    /// Manages state transitions and text display for on/off configurations.
    /// Serves as a reference implementation for building toggle UI elements.
    /// </summary>
    public class SwitchButton : MonoBehaviour
    {
        [SerializeField] TMP_Text m_Text;
        [SerializeField] string m_OnText;
        [SerializeField] string m_OffText;
        [SerializeField] bool m_IsOn;

        /// <summary>
        /// Sets the switch state to the specified boolean value.
        /// Updates the displayed text based on the new state.
        /// </summary>
        /// <param name="isOn">True to set the switch to on state, false for off state.</param>
        public void Switch(bool isOn)
        {
            m_IsOn = isOn;
            m_Text.text = isOn ? m_OnText : m_OffText;
        }

        /// <summary>
        /// Sets the switch state based on matching text input.
        /// Compares the input text with predefined on/off text values to determine the state.
        /// </summary>
        /// <param name="text">The text to match against on/off text values.</param>
        public void Switch(string text)
        {
            if (m_OnText == text)
                Switch(true);
            else if (m_OffText == text)
                Switch(false);
        }

        /// <summary>
        /// Toggles the switch state to the opposite of its current state.
        /// Changes from on to off or off to on and updates the displayed text accordingly.
        /// </summary>
        public void Switch()
        {
            m_IsOn = !m_IsOn;
            m_Text.text = m_IsOn ? m_OnText : m_OffText;
        }
        
        /// <summary>
        /// Gets the current state of the switch.
        /// Returns true if the switch is in the on position, false if off.
        /// </summary>
        public bool IsOn => m_IsOn;
    }
}