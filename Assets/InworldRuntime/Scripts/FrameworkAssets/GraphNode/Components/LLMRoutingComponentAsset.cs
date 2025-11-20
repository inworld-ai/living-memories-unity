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
    /// LLM Routing component definition (LLMRouting) for graph's json components.
    /// </summary>
    [CreateAssetMenu(fileName = "LLMRouting_Component", menuName = "Inworld/Create Component/LLMRouting", order = -1996)]
    public class LLMRoutingComponentAsset : InworldComponentAsset
    {
        [SerializeField] LLMRoutingCreationConfigPropData m_CreationConfig;
        
        public override ConfigData CreationConfig
        {
            get => m_CreationConfig;
            set
            {
                if (value is LLMRoutingCreationConfigPropData llmRoutingCreationConfigPropData)
                    m_CreationConfig = llmRoutingCreationConfigPropData;
            }
        }
        
        void OnEnable()
        {
            ComponentType = "LLMRouting";
            m_CreationConfig ??= new LLMRoutingCreationConfigPropData();
            if (string.IsNullOrEmpty(m_CreationConfig.type))
                m_CreationConfig.type = "LLMRoutingCreationConfig";
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