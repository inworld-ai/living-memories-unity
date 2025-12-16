/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Framework.Attributes;
using UnityEngine;
using Util = Inworld.Framework.InworldFrameworkUtil;
using Inworld.Framework.TextEmbedder;


namespace Inworld.Framework.Primitive
{
    /// <summary>
    /// Module for text embedding functionality within the Inworld framework.
    /// Converts text into numerical vector representations for semantic analysis and similarity comparisons.
    /// Essential for knowledge retrieval, safety checking, and other AI operations that require text understanding.
    /// Supports both remote API-based and local model-based text embedding.
    /// </summary>
    [ModelType("Remote", ExcludeTargets = new[] { "StandaloneWindows", "StandaloneWindows64" })]
    public class TextEmbedderModule: InworldFrameworkModule
    {
        [SerializeField] string m_RemoteProvider = "inworld";
        [SerializeField] string m_RemoteModelName = "BAAI/bge-large-en-v1.5";
        /// <summary>
        /// Creates and returns a TextEmbedderFactory for this module.
        /// </summary>
        /// <returns>A factory instance for creating text embedding objects.</returns>
        public override InworldFactory CreateFactory() => m_Factory ??= new TextEmbedderFactory();

        /// <summary>
        /// Sets up the configuration for text embedding operations.
        /// Configures model paths, API settings, and provider information based on the selected model type.
        /// </summary>
        /// <returns>A TextEmbedderCreationConfig instance for module initialization.</returns>
        public override InworldConfig SetupConfig()
        {
            TextEmbedderCreationConfig creationConfig = new TextEmbedderCreationConfig();
            if (ModelType == ModelType.Remote)
            {
                TextEmbedderRemoteConfig remoteConfig = new TextEmbedderRemoteConfig();
                if (!string.IsNullOrEmpty(Util.APIKey))
                    remoteConfig.APIKey = Util.APIKey;
                remoteConfig.Provider = m_RemoteProvider;
                remoteConfig.ModelName = m_RemoteModelName;
                creationConfig.RemoteConfig = remoteConfig;
                return creationConfig;
            }
            TextEmbedderLocalConfig localConfig = new TextEmbedderLocalConfig();
            localConfig.ModelPath = $"{Application.streamingAssetsPath}/{Util.TextEmbedderPath}";
            localConfig.Device = Util.GetDevice(ModelType);
            creationConfig.LocalConfig = localConfig;
            return creationConfig;
        }
    }
}