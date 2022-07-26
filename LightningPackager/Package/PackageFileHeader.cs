using NuCore.Utilities;

namespace LightningPackager
{
    /// <summary>
    /// PackageHeader
    /// 
    /// July 11, 2022
    /// 
    /// Holds the package header data for a 
    /// </summary>
    public class PackageFileHeader
    {
        public const string Magic = "feed me data!";

        public static byte[] ObfuscatedMagic = { 0xD9, 0xB4, 0xB1, 0xB1, 0xB2, 0xEE, 0xB9, 0xB1};
        public const byte FormatVersionMajor = 2;
        public const byte FormatVersionMinor = 1;

        public PackageFileMetadata Metadata { get; set; }

        public int HeaderSize
        {
            get
            {
                // + 1 as it writes byte information
                // 8 as timestamp is 8 bytes
                return (Magic.Length + 1) + 3 + 8 + (Metadata.Name.Length + 1) + (Metadata.GameVersion.Length + 1) + (Metadata.EngineVersion.Length + 1);
            }
        }

        public PackageFileHeader(PackageFileMetadata metadata)
        {
            Metadata = metadata;
        }

        public static PackageFileHeader Read(BinaryReader reader)
        {
            // reader code already checked the magic
            byte formatVersionMajor = reader.ReadByte();
            byte formatVersionMinor = reader.ReadByte();

            if (formatVersionMajor != FormatVersionMajor
                || formatVersionMinor != FormatVersionMinor)
            {
                _ = new NCException($"Incorrect package file version. Cannot load (Implemented version {FormatVersionMajor}.{FormatVersionMinor}, expected version {formatVersionMajor}." +
                    $"{formatVersionMinor})", 100, "MajorVersionHeader and MinorVersionHeader fields in WAD file not equivalent to PackageFile::MajorVersionHeader and PackageFile::MinorVersionHeader!", NCExceptionSeverity.Error);
                return null;
            }

            PackageFileMetadata metadata = new()
            {
                CompressionMode = (PackageFileCompressionMode)reader.ReadByte(),
                TimeStamp = reader.ReadInt64(),
                Name = reader.ReadString(),
                GameVersion = reader.ReadString(),
                EngineVersion = reader.ReadString()
            };

            NCLogging.Log("WAD File Metadata" +
                $"Format version: {formatVersionMajor}.{formatVersionMinor}" +
                $"TimeStamp: {DateTimeOffset.FromUnixTimeMilliseconds(metadata.TimeStamp).ToString("yyyy-MM-dd HH:mm:SS")}" +
                $"Name: {metadata.Name}" +
                $"Version: {metadata.GameVersion}" +
                $"Intended engine version: {metadata.EngineVersion}");

            PackageFileHeader header = new(metadata);

            return header;
        }

        public void Write(BinaryWriter stream)
        {
            stream.Write(Magic);
            stream.Write(FormatVersionMajor);
            stream.Write(FormatVersionMinor);
            stream.Write((byte)Metadata.CompressionMode);
            stream.Write(Metadata.TimeStamp);
            stream.Write(Metadata.Name);
            stream.Write(Metadata.GameVersion);
            stream.Write(Metadata.EngineVersion);
        }
    }
}
