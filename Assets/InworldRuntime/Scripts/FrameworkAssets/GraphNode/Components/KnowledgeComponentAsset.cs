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
    /// Knowledge component definition (KnowledgeInterface) for graph's json components.
    /// </summary>
    [CreateAssetMenu(fileName = "Knowledge_Component", menuName = "Inworld/Create Component/Knowledge", order = -1997)]
    public class KnowledgeComponentAsset : InworldComponentAsset
    {
        [SerializeField] KnowledgeCreationConfigPropData m_CreationConfig;
        
        public override ConfigData CreationConfig
        {
            get => m_CreationConfig;
            set
            {
                if (value is KnowledgeCreationConfigPropData knowledgeCreationConfigPropData)
                    m_CreationConfig = knowledgeCreationConfigPropData;
            }
        }
        
        void OnEnable()
        {
            ComponentType = "KnowledgeInterface";
            m_CreationConfig ??= new KnowledgeCreationConfigPropData();
            if (string.IsNullOrEmpty(m_CreationConfig.type))
                m_CreationConfig.type = "RemoteKnowledgeConfig";
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