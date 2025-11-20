/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using TMPro;
using UnityEngine;


namespace Inworld.Framework.Samples
{
    /// <summary>
    /// Sample UI panel for configuring local LLM settings within the Inworld framework.
    /// Provides an interface for selecting device types (CPU/CUDA) and specifying model file paths
    /// for locally hosted language models. Manages model path configuration and device type selection.
    /// Serves as a reference implementation for building local LLM configuration interfaces.
    /// </summary>
    public class LocalConfigPanel : MonoBehaviour
    {
        [SerializeField] TMP_Dropdown m_Dropdown;
        [Tooltip("Can only read models inside StreamingAssets")]
        [SerializeField] TMP_InputField m_InputField;

        void Start()
        {
            if (m_Dropdown)
            {
                m_Dropdown.ClearOptions();
                m_Dropdown.options.Add(new TMP_Dropdown.OptionData(DeviceType.CPU.ToString()));
                if (Application.platform == RuntimePlatform.WindowsEditor
                    || Application.platform == RuntimePlatform.WindowsServer
                    || Application.platform == RuntimePlatform.WindowsPlayer)
                    m_Dropdown.options.Add(new TMP_Dropdown.OptionData(DeviceType.CUDA.ToString()));
            }

            if (m_InputField && string.IsNullOrEmpty(m_InputField.text))
            {
                m_InputField.text = InworldFrameworkUtil.LLMModelPath;
            }
        }

        /// <summary>
        /// Sets the device type for local LLM processing based on the dropdown selection.
        /// Configures whether the LLM should use CPU or CUDA for inference processing.
        /// </summary>
        public void SetDeviceType()
        {
            string deviceType = m_Dropdown.options[m_Dropdown.value].text;
            if (InworldController.LLM && Enum.TryParse(deviceType, out ModelType outModelType))
                InworldController.LLM.ModelType = outModelType;
        }

        /// <summary>
        /// Sets the file path for the local LLM model.
        /// Configures the location where the local language model files are stored for loading.
        /// </summary>
        /// <param name="modelPath">The file path to the local LLM model.</param>
        public void SetModelPath(string modelPath)
        {
            if (InworldController.LLM)
                InworldController.LLM.ModelPath = modelPath;
        }
    }
}