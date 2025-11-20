/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Inworld
{
    public class InworldException : Exception
    {
        public InworldException(string errorMessage) : base(errorMessage) {}
    } 
    public class InworldLog : MonoBehaviour
    {
        [SerializeField] TMP_Text m_LogArea;

        void Awake()
        {
            Application.logMessageReceivedThreaded += OnUnityLogReceived;
        }


        void OnDisable()
        {
            Application.logMessageReceivedThreaded -= OnUnityLogReceived;
        }

        void OnUnityLogReceived(string log, string backTrace, LogType type)
        {
            if (!m_LogArea)
                return;
            switch (type)
            {
                case LogType.Error:
                case LogType.Exception:
                    m_LogArea.text += $"<color=red>{log}</color>\n";
                    break;
                case LogType.Warning:
                    m_LogArea.text += $"<color=yellow>{log}</color>\n";
                    break;
                case LogType.Log:
                    m_LogArea.text += $"{log}\n";
                    break;
            }
        }
    }
}
