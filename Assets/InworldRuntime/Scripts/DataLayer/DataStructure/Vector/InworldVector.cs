/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using Inworld.Framework.Event;
using Inworld.Framework.Graph;
using Inworld.Framework.Knowledge;
using Inworld.Framework.LLM;
using Inworld.Framework.Memory;
using Inworld.Framework.Node;
using Inworld.Framework.Safety;
using Inworld.Framework.Telemetry;
using UnityEngine;

namespace Inworld.Framework
{
    /// <summary>
    /// Defines the interface for implementing Vector strategies that bridge C# generic types to native C++ vector implementations.
    /// Provides a contract for creating, managing, and operating on native dynamic array data structures.
    /// Vectors support random access, dynamic resizing, and efficient element insertion and retrieval.
    /// </summary>
    /// <typeparam name="T">The type of elements stored in the vector.</typeparam>
    public interface IVectorStrategy<T>
    {
        /// <summary>
        /// Creates a new native vector instance.
        /// </summary>
        /// <returns>A native pointer to the newly created vector.</returns>
        IntPtr CreateNew();
        
        /// <summary>
        /// Creates a copy of an existing native vector.
        /// </summary>
        /// <param name="source">The native pointer to the source vector to copy.</param>
        /// <returns>A native pointer to the newly created vector copy.</returns>
        IntPtr CreateCopy(IntPtr source);
        
        /// <summary>
        /// Deletes a native vector instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector to delete.</param>
        void Delete(IntPtr ptr);
        
        /// <summary>
        /// Reserves memory capacity for the specified number of elements.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="nSize">The number of elements to reserve capacity for.</param>
        void Reserve(IntPtr ptr, int nSize);
        
        /// <summary>
        /// Removes all elements from the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        void Clear(IntPtr ptr);
        
        /// <summary>
        /// Adds an element to the end of the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="value">The element to add to the vector.</param>
        void PushBack(IntPtr ptr, T value);
        
        /// <summary>
        /// Gets the number of elements in the vector.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The number of elements in the vector.</returns>
        int GetSize(IntPtr ptr);
        
        /// <summary>
        /// Gets the capacity of the vector (total allocated space).
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>The capacity of the vector in number of elements.</returns>
        int GetCapacity(IntPtr ptr);
        
        /// <summary>
        /// Determines whether the vector is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <returns>True if the vector contains no elements; otherwise, false.</returns>
        bool IsEmpty(IntPtr ptr);
        
        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        T GetItem(IntPtr ptr, int index);
        
        /// <summary>
        /// Sets the element at the specified index.
        /// </summary>
        /// <param name="ptr">The native pointer to the vector.</param>
        /// <param name="index">The zero-based index of the element to set.</param>
        /// <param name="value">The value to set at the specified index.</param>
        void SetItem(IntPtr ptr, int index, T value);
    }
    
