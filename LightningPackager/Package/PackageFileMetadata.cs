﻿namespace LightningPackager
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
        public PackageFileCompressionMode CompressionMode { get; set; }
        public string Name { get; set; }
        public string GameVersion { get; set; }
        public string EngineVersion { get; set; }

        public PackageFileMetadata()
        {
            Name = "Game name here";
            GameVersion = "1.0";
            // temporary version
            EngineVersion = "1.0.138";
            CompressionMode = PackageFileCompressionMode.LZMA | PackageFileCompressionMode.XOR;
        }
    }
}
