/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Runtime.InteropServices;

namespace Inworld.Framework
{
    /// <summary>
    /// The basic class for inworld.dll
    /// All the c++ classes are translated into this class.
    /// All the c++ classes should inherit this class.
    /// </summary>
    public abstract class InworldFrameworkDllClass : IDisposable
    {
        protected IntPtr m_DLLPtr;
        /// <summary>
        /// Gets the underlying native pointer associated with this managed wrapper.
        /// </summary>
        /// <value>The <see cref="IntPtr"/> to the native object.</value>
        public IntPtr ToDLL => m_DLLPtr;
        /// <summary>
        /// Gets a value indicating whether the native pointer is valid (non-zero).
        /// </summary>
        /// <value><c>true</c> if the native pointer is non-zero; otherwise, <c>false</c>.</value>
        public virtual bool IsValid => m_DLLPtr != IntPtr.Zero;
        /// <summary>
        /// Releases unmanaged resources by unregistering the native pointer from the memory manager.
        /// </summary>
        public virtual void Dispose() => MemoryManager.UnRegister(m_DLLPtr);
    }
    
    /// <summary>
    /// Basic class for all the configs.
    /// i.e. RemoteConfig/LocalConfig/CreationConfig/ExecutionConfig/etc.
    /// </summary>
    public abstract class InworldConfig : InworldFrameworkDllClass
    {

    }

    /// <summary>
    /// Abstract class for the text filters.
    /// </summary>
    public abstract class InworldRemover : InworldFrameworkDllClass
    {
        /// <summary>
        /// Gets a value indicating whether there is another filtered item available.
        /// </summary>
        /// <value><c>true</c> if more items can be read; otherwise, <c>false</c>.</value>
        public abstract bool HasNext { get; }
        /// <summary>
        /// Gets the current filtered text result.
        /// </summary>
        /// <value>The filtered string produced by the remover.</value>
        public abstract string Result { get; }
        /// <summary>
        /// Creates an input stream view over this remover that yields string results.
        /// </summary>
        /// <value>An <see cref="InworldInputStream{T}"/> that streams <see cref="string"/> values.</value>
        public InworldInputStream<string> ToInputStream => new InworldInputStream<string>(m_DLLPtr);
    }
    
    /// <summary>
    /// Static Factory class. Use to create interfaces.
    /// </summary>
    public abstract class InworldFactory : InworldFrameworkDllClass
    {
        /// <summary>
        /// Creates a new interface instance using the specified configuration.
        /// </summary>
        /// <param name="config">The configuration used to initialize the interface.</param>
        /// <returns>An initialized <see cref="InworldInterface"/>.</returns>
        public abstract InworldInterface CreateInterface(InworldConfig config);
    }
    
    /// <summary>
    /// Interface classes. Use it to call the actual API functions.
    /// Or use it to create node (Currently)
    /// </summary>
    public abstract class InworldInterface : InworldFrameworkDllClass
    {
        
    }

    /// <summary>
    /// Abstract base class for executing operations in the Inworld framework.
    /// Provides managed-to-native interop capabilities and handles garbage collection for callback delegates.
    /// Uses GCHandle to prevent managed objects from being collected while referenced by native code.
    /// </summary>
    public abstract class InworldExecutor : InworldFrameworkDllClass
    {
        /// <summary>
        /// Handle to prevent this managed object from being garbage collected while referenced by native code.
        /// </summary>
        protected GCHandle m_Self;
    }
    /// <summary>
    /// Abstract base class for local AI model configurations.
    /// Used when running AI models locally on the device rather than using cloud services.
    /// Contains settings for local model files, device selection, and processing configurations.
    /// </summary>
    public abstract class InworldLocalConfig : InworldConfig
    {
        /// <summary>
        /// Gets or sets the file path to the local AI model.
        /// </summary>
        /// <value>The path to the model file on the local filesystem.</value>
        public abstract string ModelPath { get; set; }
        
        /// <summary>
        /// Gets or sets the computing device to use for local model processing.
        /// </summary>
        /// <value>The device configuration specifying CPU, GPU, or other acceleration options.</value>
        public abstract InworldDevice Device { get; set; }
        
        /// <summary>
        /// Gets or sets the underlying configuration object for the local model.
        /// </summary>
        /// <value>The configuration object containing model-specific settings.</value>
        public abstract InworldConfig Config { get; set; }
    }
    
    /// <summary>
    /// Abstract base class for remote AI service configurations.
    /// Used when connecting to cloud-based AI services rather than running models locally.
    /// Contains authentication and connection settings for remote API access.
    /// </summary>
    public abstract class InworldRemoteConfig : InworldConfig
    {
        /// <summary>
        /// Gets or sets the API key for authenticating with the remote service.
        /// </summary>
        /// <value>The API key string required for service authentication.</value>
        public abstract string APIKey { get; set; }
        
        /// <summary>
        /// Gets or sets the underlying configuration object for the remote service.
        /// </summary>
        /// <value>The configuration object containing service-specific settings.</value>
        public abstract InworldConfig Config { get; set; }
    }
    
    /// <summary>
    /// Abstract base class for streaming data readers that provide sequential access to data.
    /// Implements a forward-only iterator pattern for processing data streams from native DLL functions.
    /// </summary>
    /// <typeparam name="T">The type of data provided by this stream.</typeparam>
    public abstract class InworldStream<T> : InworldFrameworkDllClass
    {
        /// <summary>
        /// Gets a value indicating whether there are more items available to read from the stream.
        /// </summary>
        /// <value>True if more data is available; otherwise, false.</value>
        public abstract bool HasNext { get; }
        
        /// <summary>
        /// Reads the next item from the stream and advances the stream position.
        /// </summary>
        /// <returns>The next item of type T from the stream.</returns>
        public abstract T Read();
    }
}