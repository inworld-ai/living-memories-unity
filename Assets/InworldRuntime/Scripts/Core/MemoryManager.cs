/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inworld.Framework
{
    /// <summary>
    /// Static memory manager responsible for handling C++ DLL pointer lifecycles and reference counting.
    /// Implements a smart pointer-like reference counting system to ensure proper memory management
    /// for native C++ objects accessed from C#. Delete functions are automatically called through IDisposable.Dispose().
    /// </summary>
    public static class MemoryManager
    {
        /// <summary>
        /// Thread-safe memory pool that tracks DLL object references and their associated delete functions.
        /// Each entry contains a reference count and the cleanup function for proper memory deallocation.
        /// </summary>
        public static ConcurrentDictionary<IntPtr, (int refCount, Action<IntPtr> deleteFunc)> MemPool =
            new ConcurrentDictionary<IntPtr, (int, Action<IntPtr>)>();

        /// <summary>
        /// Gets or sets a value indicating whether debug mode is enabled for memory management operations.
        /// When true, provides additional logging and warnings for memory management issues.
        /// </summary>
        /// <value>True if debug mode is enabled; otherwise, false. Default is true.</value>
        public static bool DebugMode { get; set; } = true;

        /// <summary>
        /// Registers a DLL object pointer with the memory manager and increments its reference count.
        /// This method is typically called in constructors to track native object lifecycles.
        /// If the pointer is already registered, increments the reference count.
        /// </summary>
        /// <param name="dllRef">The native pointer to the C++ DLL object to register.</param>
        /// <param name="deleteFunc">The cleanup function to call when the object should be destroyed.</param>
        /// <returns>The same pointer that was passed in, for convenience in constructor chaining.</returns>
        public static IntPtr Register(IntPtr dllRef, Action<IntPtr> deleteFunc)
        {
            if (dllRef == IntPtr.Zero)
                return dllRef;
            if (MemPool.ContainsKey(dllRef))
            {
                if (DebugMode && MemPool[dllRef].deleteFunc != deleteFunc)
                    Debug.LogWarning($"Multiple classes point to same address. Before: {MemPool[dllRef].deleteFunc?.Method.Name} NewItem: {deleteFunc?.Method.Name}");
                MemPool[dllRef] = (MemPool[dllRef].refCount+1, MemPool[dllRef].deleteFunc);
            }
            else
                MemPool[dllRef] = (1, deleteFunc);
            return dllRef;
        }
        
        /// <summary>
        /// Unregisters a DLL object pointer from the memory manager and decrements its reference count.
        /// This method is typically called in destructors or Dispose() methods.
        /// When the reference count reaches zero, the associated delete function is called to clean up the native object.
        /// </summary>
        /// <param name="dllRef">The native pointer to the C++ DLL object to unregister.</param>
        public static void UnRegister(IntPtr dllRef)
        {
            if (dllRef == IntPtr.Zero) 
                return;

            if (!MemPool.TryGetValue(dllRef, out (int refCount, Action<IntPtr> deleteFunc) result)) 
                return;
            
            int newCount = result.refCount - 1;
            if (newCount <= 0)
            {
                if (!MemPool.TryRemove(dllRef, out _)) 
                    return;
                if(InworldFrameworkUtil.IsDebugMode)
                    Debug.Log($"{result.deleteFunc?.Method.Name} Executed");
                result.deleteFunc?.Invoke(dllRef);
            }
            else
            {
                MemPool[dllRef] = (newCount, result.deleteFunc);
            }
        }
    }
}