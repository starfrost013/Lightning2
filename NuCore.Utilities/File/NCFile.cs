﻿namespace NuCore.Utilities
{
    /// <summary>
    /// NCFile
    /// 
    /// File utilities.
    /// </summary>
    public static class NCFile
    {
        private readonly static List<string> defaultExcludedPatterns = new()
        { ".tmp", "~$", ".g.cs", ".cache", ".editorconfig", ".props", ".targets", ".vsidx", ".lock", ".v1", ".v2", ".v5.1", 
          "dgspec", "AssemblyAttributes", ".AssemblyInfo", "assets.json", ".suo", ".pdb", ".log", "test.wad", ".tlog", ".FileListAbsolute.txt", "BuildWithSkipAnalyzers",
          ".0\\apphost.exe" };

        /// <summary>
        /// Recursively copies files from one directory to another.
        /// </summary>
        /// <param name="sourceDir">The source directory to copy from.</param>
        /// <param name="destinationDir">The destination directory to copy from.</param>
        /// <param name="originalDir">Internal: The original directory</param>
        /// <param name="excludedPatterns">Patterns that are excluded</param>
        public static void RecursiveCopy(string sourceDir, string destinationDir = ".", List<string>? excludedPatterns = null)
        {
            // default exclude VS build artifacts
            if (excludedPatterns == null) excludedPatterns = defaultExcludedPatterns;

            foreach (string fileName in Directory.EnumerateFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                string relativeDestinationPath = fileName.Replace(sourceDir, "");
                // determine if we will copy
                bool performCopy = true;

                foreach (string excludedPattern in excludedPatterns)
                {
                    // skip any excluded pattern
                    if (fileName.Contains(excludedPattern, StringComparison.InvariantCultureIgnoreCase)) performCopy = false;
                }

                string finalPath = $"{destinationDir}\\{relativeDestinationPath}";
                string finalDirectory = finalPath.Substring(0, finalPath.LastIndexOf(Path.DirectorySeparatorChar));

                if (!Directory.Exists(finalDirectory)) Directory.CreateDirectory(finalDirectory);
                if (performCopy) File.Copy(fileName, finalPath, true);
            }
        }

        public static bool IsValidPath(this string str)
        {
            return (str.IndexOfAny(Path.GetInvalidFileNameChars()) == -1)
                && (str.IndexOfAny(Path.GetInvalidPathChars()) == -1);
        }
    }
}