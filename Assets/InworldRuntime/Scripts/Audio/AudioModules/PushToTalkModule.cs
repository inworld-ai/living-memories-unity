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
    /// Push-to-talk audio input module that allows manual control over when audio is transmitted.
    /// Users must hold down a specified key to enable voice transmission, providing explicit control over audio input.
    /// This is useful for applications where continuous voice detection is not desired.
    /// </summary>
    public class PushToTalkModule : PlayerEventModule
    {
        [SerializeField] KeyCode m_PushToTalkKey = KeyCode.Space;

        void Awake()
        {

        }

        /// <summary>
        /// Monitors for push-to-talk key input and controls audio transmission accordingly.
        /// The player is considered to be speaking only when the configured key is being pressed.
        /// </summary>
        /// <returns>An IEnumerator for the coroutine-based input monitoring loop.</returns>
        public override IEnumerator OnPlayerUpdate()
        {
            while (isActiveAndEnabled)
            {
                Audio.IsPlayerSpeaking = Input.GetKeyDown(m_PushToTalkKey);
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }
    }
}