/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.IO;
using Inworld.Framework.Graph;
using Inworld.Framework.Node;
using UnityEngine;
using UnityEngine.Networking;

namespace Inworld.Framework.Primitive
{
    /// <summary>
    /// Abstract base class for all Inworld framework modules within Unity.
    /// Provides common functionality for module initialization, configuration, and lifecycle management.
    /// Manages the creation and coordination of factories, configurations, interfaces, and nodes.
    /// </summary>
    public abstract class InworldFrameworkModule : MonoBehaviour
    {
        [SerializeField] protected ModelType m_ModelType = ModelType.Remote;

        protected InworldFactory m_Factory;
        protected InworldConfig m_Config;
        protected InworldInterface m_Interface;

        protected InworldGraph m_Graph;
        protected InworldNode m_Node;
        
        protected bool m_Initialized;

        /// <summary>
        /// Event triggered when the module has been successfully initialized.
        /// </summary>
        public event Action OnInitialized;
        
        /// <summary>
        /// Event triggered when the module has been terminated or disposed.
        /// </summary>
        public event Action OnTeminated;
        
        /// <summary>
        /// Event triggered when a task has started execution.
        /// </summary>
        public event Action OnTaskStarted;
        
        /// <summary>
        /// Event triggered during task execution with status updates.
        /// </summary>
        public event Action<string> OnTask;
        
        /// <summary>
        /// Event triggered when a task has finished execution.
        /// </summary>
        public event Action<string> OnTaskFinished;
        
        /// <summary>
        /// Event triggered when a task has been cancelled.
        /// </summary>
        public event Action OnTaskCancelled;
        
        /// <summary>
        /// Gets or sets the model type for this module (Remote or Local).
        /// Determines whether the module uses cloud-based or on-device AI models.
        /// </summary>
        public ModelType ModelType
        {
            get => m_ModelType;
            set => m_ModelType = value;
        }
        
        /// <summary>
        /// Gets the active interface instance for this module.
        /// Returns null if the module is not initialized.
        /// </summary>
        public InworldInterface Interface => m_Interface;

        /// <summary>
        /// Creates and returns a factory instance specific to this module type.
        /// Must be implemented by derived classes to provide appropriate factory creation.
        /// </summary>
        /// <returns>A factory instance for creating module-specific objects.</returns>
        public abstract InworldFactory CreateFactory();
        
        /// <summary>
        /// Sets up and returns a configuration instance specific to this module type.
        /// Must be implemented by derived classes to provide appropriate configuration.
        /// </summary>
        /// <returns>A configuration instance for module initialization.</returns>
        public abstract InworldConfig SetupConfig();
        
        /// <summary>
        /// Gets the active graph instance associated with this module.
        /// </summary>
        public InworldGraph Graph => m_Graph;
        
        /// <summary>
        /// Gets the active node instance associated with this module.
        /// </summary>
        public InworldNode Node => m_Node;

        /// <summary>
        /// Gets or sets the initialization state of this module.
        /// When set to true, triggers OnInitialized event if interface is available.
        /// When set to false, cleans up interface and triggers OnTerminated event.
        /// </summary>
        public virtual bool Initialized
        {
            get => m_Initialized;
            set
            {
                if (m_Initialized == value)
                    return;
                m_Initialized = value;
                if (value)
                {
                    if (m_Interface == null)
                    {
                        Debug.LogError("Initialized Error: The Inworld Interface has not been initialized.");
                        return;
                    }
                    OnInitialized?.Invoke();
                }
                else
                {
                    m_Interface = null;
                    OnTeminated?.Invoke();
                }
            }
        }
        
        /// <summary>
        /// Notifies listeners that a task has started execution.
        /// </summary>
        public void NotifyTaskStart() => OnTaskStarted?.Invoke();
        
        /// <summary>
        /// Notifies listeners of task progress with a status message.
        /// </summary>
        /// <param name="text">The status message describing current task progress.</param>
        public void NotifyTask(string text) => OnTask?.Invoke(text);
        
        /// <summary>
        /// Notifies listeners that a task has finished execution.
        /// </summary>
        /// <param name="text">The completion message describing task results.</param>
        public void NotifyTaskEnd(string text) => OnTaskFinished?.Invoke(text);
        
