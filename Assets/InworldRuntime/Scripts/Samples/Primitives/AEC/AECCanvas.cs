/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Linq;
using Inworld.Framework.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Framework.Samples
{
    /// <summary>
    /// Sample UI component demonstrating Acoustic Echo Cancellation (AEC) functionality within the Inworld framework.
    /// Provides an interface for testing AEC audio filtering by playing far-end and near-end audio samples
    /// and generating filtered output that removes echo and feedback.
    /// Serves as a reference implementation for building AEC testing and demonstration applications.
    /// </summary>
    public class AECCanvas : MonoBehaviour
    {
        [SerializeField] AudioSource m_AudioSource;
        [SerializeField] AudioClip m_Farend;
        [SerializeField] AudioClip m_Nearend;
        [SerializeField] TMP_Text m_CompleteText;
        [SerializeField] Button m_PlayFilteredButton;
        AudioChunk m_FilteredChunk;
        
        /// <summary>
        /// Plays the far-end audio sample (speaker output that causes potential echo).
        /// Demonstrates the audio that would typically be played through speakers and cause echo in the microphone.
        /// </summary>
        public void PlayFarend()
        {
            if (!m_AudioSource || !m_Farend)
                return;
            m_AudioSource.PlayOneShot(m_Farend);
        }

        /// <summary>
        /// Plays the near-end audio sample (microphone input that may contain echo).
        /// Demonstrates the audio captured by the microphone before AEC processing.
        /// </summary>
        public void PlayNearend()
        {
            if (!m_AudioSource || !m_Nearend)
                return;
            m_AudioSource.PlayOneShot(m_Nearend);
        }

        /// <summary>
        /// Processes the audio samples through AEC filtering to remove echo.
        /// Takes both far-end and near-end audio, applies echo cancellation, and prepares the filtered result for playback.
        /// Updates the UI to indicate when filtering is complete and enables the filtered audio playback button.
        /// </summary>
        public void FilterAudio()
        {
            AudioChunk farendChunk = WavUtility.GenerateAudioChunk(m_Farend);
            AudioChunk nearendChunk = WavUtility.GenerateAudioChunk(m_Nearend);
            m_FilteredChunk = InworldController.AEC.FilterAudio(nearendChunk, farendChunk);
            if (m_FilteredChunk == null) 
                return;
            if (m_CompleteText)
                m_CompleteText.text = "Audio Generated!";
            if (m_PlayFilteredButton)
                m_PlayFilteredButton.interactable = true;
        }

        /// <summary>
        /// Plays the AEC-filtered audio result.
        /// Converts the filtered audio chunk back to an AudioClip and plays it to demonstrate the echo cancellation effect.
        /// Only available after FilterAudio() has been successfully called.
        /// </summary>
        public void PlayFiltered()
        {
            if (m_FilteredChunk == null) 
                return;
            AudioClip result = WavUtility.GenerateAudioClip(m_FilteredChunk, "Filtered");
            if (result)
            {
                m_AudioSource.PlayOneShot(result);
            }
        }
    }
}