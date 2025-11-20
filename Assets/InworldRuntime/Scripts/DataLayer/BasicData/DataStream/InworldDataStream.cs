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
    /// The Interface for the detailed object for the data stream.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataStreamStrategy<T>
    {
        IntPtr CreateFromInputStream(InworldInputStream<T> inputs, CancellationContext cancellationContext);
        IntPtr CreateFromBaseData(InworldBaseData source);
        IntPtr CreateCopy(IntPtr source);
        string ToString(IntPtr ptr);
        void Delete(IntPtr ptr);
        InworldInputStream<T> ToInputStream(IntPtr ptr);
        bool IsValid(IntPtr ptr);
        CancellationContext CancellationContext(IntPtr ptr);
        void Accept(IBaseDataVisitor visitor, InworldDataStream<T> stream);
    }

    /// <summary>
    /// DataStream Class. All the method is implemented by the strategy of the specific item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InworldDataStream<T> : InworldBaseData
    {
        static Dictionary<Type, object> s_Strategies = new Dictionary<Type, object>        
        {
            { typeof(string), new StringDataStreamStrategy() },
            { typeof(TTSOutput), new TTSOutputDataStreamStrategy() },
            { typeof(InworldContent), new ContentDataStreamStrategy() },
            { typeof(SpeechChunk), new SpeechChunkDataStreamStrategy() },
            { typeof(InworldBaseData), new BaseDataDataStreamStrategy()},
        };
        
        static IDataStreamStrategy<T> Strategy 
        {
            get
            {
                if (s_Strategies.TryGetValue(typeof(T), out object strategy)) 
                    return (IDataStreamStrategy<T>)strategy;
                Debug.LogError($"Type {typeof(T)} is not supported");
                return null;
            }
        }
        
        /// <summary>
        /// Move Constructor
        /// </summary>
        /// <param name="rhs">The pointer of the inworldDataStream</param>
        public InworldDataStream(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, Strategy.Delete);
        }

        /// <summary>
        /// Constructor by input stream.
        /// </summary>
        /// <param name="inputs">The instance of the inputStream</param>
        public InworldDataStream(InworldInputStream<T> inputs, CancellationContext cancellationContext)
        {
            IDataStreamStrategy<T> currentStrategy = Strategy;
            m_DLLPtr = MemoryManager.Register(currentStrategy.CreateFromInputStream(inputs, cancellationContext), currentStrategy.Delete);
        }
        
        /// <summary>
        /// Constructor by base class
        /// </summary>
        /// <param name="rhs">InworldBaseData</param>
        public InworldDataStream(InworldBaseData rhs)
        {
            IDataStreamStrategy<T> currentStrategy = Strategy;
            m_DLLPtr = MemoryManager.Register(currentStrategy.CreateFromBaseData(rhs), currentStrategy.Delete);
        }

        /// <summary>
        /// Move Constructor
        /// </summary>
        /// <param name="rhs">DataStream</param>
        public InworldDataStream(InworldDataStream<T> rhs)
        {
            IDataStreamStrategy<T> currentStrategy = Strategy;
            m_DLLPtr = MemoryManager.Register(currentStrategy.CreateCopy(rhs.ToDLL), currentStrategy.Delete);
        }

        /// <summary>
        /// Get if it's valid.
        /// </summary>
        public override bool IsValid => Strategy?.IsValid(m_DLLPtr) ?? false;

        /// <summary>
        /// Get its cancellation context.
        /// </summary>
        public CancellationContext CancellationContext => Strategy?.CancellationContext(m_DLLPtr);
        /// <summary>
        /// Convert to string
        /// </summary>
        public override string ToString()
        {
            return Strategy.ToString(m_DLLPtr);
        }

        /// <summary>
        /// Convert to InworldInputStream
        /// </summary>
        /// <returns></returns>
        public InworldInputStream<T> ToInputStream()
        {
            return Strategy.ToInputStream(m_DLLPtr);
        }

        /// <summary>
        /// Let Visitor Accept.
        /// </summary>
        /// <param name="visitor"></param>
        public override void Accept(IBaseDataVisitor visitor)
        {
            Strategy?.Accept(visitor, this);
        }
    }
}