        /// <summary>
        /// Cancels the current task and notifies listeners.
        /// </summary>
        public void CancelTask() => OnTaskCancelled?.Invoke();
        
        /// <summary>
        /// Initializes the module synchronously by creating factory, configuration, and interface.
        /// Sets up the complete module pipeline required for operation.
        /// </summary>
        /// <returns>True if initialization succeeded, false otherwise.</returns>
        public virtual bool Initialize()
        {
            m_Factory ??= CreateFactory();
            if (m_Factory == null)
            {
                Debug.LogError($"Failed to initialize Factory: {GetType().Name}");
                return false;
            }

            if (m_Config != null)
            {
                m_Config.Dispose();
                return false;
            }
                
            m_Config = SetupConfig();
            if (m_Config == null)
            {
                Debug.LogError($"Failed to initialize Config: {GetType().Name}");
                return false;
            }

            if (m_Interface != null)
            {
                m_Interface.Dispose();
                m_Interface = null;
            }
            m_Interface = m_Factory.CreateInterface(m_Config);
            Initialized = m_Interface != null;
            if (Initialized)
                Debug.Log($"[{GetType().Name}] Initialized Successfully.");
            else
                Debug.LogError($"Failed to initialize Interface: {GetType().Name}");
            return Initialized;
        }
        
        protected virtual async Awaitable MoveAssetsToDiskAsync()
        {
            string srcPath = Path.Combine(Application.streamingAssetsPath, InworldFrameworkUtil.SafetyWeightPath);
            string dstPath = Path.Combine(Application.persistentDataPath, InworldFrameworkUtil.SafetyWeightPath);

            if (File.Exists(dstPath))
                return; 

            using (var req = UnityWebRequest.Get(srcPath))
            {
                await req.SendWebRequest();

#if UNITY_2020_2_OR_NEWER
                if (req.result != UnityWebRequest.Result.Success)
#else
                if (req.isNetworkError || req.isHttpError)
#endif
                {
                    Debug.LogError($"Failed to load safety model from StreamingAssets: {req.error}\nPath: {srcPath}");
                    return;
                }
                if (!Directory.Exists(Application.persistentDataPath))
                    Directory.CreateDirectory(Application.persistentDataPath);
                if (!Directory.Exists(Application.persistentDataPath + "/safety"))
                    Directory.CreateDirectory(Application.persistentDataPath+ "/safety");
                File.WriteAllBytes(dstPath, req.downloadHandler.data);
                Debug.Log($"Safety model written to: {dstPath}");
            }
        }
        
        /// <summary>
        /// Initializes the module asynchronously by creating factory, configuration, and interface.
        /// Performs interface creation on a background thread for improved performance.
        /// </summary>
        /// <returns>A task that completes with true if initialization succeeded, false otherwise.</returns>
        public virtual async Awaitable<bool> InitializeAsync()
        {
            if (m_Factory == null)
            {
                m_Factory = CreateFactory();
                if (m_Factory == null)
                {
                    Debug.LogError($"Failed to initialize Factory: {GetType().Name}");
                    return false;
                }
            }
            if (m_Config != null)
            {
                m_Config.Dispose();
                m_Config = null;
            }
#if UNITY_ANDROID
            await MoveAssetsToDiskAsync();
#endif
            m_Config = SetupConfig(); 
            if (m_Config == null)
            {
                Debug.LogError($"Failed to initialize Config: {GetType().Name}");
                return false;
            }
            if (m_Interface != null)
            {
                m_Interface.Dispose();
                m_Interface = null;
            }
            await Awaitable.BackgroundThreadAsync();
            m_Interface = m_Factory.CreateInterface(m_Config);
            await Awaitable.MainThreadAsync();
            Initialized = m_Interface != null;
            if (Initialized)
                Debug.Log($"[{GetType().Name}] Initialized Successfully.");
            else
                Debug.LogError($"Failed to initialize Interface: {GetType().Name}");
            return Initialized;
        }
        
        protected virtual void OnDestroy()
        {
            m_Interface?.Dispose();
            m_Config?.Dispose();
            m_Factory?.Dispose();
        }
    }
}