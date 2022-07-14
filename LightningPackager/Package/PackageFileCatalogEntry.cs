using NuCore.Utilities;

namespace LightningPackager
{
    /// <summary>
    /// PackageFileCatalogEntry
    /// 
    /// July 11, 2022
    /// 
    /// Defines a catalog entry for a package file.
    /// </summary>
    public class PackageFileCatalogEntry
    {
        public string Path { get; set; }

        public DateTime TimeStamp { get; set; }

        public ulong Start { get; set; }

        internal long Size { get; set; }

        internal uint Length
        {
            get
            {
                // + 1 as streamwriter puts a length byte 

                // + 8 for timestamp
                // + 16 for two ulongs
                return (uint)(Path.Length + 1) + 8 + 16; 
            }
        }

        public PackageFileCatalogEntry(string path)
        {
            Path = path;
            if (!File.Exists(Path)) _ = new NCException($"Attempted to add a non-existent file ({path}) to a PackageFileCatalog!", 96, "PackageFileCatalogEntry constructor: Path does not exist!", NCExceptionSeverity.FatalError);
            
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Path);

            FileInfo fileInfo = new FileInfo(Path);
            writer.Write(new DateTimeOffset(fileInfo.LastWriteTimeUtc).ToUnixTimeSeconds());
            writer.Write(Start);
            writer.Write(Size);
        }

    }
}
