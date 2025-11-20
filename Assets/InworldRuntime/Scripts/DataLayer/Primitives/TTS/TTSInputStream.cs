/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using UnityEngine;

namespace Inworld.Framework.TTS
{
    /// <summary>
    /// TTSInputStream class implementation - currently commented out.
    /// This class was intended to provide input stream functionality for TTS operations
    /// but is currently disabled in the codebase. The implementation remains commented
    /// for potential future use or reference.
    /// </summary>
    // public class TTSInputStream : InworldStream
    // {
    //     public TTSInputStream(IntPtr rhs) => m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_InputStream_SpeechChunk_delete);
    //
    //     public override bool HasNext => m_DLLPtr != IntPtr.Zero && InworldInterop.inworld_InputStream_SpeechChunk_HasNext(m_DLLPtr);
    //
    //     public override IntPtr Read()
    //     {
    //         return m_DLLPtr == IntPtr.Zero ? IntPtr.Zero : InworldInterop.inworld_InputStream_SpeechChunk_Read(m_DLLPtr);
    //     }
    //
    //     public InworldSpeechChunk ReadSpeechChunk()
    //     {
    //         IntPtr ptr = Read();
    //         if (ptr == IntPtr.Zero)
    //             return null;
    //         
    //         if (InworldInterop.inworld_StatusOr_SpeechChunk_ok(ptr))
    //             return new InworldSpeechChunk(InworldInterop.inworld_StatusOr_SpeechChunk_value(ptr));
    //         
    //         IntPtr status = InworldInterop.inworld_StatusOr_SpeechChunk_status(ptr);
    //         Debug.LogError(InworldInterop.inworld_Status_ToString(status));
    //         return null;
    //     }
    // }
}