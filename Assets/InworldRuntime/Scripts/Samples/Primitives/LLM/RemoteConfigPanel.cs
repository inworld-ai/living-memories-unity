/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Linq;
using Inworld.Framework.Assets;
using TMPro;
using UnityEngine;

namespace Inworld.Framework.Samples
{
    /// <summary>
    /// Sample UI panel for configuring remote LLM settings within the Inworld framework.
    /// Provides an interface for selecting LLM providers and models from predefined lists,
    /// with support for custom provider/model configuration. Manages dropdown selections
    /// and provides links to documentation for supported models.
    /// Serves as a reference implementation for building remote LLM configuration interfaces.
    /// </summary>
    public class RemoteConfigPanel : MonoBehaviour
    {
        [SerializeField] ModelProviders m_ModelProviders;
        [SerializeField] GameObject m_DropDownPanel;
        [SerializeField] GameObject m_InputPanel;
        [SerializeField] TMP_Dropdown m_ProviderDropdown;
        [SerializeField] TMP_Dropdown m_ModelDropdown;
        
        /// <summary>
        /// Opens the Inworld documentation URL for supported text completion models.
        /// Provides users with detailed information about available model options and configuration.
        /// </summary>
        public void OpenURL() => Application.OpenURL("https://beta.docs.inworld.ai/docs/framework/models#text-completion");
        const string k_Others = "Others";
        
        /// <summary>
        /// Handles provider selection from the dropdown menu.
        /// Updates the model dropdown with available models for the selected provider,
        /// or switches to custom input mode if "Others" is selected.
        /// </summary>
        public void SelectProvider()
        {
            string providerName = m_ProviderDropdown.options[m_ProviderDropdown.value].text;
            if (string.Equals(providerName, k_Others, StringComparison.CurrentCultureIgnoreCase))
            {
                m_DropDownPanel.SetActive(false);
                m_InputPanel.SetActive(true);
                return;
            }
            ModelProviderData data = m_ModelProviders.providers.FirstOrDefault(p => p.providerName == providerName);
            if (data == null)
            {
                Debug.Log($"Provider {providerName} is not found");
                return;
            }
            m_ModelDropdown.ClearOptions();
            foreach (string model in data.models)
            {
                m_ModelDropdown.options.Add(new TMP_Dropdown.OptionData(model));
            }
        }

        /// <summary>
        /// Handles model selection from the dropdown menu.
        /// Logs the selected provider and model combination for debugging purposes.
        /// </summary>
        public void SelectModel()
        {
            Debug.LogWarning("Selected Provider: " + m_ProviderDropdown.options[m_ProviderDropdown.value].text);
            Debug.LogWarning("Selected Model: " + m_ModelDropdown.options[m_ModelDropdown.value].text);
        }
        
        /// <summary>
        /// Sets the remote LLM provider configuration.
        /// Updates the LLM module with the specified provider for remote model access.
        /// </summary>
        /// <param name="provider">The name of the LLM provider to use for remote requests.</param>
        public void SetRemoteConfigModelProvider(string provider)
        {
            if (InworldController.LLM)
                InworldController.LLM.Provider = provider;
        }

        /// <summary>
        /// Sets the remote LLM model name configuration.
        /// Updates the LLM module with the specified model name for remote inference.
        /// </summary>
        /// <param name="modelName">The name of the specific model to use from the provider.</param>
        public void SetRemoteConfigModelName(string modelName)
        {
            if (InworldController.LLM)
                InworldController.LLM.ModelName = modelName;
        }
        
        /// <summary>
        /// Handles text input field end edit events.
        /// Logs the final value for debugging purposes when users finish editing text fields.
        /// </summary>
        /// <param name="value">The final text value after editing is complete.</param>
        public void OnEndEdit(string value)
        {
            Debug.LogWarning($"End Edit: {value}");
        }

        void Start()
        {
            if (!m_ProviderDropdown || !m_ModelProviders)
                return;
            m_ProviderDropdown.ClearOptions();
            foreach (ModelProviderData data in m_ModelProviders.providers)
            {
                m_ProviderDropdown.options.Add(new TMP_Dropdown.OptionData(data.providerName));
            }
        }
    }
}