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
    public class InworldAudio : InworldBaseData
    {
        public InworldAudio(InworldVector<float> data, int size)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_Audio_new(data.ToDLL, size),
                InworldInterop.inworld_Audio_delete);
        }

        public InworldAudio(InworldBaseData rhs)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_Audio(rhs.ToDLL),
                InworldInterop.inworld_Audio_delete);
        }
        
        public InworldAudio(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_Audio_delete);
        }

        public override bool IsValid => InworldInterop.inworld_Audio_is_valid(m_DLLPtr);
        public int SampleRate => InworldInterop.inworld_Audio_sample_rate(m_DLLPtr);

        public AudioClip AudioClip
        {
            get
            {
                if (!IsValid || Waveform == null)
                    return null;
                int sampleRate = SampleRate;
                List<float> data = Waveform.ToList();
                AudioClip audioClip = AudioClip.Create("TTS", data.Count, 1, sampleRate, false);
                audioClip.SetData(data.ToArray(), 0);
                return audioClip;
            }
        }

        public override string ToString()
        {
            return InworldInterop.inworld_Audio_ToString(m_DLLPtr);
        }

        public InworldVector<float> Waveform => new InworldVector<float>(InworldInterop.inworld_Audio_waveform(m_DLLPtr));

        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}