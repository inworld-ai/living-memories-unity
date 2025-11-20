/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Framework.Samples
{
    /// <summary>
    /// Sample UI panel for creating conversational AI characters within the Inworld framework.
    /// Provides an interface for configuring character attributes including gender, voice selection,
    /// name, role, motivation, and description. Manages voice options based on selected gender
    /// and integrates with the character interaction system.
    /// Serves as a reference implementation for building character creation interfaces.
    /// </summary>
    public class CharacterCreationPanel : MonoBehaviour
    {
        [SerializeField] Toggle m_MaleToggle;
        [SerializeField] Toggle m_FemaleToggle;
        [SerializeField] TMP_Dropdown m_VoiceDropDown;
        [SerializeField] List<string> m_MaleVoices;
        [SerializeField] List<string> m_FemaleVoices;
        [SerializeField] CharacterInteractionPanel m_InteractionPanel;
        ConversationalCharacterData m_CharacterData = new ConversationalCharacterData();
        string m_CurrentVoiceID;
        
        int m_CurrentMaleVoice;
        int m_CurrentFemaleVoice;
        
        const string k_FemalePronoun = "She/Hers";
        const string k_MalePronoun = "He/Him";

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            SwitchVoiceCategory();
            ChangeVoice();
        }

        /// <summary>
        /// Switches the voice selection dropdown based on the selected gender.
        /// Updates available voice options and pronouns when gender toggles are changed.
        /// Maintains separate voice selections for male and female categories.
        /// </summary>
        public void SwitchVoiceCategory()
        {
            if (m_FemaleToggle.isOn)
            {
                m_VoiceDropDown.ClearOptions();
                foreach (string voiceId in m_FemaleVoices)
                {
                    m_VoiceDropDown.options.Add(new TMP_Dropdown.OptionData(voiceId));
                }
                m_CharacterData.pronouns = k_FemalePronoun;
                m_VoiceDropDown.value = m_CurrentFemaleVoice;
            }
            else if (m_MaleToggle.isOn)
            {
                m_VoiceDropDown.ClearOptions();
                foreach (string voiceId in m_MaleVoices)
                {
                    m_VoiceDropDown.options.Add(new TMP_Dropdown.OptionData(voiceId));
                }
                m_CharacterData.pronouns = k_MalePronoun;
                m_VoiceDropDown.value = m_CurrentMaleVoice;
            }
            m_VoiceDropDown.RefreshShownValue();
        }

        /// <summary>
        /// Sets the character's display name.
        /// Updates the character data with the provided name for use in conversations.
        /// </summary>
        /// <param name="charName">The name to assign to the character.</param>
        public void SetCharacterName(string charName) => m_CharacterData.name = charName;
        
        /// <summary>
        /// Sets the character's role or occupation.
        /// Defines the character's professional or social function for context in conversations.
        /// </summary>
        /// <param name="role">The role to assign to the character (e.g., "teacher", "friend", "shopkeeper").</param>
        public void SetCharacterRole(string role) => m_CharacterData.role = role;
        
        /// <summary>
        /// Sets the character's underlying motivations and goals.
        /// Provides context for the character's behavior and decision-making in conversations.
        /// </summary>
        /// <param name="motivation">The motivational description for the character.</param>
        public void SetCharacterMotivation(string motivation) => m_CharacterData.motivation = motivation;
        
        /// <summary>
        /// Sets the character's detailed description including personality and background.
        /// Provides comprehensive context for how the character should behave and respond.
        /// </summary>
        /// <param name="description">The detailed description of the character.</param>
        public void SetCharacterDescription(string description) => m_CharacterData.description = description;
        
        /// <summary>
        /// Updates the selected voice based on the current dropdown selection.
        /// Tracks separate voice preferences for male and female categories and updates the current voice ID.
        /// </summary>
        public void ChangeVoice()
        {
            if (m_FemaleToggle.isOn)
                m_CurrentFemaleVoice = m_VoiceDropDown.value;
            else if (m_MaleToggle.isOn)
                m_CurrentMaleVoice = m_VoiceDropDown.value;
            m_CurrentVoiceID = m_VoiceDropDown.options[m_VoiceDropDown.value].text;
        }

        /// <summary>
        /// Finalizes character creation and initiates the character interaction system.
        /// Passes the configured character data and voice selection to the character interaction controller.
        /// </summary>
        public void Proceed()
        {
            m_InteractionPanel.OnCharacterCreated(m_CharacterData, m_CurrentVoiceID);
        }
    }

}
