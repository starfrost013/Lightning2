namespace LightningPackager
{
    public class PackageFileMetadata
    {
        public PackageFileCompressionMode CompressionMode { get; set; }
        public string Name { get; set; }
        public string GameVersion { get; set; }
        public string EngineVersion { get; set; }
    }
}
