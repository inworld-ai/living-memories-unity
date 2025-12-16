using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;


namespace Inworld
{
    public class InworldBuildProcessor: IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        // Work on it as late as possible.
        public int callbackOrder => 900;
        const string k_StreamingAssetsRoot = "Assets/StreamingAssets";
        const string k_BackupRoot         = "Assets/StreamingAssets_AndroidBackup";
        static readonly string[] s_FoldersToKeep = { "safety", "vad" };
        public void OnPreprocessBuild(BuildReport report)
        {
            if (report.summary.platform != BuildTarget.Android && report.summary.platform != BuildTarget.iOS)
                return;

            if (!AssetDatabase.IsValidFolder(k_StreamingAssetsRoot))
                return;

            PrepareBackupFolder();
            MoveUnwantedStreamingAssetsToBackup();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(); 
        }
        
        public void OnPostprocessBuild(BuildReport report)
        {
            switch (report.summary.platform)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    CopyICUData(report);
                    break;
                case BuildTarget.iOS:
                case BuildTarget.Android:
                    Debug.Log($"Summary Path: {report.summary.outputPath}");
                    // Restore everything that was moved out before the build.
                    RestoreBackupToStreamingAssets();
                    DeleteBackupFolder();
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    break;
            }
        }

        static void CopyICUData(BuildReport report)
        {
            string srcDir = "Assets/InworldRuntime/Plugins/Windows/icu_data";
            if (!Directory.Exists(srcDir))
                return;
            string outputPath = report.summary.outputPath;
            string buildDir = Path.GetDirectoryName(outputPath) ?? "";
            string exeNameNoExt = Path.GetFileNameWithoutExtension(outputPath);
            string dataDir = Path.Combine(buildDir, exeNameNoExt + "_Data");
            string pluginsDir = Path.Combine(dataDir, "Plugins");
            string pluginsX64Dir = Path.Combine(pluginsDir, "x86_64");

            CopyDirectory(srcDir, Path.Combine(pluginsDir, "icu_data"));
            CopyDirectory(srcDir, Path.Combine(pluginsX64Dir, "icu_data"));
        }

        static void CopyDirectory(string sourceDir, string targetDir)
        {
            if (!Directory.Exists(sourceDir))
                return;

            if (Directory.Exists(targetDir))
                Directory.Delete(targetDir, true);

            Directory.CreateDirectory(targetDir);

            foreach (string dirPath in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourceDir, targetDir));
            }
            foreach (string filePath in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                string destFile = filePath.Replace(sourceDir, targetDir);
                File.Copy(filePath, destFile, true);
            }
        }
        
        static void MoveUnwantedStreamingAssetsToBackup()
        {
            // Move subfolders
            string[] subFolders = AssetDatabase.GetSubFolders(k_StreamingAssetsRoot);
            foreach (string folder in subFolders)
            {
                string folderName = Path.GetFileName(folder);
                if (ShouldKeepFolder(folderName))
                    continue;

                string targetPath = Path.Combine(k_BackupRoot, folderName).Replace("\\", "/");
                MoveAssetIfPossible(folder, targetPath);
            }

            // Move loose files under StreamingAssets root
            string[] assetGuids = AssetDatabase.FindAssets(string.Empty, new[] { k_StreamingAssetsRoot });
            foreach (string guid in assetGuids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                // Skip folders and already-moved paths
                if (AssetDatabase.IsValidFolder(assetPath))
                    continue;
                if (!IsDirectChildOf(assetPath, k_StreamingAssetsRoot))
                    continue;

                string fileName = Path.GetFileName(assetPath);
                string targetPath = Path.Combine(k_BackupRoot, fileName).Replace("\\", "/");
                MoveAssetIfPossible(assetPath, targetPath);
            }
        }
        
        static void PrepareBackupFolder()
        {
            if (!AssetDatabase.IsValidFolder("Assets"))
                return;

            if (!AssetDatabase.IsValidFolder(k_BackupRoot))
            {
                string parent = "Assets";
                string newFolderName = Path.GetFileName(k_BackupRoot);
                AssetDatabase.CreateFolder(parent, newFolderName);
            }
        }
        
        static void RestoreBackupToStreamingAssets()
        {
            if (!AssetDatabase.IsValidFolder(k_BackupRoot))
                return;

            // Restore subfolders and files from backup root.
            string[] subFolders = AssetDatabase.GetSubFolders(k_BackupRoot);
            foreach (string folder in subFolders)
            {
                string folderName = Path.GetFileName(folder);
                string targetPath = Path.Combine(k_StreamingAssetsRoot, folderName).Replace("\\", "/");
                MoveAssetIfPossible(folder, targetPath);
            }

            string[] assetGuids = AssetDatabase.FindAssets(string.Empty, new[] { k_BackupRoot });
            foreach (string guid in assetGuids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                // Skip folders; they were handled above.
                if (AssetDatabase.IsValidFolder(assetPath))
                    continue;
                if (!IsDirectChildOf(assetPath, k_BackupRoot))
                    continue;

                string fileName = Path.GetFileName(assetPath);
                string targetPath = Path.Combine(k_StreamingAssetsRoot, fileName).Replace("\\", "/");
                MoveAssetIfPossible(assetPath, targetPath);
            }
        }

        static void DeleteBackupFolder()
        {
            if (!AssetDatabase.IsValidFolder(k_BackupRoot))
                return;

            AssetDatabase.DeleteAsset(k_BackupRoot);
        }
        
        static bool ShouldKeepFolder(string folderName)
        {
            foreach (string keep in s_FoldersToKeep)
            {
                if (string.Equals(folderName, keep, System.StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        static bool IsDirectChildOf(string assetPath, string parentFolder)
        {
            if (!assetPath.StartsWith(parentFolder + "/", System.StringComparison.OrdinalIgnoreCase))
                return false;

            string relative = assetPath.Substring(parentFolder.Length + 1);
            return !relative.Contains("/");
        }

        static void MoveAssetIfPossible(string from, string to)
        {
            if (!AssetDatabase.LoadMainAssetAtPath(from))
                return;

            // If the target already exists, delete it first to avoid MoveAsset errors.
            if (AssetDatabase.LoadMainAssetAtPath(to))
            {
                AssetDatabase.DeleteAsset(to);
            }
            AssetDatabase.MoveAsset(from, to);
        }
    }
}