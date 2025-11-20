using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Inworld
{
    public class InworldBuildProcessor: IPostprocessBuildWithReport
    {
        // Work on it as late as possible.
        public int callbackOrder => 900;

        public void OnPostprocessBuild(BuildReport report)
        {
            // YAN: Currently only works for Windows.
            if (report.summary.platform != BuildTarget.StandaloneWindows &&
                report.summary.platform != BuildTarget.StandaloneWindows64)
                return;

            string srcDir = "Assets/InworldRuntime/Plugins/icu_data";
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
    }
}