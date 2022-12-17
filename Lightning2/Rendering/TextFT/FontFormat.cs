
namespace LightningGL.Rendering.TextFT
{
    /// <summary>
    /// Enumerates valid font formats.
    /// Internal enum.
    /// </summary>
    internal enum FontFormat
    {
        /// <summary>
        /// TrueType
        /// </summary>
        TTF = 0,

        /// <summary>
        /// TrueType collection
        /// </summary>
        TTC = 1,

        /// <summary>
        /// OpenType
        /// </summary>
        OTF = 2,

        /// <summary>
        /// OpenType collection
        /// </summary>
        OTC = 3,

        /// <summary>
        /// CFF
        /// </summary>
        CFF = 4,

        /// <summary>
        /// WOFF WebFont
        /// </summary>
        WOFF = 5,

        /// <summary>
        /// Adobe Type 1 PFA/PFB
        /// Postscript Type 42
        /// </summary>
        PFA = 6,

        /// <summary>
        /// Type 1 PFB
        /// </summary>
        PFB = 7,

        /// <summary>
        /// CID Type 1
        /// </summary>
        CID = 7,

        /// <summary>
        /// SFNT bitmap font format e.g. emoji
        /// </summary>
        SFNT = 8,

        /// <summary>
        /// X11 PCF font format
        /// </summary>
        PCF = 9,

        /// <summary>
        /// Windows legacy FNT file
        /// </summary>
        FNT = 10,

        /// <summary>
        /// BDF
        /// </summary>
        BDF = 11,

        /// <summary>
        /// PFR.
        /// </summary>
        PFR = 12,

        /// <summary>
        /// Maximum font sentinel value
        /// </summary>
        MAX_FONT = PFR,
    }
}
