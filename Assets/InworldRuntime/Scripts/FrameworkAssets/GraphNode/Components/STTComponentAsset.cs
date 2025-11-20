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
    /// STT component definition (STTInterface) for graph's json components.
    /// </summary>
    [CreateAssetMenu(fileName = "STT_Component", menuName = "Inworld/Create Component/STT", order = -1999)]
    public class STTComponentAsset : InworldComponentAsset
    {
        [SerializeField] STTCreationConfigPropData m_CreationConfig;
        
        public override ConfigData CreationConfig
        {
            get => m_CreationConfig;
            set
            {
                if (value is STTCreationConfigPropData sttCreationConfigPropData)
                    m_CreationConfig = sttCreationConfigPropData;
            }
        }
        
        void OnEnable()
        {
            ComponentType = "STTInterface";
            m_CreationConfig ??= new STTCreationConfigPropData();
            if (string.IsNullOrEmpty(m_CreationConfig.type))
                m_CreationConfig.type = "LocalSTTConfig";
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