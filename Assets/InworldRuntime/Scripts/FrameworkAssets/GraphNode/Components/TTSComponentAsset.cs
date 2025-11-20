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
    /// TTS component definition (TTSInterface) for graph's json components.
    /// </summary>
    [CreateAssetMenu(fileName = "TTS_Component", menuName = "Inworld/Create Component/TTS", order = -1998)]
    public class TTSComponentAsset : InworldComponentAsset
    {
        [SerializeField] TTSCreationConfigPropData m_CreationConfig;
        
        public override ConfigData CreationConfig
        {
            get => m_CreationConfig;
            set
            {
                if (value is TTSCreationConfigPropData ttsCreationConfigPropData)
                    m_CreationConfig = ttsCreationConfigPropData;
            }
        }
        
        void OnEnable()
        {
            ComponentType = "TTSInterface";
            m_CreationConfig ??= new TTSCreationConfigPropData();
            if (string.IsNullOrEmpty(m_CreationConfig.type))
            {
                m_CreationConfig.type = "RemoteTTSConfig";
            }
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