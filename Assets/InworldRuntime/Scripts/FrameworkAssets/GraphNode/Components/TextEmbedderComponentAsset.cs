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
    /// Text Embedder component definition (TextEmbedderInterface) for graph's json components.
    /// </summary>
    [CreateAssetMenu(fileName = "TextEmbedder_Component", menuName = "Inworld/Create Component/TextEmbedder", order = -1995)]
    public class TextEmbedderComponentAsset : InworldComponentAsset
    {
        [SerializeField] TextEmbedderCreationConfigPropData m_CreationConfig;
        
        public override ConfigData CreationConfig
        {
            get => m_CreationConfig;
            set
            {
                if (value is TextEmbedderCreationConfigPropData textEmbedderCreationConfigPropData)
                    m_CreationConfig = textEmbedderCreationConfigPropData;
            }
        }
        
        void OnEnable()
        {
            ComponentType = "TextEmbedderInterface";
            m_CreationConfig ??= new TextEmbedderCreationConfigPropData();
            if (string.IsNullOrEmpty(m_CreationConfig.type))
                m_CreationConfig.type = "RemoteTextEmbedderConfig";
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