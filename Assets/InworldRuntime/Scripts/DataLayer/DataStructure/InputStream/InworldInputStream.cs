/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using Inworld.Framework.TTS;
using UnityEngine;

namespace Inworld.Framework
{
    /// <summary>
    /// Defines the interface for implementing InputStream strategies that bridge C# generic types to native C++ stream implementations.
    /// Provides a contract for managing and operating on native input stream data structures that deliver sequential data.
    /// </summary>
    /// <typeparam name="T">The type of data provided by this input stream.</typeparam>
    public interface IInputStreamStrategy<T>
    {
        /// <summary>
        /// Deletes a native input stream instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream to delete.</param>
        void Delete(IntPtr ptr);
        
        /// <summary>
        /// Determines whether the input stream contains valid data.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream.</param>
        /// <returns>True if the input stream is valid and functional; otherwise, false.</returns>
        bool IsValid(IntPtr ptr);
        
        /// <summary>
        /// Determines whether there are more items available to read from the input stream.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream.</param>
        /// <returns>True if more data is available; otherwise, false.</returns>
        bool HasNext(IntPtr ptr);
        
        /// <summary>
        /// Reads the next item from the input stream and advances the stream position.
        /// </summary>
        /// <param name="ptr">The native pointer to the input stream.</param>
        /// <returns>The next item of type T from the stream.</returns>
        T Read(IntPtr ptr);
    }
    
    /// <summary>
    /// Generic input stream implementation that provides managed access to native C++ input stream data structures.
    /// Uses a strategy pattern to support different data types through specialized implementations.
    /// Supports sequential reading of data elements from various sources within the Inworld framework.
    /// </summary>
    /// <typeparam name="T">The type of data provided by this input stream.</typeparam>
    public class InworldInputStream<T> : InworldStream<T>
    {
        static readonly Dictionary<Type, object> s_Strategies = new Dictionary<Type, object>
        {
            { typeof(string), new StringInputStreamStrategy() },
            { typeof(InworldBaseData), new BaseDataInputStreamStrategy() },
            { typeof(InworldContent), new ContentInputStreamStrategy() },
            { typeof(SpeechChunk), new SpeechChunkInputStreamStrategy() },
            { typeof(TTSOutput), new TTSOutputInputStreamStrategy() }
        };
        
        static IInputStreamStrategy<T> Strategy 
        {
            get
            {
                if (s_Strategies.TryGetValue(typeof(T), out object strategy)) 
                    return (IInputStreamStrategy<T>)strategy;
                Debug.LogError($"Type {typeof(T)} is not supported");
                return null;
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the InworldInputStream class from an existing native pointer.
        /// Used for wrapping existing native input stream objects created by the C++ library.
        /// </summary>
        /// <param name="rhs">Native pointer to an existing input stream instance.</param>
        public InworldInputStream(IntPtr rhs)
        {
            IInputStreamStrategy<T> currentStrategy = Strategy;
            if (currentStrategy == null)
                return;
            m_DLLPtr = MemoryManager.Register(rhs, currentStrategy.Delete);
        }

        /// <summary>
        /// Gets a value indicating whether this input stream contains valid data and is functional.
        /// </summary>
        /// <value>True if the input stream is valid; otherwise, false.</value>
        public override bool IsValid => Strategy?.IsValid(m_DLLPtr) ?? false;
        
        /// <summary>
        /// Gets a value indicating whether there are more items available to read from the input stream.
        /// </summary>
        /// <value>True if more data is available; otherwise, false.</value>
        public override bool HasNext => Strategy?.HasNext(m_DLLPtr) ?? false;

        /// <summary>
        /// Reads the next item from the input stream and advances the stream position.
        /// </summary>
        /// <returns>The next item of type T from the stream, or the default value if the stream is invalid.</returns>
        public override T Read()
        {
            IInputStreamStrategy<T> currentStrategy = Strategy;
            if (currentStrategy == null)
                return default;
            return Strategy.Read(m_DLLPtr);
        }
    }
}