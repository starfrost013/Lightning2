
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
        /// Basic obfuscation with XOR
        /// </summary>
        XOR = 2
    }
}
