using NuCore.Utilities;

namespace LightningPackager
{
    /// <summary>
    /// PackageFile
    /// 
    /// basic (.wad) file format for packaging Lightning games. Uses LZMA + a basic file catalog. Supports folders.  
    /// </summary>
    public class PackageFile
    {
        public PackageHeader Header { get; set; }

        public PackageFileCatalog Catalog { get; set; }

        public PackageFile()
        {
            Header = new PackageHeader();
            Catalog = new PackageFileCatalog(); 
        }

        public void AddEntry(PackageFileCatalogEntry entry) => Catalog.AddEntry(entry);

        public void Write(string path)
        {
            NCLogging.Log($"Generating WAD file and writing it to {path}...");

            using (BinaryWriter stream = new BinaryWriter(new FileStream(path, FileMode.Create)))
            {
                NCLogging.Log("Writing header...");
                Header.Write(stream);

                NCLogging.Log("Writing file catalog...");
                Catalog.Write(stream, Header.HeaderSize);
            }

        }
    }
}
