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
    /// Defines the interface for implementing Map strategies that bridge C# generic types to native C++ map implementations.
    /// Provides a contract for creating, managing, and operating on native ordered map data structures.
    /// Maps maintain key-value pairs in sorted order based on the key comparison.
    /// </summary>
    /// <typeparam name="TKey">The type of keys stored in the map.</typeparam>
    /// <typeparam name="TValue">The type of values stored in the map.</typeparam>
    public interface IMapStrategy<TKey, TValue>
    {
        /// <summary>
        /// Creates a new native map instance.
        /// </summary>
        /// <returns>A native pointer to the newly created map.</returns>
        IntPtr CreateNew();
        
        /// <summary>
        /// Deletes a native map instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the map to delete.</param>
        void Delete(IntPtr ptr);
        
        /// <summary>
        /// Gets the number of key-value pairs in the map.
        /// </summary>
        /// <param name="ptr">The native pointer to the map.</param>
        /// <returns>The number of elements in the map.</returns>
        int Size(IntPtr ptr);
        
        /// <summary>
        /// Determines whether the map is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the map.</param>
        /// <returns>True if the map contains no elements; otherwise, false.</returns>
        bool IsEmpty(IntPtr ptr);
        
        /// <summary>
        /// Removes all key-value pairs from the map.
        /// </summary>
        /// <param name="ptr">The native pointer to the map.</param>
        void Clear(IntPtr ptr);
        
        /// <summary>
        /// Determines whether the map contains the specified key.
        /// </summary>
        /// <param name="ptr">The native pointer to the map.</param>
        /// <param name="key">The key to locate in the map.</param>
        /// <returns>True if the map contains the key; otherwise, false.</returns>
        bool ContainsKey(IntPtr ptr, TKey key);
        
        /// <summary>
        /// Removes the key-value pair with the specified key from the map.
        /// </summary>
        /// <param name="ptr">The native pointer to the map.</param>
        /// <param name="key">The key of the element to remove.</param>
        void DeleteKey(IntPtr ptr, TKey key);
        
        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="ptr">The native pointer to the map.</param>
        /// <param name="key">The key whose value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        TValue GetValue(IntPtr ptr, TKey key);
        
        /// <summary>
        /// Sets the value associated with the specified key.
        /// </summary>
        /// <param name="ptr">The native pointer to the map.</param>
        /// <param name="key">The key whose value to set.</param>
        /// <param name="value">The value to associate with the key.</param>
        void SetValue(IntPtr ptr, TKey key, TValue value);
    }
    
    /// <summary>
    /// Generic ordered map implementation that provides managed access to native C++ map data structures.
    /// Uses a strategy pattern to support different key-value type combinations through specialized implementations.
    /// Maintains elements in sorted order and supports efficient key-based lookup and range operations.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the map.</typeparam>
    /// <typeparam name="TValue">The type of values in the map.</typeparam>
    public class InworldMap<TKey, TValue> : InworldFrameworkDllClass
    {
        static Dictionary<(Type, Type), object> s_Strategies = new Dictionary<(Type, Type), object>()        
        {
            { (typeof(string), typeof(string)), new StringStringMapStrategy() },
        };
        
        static IMapStrategy<TKey, TValue> Strategy 
        {
            get
            {
                if (s_Strategies.TryGetValue((typeof(TKey), typeof(TValue)), out object strategy)) 
                    return (IMapStrategy<TKey, TValue>)strategy;
                Debug.LogError($"HashMap type <{typeof(TKey)}, {typeof(TValue)}> is not supported");
                return null;
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the InworldMap class.
        /// Creates a new native map using the appropriate strategy for the specified key-value types.
        /// </summary>
        public InworldMap()
        {
            IMapStrategy<TKey, TValue> currentStrategy = Strategy;
            if (currentStrategy == null)
                return;
            m_DLLPtr = MemoryManager.Register(currentStrategy.CreateNew(), currentStrategy.Delete);        
        }

        /// <summary>
        /// Initializes a new instance of the InworldMap class from an existing native pointer.
        /// Used for wrapping existing native map objects created by the C++ library.
        /// </summary>
        /// <param name="rhs">Native pointer to an existing map instance.</param>
        public InworldMap(IntPtr rhs)
        {
            IMapStrategy<TKey, TValue> currentStrategy = Strategy;
            if (currentStrategy == null)
                return;
            m_DLLPtr = MemoryManager.Register(rhs, currentStrategy.Delete);
        }

        /// <summary>
        /// Gets the number of key-value pairs contained in the map.
        /// </summary>
        /// <value>The number of elements in the map, or -1 if the map is invalid.</value>
        public int Size => Strategy?.Size(m_DLLPtr) ?? -1;

        /// <summary>
        /// Gets a value indicating whether the map is empty.
        /// </summary>
        /// <value>True if the map contains no elements; otherwise, false.</value>
        public bool IsEmpty => Strategy?.IsEmpty(m_DLLPtr) ?? false;

        /// <summary>
        /// Removes all key-value pairs from the map.
        /// </summary>
        public void Clear() => Strategy?.Clear(m_DLLPtr);

        /// <summary>
        /// Determines whether the map contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the map.</param>
        /// <returns>True if the map contains an element with the specified key; otherwise, false.</returns>
        public bool ContainsKey(TKey key) => Strategy?.ContainsKey(m_DLLPtr, key) ?? false;

        /// <summary>
        /// Removes the element with the specified key from the map.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        public void DeleteKey(TKey key) => Strategy?.DeleteKey(m_DLLPtr, key);

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
                IMapStrategy<TKey, TValue> currentStrategy = Strategy;
                if (currentStrategy == null)
                    return default;
                return currentStrategy.GetValue(m_DLLPtr, key);
            }
            set
            {
                IMapStrategy<TKey, TValue> currentStrategy = Strategy;
                if (currentStrategy == null)
                    return;
                currentStrategy.SetValue(m_DLLPtr, key, value);
            }
        }
    }
}