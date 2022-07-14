using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightningPackager
{
    /// <summary>
    /// PackageHeader
    /// 
    /// July 11, 2022
    /// 
    /// Holds the package header data for a 
    /// </summary>
    public class PackageHeader
    {
        public const string Magic = "feed me data!";

        public const byte FormatVersionMajor = 1;
        public const byte FormatVersionMinor = 3;

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

        public PackageHeader()
        {
            Metadata = new PackageFileMetadata();

            Metadata.Name = "Game name here";
            Metadata.GameVersion = "1.0";
            // temporary version
            Metadata.EngineVersion = "1.0.138";
            Metadata.CompressionMode = PackageFileCompressionMode.LZMA;
        }

        public static long TimeStamp => DateTimeOffset.Now.ToUnixTimeSeconds();

        public void Write(BinaryWriter stream)
        {
            stream.Write(Magic);
            stream.Write(FormatVersionMajor);
            stream.Write(FormatVersionMinor);
            stream.Write((byte)Metadata.CompressionMode);
            stream.Write(TimeStamp);
            stream.Write(Metadata.Name);
            stream.Write(Metadata.GameVersion);
            stream.Write(Metadata.EngineVersion);
        }
    }
}
