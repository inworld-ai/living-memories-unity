/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections;
using UnityEngine;

namespace Inworld.Framework.Audio
{
    /// <summary>
    /// Base class for audio modules that handle player audio events and voice detection.
    /// Provides automatic lifecycle management for player audio monitoring and implements the IPlayerAudioEventHandler interface.
    /// Derived classes should override OnPlayerUpdate to implement specific voice detection logic.
    /// </summary>
    public class PlayerEventModule : InworldAudioModule, IPlayerAudioEventHandler
    {
        protected virtual void OnEnable()
        {
            StartModule(OnPlayerUpdate());
        }

        protected virtual void OnDisable()
        {
            StopModule();
        }
        
        /// <summary>
        /// The main coroutine for monitoring player audio activity and determining when audio data should be transmitted.
        /// Base implementation considers the player to always be speaking; derived classes should override for specific detection logic.
        /// </summary>
        /// <returns>An IEnumerator for the coroutine-based player audio monitoring loop.</returns>
        public virtual IEnumerator OnPlayerUpdate()
        {
            while (isActiveAndEnabled)
            {
                Audio.IsPlayerSpeaking = true;
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        /// <summary>
        /// Enables voice detection by activating this module.
        /// Starts the player audio monitoring coroutine.
        /// </summary>
        public void StartVoiceDetecting() => enabled = true;
        
        /// <summary>
        /// Disables voice detection by deactivating this module.
        /// Stops the player audio monitoring coroutine.
        /// </summary>
        public void StopVoiceDetecting() => enabled = false;
    }
}