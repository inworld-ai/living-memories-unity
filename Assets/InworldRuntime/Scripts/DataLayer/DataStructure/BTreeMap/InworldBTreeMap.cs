using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inworld.Framework
{
    public interface IBTreeMapStrategy<TKey, TValue>
    {
        IntPtr CreateNew();

        void Delete(IntPtr ptr);

        int Size(IntPtr ptr);

        bool IsEmpty(IntPtr ptr);

        void Clear(IntPtr ptr);

        bool ContainsKey(IntPtr ptr, TKey key);

        TValue GetValue(IntPtr ptr, TKey key);

        void SetValue(IntPtr ptr, TKey key, TValue value);
    }
    public class InworldBTreeMap<TKey, TValue> : InworldFrameworkDllClass
    {
        static Dictionary<(Type, Type), object> s_Strategies = new Dictionary<(Type, Type), object>()        
        {
            { (typeof(string), typeof(string)), new StringToStringBTreeMapStrategy() }
        };
        
        static IBTreeMapStrategy<TKey, TValue> Strategy 
        {
            get
            {
                if (s_Strategies.TryGetValue((typeof(TKey), typeof(TValue)), out object strategy)) 
                    return (IBTreeMapStrategy<TKey, TValue>)strategy;
                Debug.LogError($"HashMap type <{typeof(TKey)}, {typeof(TValue)}> is not supported");
                return null;
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the InworldBTreeMap class.
        /// Creates a new native hash map using the appropriate strategy for the specified key-value types.
        /// </summary>
        public InworldBTreeMap()
        {
            IBTreeMapStrategy<TKey, TValue> currentStrategy = Strategy;
            if (currentStrategy == null)
                return;
            m_DLLPtr = MemoryManager.Register(currentStrategy.CreateNew(), currentStrategy.Delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldBTreeMap class from an existing native pointer.
        /// Used for wrapping existing native hash map objects created by the C++ library.
        /// </summary>
        /// <param name="rhs">Native pointer to an existing hash map instance.</param>
        public InworldBTreeMap(IntPtr rhs)
        {
            IBTreeMapStrategy<TKey, TValue> currentStrategy = Strategy;
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
                IBTreeMapStrategy<TKey, TValue> currentStrategy = Strategy;
                if (currentStrategy == null)
                    return default;
                return currentStrategy.GetValue(m_DLLPtr, key);
            }
            set
            {
                IBTreeMapStrategy<TKey, TValue> currentStrategy = Strategy;
                if (currentStrategy == null)
                    return;
                currentStrategy.SetValue(m_DLLPtr, key, value);
            }
        }
    }
}