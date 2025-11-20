/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

#if UNITY_EDITOR
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Inworld
{
    public class DependencyDownloaderWindow : EditorWindow
    {
        const string k_PluginsPath = "Assets/InworldRuntime/Plugins";
        const string k_StreamingAssetsPath = "Assets/StreamingAssets";

        const string k_PluginDownloadUrl =
            "https://storage.googleapis.com/assets-inworld-ai/unity-packages/Plugins_250815.zip";
        const string k_StreamingAssetsDownloadUrl = "https://storage.googleapis.com/assets-inworld-ai/unity-packages/StreamingAssets.zip";

        public static void ShowWindow()
        {
            DependencyDownloaderWindow window = GetWindow<DependencyDownloaderWindow>(true, "Dependency Downloader", true);
            window.minSize = new Vector2(460, 180);
            window.maxSize = new Vector2(800, 300);
            window.ShowUtility();
        }

        async void OnGUI()
        {
            bool missingPlugins = !Directory.Exists(k_PluginsPath);
            bool missingStreaming = !Directory.Exists(k_StreamingAssetsPath);
            EditorGUILayout.Space(4);

            if (!missingPlugins && !missingStreaming)
            {
                EditorGUILayout.HelpBox("DLLs and Models are downloaded", MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox(
                    "- Assets/InworldRuntime/Plugins\n- Assets/StreamingAssets",
                    MessageType.Warning);
            }

            EditorGUILayout.Space(6);

            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledScope(!missingPlugins))
                {
                    if (GUILayout.Button("Download Plugins", GUILayout.Height(28)))
                    {
                        await DownloadAndInstall(k_PluginDownloadUrl, k_PluginsPath, "Plugins");
                    }
                }
                using (new EditorGUI.DisabledScope(!missingStreaming))
                {
                    if (GUILayout.Button("Download StreamingAssets", GUILayout.Height(28)))
                    {
                        await DownloadAndInstall(k_StreamingAssetsDownloadUrl, k_StreamingAssetsPath, "StreamingAssets");
                    }
                }
                if (GUILayout.Button("Later", GUILayout.Height(28)))
                {
                    Close();
                }
            }

            EditorGUILayout.Space(6);

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                DrawStatusRow("Assets/InworldRuntime/Plugins", !missingPlugins);
                DrawStatusRow("Assets/StreamingAssets", !missingStreaming);
            }
        }

        static void DrawStatusRow(string label, bool exists)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(label);
                GUILayout.FlexibleSpace();
                GUIContent status = exists ? new GUIContent("Exist") : new GUIContent("Missing");
                Color old = GUI.color;
                GUI.color = exists ? new Color(0.25f, 0.7f, 0.3f) : new Color(0.85f, 0.4f, 0.2f);
                GUILayout.Label(status);
                GUI.color = old;
            }
        }
        
        async Task DownloadAndInstall(string zipUrl, string targetFolder, string expectedTopLevel)
        {
            string tempDir = null;
            string zipPath = null;
            string extractDir = null;
            try
            {
                Repaint();
                EditorUtility.DisplayProgressBar("Downloading", $"Start Downloading {targetFolder}...", 0.1f);

                tempDir = Path.Combine(Path.GetTempPath(), "InworldUnityTemp");
                if (!Directory.Exists(tempDir))
                    Directory.CreateDirectory(tempDir);
                zipPath = Path.Combine(tempDir, Guid.NewGuid().ToString("N") + ".zip");
                extractDir = Path.Combine(tempDir, Guid.NewGuid().ToString("N"));

                using (HttpClient httpClient = new HttpClient())
                await using (Stream httpStream = await httpClient.GetStreamAsync(zipUrl))
                await using (FileStream fileStream = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await httpStream.CopyToAsync(fileStream);
                }

                EditorUtility.DisplayProgressBar("Unzipping", $"Start Unzipping {targetFolder}...", 0.6f);
                Directory.CreateDirectory(extractDir);
                ZipFile.ExtractToDirectory(zipPath, extractDir);

                if (!Directory.Exists(targetFolder))
                    Directory.CreateDirectory(targetFolder);

                string candidate = Path.Combine(extractDir, expectedTopLevel);
                string sourceToMerge = Directory.Exists(candidate) ? candidate : extractDir;

                EditorUtility.DisplayProgressBar("Installing", "Copy files to directory...", 0.85f);
                MergeDirectories(sourceToMerge, targetFolder);

                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("Finished", $"Extracted to {targetFolder}", "OK");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                EditorUtility.DisplayDialog("Failed", e.Message, "OK");
            }
            finally
            {
                EditorUtility.ClearProgressBar();
                Repaint();
                try
                {
                    if (!string.IsNullOrEmpty(zipPath) && File.Exists(zipPath))
                        File.Delete(zipPath);
                    if (!string.IsNullOrEmpty(extractDir) && Directory.Exists(extractDir))
                        Directory.Delete(extractDir, true);
                }
                catch (Exception cleanupErr)
                {
                    Debug.LogWarning($"Clear temp file failed: {cleanupErr.Message}");
                }
            }
        }

        static void MergeDirectories(string sourceDir, string destDir)
        {
            foreach (string dir in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
            {
                string relative = dir.Substring(sourceDir.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                string target = Path.Combine(destDir, relative);
                if (!Directory.Exists(target))
                    Directory.CreateDirectory(target);
            }
            foreach (string file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                string relative = file.Substring(sourceDir.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                string target = Path.Combine(destDir, relative);
                string targetDir = Path.GetDirectoryName(target);
                if (!string.IsNullOrEmpty(targetDir) && !Directory.Exists(targetDir))
                    Directory.CreateDirectory(targetDir);
                File.Copy(file, target, true);
            }
        }
    }
}
#endif


