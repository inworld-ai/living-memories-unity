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
    /// ScriptableObject that defines creation configuration settings for AI models within the Inworld framework.
    /// This asset can be created through Unity's Create menu and used to configure model deployment options.
    /// Used for specifying whether to use remote or local AI models and their associated parameters.
    /// </summary>
    [CreateAssetMenu(fileName = "New Creation Config", menuName = "Inworld/Creation Config", order = 0)]
    public class CreationConfigAsset : ScriptableObject
    {
        /// <summary>
        /// The type of model deployment to use (Remote or Local).
        /// Determines whether the AI model runs on remote servers or locally on the device.
        /// </summary>
        public ModelType modelType = ModelType.Remote;
        
        /// <summary>
        /// The provider name for remote model services.
        /// Specifies which AI service provider to use when modelType is set to Remote.
        /// </summary>
        [Header("Remote:")] 
        public string provider;
        
        /// <summary>
        /// The specific model name to use with the remote provider.
        /// Identifies the particular AI model variant to deploy from the provider's catalog.
        /// </summary>
        public string modelName;
        
        /// <summary>
        /// The file path to the local model when using local deployment.
        /// Specifies the location of the AI model files on the local filesystem for local execution.
        /// </summary>
        [Header("Local")]
        public string modelPath;
    }
}