namespace LightningGL
{
    /// <summary>
    /// FontSmoothingType
    /// 
    /// March 12, 2022
    /// 
    /// Enumerates the valid font smoothing types used for the TextAPI.
    /// </summary>
    public enum FontSmoothingType
    {
        /// <summary>
        /// Indicates that the text will be blended and antialiased. The default
        /// </summary>
        Default = 0,

        /// <summary>
        /// Indicates that the text will be shaded using a background color.
        /// </summary>
        Shaded = 1,

        /// <summary>
        /// Indicates that no preprocessing will be done to the text before it is rendered.
        /// </summary>
        Solid = 2,
    }
}
