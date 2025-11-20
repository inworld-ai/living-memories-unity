/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;

namespace Inworld.Framework.Graph
{
    /// <summary>
    /// LLM component definition (LLMInterface) for graph's json components.
    /// </summary>
    [CreateAssetMenu(fileName = "LLM_Component", menuName = "Inworld/Create Component/LLM", order = -2000)]
    public class LLMComponentAsset : InworldComponentAsset
    {
        [SerializeField] LLMCreationConfigPropData m_CreationConfig;
        
        public override ConfigData CreationConfig
        {
            get => m_CreationConfig;
            set
            {
                if (value is LLMCreationConfigPropData llmCreationConfigPropData)
                    m_CreationConfig = llmCreationConfigPropData;
            }
        }

        void OnEnable()
        {
            ComponentType = "LLMInterface";
            m_CreationConfig ??= new LLMCreationConfigPropData();
            if (string.IsNullOrEmpty(m_CreationConfig.type))
                m_CreationConfig.type = "RemoteLLMConfig";
        }
        
        public override ComponentData ComponentData => new ComponentData
        {
            // YAN: For custom edges, it's like that.
            //      For the components, it's implemented in the child class.
            id = ID,
            type = ComponentType,
            creationConfig = m_CreationConfig
        };
    }
}