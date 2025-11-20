/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

#if UNITY_EDITOR
using Inworld.Framework.Graph;
using UnityEditor;
using UnityEngine;

namespace Inworld.Framework.Editor
{
    [CustomEditor(typeof(InworldGraphAsset))]
    public class InworldGraphAssetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Space(10);

            if (!GUILayout.Button("Open in Graph Editor")) 
                return;
            InworldGraphEditorWindow window = EditorWindow.GetWindow<InworldGraphEditorWindow>();
            window.SetGraphAsset(target as InworldGraphAsset);
        }
    }
}
#endif