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
        public const byte FormatVersionMinor = 2;

        public string Name { get; set; }
        public string GameVersion { get; set; }
        public string EngineVersion { get; set; }

        public int HeaderSize
        {
            get
            {
                // + 1 as it writes byte information
                // 8 as timestamp is 8 bytes
                return (Magic.Length + 1) + 2 + 8 + (Name.Length + 1) + (GameVersion.Length + 1) + (EngineVersion.Length + 1);
            }
        }

        public PackageHeader()
        {
            Name = "Game name here";
            GameVersion = "1.0";
            // temporary version
            EngineVersion = "1.0.138";
        }

        public static long TimeStamp => DateTimeOffset.Now.ToUnixTimeSeconds();

        public void Write(BinaryWriter stream)
        {
            stream.Write(Magic);
            stream.Write(FormatVersionMajor);
            stream.Write(FormatVersionMinor);
            stream.Write(TimeStamp);
            stream.Write(Name);
            stream.Write(GameVersion);
            stream.Write(EngineVersion);
        }
    }
}
