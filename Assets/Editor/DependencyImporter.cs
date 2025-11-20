/*************************************************************************************************
 * Copyright 2022-2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/
#if UNITY_EDITOR
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Inworld
{
    [InitializeOnLoad]
    public class DependencyImporter : AssetPostprocessor
    {
        const string k_PkgName = "InworldRuntime";
        const string k_ExtraAssets = "InworldExtraAssets";
        const string k_ExtraAssetsPkgPath = "Assets/InworldExtraAssets.unitypackage";
        const string k_InworldPath = "Assets/InworldRuntime";

        
        static readonly string[] s_DependentPackages = {
            "com.unity.nuget.newtonsoft-json",
            "com.unity.cloud.gltfast"
        };
        
        static DependencyImporter()
        {
            AssetDatabase.importPackageCompleted += name =>
            {
                if (name.Contains(k_PkgName))
                    InstallDependencies();
                else if (name == k_ExtraAssets)
                {
                    _InstallTMP();
                    CheckDllModels();
                }
            };
        }

        [MenuItem("Inworld/Install Dependencies")]
        public static async void InstallDependencies()
        {
            Debug.Log("Import Dependency Packages...");
            foreach (string dependentPackage in s_DependentPackages)
            {
                await _AddUnityPackage(dependentPackage);
            }
            if (!Directory.Exists(k_InworldPath) 
                && File.Exists(k_ExtraAssetsPkgPath))
                AssetDatabase.ImportPackage(k_ExtraAssetsPkgPath, false);
        }
        
        [MenuItem("Inworld/Check DLL and Models")]
        public static void CheckDllModels()
        {
            Debug.Log("Check DLL and Models...");
            DependencyDownloaderWindow.ShowWindow();
        }

        static async Task _AddUnityPackage(string package)
        {
            ListRequest listRequest = Client.List();

            while (!listRequest.IsCompleted)
            {
                await Task.Yield();
            }
            if (listRequest.Status != StatusCode.Success)
            {
                Debug.LogError(listRequest.Error.ToString());
                return;
            }
            if (listRequest.Result.Any(x => x.name == package))
            {
                Debug.Log($"{package} Found.");
                return;
            }
            AddRequest addRequest = Client.Add(package);
            while (!addRequest.IsCompleted)
            {
                await Task.Yield();
            }
            if (addRequest.Status != StatusCode.Success)
            {
                Debug.LogError($"Failed to add {package}.");
                return;
            }
            Debug.Log($"Import {package} Completed");
        }
        
        static void _InstallTMP()
        {
            if (File.Exists("Assets/TextMesh Pro/Resources/TMP Settings.asset"))
                return;
            string packageFullPath = TMP_EditorUtility.packageFullPath;
            AssetDatabase.ImportPackage(packageFullPath + "/Package Resources/TMP Essential Resources.unitypackage", false);
        }
    }
}
#endif
