using System.Collections.Generic;
using System.IO;

namespace NuCore.Utilities
{
    /// <summary>
    /// NCFile
    /// 
    /// August 11, 2022
    /// 
    /// File utilities.
    /// </summary>
    public static class NCFile
    {
        /// <summary>
        /// Recursively copies files.
        /// </summary>
        /// <param name="sourceDir">The source directory to copy from.</param>
        /// <param name="destinationDir">The destination directory to copy from.</param>
        /// <param name="originalDir">Internal: The original directory</param>
        /// <param name="excludedPatterns">Patterns that are excluded</param>
        public static void RecursiveCopy(string sourceDir, string destinationDir = null, string originalDir = null, List<string> excludedPatterns = null)
        {
            if (destinationDir == null) destinationDir = ".";
            if (excludedPatterns == null) excludedPatterns = new List<string> { ".tmp", "~$", ".g.cs", ".cache", 
                ".editorconfig", ".props", ".targets", ".vsidx", ".lock", ".v1", ".v2", "dgspec", "AssemblyAttributes", 
                "basic.AssemblyInfo", "assets.json", ".suo" };

            if (originalDir == null) originalDir = sourceDir;

            // this is really really bad
            foreach (string dirName in Directory.GetDirectories(sourceDir))
            {
                foreach (string fileName in Directory.GetFiles(dirName))
                {
                    string relativeDestinationPath = fileName.Replace(originalDir, "");
                    // determine if we will copy
                    bool performCopy = true;

                    foreach (string excludedPattern in excludedPatterns)
                    {
                        // skip any excluded pattern
                        if (fileName.Contains(excludedPattern)) performCopy = false;
                    }

                    string finalPath = $"{destinationDir}\\{relativeDestinationPath}";
                    string finalDirectory = finalPath.Substring(0, finalPath.LastIndexOf(Path.DirectorySeparatorChar));

                    if (!Directory.Exists(finalDirectory)) Directory.CreateDirectory(finalDirectory);
                    if (performCopy) File.Copy(fileName, finalPath, true);
                }

                if (Directory.GetDirectories(dirName).Length > 0) RecursiveCopy(dirName, destinationDir, originalDir);
            }
        }
    }
}