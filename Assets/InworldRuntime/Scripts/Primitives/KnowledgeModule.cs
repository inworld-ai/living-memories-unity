/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/
using System;
using System.Collections.Generic;
using Inworld.Framework.Attributes;
using Inworld.Framework.Event;
using Inworld.Framework.Knowledge;
using Inworld.Framework.TextEmbedder;
using UnityEngine;
using Util = Inworld.Framework.InworldFrameworkUtil;


namespace Inworld.Framework.Primitive
{
    /// <summary>
    /// Module for managing knowledge bases and information retrieval within the Inworld framework.
    /// Provides functionality for compiling, storing, and querying knowledge content for AI interactions.
    /// Integrates with text embedding services for semantic search and knowledge matching.
    /// </summary>
    [ModelType("Remote", ExcludeTargets = new[] { "StandaloneWindows", "StandaloneWindows64" })]
    public class KnowledgeModule : InworldFrameworkModule
    {
        [SerializeField] int m_MaxCharsPerChunk = 200;
        [SerializeField] int m_MaxChunksPerDoc = 100;

        /// <summary>
        /// Event triggered when knowledge content has been removed from the knowledge base.
        /// </summary>
        public event Action<string> OnKnowledgeRemoved;
        
        /// <summary>
        /// Event triggered when knowledge content has been compiled and chunked.
        /// Provides the knowledge ID and the resulting list of compiled chunks.
        /// </summary>
        public event Action<string, List<string>> OnKnowledgeCompiled;
        
        /// <summary>
        /// Event triggered when knowledge retrieval responds with relevant information.
        /// Provides the list of knowledge chunks that match the query.
        /// </summary>
        public event Action<List<string>> OnKnowledgeRespond;

        /// <summary>
        /// Gets the compile configuration for knowledge processing.
        /// Returns null if the module is not properly configured.
        /// </summary>
        public KnowledgeCompileConfig CompileConfig
        {
            get
            {
                if (m_Config == null)
                    return null;
                InworldConfig config = m_Config is KnowledgeLocalConfig localConfig ? localConfig.Config 
                    : m_Config is KnowledgeRemoteConfig remoteConfig ? remoteConfig.Config : null;
                if (config is KnowledgeCompileConfig compileConfig)
                    return compileConfig;
                return null;
            }
        }
        
