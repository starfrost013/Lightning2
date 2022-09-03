namespace LightningPackager
{
    /// <summary>
    /// PackageFileMetadata
    /// 
    /// July 14, 2022
    /// 
    /// Holds metadata for a WAD file.
    /// </summary>
    public class PackageFileMetadata
    {
        public long TimeStamp { get; set; }
        public PackageFileCompressionMode CompressionMode { get; set; }
        public string Name { get; set; }
        public string GameVersion { get; set; }
        public string EngineVersion { get; set; }

        public PackageFileMetadata()
        {
            TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            Name = "Game name here";
            GameVersion = "1.0";
            // temporary version
            EngineVersion = PackagerVersion.LIGHTNING_VERSION_BUILD_STRING;
            CompressionMode = PackageFileCompressionMode.XOR;
        }
    }
}
