using System.Collections.Generic;
using System.IO;

namespace NuCore.Utilities
{
    public static class NCFile
    {
        public static void RecursiveCopy(string sourceDir, string destinationDir = null, List<string> excludedPatterns = null)
        {
            if (destinationDir == null) destinationDir = ".";
            if (excludedPatterns == null) excludedPatterns = new List<string> { ".tmp", "~$" };

            // this is really really bad
            foreach (string dirName in Directory.GetDirectories(sourceDir))
            {
                foreach (string fileName in Directory.GetFiles(dirName))
                {
                    string destinationDirectory = destinationDir.Replace(sourceDir, "");
                    string destinationPath = fileName.Replace($"..{Path.DirectorySeparatorChar}", "");
                    
                    // don't duplicate the first directory
                    destinationPath = destinationPath.Substring(destinationPath.IndexOf(Path.DirectorySeparatorChar));
                    // determine if we will copy
                    bool performCopy = true;
                    // check all excluded patterns
                    foreach (string excludedPattern in excludedPatterns)
                    {
                        // skip any excluded pattern
                        if (fileName.Contains(excludedPattern)) performCopy = false;
                    }

                    string finalFilePath = $@"{destinationDirectory}\{destinationPath}";
                    string finalDirPath = finalFilePath.Substring(0, finalFilePath.LastIndexOf(Path.DirectorySeparatorChar));

                    // create the directory if it does not exist
                    if (!Directory.Exists(finalDirPath)) Directory.CreateDirectory(finalDirPath);

                    if (performCopy) File.Copy(fileName, finalFilePath, true);
                }

                if (Directory.GetDirectories(dirName).Length > 0) RecursiveCopy(dirName, destinationDir);
            }
        }
    }
}
