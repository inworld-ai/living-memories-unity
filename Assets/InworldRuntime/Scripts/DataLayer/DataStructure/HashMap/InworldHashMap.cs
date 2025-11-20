/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using Inworld.Framework.Graph;
using Inworld.Framework.Node;
using UnityEngine;

namespace Inworld.Framework
{
    /// <summary>
    /// Defines the interface for implementing HashMap strategies that bridge C# generic types to native C++ hash map implementations.
    /// Provides a contract for creating, managing, and operating on native hash map data structures.
    /// </summary>
    /// <typeparam name="TKey">The type of keys stored in the hash map.</typeparam>
    /// <typeparam name="TValue">The type of values stored in the hash map.</typeparam>
    public interface IHashMapStrategy<TKey, TValue>
    {
        /// <summary>
        /// Creates a new native hash map instance.
        /// </summary>
        /// <returns>A native pointer to the newly created hash map.</returns>
        IntPtr CreateNew();
        
        /// <summary>
        /// Deletes a native hash map instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map to delete.</param>
        void Delete(IntPtr ptr);
        
        /// <summary>
        /// Gets the number of key-value pairs in the hash map.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <returns>The number of elements in the hash map.</returns>
        int Size(IntPtr ptr);
        
        /// <summary>
        /// Determines whether the hash map is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <returns>True if the hash map contains no elements; otherwise, false.</returns>
        bool IsEmpty(IntPtr ptr);
        
        /// <summary>
        /// Removes all key-value pairs from the hash map.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        void Clear(IntPtr ptr);
        
        /// <summary>
        /// Determines whether the hash map contains the specified key.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <param name="key">The key to locate in the hash map.</param>
        /// <returns>True if the hash map contains the key; otherwise, false.</returns>
        bool ContainsKey(IntPtr ptr, TKey key);
        
        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <param name="key">The key whose value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        TValue GetValue(IntPtr ptr, TKey key);
        
        /// <summary>
        /// Sets the value associated with the specified key.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <param name="key">The key whose value to set.</param>
        /// <param name="value">The value to associate with the key.</param>
        void SetValue(IntPtr ptr, TKey key, TValue value);
    }
    
    /// <summary>
    /// Generic hash map implementation that provides managed access to native C++ hash map data structures.
    /// Uses a strategy pattern to support different key-value type combinations through specialized implementations.
    /// Supports efficient key-based lookup and storage operations for Inworld framework data types.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the hash map.</typeparam>
    /// <typeparam name="TValue">The type of values in the hash map.</typeparam>
    public class InworldHashMap<TKey, TValue> : InworldFrameworkDllClass
    {
        static Dictionary<(Type, Type), object> s_Strategies = new Dictionary<(Type, Type), object>()        
        {
            { (typeof(string), typeof(InworldNode)), new StringToNodeHashMapStrategy() },
            { (typeof(string), typeof(InworldHashSet<string>)), new StringToHashSetStringHashMapStrategy() },
            { (typeof(string), typeof(InworldLoop)), new StringToLoopHashMapStrategy() },
            { (typeof(string), typeof(string)), new StringToStringHashMapStrategy() }
        };
        
        static IHashMapStrategy<TKey, TValue> Strategy 
        {
            get
            {
                if (s_Strategies.TryGetValue((typeof(TKey), typeof(TValue)), out object strategy)) 
                    return (IHashMapStrategy<TKey, TValue>)strategy;
                Debug.LogError($"HashMap type <{typeof(TKey)}, {typeof(TValue)}> is not supported");
                return null;
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the InworldHashMap class.
        /// Creates a new native hash map using the appropriate strategy for the specified key-value types.
        /// </summary>
        public InworldHashMap()
        {
            IHashMapStrategy<TKey, TValue> currentStrategy = Strategy;
            if (currentStrategy == null)
                return;
            m_DLLPtr = MemoryManager.Register(currentStrategy.CreateNew(), currentStrategy.Delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldHashMap class from an existing native pointer.
        /// Used for wrapping existing native hash map objects created by the C++ library.
        /// </summary>
        /// <param name="rhs">Native pointer to an existing hash map instance.</param>
        public InworldHashMap(IntPtr rhs)
        {
            IHashMapStrategy<TKey, TValue> currentStrategy = Strategy;
            if (currentStrategy == null)
                return;
            m_DLLPtr = MemoryManager.Register(rhs, currentStrategy.Delete);
        }

        /// <summary>
        /// Gets the number of key-value pairs contained in the hash map.
        /// </summary>
        /// <value>The number of elements in the hash map, or -1 if the hash map is invalid.</value>
        public int Size => Strategy?.Size(m_DLLPtr) ?? -1;

        /// <summary>
        /// Gets a value indicating whether the hash map is empty.
        /// </summary>
        /// <value>True if the hash map contains no elements; otherwise, false.</value>
        public bool IsEmpty => Strategy?.IsEmpty(m_DLLPtr) ?? false;

        /// <summary>
        /// Removes all key-value pairs from the hash map.
        /// </summary>
        public void Clear() => Strategy?.Clear(m_DLLPtr);

        /// <summary>
        /// Determines whether the hash map contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the hash map.</param>
        /// <returns>True if the hash map contains an element with the specified key; otherwise, false.</returns>
        public bool ContainsKey(TKey key) => Strategy?.ContainsKey(m_DLLPtr, key) ?? false;

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// If the key does not exist when getting, returns the default value for the type.
        /// If the key does not exist when setting, adds a new key-value pair.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key.</returns>
        public TValue this[TKey key]
        {
            get
            {
                IHashMapStrategy<TKey, TValue> currentStrategy = Strategy;
                if (currentStrategy == null)
                    return default;
                return currentStrategy.GetValue(m_DLLPtr, key);
            }
            set
            {
                IHashMapStrategy<TKey, TValue> currentStrategy = Strategy;
                if (currentStrategy == null)
                    return;
                currentStrategy.SetValue(m_DLLPtr, key, value);
            }
        }
    }
}