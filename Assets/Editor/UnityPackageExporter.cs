/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Inworld
{
	/// <summary>
	///     This file would be called by commands, for auto-generate Unity packages.
	/// </summary>
	public static class UnityPackageExporter
    {
        const string k_EditorPath = "Assets/Editor";
        const string k_PackageName = "InworldRuntime";
        const string k_PackagePath = "Assets/InworldRuntime.unitypackage";

        // The path to the package under the `Assets/` folder.
        const string k_FullPackagePath = "Assets/InworldRuntime";
        const string k_ExtraPackagePath = "Assets/InworldExtraAssets.unitypackage";

        /// <summary>
        ///     Call it via outside command line to export package.
        /// </summary>
        [MenuItem("Inworld/Export Package/Runtime/Full", false, 102)]
        public static void ExportFull()
        {
            ExportExtraAssets();
            string[] assetPaths =
            {
                k_EditorPath,
                k_ExtraPackagePath
            }; 
            AssetDatabase.ExportPackage(assetPaths, k_PackagePath, ExportPackageOptions.Recurse);
        }

        [MenuItem("Inworld/Export Package/Runtime/Extra Assets", false, 101)]
        public static void ExportExtraAssets()
        {
            List<string> folders = new List<string>(AssetDatabase.GetSubFolders("Assets/InworldRuntime"));
            folders.Remove("Assets/InworldRuntime/Plugins");
            AssetDatabase.ExportPackage(folders.ToArray(), k_ExtraPackagePath, ExportPackageOptions.Recurse); 
        }
    }
}
