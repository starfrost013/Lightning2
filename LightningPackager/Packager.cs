using NuCore.Utilities;

namespace LightningPackager
{
    /// <summary>
    /// Packager
    /// 
    /// July 2, 2022: Defines the main class of the Lightning packager
    /// </summary>
    public static class Packager
    {
        public static bool LoadPackage(string path)
        {
            NCLogging.Log($"Loading WAD file from {path}...");

            // delete any existing package
            if (File.Exists(path)) File.Delete(path);
            return true; // temp
        }

        public static bool GeneratePackage(PackageFile packageFile, 
            string path, PackageFileMetadata metadata)
        {
            NCLogging.Log("Generating WAD file...");

            try
            {
                packageFile.Header.Metadata.Name = metadata.Name;
                packageFile.Header.Metadata.GameVersion = metadata.GameVersion;
                packageFile.Header.Metadata.EngineVersion = metadata.EngineVersion;
                packageFile.Write(path);
                return true; 
            }
            catch (Exception err)
            {
                NCLogging.Log($"A fatal error occurred while generating a package: \n\n{err}");
                return false;
            }

        }
    }
}