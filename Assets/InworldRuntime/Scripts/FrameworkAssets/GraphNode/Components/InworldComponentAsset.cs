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
    /// Base ScriptableObject for graph components (e.g., LLM/STT/TTS/Knowledge/TextEmbedder/Routing).
    /// Mirrors the 'components' entries in json fi.
    /// </summary>
    [CreateAssetMenu(fileName = "Inworld_Component", menuName = "Inworld/Create Component/Default", order = -1995)]
    public class InworldComponentAsset : ScriptableObject
    {
        [Header("Component Identity")] 
        [SerializeField] protected string m_ID = "component";
        [SerializeField] protected string m_Type = "Component";
        
        public string ID
        {
            get => string.IsNullOrEmpty(m_ID) ? name : m_ID;
            set => m_ID = value;
        }
        public string ComponentType
        {
            get => m_Type;
            protected set => m_Type = value;
        }
        
        public virtual ConfigData CreationConfig { get; set; }

        public virtual ComponentData ComponentData => new ComponentData
        {
            // YAN: For custom edges, it's like that.
            //      For the components, it's implemented in the child class.
            id = ID,
            type = ComponentType
        };
    }
}


