using NuCore.Utilities;
using System.IO;

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
        /// <summary>
        /// Path to the file.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Private: Real (relative to archive) path written to the file.
        /// </summary>
        public string RealPath
        {
            get
            {
                // remove both possible path separator characters (we can't use pathseparatorcharacter here)
                string realPath = FilePath;
                realPath = realPath.Replace(PackageFile.InFolder, "");
                return realPath;
            }
        }

        public DateTime TimeStamp { get; set; }

        public uint Crc32 { get; set; }
        
        public long Start { get; set; }

        internal long Size { get; set; }

        internal long Length
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
        public PackageFileCatalogEntry(string path, bool extract = false)
        {
            FilePath = path;

            if (!extract)
            {
                if (!File.Exists(FilePath)) _ = new NCException($"Attempted to add a non-existent file ({path}) to a PackageFileCatalog!", 96, "PackageFileCatalogEntry constructor: Path does not exist!", NCExceptionSeverity.FatalError, null, true);

                FileInfo fileInfo = new FileInfo(FilePath);
                Size = fileInfo.Length;
                TimeStamp = fileInfo.LastWriteTimeUtc;
            }

        }

        internal static PackageFileCatalogEntry Read(BinaryReader reader)
        {
            string path = reader.ReadString();
            PackageFileCatalogEntry entry = new PackageFileCatalogEntry(path, true);

            entry.TimeStamp = DateTimeOffset.FromUnixTimeSeconds(reader.ReadInt64()).DateTime;
            entry.Crc32 = reader.ReadUInt32();
            entry.Start = reader.ReadInt64();
            entry.Size = reader.ReadInt64();

            //todo: calculate crc32

            return entry;
        }

        internal void Extract(BinaryReader reader, string outFolder)
        {
            string finalPath = Path.Combine(outFolder, FilePath);

            // needed for relative paths
            if (finalPath == FilePath) finalPath = $"{outFolder}\\{FilePath}";

            if (File.Exists(finalPath)) File.Delete(finalPath);

            // remove the final part of the directory so that we don't create the filename as a directory
            string[] finalPathDirectories = finalPath.Split('\\');

            string finalDirectory = null;

            for (int curDirectoryId = 0; curDirectoryId < (finalPathDirectories.Length - 1); curDirectoryId++)
            {
                string curDirectory = finalPathDirectories[curDirectoryId];

                // first directory - don't add a useless \\
                if (curDirectoryId == 0)
                {
                    finalDirectory = curDirectory; //append to string
                }
                else
                {
                    finalDirectory = $"{finalDirectory}\\{curDirectory}"; //append to string
                }
            }

            // create the directory if it does not already exist
            if (!Directory.Exists(finalDirectory)) Directory.CreateDirectory(finalDirectory);

            // seek to the start of the file
            reader.BaseStream.Seek(Start, SeekOrigin.Begin);
            byte[] fileData = reader.ReadBytes(Convert.ToInt32(Size));

            // Write using writeallbytes
            File.WriteAllBytes(finalPath, fileData) ;

            // will be this value if we do not specify CRC32 compression mode
            if (Crc32 != default(uint))
            {
                CRC32.NextBytes(fileData);
                uint realCrc32 = CRC32.Result;

                string validationString = $"CRC32 of original file = {Crc32.ToString("X")}, CRC of extracted file = {realCrc32.ToString("X")}";
                NCLogging.Log(validationString);
                if (Crc32 != realCrc32) _ = new NCException($"File {RealPath} is corrupt: {validationString}!", 116, "Calculated CRC32 for PackageFileCatalogEntry is not the same as PackageFileCatalogEntry::Crc32");
            }


        }

        internal void Write(BinaryWriter writer)
        {
            writer.Write(RealPath);
            long timeStamp = (new DateTimeOffset(TimeStamp).ToUnixTimeSeconds());
            writer.Write(timeStamp);
            writer.Write(Crc32);
            writer.Write(Start);
            writer.Write(Size);
        }
    }
}