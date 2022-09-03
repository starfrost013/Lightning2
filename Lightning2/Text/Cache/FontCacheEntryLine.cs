namespace LightningGL
{
    /// <summary>
    /// FontCacheEntryLine
    /// 
    /// September 3, 2022
    /// 
    /// Definesa a single line of text in the Font Cache.
    /// </summary>
    internal class FontCacheEntryLine
    {
        /// <summary>
        /// Handle to each line of the the SDL_Texture* of this font image.
        /// </summary>
        internal IntPtr Handle { get; set; }

        /// <summary>
        /// The lines of the font of this font cache entry.
        /// </summary>
        internal string Text { get; set; }

        /// <summary>
        /// The size of this text.
        /// </summary>
        internal Vector2 Size { get; set; }
    }
}
