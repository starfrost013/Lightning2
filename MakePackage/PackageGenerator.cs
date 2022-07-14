using LightningPackager;
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
            if (!Directory.Exists(CommandLine.InFolder))
            {
                NCLogging.Log($"Error: The directory {CommandLine.InFolder} does not exist!", ConsoleColor.Red, false, false);
                return false;
            }

            PackageFile packageFile = new PackageFile();

            foreach (string fileName in Directory.GetFiles(CommandLine.InFolder))
            {
                packageFile.AddEntry(new PackageFileCatalogEntry(fileName));
            }

            return Packager.GeneratePackage(packageFile,
                CommandLine.OutFile,
                CommandLine.Name, 
                CommandLine.GameVersion,
                CommandLine.EngineVersion);
        }

        public static void AddRecursively(PackageFile packageFile, string curDirectory = null)
        {
            if (curDirectory == null) curDirectory = CommandLine.InFolder;

            foreach (string fileName in Directory.GetFiles(curDirectory))
            {
                packageFile.AddEntry(new PackageFileCatalogEntry(fileName));
            }

            foreach (string dirName in Directory.GetDirectories(curDirectory))
            {
                if (Directory.GetDirectories(dirName).Length > 0)
                {
                    AddRecursively(packageFile, curDirectory);
                }
            }
        }

    }

}