        /// <summary>
        /// Gets or sets the maximum number of characters per knowledge chunk.
        /// Affects how knowledge content is divided during compilation.
        /// </summary>
        public int MaxCharsPerChunk
        {
            get => m_MaxCharsPerChunk;
            set
            {
                m_MaxCharsPerChunk = value;
                if (CompileConfig?.ParsingConfig != null) 
                    CompileConfig.ParsingConfig.MaxCharsPerChunk = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of chunks per document.
        /// Limits the granularity of knowledge segmentation during compilation.
        /// </summary>
        public int MaxChunksPerDoc
        {
            get => m_MaxChunksPerDoc;
            set
            {
                m_MaxChunksPerDoc = value;
                if (CompileConfig?.ParsingConfig != null) 
                    CompileConfig.ParsingConfig.MaxChunksPerDoc = value;
            }
        }
        
        /// <summary>
        /// Sets up the text embedder interface for semantic knowledge processing.
        /// Required for advanced knowledge matching and retrieval operations.
        /// </summary>
        /// <param name="textEmbedderInterface">The text embedder interface to use for knowledge processing.</param>
        public void SetupEmbedder(TextEmbedderInterface textEmbedderInterface)
        {
            m_Factory ??= new KnowledgeFactory();
            if (m_Factory is KnowledgeFactory knowledgeFactory)
                knowledgeFactory.SetupEmbedder(textEmbedderInterface);
        }

        /// <summary>
        /// Removes knowledge content from the knowledge base.
        /// Triggers OnKnowledgeRemoved event if the removal is successful.
        /// </summary>
        /// <param name="knowledgeID">The unique identifier of the knowledge to remove.</param>
        /// <returns>True if the knowledge was successfully removed, false otherwise.</returns>
        public bool RemoveKnowledge(string knowledgeID)
        {
            if (!Initialized || !(m_Interface is KnowledgeInterface knowledgeInterface))
                return false;
            bool result = knowledgeInterface.RemoveKnowledge(knowledgeID);
            if (result)
                OnKnowledgeRemoved?.Invoke(knowledgeID);
            return result;
        }
        
        /// <summary>
        /// Compiles raw knowledge content into processable chunks.
        /// Breaks down large knowledge documents into smaller, searchable segments.
        /// Triggers OnKnowledgeCompiled event with the resulting chunks.
        /// </summary>
        /// <param name="knowledgeID">The unique identifier for this knowledge set.</param>
        /// <param name="knowledges">List of raw knowledge strings to compile.</param>
        /// <returns>List of compiled knowledge chunks, or null if compilation failed.</returns>
        public List<string> CompileKnowledges(string knowledgeID, List<string> knowledges)
        {
            if (!Initialized || !(m_Interface is KnowledgeInterface knowledgeInterface))
                return null;
            InworldVector<string> inputKnowledges = new InworldVector<string>();
            inputKnowledges.AddRange(knowledges);
            InworldVector<string> outputKnowledges = knowledgeInterface.CompileKnowledge(knowledgeID, inputKnowledges);
            int nSize = outputKnowledges?.Size ?? -1;
            List<string> result = new List<string>();
            for (int i = 0; i < nSize; i++)
                result.Add(outputKnowledges?[i]);
            OnKnowledgeCompiled?.Invoke(knowledgeID, result);
            return result;
        }

        /// <summary>
        /// Retrieves relevant knowledge content based on provided knowledge IDs and optional event history.
        /// Performs semantic matching to find the most relevant knowledge for the current context.
        /// Triggers OnKnowledgeRespond event with the retrieved knowledge.
        /// </summary>
        /// <param name="knowledgeIDs">List of knowledge identifiers to search within.</param>
        /// <param name="eventHistory">Optional conversation history to provide context for knowledge retrieval.</param>
        /// <returns>List of relevant knowledge strings, or null if retrieval failed.</returns>
        public List<string> GetKnowledges(List<string> knowledgeIDs, List<InworldEvent> eventHistory = null)
        {
            if (!Initialized || !(m_Interface is KnowledgeInterface knowledgeInterface))
                return null;
            InworldVector<string> inputKnowledges = new InworldVector<string>();
            inputKnowledges.AddRange(knowledgeIDs);
            InworldVector<InworldEvent> inputEvents = new InworldVector<InworldEvent>();
            if (eventHistory != null && eventHistory.Count != 0)
                inputEvents.AddRange(eventHistory);
            InworldVector<string> output = knowledgeInterface.GetKnowledge(inputKnowledges, inputEvents);
            int nSize = output?.Size ?? -1;
            List<string> result = new List<string>();
            for (int i = 0; i < nSize; i++)
                result.Add(output?[i]);
            OnKnowledgeRespond?.Invoke(result);
            return result;
        }
        
        /// <summary>
        /// Creates and returns a KnowledgeFactory for this module.
        /// </summary>
        /// <returns>A factory instance for creating knowledge processing objects.</returns>
        public override InworldFactory CreateFactory() => m_Factory ??= new KnowledgeFactory();

        /// <summary>
        /// Sets up the configuration for knowledge processing operations.
        /// Configures text embedder integration and processing parameters based on model type.
        /// </summary>
        /// <returns>A knowledge configuration instance for module initialization.</returns>
        public override InworldConfig SetupConfig()
        {
            if (!InworldController.TextEmbedder || !InworldController.TextEmbedder.Initialized) 
                return null;
            InworldController.Knowledge.SetupEmbedder(InworldController.TextEmbedder.Interface as TextEmbedderInterface);

            if (ModelType == ModelType.Remote)
            {
                KnowledgeRemoteConfig remoteConfig = new KnowledgeRemoteConfig();
                if (!string.IsNullOrEmpty(Util.APIKey))
                    remoteConfig.APIKey = Util.APIKey;
                remoteConfig.Config = _SetupCompileConfig();
                return remoteConfig;
            }
            KnowledgeLocalConfig localConfig = new KnowledgeLocalConfig();
            localConfig.Config = _SetupCompileConfig();
            return localConfig;
        }

        InworldConfig _SetupCompileConfig()
        {
            InworldParsingConfig parsingConfig = new InworldParsingConfig();
            parsingConfig.MaxCharsPerChunk = m_MaxCharsPerChunk;
            parsingConfig.MaxChunksPerDoc = m_MaxChunksPerDoc;
            KnowledgeCompileConfig compileConfig = new KnowledgeCompileConfig();
            compileConfig.ParsingConfig = parsingConfig;
            return compileConfig;
        }
    }
}