    /// <summary>
    /// Generic dynamic array implementation that provides managed access to native C++ vector data structures.
    /// Uses a strategy pattern to support different element types through specialized implementations.
    /// Supports efficient random access, dynamic resizing, and standard vector operations for Inworld framework data types.
    /// </summary>
    /// <typeparam name="T">The type of elements in the vector.</typeparam>
    public class InworldVector<T> : InworldFrameworkDllClass
    {
        static readonly Dictionary<Type, object> s_Strategies = new Dictionary<Type, object>
        {
            { typeof(InworldBaseData), new BaseDataVectorStrategy() },
            { typeof(char), new CharVectorStrategy() },
            { typeof(ChatMessage), new ChatMessageVectorStrategy() },
            { typeof(CompiledIntent), new CompiledIntentVectorStrategy() },
            { typeof(CustomConfigWrapper), new CustomConfigWrapperVectorStrategy() },
            { typeof(DetectedTopic), new DetectedTopicVectorStrategy() },
            { typeof(InworldDevice), new DeviceVectorStrategy() },
            { typeof(DictionaryRule), new DictionaryRuleVectorStrategy() },
            { typeof(InworldEdge), new EdgeVectorStrategy() },
            { typeof(EntityMatch), new EntityMatchVectorStrategy() },
            { typeof(Entity), new EntityVectorStrategy() },
            { typeof(InworldEvent), new EventVectorStrategy() },
            { typeof(InworldVector<float>), new Float2DVectorStrategy() },
            { typeof(float), new FloatVectorStrategy() },
            { typeof(InworldGoal), new GoalVectorStrategy() },
            { typeof(IntentMatch), new IntentMatchVectorStrategy() },
            { typeof(InworldIntent), new InworldIntentVectorStrategy() },
            { typeof(KeywordGroup), new KeywordGroupVectorStrategy() },
            { typeof(KeywordMatch), new KeywordMatchVectorStrategy() },
            { typeof(KnowledgeCollection), new KnowledgeCollectionVectorStrategy() },
            { typeof(KnowledgeRecord), new KnowledgeRecordVectorStrategy() },
            { typeof(LLMRoutingConfig), new LLMRoutingConfigVectorStrategy() },
            { typeof(InworldMap<string, string>), new MapStringVectorStrategy() },
            { typeof(InworldMessage), new MessagesVectorStrategy() },
            { typeof(InworldNode), new NodeVectorStrategy() },
            { typeof(PhonemeStamp), new PhonemeStampVectorStrategy() },
            { typeof(StatusCode), new StatusCodeVectorStrategy() },
            { typeof(string), new StringVectorStrategy() },
            { typeof(InworldSpan), new TelemetrySpanVectorStrategy() },
            { typeof(InworldText), new TextVectorStrategy() },
            { typeof(ToolCallData), new ToolCallDataVectorStrategy() },
            { typeof(ToolCallResult), new ToolCallResultVectorStrategy() },
            { typeof(ToolCall), new ToolCallVectorStrategy() },
            { typeof(ToolData), new ToolDataVectorStrategy() },
            { typeof(InworldTool), new ToolVectorStrategy() },
            { typeof(TopicThreshold), new TopicThresholdVectorStrategy() },
        };

