﻿using LightningPackager;
using NuCore.Utilities;

namespace MakePackage
{
    /// <summary>
    /// PackageGenerator
    /// 
    /// July 14, 2022
    /// 
    /// Generates a package.
    /// </summary>
    public static class PackageGenerator
    {
        public static bool StandardRun()
        {
            if (CommandLine.InFile == null)
            {
                if (!Directory.Exists(CommandLine.InFolder))
                {
                    NCLogging.Log($"Error: The directory {CommandLine.InFolder} does not exist!", ConsoleColor.Red, false, false);
                    return false;
                }

                PackageFileMetadata metadata = new PackageFileMetadata();

                metadata.Name = CommandLine.Name;
                metadata.GameVersion = CommandLine.GameVersion;
                metadata.EngineVersion = CommandLine.EngineVersion;
                metadata.CompressionMode = CommandLine.CompressionMode;

                PackageFile packageFile = new PackageFile(metadata);

                foreach (string fileName in Directory.GetFiles(CommandLine.InFolder))
                {
                    if (CommandLine.AllowBinaries
                        || (!fileName.Contains(".exe")
                        && !fileName.Contains(".dll")
                        && !fileName.Contains(".sys")
                        && !fileName.Contains(".ocx")
                        && !fileName.Contains(".scr")
                        && !fileName.Contains(".cpl")
                        && !fileName.Contains(".winmd")
                        && !fileName.Contains(".rll")))
                    {
                        string relativeFileName = fileName.Replace(CommandLine.InFolder, "");
                        packageFile.AddEntry(new PackageFileCatalogEntry(relativeFileName));
                    }
                }

                AddRecursively(packageFile);

                return Packager.GeneratePackage(packageFile,
                    CommandLine.OutFile);
            }
            else
            {
                if (!File.Exists(CommandLine.InFile))
                {
                    NCLogging.Log($"Error: The file {CommandLine.InFile} does not exist!", ConsoleColor.Red, false, false);
                    return false;
                }

                return Packager.LoadPackage(CommandLine.InFile, CommandLine.OutFolder);
            }
        }

        public static void AddRecursively(PackageFile packageFile, string curDirectory = null)
        {
            if (curDirectory == null) curDirectory = CommandLine.InFolder;

            foreach (string dirName in Directory.GetDirectories(curDirectory))
            {
                foreach (string fileName in Directory.GetFiles(dirName))
                {
                    if (CommandLine.AllowBinaries
                        || (!fileName.Contains(".exe")
                        && !fileName.Contains(".dll")
                        && !fileName.Contains(".sys")
                        && !fileName.Contains(".ocx")
                        && !fileName.Contains(".scr")
                        && !fileName.Contains(".cpl")
                        && !fileName.Contains(".winmd")
                        && !fileName.Contains(".rll")))
                    {
                        string relativeFileName = fileName.Replace(CommandLine.InFolder, "");
                        packageFile.AddEntry(new PackageFileCatalogEntry(relativeFileName));
                    }
                }

                if (Directory.GetDirectories(dirName).Length > 0)
                {
                    AddRecursively(packageFile, curDirectory);
                }
            }
        }

    }

}
