
namespace LightningPackager
{
    /// <summary>
    /// PackageFileCompressionMode
    /// 
    /// July 14, 2022
    /// 
    /// Enumerates valid package file compression modes.
    /// </summary>
    public enum PackageFileCompressionMode : byte
    {
        /// <summary>
        /// No compression.
        /// </summary>
        None = 0,

        /// <summary>
        /// LZMA compression using ManagedLZMA.
        /// </summary>
        LZMA = 1,

        /// <summary>
        /// LZMA compression using ManagedLZMA, followed by basic obfuscation with XOR
        /// </summary>
        LZMA_XOR = 2
    }
}
