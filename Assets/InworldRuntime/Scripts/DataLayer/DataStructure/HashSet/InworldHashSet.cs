/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inworld.Framework
{
    /// <summary>
    /// Defines the interface for implementing HashSet strategies that bridge C# generic types to native C++ hash set implementations.
    /// Provides a contract for creating, managing, and operating on native hash set data structures.
    /// </summary>
    /// <typeparam name="T">The type of elements stored in the hash set.</typeparam>
    public interface IHashSetStrategy<T>
    {
        /// <summary>
        /// Creates a new native hash set instance.
        /// </summary>
        /// <returns>A native pointer to the newly created hash set.</returns>
        IntPtr CreateNew();
        
        /// <summary>
        /// Deletes a native hash set instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash set to delete.</param>
        void Delete(IntPtr ptr);
        
        /// <summary>
        /// Removes all elements from the hash set.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash set.</param>
        void Clear(IntPtr ptr);
        
        /// <summary>
        /// Determines whether the hash set contains the specified element.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash set.</param>
        /// <param name="value">The element to locate in the hash set.</param>
        /// <returns>True if the hash set contains the element; otherwise, false.</returns>
        bool Contains(IntPtr ptr, T value);
        
        /// <summary>
        /// Adds an element to the hash set.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash set.</param>
        /// <param name="value">The element to add to the hash set.</param>
        void Add(IntPtr ptr, T value);
        
        /// <summary>
        /// Determines whether the hash set is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash set.</param>
        /// <returns>True if the hash set contains no elements; otherwise, false.</returns>
        bool IsEmpty(IntPtr ptr);
        
        /// <summary>
        /// Gets the number of elements in the hash set.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash set.</param>
        /// <returns>The number of elements in the hash set.</returns>
        int Size(IntPtr ptr);
    }
    
    /// <summary>
    /// Generic hash set implementation that provides managed access to native C++ hash set data structures.
    /// Uses a strategy pattern to support different element types through specialized implementations.
    /// Supports efficient element lookup, insertion, and set operations for Inworld framework data types.
    /// </summary>
    /// <typeparam name="T">The type of elements in the hash set.</typeparam>
    public class InworldHashSet<T> : InworldFrameworkDllClass
    {
        static Dictionary<Type, object> s_Strategies = new Dictionary<Type, object>()        
        {
            { typeof(string), new StringHashSetStrategy() },
        };
        
        static IHashSetStrategy<T> Strategy 
        {
            get
            {
                if (s_Strategies.TryGetValue(typeof(T), out object strategy)) 
                    return (IHashSetStrategy<T>)strategy;
                Debug.LogError($"Type {typeof(T)} is not supported");
                return null;
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the InworldHashSet class.
        /// Creates a new native hash set using the appropriate strategy for the specified element type.
        /// </summary>
        public InworldHashSet()
        {
            IHashSetStrategy<T> currentStrategy = Strategy;
            if (currentStrategy == null)
                return;
            m_DLLPtr = MemoryManager.Register(currentStrategy.CreateNew(), currentStrategy.Delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldHashSet class from an existing native pointer.
        /// Used for wrapping existing native hash set objects created by the C++ library.
        /// </summary>
        /// <param name="rhs">Native pointer to an existing hash set instance.</param>
        public InworldHashSet(IntPtr rhs)
        {
            IHashSetStrategy<T> currentStrategy = Strategy;
            if (currentStrategy == null)
                return;
            m_DLLPtr = MemoryManager.Register(rhs, currentStrategy.Delete);
        }

        /// <summary>
        /// Removes all elements from the hash set.
        /// After this operation, the hash set will be empty.
        /// </summary>
        public void Clear()
        {
            Strategy?.Clear(m_DLLPtr);
        }

        /// <summary>
        /// Determines whether the hash set contains the specified element.
        /// </summary>
        /// <param name="value">The element to locate in the hash set.</param>
        /// <returns>True if the hash set contains the element; otherwise, false.</returns>
        public bool Contains(T value) => Strategy?.Contains(m_DLLPtr, value) ?? false;

        /// <summary>
        /// Adds the specified element to the hash set.
        /// If the element already exists, this operation has no effect.
        /// </summary>
        /// <param name="value">The element to add to the hash set.</param>
        public void Add(T value) => Strategy?.Add(m_DLLPtr, value);

        /// <summary>
        /// Gets a value indicating whether the hash set is empty.
        /// </summary>
        /// <value>True if the hash set contains no elements; otherwise, false.</value>
        public bool IsEmpty => Strategy?.IsEmpty(m_DLLPtr) ?? false;

        /// <summary>
        /// Gets the number of elements contained in the hash set.
        /// </summary>
        /// <value>The number of elements in the hash set, or -1 if the hash set is invalid.</value>
        public int Size => Strategy?.Size(m_DLLPtr) ?? -1;
    }
}