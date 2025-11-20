
/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using Inworld.Framework.Event;
using Inworld.Framework.Knowledge;
using Inworld.Framework.Samples.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Framework.Samples
{
    /// <summary>
    /// Sample UI panel for interacting with the knowledge base within the Inworld framework.
    /// Provides an interface for adding new knowledge entries and querying existing knowledge.
    /// Supports both knowledge addition and retrieval operations with visual feedback through bubble UI elements.
    /// Integrates with the knowledge module to demonstrate knowledge management and search capabilities.
    /// Serves as a reference implementation for building knowledge interaction interfaces.
    /// </summary>
    public class KnowledgeInteractionPanel : MonoBehaviour
    {
        [SerializeField] KnowledgeData m_KnowledgeData;
        [SerializeField] RectTransform m_ContentAnchor;
        [SerializeField] TMP_InputField m_InputContent;
        [SerializeField] SwitchButton m_AddQuery;
        [SerializeField] KnowledgeBubble m_Bubble;
        [SerializeField] KnowledgeBubble m_ResultBubble;
        [SerializeField] string m_ResultTitle = "Knowledge Found";
        protected readonly List<KnowledgeBubble> m_Bubbles = new List<KnowledgeBubble>();
        List<InworldUIElement> m_UIElements = new List<InworldUIElement>();
        
        void OnEnable()
        {
            m_UIElements.AddRange(GetComponentsInChildren<InworldUIElement>(true));
            InworldController.Knowledge.OnKnowledgeCompiled += OnKnowledgeCompiled;
            InworldController.Knowledge.OnKnowledgeRemoved += OnKnowledgeRemoved;
            InworldController.Knowledge.OnKnowledgeRespond += OnKnowledgeRespond;
        }
        void OnDisable()
        {
            if (InworldController.Instance && InworldController.Knowledge)
            {
                InworldController.Knowledge.OnKnowledgeCompiled -= OnKnowledgeCompiled;
                InworldController.Knowledge.OnKnowledgeRemoved -= OnKnowledgeRemoved;
                InworldController.Knowledge.OnKnowledgeRespond -= OnKnowledgeRespond;
            }
        }

        void Update()
        {
            if (!m_AddQuery || !Input.GetKeyUp(KeyCode.Return)) 
                return;
            AddOrQueryKnowledge();
        }

        /// <summary>
        /// Opens the input panel for adding new knowledge entries.
        /// Enables all UI elements and switches the interface to knowledge addition mode.
        /// </summary>
        public void OpenAddInputPanel()
        {
            foreach (InworldUIElement element in m_UIElements)
                element.Interactable = true;
            m_AddQuery.Switch(false);
        }

        /// <summary>
        /// Opens the input panel for querying existing knowledge.
        /// Enables all UI elements and switches the interface to knowledge query mode.
        /// </summary>
        public void OpenQueryInputPanel()
        {
            foreach (InworldUIElement element in m_UIElements)
                element.Interactable = true;
            m_AddQuery.Switch(true);
        }

        void OnKnowledgeRemoved(string knowledgeID)
        {
            _RemoveBubbles(knowledgeID);
        }
        void OnKnowledgeRespond(List<string> data)
        {
            _ClearScreen();
            foreach (string result in data)
            {
                InsertBubble(m_ResultBubble, m_ResultTitle, result);
            }
        }
        void OnKnowledgeCompiled(string knowledgeID, List<string> knowledges)
        {
            _ClearScreen(knowledgeID);
            foreach (string knowledge in knowledges)
            {
                InsertBubble(m_Bubble, knowledgeID, knowledge);
            }
        }
        protected virtual void InsertBubble(KnowledgeBubble bubble, string knowledgeID, string content)
        {
            KnowledgeBubble outBubble = Instantiate(bubble, m_ContentAnchor);
            outBubble.SetBubble(knowledgeID, content);
            m_Bubbles.Add(outBubble);
            _UpdateContent();
        }
        
        void _UpdateContent() => LayoutRebuilder.ForceRebuildLayoutImmediate(m_ContentAnchor);
        void _ClearScreen(string knowledgeID = null)
        {
            if (string.IsNullOrEmpty(knowledgeID))
            {
                foreach (KnowledgeBubble bubble in m_Bubbles)
                {
                    DestroyImmediate(bubble.gameObject);
                }
                m_Bubbles.Clear();
            }
            else
            {
                foreach (KnowledgeBubble bubble in m_Bubbles)
                {
                    if (bubble.Title == knowledgeID || bubble.Title == m_ResultTitle)
                        DestroyImmediate(bubble.gameObject);
                }
                m_Bubbles.RemoveAll(k => k.Title == knowledgeID || k.Title == m_ResultTitle);
            }
        }
        void _RemoveBubbles(string knowledgeID)
        {
            foreach (KnowledgeBubble bubble in m_Bubbles.Where(bubble => knowledgeID == bubble.Title))
            {
                DestroyImmediate(bubble.gameObject);
            }
            m_Bubbles.RemoveAll(k => k.Title == knowledgeID);
        }
        void _QueryKnowledge()
        {
            InworldEvent inworldEvent = new InworldEvent();
            InworldAgentSpeech speech = inworldEvent.Speech;
            speech.AgentName = InworldFrameworkUtil.PlayerName;
            speech.Utterance = m_InputContent.text;
            if (InworldController.Knowledge)
                InworldController.Knowledge.GetKnowledges(m_KnowledgeData.IDs, new List<InworldEvent>{inworldEvent});
        }
        void _AddKnowledge()
        {
            if (m_KnowledgeData)
                m_KnowledgeData.AddKnowledge("test", m_InputContent.text);
        }
        
        /// <summary>
        /// Performs the currently selected operation (add or query) based on the switch button state.
        /// Either adds new knowledge or queries existing knowledge depending on the interface mode.
        /// Clears the input field after the operation is completed.
        /// </summary>
        public void AddOrQueryKnowledge()
        {
            if (!m_AddQuery)
                return;
            if (m_AddQuery.IsOn) // QUERY
                _QueryKnowledge();
            else
                _AddKnowledge();
            m_InputContent.text = "";
        }

        /// <summary>
        /// Clears all knowledge display bubbles from the interface.
        /// Removes all visual representations of knowledge entries and query results.
        /// </summary>
        public void ClearKnowledge()
        {
            _ClearScreen();
        }
    }
}