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
        /// <summary>
        /// Relative path to the game's content directory.
        /// If no package is loaded, this value is <c>null</c>, otherwise it is a relative path to the game's content directory.
        /// </summary>
        public static string ContentDirectory { get; private set; }

        public static bool LoadPackage(string path, string outDir = ".")
        {
            NCLogging.Log($"Loading WAD file from {path}...");

            // delete any existing package
            if (!File.Exists(path))
            {
                NCError.ShowErrorBox($"Error: {path} does not exist, cannot load package!", 100, "Packager::LoadPackage path parameter does not exist!", NCErrorSeverity.Error, null, true);
                return false;
            }

            PackageFileMetadata metadata = new PackageFileMetadata();

            PackageFile packageFile = new PackageFile(metadata);

            bool successful = packageFile.Extract(path, outDir);

            // extract the package
            if (successful) ContentDirectory = outDir;

            return successful;
        }

        public static bool GeneratePackage(PackageFile packageFile, string inFolder, string path)
        {
            NCLogging.Log("Generating WAD file...");

            // delete any files that may exist at our path
            if (File.Exists(path)) File.Delete(path);

            PackageFile.InFolder = inFolder;
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

        public static void Shutdown(bool deleteAll)
        {
            try
            {
                // delete the content directory recursively
                if (deleteAll)
                {
                    NCLogging.Log("Cleaning up game content directory...");

                    foreach (string fileName in Directory.EnumerateFiles(ContentDirectory, "*", SearchOption.AllDirectories))
                    {
                        if (!fileName.Contains("engine.ini", StringComparison.InvariantCultureIgnoreCase))
                        {
                            File.Delete(fileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NCError.ShowErrorBox($"An error occurred cleaning up the game content directory.\n\n{ex}", 109, 
                    $"An exception occurred in Packager::Shutdown with deleteAll = {deleteAll}", NCErrorSeverity.Warning);
            }
        }
    }
}