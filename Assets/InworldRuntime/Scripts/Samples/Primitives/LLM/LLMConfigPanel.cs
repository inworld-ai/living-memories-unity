/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using Inworld.Framework.Samples.UI;

using UnityEngine;


namespace Inworld.Framework.Samples
{
    /// <summary>
    /// Sample main configuration panel for LLM settings within the Inworld framework.
    /// Provides a centralized interface for managing local/remote LLM configurations,
    /// framework connection, and text generation parameters. Coordinates between
    /// different configuration sub-panels and manages UI state during initialization.
    /// Serves as a reference implementation for building comprehensive LLM configuration interfaces.
    /// </summary>
    public class LLMConfigPanel : MonoBehaviour
    {
        [SerializeField] SwitchButton m_ConnectButton;
        [SerializeField] SwitchButton m_RemoteSwitchButton;
        [SerializeField] LocalConfigPanel m_LocalConfigPanel;
        [SerializeField] RemoteConfigPanel m_RemoteConfigPanel;
        [SerializeField] TextConfigPanel m_TextConfigPanel;
        List<InworldUIElement> m_UIElements = new List<InworldUIElement>();

        void Start()
        {
            m_UIElements.AddRange(GetComponentsInChildren<InworldUIElement>(true));
        }

        void OnEnable()
        {
            InworldController.Instance.OnFrameworkInitialized += OnFrameworkInitialized;
        }


        void OnDisable()
        {
            if (InworldController.Instance)
                InworldController.Instance.OnFrameworkInitialized -= OnFrameworkInitialized;
        }



        void OnFrameworkInitialized()
        {
            if (m_ConnectButton)
                m_ConnectButton.Switch(true);
            foreach (InworldUIElement element in m_UIElements)
                element.Interactable = true;
        }
        

        /// <summary>
        /// Initiates connection to the Inworld framework and disables UI during initialization.
        /// Triggers the asynchronous initialization process for all framework components.
        /// </summary>
        public void Connect()
        {
            foreach (InworldUIElement element in m_UIElements)
                element.Interactable = false;
            InworldController.Instance.InitializeAsync();
        }
        
        /// <summary>
        /// Switches between local and remote LLM configuration modes.
        /// Toggles the visibility of local and remote configuration panels based on current state.
        /// </summary>
        public void SwitchRemote()
        {
            if (!m_RemoteSwitchButton)
                return;
            m_RemoteSwitchButton.Switch();
            if (m_RemoteSwitchButton.IsOn) // LOCAL
            {
                if (m_RemoteConfigPanel)
                    m_RemoteConfigPanel.gameObject.SetActive(false);
                if (m_LocalConfigPanel)
                    m_LocalConfigPanel.gameObject.SetActive(true);
            }
            else // REMOTE
            {
                if (m_RemoteConfigPanel)
                    m_RemoteConfigPanel.gameObject.SetActive(true);
                if (m_LocalConfigPanel)
                    m_LocalConfigPanel.gameObject.SetActive(false);
            }
        }


        /// <summary>
        /// Resets all text generation parameters to default values and reinitializes the framework.
        /// Combines parameter reset with framework reinitialization for a complete configuration refresh.
        /// </summary>
        public void ResetAndApply()
        {
            if (m_TextConfigPanel)
                m_TextConfigPanel.ResetDefault();
            InworldController.Instance.InitializeAsync();
        }

        /// <summary>
        /// Applies the current configuration settings by reinitializing the framework.
        /// Triggers framework reinitialization to apply any configuration changes made in the UI.
        /// </summary>
        public void Apply()
        {
            InworldController.Instance.InitializeAsync();
        }
    }
}