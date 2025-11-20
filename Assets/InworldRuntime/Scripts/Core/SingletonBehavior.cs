/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;

namespace Inworld
{
    /// <summary>
    /// Generic singleton MonoBehaviour base class that ensures only one instance of a component exists in the scene.
    /// Provides thread-safe lazy initialization and automatic discovery of existing instances.
    /// Use this as a base class for components that should have only one instance throughout the application lifecycle.
    /// </summary>
    /// <typeparam name="T">The type of the singleton component, must inherit from UnityEngine.Object.</typeparam>
    public class SingletonBehavior<T> : MonoBehaviour where T : Object
    {
        static T __inst;

        /// <summary>
        /// Gets the singleton instance of type T.
        /// If no instance exists, automatically searches for an existing component in the scene (including inactive objects).
        /// Returns null if no instance is found rather than creating a new one.
        /// </summary>
        /// <value>The singleton instance of type T, or null if no instance exists in the scene.</value>
        public static T Instance
        {
            get
            {
                if (__inst)
                    return __inst;
                __inst = FindFirstObjectByType<T>(FindObjectsInactive.Include);
                return __inst;
            }
        }
    }
}
