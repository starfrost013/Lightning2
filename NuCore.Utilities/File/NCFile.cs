using System.Collections.Generic;
using System.IO;

namespace NuCore.Utilities
{
    public static class NCFile
    {
        public static void RecursiveCopy(string sourceDir, string destinationDir = null, List<string> excludedPatterns = null)
        {
            if (destinationDir == null) destinationDir = ".";
            if (excludedPatterns == null) excludedPatterns = new List<string> { ".tmp", "~$"};

            // this is really really bad
            foreach (string dirName in Directory.GetDirectories(sourceDir))
            {
                foreach (string fileName in Directory.GetFiles(dirName))
                {
                    string finalDirectory = destinationDir.Replace(sourceDir, "");
                    string finalPath = fileName.Replace(sourceDir, "");
                    string finalRelativeDirectory = Path.Combine(finalDirectory, dirName.Replace(sourceDir, ""));

                    // determine if we will copy
                    bool performCopy = true;
                    // check all excluded patterns
                    foreach (string excludedPattern in excludedPatterns)
                    {
                        // skip any excluded pattern
                        if (fileName.Contains(excludedPattern))
                        {
                            performCopy = false;
                        }
                    }

                    // create the directory if it does not exist
                    if (!Directory.Exists(finalRelativeDirectory)) Directory.CreateDirectory(finalRelativeDirectory);
                    if (performCopy) File.Copy(fileName, Path.Combine(finalDirectory, finalPath), true);

                }

                if (Directory.GetDirectories(dirName).Length > 0) RecursiveCopy(dirName);
            }
        }
    }
}
