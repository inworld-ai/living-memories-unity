/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;

namespace Inworld.Framework.Samples
{
    /// <summary>
    /// Sample UI component for managing loading state during framework initialization.
    /// Displays a loading interface while the Inworld framework initializes and automatically
    /// transitions to the character creation panel once initialization is complete.
    /// Serves as a reference implementation for building loading screen functionality.
    /// </summary>
    public class LoadingPanel : MonoBehaviour
    {
        [SerializeField] GameObject m_CharacterCreationPanel;
        void OnEnable()
        {
            InworldController.Instance.OnFrameworkInitialized += OnFrameworkInitialized;
        }

        void OnDisable()
        {
            if (!InworldController.Instance)
                return;
            InworldController.Instance.OnFrameworkInitialized -= OnFrameworkInitialized;
        }
        
        void OnFrameworkInitialized()
        {
            if (m_CharacterCreationPanel)
                m_CharacterCreationPanel.SetActive(true);
            gameObject.SetActive(false);
        }

    }
}