        static IVectorStrategy<T> Strategy 
        {
            get
            {
                if (s_Strategies.TryGetValue(typeof(T), out object strategy)) 
                    return (IVectorStrategy<T>)strategy;
                Debug.LogError($"Type {typeof(T)} is not supported");
                return null;
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the InworldVector class.
        /// Creates a new native vector using the appropriate strategy for the specified element type.
        /// </summary>
        public InworldVector()
        {
            IVectorStrategy<T> currentStrategy = Strategy;
            if (currentStrategy == null)
                return;
            m_DLLPtr = MemoryManager.Register(currentStrategy.CreateNew(), currentStrategy.Delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldVector class from an existing native pointer.
        /// Used for wrapping existing native vector objects created by the C++ library.
        /// </summary>
        /// <param name="rhs">Native pointer to an existing vector instance.</param>
        public InworldVector(IntPtr rhs)
        {
            IVectorStrategy<T> currentStrategy = Strategy;
            if (currentStrategy == null)
                return;
            m_DLLPtr = MemoryManager.Register(rhs, currentStrategy.Delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldVector class as a copy of another vector.
        /// Creates a new vector containing copies of all elements from the source vector.
        /// </summary>
        /// <param name="rhs">The source vector to copy from.</param>
        public InworldVector(InworldVector<T> rhs)
        {
            IVectorStrategy<T> currentStrategy = Strategy;
            if (currentStrategy == null)
                return;
            m_DLLPtr = MemoryManager.Register(currentStrategy.CreateCopy(rhs.ToDLL), currentStrategy.Delete);
        }

        /// <summary>
        /// Reserves memory capacity for the specified number of elements.
        /// This can improve performance when the final size is known in advance.
        /// </summary>
        /// <param name="nSize">The number of elements to reserve capacity for.</param>
        public void Reserve(int nSize)
        {
            if (m_DLLPtr == IntPtr.Zero)
                return;
            Strategy?.Reserve(m_DLLPtr, nSize);
        }

        /// <summary>
        /// Removes all elements from the vector.
        /// After this operation, the vector will be empty but retain its allocated capacity.
        /// </summary>
        public void Clear()
        {
            if (m_DLLPtr == IntPtr.Zero)
                return;
            Strategy?.Clear(m_DLLPtr);
        }

        /// <summary>
        /// Adds an element to the end of the vector.
        /// The vector will automatically resize if necessary to accommodate the new element.
        /// </summary>
        /// <param name="input">The element to add to the vector.</param>
        public void Add(T input)
        {
            if (m_DLLPtr == IntPtr.Zero)
                return;
            Strategy?.PushBack(m_DLLPtr, input);
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// Provides random access to vector elements.
        /// </summary>
        /// <param name="nIndex">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public T this[int nIndex] 
        {
            get
            {
                if (m_DLLPtr == IntPtr.Zero || Strategy == null)
                    return default;
                return Strategy.GetItem(m_DLLPtr, nIndex);
            }
            set
            {
                if (m_DLLPtr == IntPtr.Zero)
                    return;
                Strategy?.SetItem(m_DLLPtr, nIndex, value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the vector is empty.
        /// </summary>
        /// <value>True if the vector contains no elements; otherwise, false.</value>
        public bool IsEmpty
        {
            get
            {
                if (m_DLLPtr == IntPtr.Zero || Strategy == null)
                    return true;
                return Strategy.IsEmpty(m_DLLPtr);
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the vector.
        /// </summary>
        /// <value>The number of elements in the vector, or -1 if the vector is invalid.</value>
        public int Size
        {
            get
            {
                if (m_DLLPtr == IntPtr.Zero || Strategy == null)
                    return -1;
                return Strategy.GetSize(m_DLLPtr);
            }
        }

        /// <summary>
        /// Gets the total number of elements the vector can hold without resizing.
        /// </summary>
        /// <value>The capacity of the vector, or -1 if the vector is invalid.</value>
        public int Capacity
        {
            get
            {
                if (m_DLLPtr == IntPtr.Zero || Strategy == null)
                    return -1;
                return Strategy.GetCapacity(m_DLLPtr);
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the vector (alternative property).
        /// This property provides the same functionality as Size for compatibility.
        /// </summary>
        /// <value>The number of elements in the vector, or -1 if the vector is invalid.</value>
        public int Count
        {
            get
            {
                if (m_DLLPtr == IntPtr.Zero || Strategy == null)
                    return -1;
                return Strategy.GetSize(m_DLLPtr);
            }
        }

        /// <summary>
        /// Converts the vector to a standard .NET List containing all elements.
        /// Creates a new List&lt;T&gt; and copies all elements from the vector in order.
        /// </summary>
        /// <returns>A new List&lt;T&gt; containing copies of all elements in the vector.</returns>
        public List<T> ToList()
        {
            List<T> list = new List<T>();
            for (int i = 0; i < Size; i++)
                list.Add(this[i]);
            return list;
        }

        /// <summary>
        /// Replaces all elements in the vector with elements from the specified list.
        /// Clears the current vector contents and adds all elements from the provided list.
        /// </summary>
        /// <param name="list">The list containing elements to copy into the vector.</param>
        public void FromList(IList<T> list)
        {
            Clear();
            foreach (T item in list)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Adds a range of elements to the end of the vector.
        /// Appends all elements from the specified enumerable collection to the vector.
        /// </summary>
        /// <param name="range">The collection of elements to add to the vector.</param>
        public void AddRange(IEnumerable<T> range)
        {
            foreach (T item in range)
            {
                Add(item);
            }
        }
    }
}