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
using UnityEngine.UIElements;


namespace Inworld.Framework.Editor
{
    public class InworldGraphEditorWindow : EditorWindow
    {
        InworldGraphView m_GraphView;
        [SerializeField] InworldGraphAsset m_GraphAsset;
        const string k_LastGraphGuidKey = "InworldGraphEditorWindow_LastGraphGUID";
        
        [MenuItem("Inworld/Graph Editor")]
        public static void OpenWindow()
        {
            var window = GetWindow<InworldGraphEditorWindow>();
            window.titleContent = new GUIContent("Inworld Graph Editor");
        }

        void CreateGUI()
        {
            CreateGraphView();
            CreateToolbar();
            RestoreGraphOnCreateGUI();
        }

        void CreateGraphView()
        {
            m_GraphView = new InworldGraphView(this);
            m_GraphView.StretchToParentSize();
            rootVisualElement.Add(m_GraphView);
        }

        void CreateToolbar()
        {
            VisualElement toolbar = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f),
                    paddingLeft = 5,
                    paddingRight = 5,
                    paddingTop = 5,
                    paddingBottom = 5,
                    borderBottomWidth = 1,
                    borderBottomColor = new Color(0.1f, 0.1f, 0.1f, 1f)
                }
            };
            
            Button saveButton = new Button(() => SaveGraph())
            {
                text = "Save",
                style =
                {
                    marginRight = 5
                }
            };

            Button loadButton = new Button(() => LoadGraph())
            {
                text = "Load",
                style =
                {
                    marginRight = 5
                }
            };
            toolbar.Add(saveButton);
            toolbar.Add(loadButton);
            rootVisualElement.Add(toolbar);
        }

        void SaveGraph()
        {
            if (m_GraphAsset == null) 
                return;
            m_GraphView.SaveGraph(m_GraphAsset);
            foreach (InworldNodeAsset nodeAsset in m_GraphAsset.Nodes)
            {
                EditorUtility.SetDirty(nodeAsset);
            }
            EditorUtility.SetDirty(m_GraphAsset);
            AssetDatabase.SaveAssets();
            // Export JSON alongside .asset using unity_character_engine style (minimal main graph).
            string dataDir = $"Assets/Data/{m_GraphAsset.Name}";
            if (!AssetDatabase.IsValidFolder("Assets/Data"))
                AssetDatabase.CreateFolder("Assets", "Data");
            if (!AssetDatabase.IsValidFolder(dataDir))
                AssetDatabase.CreateFolder("Assets/Data", m_GraphAsset.Name);
            string path = AssetDatabase.GetAssetPath(m_GraphAsset);
            if (!string.IsNullOrEmpty(path))
            {
                string guid = AssetDatabase.AssetPathToGUID(path);
                if (!string.IsNullOrEmpty(guid))
                    EditorPrefs.SetString(k_LastGraphGuidKey, guid);
            }
            if (!m_GraphAsset.NeedSaveAsJson) 
                return;
            string outputPath = $"{dataDir}/{m_GraphAsset.Name}.json";
            m_GraphAsset.SaveJson(outputPath);
            
        }

        void LoadGraph()
        {
            string path = EditorUtility.OpenFilePanel("Load Graph", "Assets", "asset");
            if (string.IsNullOrEmpty(path)) 
                return;
            path = FileUtil.GetProjectRelativePath(path);
            m_GraphAsset = AssetDatabase.LoadAssetAtPath<InworldGraphAsset>(path);
            if (m_GraphAsset == null) 
                return;
            m_GraphView.LoadGraph(m_GraphAsset);
            string guid = AssetDatabase.AssetPathToGUID(path);
            if (!string.IsNullOrEmpty(guid))
                EditorPrefs.SetString(k_LastGraphGuidKey, guid);
        }

        public void SetGraphAsset(InworldGraphAsset graphAsset)
        {
            m_GraphAsset = graphAsset;
            m_GraphView?.LoadGraph(m_GraphAsset);
            string path = AssetDatabase.GetAssetPath(m_GraphAsset);
            if (string.IsNullOrEmpty(path)) 
                return;
            string guid = AssetDatabase.AssetPathToGUID(path);
            if (!string.IsNullOrEmpty(guid))
                EditorPrefs.SetString(k_LastGraphGuidKey, guid);
        }
        
        public void SaveCurrentGraph() => SaveGraph();

        void RestoreGraphOnCreateGUI()
        {
            if (m_GraphAsset)
            {
                m_GraphView.LoadGraph(m_GraphAsset);
                return;
            }
            string guid = EditorPrefs.GetString(k_LastGraphGuidKey, string.Empty);
            if (string.IsNullOrEmpty(guid))
                return;
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path))
                return;
            InworldGraphAsset asset = AssetDatabase.LoadAssetAtPath<InworldGraphAsset>(path);
            if (!asset) 
                return;
            m_GraphAsset = asset;
            m_GraphView.LoadGraph(m_GraphAsset);
        }
    }
}
#endif