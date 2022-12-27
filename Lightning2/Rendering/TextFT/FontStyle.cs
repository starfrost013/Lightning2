
namespace LightningGL
{
    [Flags]
    /// <summary>
    /// FontStyle
    /// 
    /// Enumerates font styles in the freetype text engine
    /// </summary>
    public enum FontStyle
    {
        /// <summary>
        /// Normal font style.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Bold font styling.
        /// </summary>
        Bold = 0x1,

        /// <summary>
        /// Italic font styling.
        /// </summary>
        Italic = 0x2,

        /// <summary>
        /// Underline font styling.
        /// </summary>
        Underline = 0x4,

        /// <summary>
        /// Strikeojt font styling.
        /// </summary>
        Strikeout = 0x8,    
    }
}
