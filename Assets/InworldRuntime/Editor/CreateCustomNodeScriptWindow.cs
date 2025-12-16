#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Inworld.Framework.Editor
{
    public class CreateCustomNodeScriptWindow : EditorWindow
    {
        string m_ClassName = "MyCustomNodeAsset";
        string m_TargetFolder = "Assets";
        Action<string> m_OnCreated;

        public static void Show(string title, Action<string> onCreated)
        {
            CreateCustomNodeScriptWindow window = CreateInstance<CreateCustomNodeScriptWindow>();
            window.titleContent = new GUIContent(title);
            window.minSize = new Vector2(460, 140);
            window.maxSize = new Vector2(700, 220);
            window.m_OnCreated = onCreated;
            Rect rect = new Rect(Screen.width / 2f - 230, Screen.height / 2f - 70, 460, 140);
            window.position = rect;
            window.ShowPopup();
        }

        void OnGUI()
        {
            GUILayout.Label("Create a new CustomNode Script", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            m_ClassName = EditorGUILayout.TextField("Class Name:", m_ClassName);
            using (new EditorGUILayout.HorizontalScope())
            {
                m_TargetFolder = EditorGUILayout.TextField("Save at", m_TargetFolder);
                if (GUILayout.Button("Select...", GUILayout.Width(80)))
                {
                    string selected = EditorUtility.OpenFolderPanel("Select target direactory", Application.dataPath, "");
                    if (!string.IsNullOrEmpty(selected))
                    {
                        if (selected.StartsWith(Application.dataPath))
                        {
                            m_TargetFolder = "Assets" + selected.Substring(Application.dataPath.Length);
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Invalid Path", "We can only support path within Assets/", "OK");
                        }
                    }
                }
            }

            EditorGUILayout.Space();
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Create", GUILayout.Height(26)))
                {
                    if (ValidateInputs() && CreateScript())
                    {
                        Close();
                        m_OnCreated?.Invoke(m_ClassName);
                    }
                }
                if (GUILayout.Button("Cancel", GUILayout.Height(26)))
                {
                    Close();
                }
            }
        }

        bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(m_ClassName))
            {
                EditorUtility.DisplayDialog("Error", "ClassName cannot be null", "OK");
                return false;
            }
            if (!m_ClassName.EndsWith("NodeAsset", StringComparison.Ordinal))
            {
                if (!EditorUtility.DisplayDialog("Name convention",
                        "It's better to end with 'NodeAsset', like MyOperationNodeAsset. Do you want to continue?",
                        "OK", "Cancel"))
                    return false;
            }
            if (string.IsNullOrEmpty(m_TargetFolder) || !AssetDatabase.IsValidFolder(m_TargetFolder))
            {
                EditorUtility.DisplayDialog("Error", "Invalid Path. Please save under Assets/", "OK");
                return false;
            }
            string targetPath = Path.Combine(m_TargetFolder, m_ClassName + ".cs");
            if (File.Exists(targetPath))
            {
                EditorUtility.DisplayDialog("Error", "File exist. Please choose a different name", "OK");
                return false;
            }
            return true;
        }

        bool CreateScript()
        {
            try
            {
                SaveCurrentGraph();
                string script = GetTemplate(m_ClassName);
                string targetPath = Path.Combine(m_TargetFolder, m_ClassName + ".cs");
                File.WriteAllText(targetPath, script);
                AssetDatabase.ImportAsset(targetPath);
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<MonoScript>(targetPath);
                EditorGUIUtility.PingObject(Selection.activeObject);
                // Open in the configured external script editor
                if (Selection.activeObject)
                    AssetDatabase.OpenAsset(Selection.activeObject);
                // Close Graph Editor window as requested
                CloseGraphEditorWindow();
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[InworldFramework] Create script failed.: {ex.Message}");
                EditorUtility.DisplayDialog("Error", "Create script failed. For more information, please check Console。", "OK");
                return false;
            }
        }

        static void SaveCurrentGraph()
        {
            InworldGraphEditorWindow[] windows = Resources.FindObjectsOfTypeAll<InworldGraphEditorWindow>();
            if (windows == null || windows.Length <= 0) 
                return;
            try
            {
                windows[0].SaveCurrentGraph();
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[InworldFramework] Autosave Graph Failed: {ex.Message}");
            }
        }
        
        static string GetTemplate(string className)
        {
            return "using Inworld.Framework;\n" +
                   "using Inworld.Framework.Graph;\n" +
                   "using UnityEngine;\n\n" +
                   "// Create custom node inherit from CustomNodeAsset。\n" +
                   "// Rewrite ProcessBaseData for your own logic. \n" +
                   $"public class {className} : CustomNodeAsset\n" +
                   "{\n" +
                   "    public override string NodeTypeName => \"" + className + "\";\n\n" +
                   "    protected override InworldBaseData ProcessBaseData(InworldVector<InworldBaseData> inputs)\n" +
                   "    {\n" +
                   "        // TODO: Write your logic here. For example: \n" +
                   "        if (inputs.Size == 0)\n" +
                   "            return new InworldError(\"No input data\", StatusCode.DataLoss);\n" +
                   "        return inputs[0];\n" +
                   "    }\n" +
                   "}\n";
        }
        
        static void CloseGraphEditorWindow()
        {
            InworldGraphEditorWindow[] windows = Resources.FindObjectsOfTypeAll<InworldGraphEditorWindow>();
            if (windows == null || windows.Length == 0)
                return;
            foreach (InworldGraphEditorWindow w in windows)
                w.Close();
        }
    }
}
#endif


