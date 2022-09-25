using LightningBase;
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
    internal class PackageFileHeader
    {
        internal const string Magic = "feed me data!";

        internal static byte[] ObfuscatedMagic = { 0xD9, 0xB4, 0xB1, 0xB1, 0xB2, 0xEE, 0xB9, 0xB1 };
        internal const byte FormatVersionMajor = 2;
        internal const byte FormatVersionMinor = 2;

        internal PackageFileMetadata Metadata { get; set; }

        internal int HeaderSize
        {
            get
            {
                // + 1 as it writes the length of the string before the string
                // 3 for versioning information and compression mode
                // 8 as timestamp is 8 bytes
                return (Magic.Length + 1) + 3 + 8 + (Metadata.Name.Length + 1) + (Metadata.GameVersion.Length + 1) + (Metadata.EngineVersion.Length + 1);
            }
        }

        internal PackageFileHeader(PackageFileMetadata metadata)
        {
            Metadata = metadata;
        }

        internal static PackageFileHeader Read(BinaryReader reader)
        {
            string magic = reader.ReadString();

            if (magic != Magic)
            {
                _ = new NCException($"Not a WAD file or magic is corrupt (expected {Magic}, got {magic}!", 99, $"PackageFile::Read - could not identify obfuscated or non-obfuscated header!", NCExceptionSeverity.Error, null, true);
                return null;
            }

            byte formatVersionMajor = reader.ReadByte();
            byte formatVersionMinor = reader.ReadByte();

            if (formatVersionMajor != FormatVersionMajor
                || formatVersionMinor != FormatVersionMinor)
            {
                _ = new NCException($"Incorrect package file version. Cannot load (Implemented version {FormatVersionMajor}.{FormatVersionMinor}, expected version {formatVersionMajor}." +
                    $"{formatVersionMinor})", 100, "MajorVersionHeader and MinorVersionHeader fields in WAD file not equivalent to PackageFile::MajorVersionHeader and PackageFile::MinorVersionHeader!", NCExceptionSeverity.Error, null, true);
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

            // perform an engine version check on the WAD
            // special case 1.0.138 (1.0 placeholder for this functionality)

            bool failedCompatCheck = (metadata.EngineVersion == "1.0.138");

            // don't try to parse versions older than 1.1 that are incompatible
            if (!failedCompatCheck)
            {
                // check engine version
                string[] engineVersionComponents = metadata.EngineVersion.Split('.');
                int major = Convert.ToInt32(engineVersionComponents[0]),
                    minor = Convert.ToInt32(engineVersionComponents[1]),
                    revision = Convert.ToInt32(engineVersionComponents[2]),
                    build = Convert.ToInt32(engineVersionComponents[3]);

                if (major != LightningVersion.LIGHTNING_VERSION_MAJOR
                    || minor != LightningVersion.LIGHTNING_VERSION_MINOR) failedCompatCheck = true;

                // print a warning message on wrong revision
                if (revision != LightningVersion.LIGHTNING_VERSION_REVISION
                    || build != LightningVersion.LIGHTNING_VERSION_BUILD)
                {
                    _ = new NCException($"Incorrect engine patch version. You may encounter issues with this game not anticipated by the developers! (expected version {LightningVersion.LIGHTNING_VERSION_BUILD_STRING}, got {metadata.EngineVersion}!)", 138,
                        "Revision and build values of a WAD's EngineVersion were not identical to the values in LightningBase's LightningVersion::LIGHTNING_VERSION_REVISION and PackagerVersion::LIGHTNING_VERSION_BUILD respectively.!",
                        NCExceptionSeverity.Warning, null, false);
                }
            }

            if (failedCompatCheck) _ = new NCException("This WAD file is incompatible with this version of Lightning.\n\n" +
                $"WAD Version: {metadata.EngineVersion}\n" +
                $"Lightning Version: {LightningVersion.LIGHTNING_VERSION_BUILD_STRING} \n\n" +
                $"Only versions that have the same major and minor version are compatible with each other. Either regenerate your game WAD using MakePackage.exe to be compatible with the latest " +
                $"version of the engine,, or your game has somehow been bundled with an incompatible engine version - in which case you should contact the game developer for a fix.",
                137, $"PackageFile header parser: Major and minor versions were not identical to the values in LightningBase's LightningVersion::LIGHTNING_VERSION_MAJOR and PackagerVersion::LIGHTNING_VERSION_MINOR",
                NCExceptionSeverity.FatalError);

            NCLogging.Log("WAD File Metadata:\n" +
                $"Format version: {formatVersionMajor}.{formatVersionMinor}\n" +
                $"TimeStamp: {DateTimeOffset.FromUnixTimeSeconds(metadata.TimeStamp):yyyy-MM-dd HH:mm:ss}\n" +
                $"Name: {metadata.Name}\n" +
                $"Version: {metadata.GameVersion}\n" +
                $"Intended engine version: {metadata.EngineVersion}\n");

            PackageFileHeader header = new(metadata);

            return header;
        }

        internal void Write(BinaryWriter stream)
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
