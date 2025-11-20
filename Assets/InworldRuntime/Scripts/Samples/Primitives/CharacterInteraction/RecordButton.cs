/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;
using UnityEngine.EventSystems;

namespace Inworld.Framework.Samples
{
    /// <summary>
    /// Sample UI component implementing a push-to-talk recording button for voice interactions.
    /// Provides press-and-hold functionality to control microphone recording state through pointer events.
    /// Integrates with the Inworld audio system to enable voice input for character conversations.
    /// Serves as a reference implementation for building voice-controlled UI elements.
    /// </summary>
    public class RecordButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        /// <summary>
        /// Handles pointer down events to start voice recording.
        /// Activates the player speaking state when the button is pressed down.
        /// </summary>
        /// <param name="eventData">Pointer event data containing press information.</param>
        public void OnPointerDown(PointerEventData eventData)
        {
            InworldController.Audio.IsPlayerSpeaking = true;
        }

        /// <summary>
        /// Handles pointer up events to stop voice recording.
        /// Deactivates the player speaking state when the button is released.
        /// </summary>
        /// <param name="eventData">Pointer event data containing release information.</param>
        public void OnPointerUp(PointerEventData eventData)
        {
            InworldController.Audio.IsPlayerSpeaking = false;
        }
    }
}