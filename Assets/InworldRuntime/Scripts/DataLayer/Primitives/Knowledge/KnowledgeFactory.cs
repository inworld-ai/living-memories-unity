/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.TextEmbedder;
using UnityEngine;

namespace Inworld.Framework.Knowledge
{
    /// <summary>
    /// Factory class for creating knowledge interfaces within the Inworld framework.
    /// Manages the creation and initialization of knowledge components for information retrieval and processing.
    /// Used for instantiating knowledge interfaces with proper configuration and text embedding support.
    /// </summary>
    public class KnowledgeFactory : InworldFactory
    {
        TextEmbedderInterface m_TextEmbedder;
        
        /// <summary>
        /// Initializes a new instance of the KnowledgeFactory class with default settings.
        /// </summary>
        public KnowledgeFactory()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_KnowledgeFactory_new(), 
                InworldInterop.inworld_KnowledgeFactory_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the KnowledgeFactory class from a native pointer.
        /// </summary>
        /// <param name="ptr">The native pointer to the knowledge factory object.</param>
        public KnowledgeFactory(IntPtr ptr) => m_DLLPtr = MemoryManager.Register(ptr, InworldInterop.inworld_KnowledgeFactory_delete);
        
        /// <summary>
        /// Creates a knowledge interface instance using the provided configuration.
        /// Instantiates and configures a knowledge interface for information retrieval.
        /// Requires a valid text embedder interface to be set up beforehand.
        /// </summary>
        /// <param name="config">The configuration settings for the knowledge interface. Must be either KnowledgeRemoteConfig or KnowledgeLocalConfig.</param>
        /// <returns>A KnowledgeInterface instance if creation succeeds; otherwise, null.</returns>
        public override InworldInterface CreateInterface(InworldConfig config)
        {
            if (m_TextEmbedder == null || !m_TextEmbedder.IsValid)
            {
                Debug.LogError("Need to have a TextEmbedderInterface attached first.");
                return null;
            }
            if (config is KnowledgeRemoteConfig remoteConfig)
                return CreateInterface(remoteConfig);
            if (config is KnowledgeLocalConfig localConfig)
                return CreateInterface(localConfig); 
            return null;
        }

        /// <summary>
        /// Sets up the text embedder interface required for knowledge processing.
        /// The text embedder is used to convert text into vector embeddings for similarity search.
        /// </summary>
        /// <param name="textEmbedder">The text embedder interface to use for knowledge operations.</param>
        public void SetupEmbedder(TextEmbedderInterface textEmbedder) => m_TextEmbedder = textEmbedder; 
        
        //TODO(Yan): Replace optionalEmbedder to CreationContext.
        /// <summary>
        /// Creates a knowledge interface using remote configuration settings.
        /// Configures the knowledge system to use remote knowledge services.
        /// </summary>
        /// <param name="remoteConfig">The remote configuration for knowledge processing.</param>
        /// <returns>A KnowledgeInterface instance if creation succeeds; otherwise, null.</returns>
        InworldInterface CreateInterface(KnowledgeRemoteConfig remoteConfig)
        {
            ComponentStore componentStore = new ComponentStore();
            componentStore.AddTextEmbedderInterface("textEmbedder", m_TextEmbedder);
            InworldCreationContext inworldCreationContext = new InworldCreationContext(componentStore);
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_KnowledgeFactory_CreateKnowledge_rcinworld_CreationContext_rcinworld_RemoteKnowledgeConfig
                    (m_DLLPtr, inworldCreationContext.ToDLL, remoteConfig.ToDLL),
                InworldInterop.inworld_StatusOr_KnowledgeInterface_status,
                InworldInterop.inworld_StatusOr_KnowledgeInterface_ok,
                InworldInterop.inworld_StatusOr_KnowledgeInterface_value,
                InworldInterop.inworld_StatusOr_KnowledgeInterface_delete
            );
            return result != IntPtr.Zero ? new KnowledgeInterface(result) : null;
        }
        
        //TODO(Yan): Replace optionalEmbedder to CreationContext.
        /// <summary>
        /// Creates a knowledge interface using local configuration settings.
        /// Configures the knowledge system to use local knowledge processing.
        /// </summary>
        /// <param name="localConfig">The local configuration for knowledge processing.</param>
        /// <returns>A KnowledgeInterface instance if creation succeeds; otherwise, null.</returns>
        InworldInterface CreateInterface(KnowledgeLocalConfig localConfig)
        {
            ComponentStore componentStore = new ComponentStore();
            localConfig.EmbedderComponentID = "textEmbedder";
            componentStore.AddTextEmbedderInterface("textEmbedder", m_TextEmbedder);
            InworldCreationContext inworldCreationContext = new InworldCreationContext(componentStore);
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_KnowledgeFactory_CreateKnowledge_rcinworld_CreationContext_rcinworld_LocalKnowledgeConfig
                    (m_DLLPtr, inworldCreationContext.ToDLL, localConfig.ToDLL),
                InworldInterop.inworld_StatusOr_KnowledgeInterface_status,
                InworldInterop.inworld_StatusOr_KnowledgeInterface_ok,
                InworldInterop.inworld_StatusOr_KnowledgeInterface_value,
                InworldInterop.inworld_StatusOr_KnowledgeInterface_delete
            );
            return result != IntPtr.Zero ? new KnowledgeInterface(result) : null;
        }
    }
}