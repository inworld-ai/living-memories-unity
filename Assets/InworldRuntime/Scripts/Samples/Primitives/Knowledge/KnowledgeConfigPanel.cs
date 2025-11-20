/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using Inworld.Framework.Knowledge;
using Inworld.Framework.TextEmbedder;
using Inworld.Framework.Samples.UI;
using TMPro;
using UnityEngine;


namespace Inworld.Framework.Samples
{
    /// <summary>
    /// Sample UI panel for configuring knowledge base settings within the Inworld framework.
    /// Provides an interface for setting chunking parameters, connecting to the framework,
    /// and compiling knowledge data for use in AI interactions.
    /// Integrates with the knowledge module to manage text processing and embedding configurations.
    /// Serves as a reference implementation for building knowledge management interfaces.
    /// </summary>
    public class KnowledgeConfigPanel : MonoBehaviour
    {
        const int k_DefaultMaxCharsPerChunk = 200;
        const int k_DefaultMaxChunksPerDoc = 100;

        [SerializeField] KnowledgeData m_KnowledgeData;
        [SerializeField] SwitchButton m_ConnectButton;
        [SerializeField] TMP_InputField m_InputMaxChars;
        [SerializeField] TMP_InputField m_InputMaxChunks;

        List<InworldUIElement> m_UIElements = new List<InworldUIElement>();
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            m_UIElements.AddRange(GetComponentsInChildren<InworldUIElement>(true));
            ResetDefault();
        }
        
        void OnEnable()
        {
            InworldController.Instance.OnFrameworkInitialized += OnFrameworkInitialized;
        }

        void OnDisable()
        {
            if (InworldController.Instance)
                InworldController.Instance.OnFrameworkInitialized -= OnFrameworkInitialized;
        }

        /// <summary>
        /// Initiates connection to the Inworld framework and disables UI during initialization.
        /// Triggers the asynchronous initialization process for all framework components.
        /// </summary>
        public void Connect()
        {
            foreach (InworldUIElement element in m_UIElements)
                element.Interactable = false;
            InworldController.Instance.InitializeAsync();
        }

        /// <summary>
        /// Sets the maximum number of characters per knowledge chunk.
        /// Controls how text documents are divided into smaller, manageable pieces for processing.
        /// </summary>
        /// <param name="strMaxCharsPerChunk">String representation of the maximum characters per chunk.</param>
        public void SetMaxCharsPerChunk(string strMaxCharsPerChunk)
        {
            if (InworldController.Knowledge && int.TryParse(strMaxCharsPerChunk, out int nMaxCharsPerChunk))
                InworldController.Knowledge.MaxCharsPerChunk = nMaxCharsPerChunk;
        }
        
        /// <summary>
        /// Sets the maximum number of chunks per document.
        /// Limits how many chunks a single document can be divided into during processing.
        /// </summary>
        /// <param name="strMaxChunksPerDoc">String representation of the maximum chunks per document.</param>
        public void SetMaxChunksPerDoc(string strMaxChunksPerDoc)
        {
            if (InworldController.Knowledge && int.TryParse(strMaxChunksPerDoc, out int nMaxChunksPerDoc))
                InworldController.Knowledge.MaxChunksPerDoc = nMaxChunksPerDoc;
        }
        
        /// <summary>
        /// Compiles the knowledge data for use in AI interactions.
        /// Processes and prepares knowledge entries for embedding and retrieval operations.
        /// Only executes if the knowledge module is properly initialized.
        /// </summary>
        public void CompileKnowledge()
        {
            if (InworldController.Knowledge?.Initialized ?? false)
                m_KnowledgeData.CompileKnowledges();
        }

        /// <summary>
        /// Resets configuration values to their default settings.
        /// Restores the maximum characters per chunk and maximum chunks per document to predefined values.
        /// </summary>
        public void ResetDefault()
        {
            if (m_InputMaxChars)
                m_InputMaxChars.text = k_DefaultMaxCharsPerChunk.ToString();
            
            if (m_InputMaxChunks)
                m_InputMaxChunks.text = k_DefaultMaxChunksPerDoc.ToString();
        }
        
        void OnFrameworkInitialized()
        {
            if (m_ConnectButton)
                m_ConnectButton.Switch(true);
            foreach (InworldUIElement element in m_UIElements)
                element.Interactable = true;
            if (!InworldController.Knowledge || !m_KnowledgeData || !InworldController.TextEmbedder || !m_KnowledgeData) 
                return;
            m_KnowledgeData.CompileKnowledges();
        }
    }
}
