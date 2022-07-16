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

        /// <summary>
        /// Private: Real (relative to archive) path written to the file.
        /// </summary>
        private string RealPath
        {
            get
            {
                // remove both possible path separator characters (we can't use pathseparatorcharacter here)
                string realPath = Path;
                realPath = realPath.Replace("../", "");
                realPath = realPath.Replace("..\\", "");
                return realPath;
            }
        }
        public DateTime TimeStamp { get; set; }

        public uint Crc32 { get; set; }
        
        public ulong Start { get; set; }

        internal long Size { get; set; }

        internal uint Length
        {
            get
            {
                // + 1 as streamwriter puts a length byte 

                // + 4 for Crc32
                // + 8 for timestamp
                // + 16 for two ulongs (start and size)
                return (uint)(RealPath.Length + 1) + 4 + 8 + 16; 
            }
        }

        public PackageFileCatalogEntry(string path)
        {
            Path = path;
            if (!File.Exists(Path)) _ = new NCException($"Attempted to add a non-existent file ({path}) to a PackageFileCatalog!", 96, "PackageFileCatalogEntry constructor: Path does not exist!", NCExceptionSeverity.FatalError);

            FileInfo fileInfo = new FileInfo(Path);
            Size = fileInfo.Length;
            TimeStamp = fileInfo.LastWriteTimeUtc;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(RealPath);
            writer.Write(new DateTimeOffset(TimeStamp).ToUnixTimeSeconds());
            writer.Write(Crc32);
            writer.Write(Start);
            writer.Write(Size);
        }

    }
}
