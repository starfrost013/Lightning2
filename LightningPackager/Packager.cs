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
        public static bool LoadPackage(string path, string outDir)
        {
            NCLogging.Log($"Loading WAD file from {path}...");

            // delete any existing package
            if (!File.Exists(path))
            {
                _ = new NCException($"Error: {path} does not exist, cannot load package!", 100, "Packager::LoadPackage path parameter does not exist!", NCExceptionSeverity.Error, null, true);
                return false;
            }

            PackageFileMetadata metadata = new PackageFileMetadata();

            PackageFile packageFile = new PackageFile(metadata);

            // extract the package
            return packageFile.Extract(path, outDir);
        }

        public static bool GeneratePackage(PackageFile packageFile, 
            string path)
        {
            NCLogging.Log("Generating WAD file...");

            // delete any files that may exist at our path
            if (File.Exists(path)) File.Delete(path);

            try
            {
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