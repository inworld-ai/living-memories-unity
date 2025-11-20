using System;
using Inworld.Framework.Node;

namespace Inworld.Framework
{
    public class StringToStringHashMapStrategy : IHashMapStrategy<string, string>
    {
        /// <summary>
        /// Creates a new native hash map instance for string-to-string mappings.
        /// </summary>
        /// <returns>A native pointer to the newly created string-to-string hash map.</returns>
        public IntPtr CreateNew() => InworldInterop.inworld_HashMap_StringToString_new();

        /// <summary>
        /// Deletes a native string-to-string hash map instance and frees its memory.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map to delete.</param>
        public void Delete(IntPtr ptr) => InworldInterop.inworld_HashMap_StringToString_delete(ptr);

        /// <summary>
        /// Gets the number of string-to-string mappings in the hash map.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <returns>The number of key-value pairs in the hash map.</returns>
        public int Size(IntPtr ptr) => InworldInterop.inworld_HashMap_StringToString_size(ptr);

        /// <summary>
        /// Determines whether the string-to-string hash map is empty.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <returns>True if the hash map contains no elements; otherwise, false.</returns>
        public bool IsEmpty(IntPtr ptr) => InworldInterop.inworld_HashMap_StringToString_empty(ptr);

        /// <summary>
        /// Removes all string-to-string mappings from the hash map.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        public void Clear(IntPtr ptr) => InworldInterop.inworld_HashMap_StringToString_clear(ptr);

        /// <summary>
        /// Determines whether the hash map contains the specified string key.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <param name="key">The string key to locate in the hash map.</param>
        /// <returns>True if the hash map contains the specified key; otherwise, false.</returns>
        public bool ContainsKey(IntPtr ptr, string key) 
            => InworldInterop.inworld_HashMap_StringToString___contains__(ptr, key);

        /// <summary>
        /// Gets the InworldNode value associated with the specified string key.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <param name="key">The string key whose associated node to retrieve.</param>
        /// <returns>The InworldNode object associated with the specified key.</returns>
        public string GetValue(IntPtr ptr, string key) 
            => InworldInterop.inworld_HashMap_StringToString___getitem__(ptr, key);

        /// <summary>
        /// Sets the InworldNode value associated with the specified string key.
        /// </summary>
        /// <param name="ptr">The native pointer to the hash map.</param>
        /// <param name="key">The string key with which to associate the node.</param>
        /// <param name="value">The InworldNode object to associate with the key.</param>
        public void SetValue(IntPtr ptr, string key, string value) 
            => InworldInterop.inworld_HashMap_StringToString___setitem__(ptr, key, value);
    }
}