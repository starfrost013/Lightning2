
namespace LightningGL
{
    /// <summary>
    /// Glyph
    /// 
    /// Holds a cached freetype glyph.
    /// DO NOT INSERT INTO THE HIERARCHY!
    /// </summary>
    internal unsafe class Glyph : Texture
    {
        public Glyph(string name, int sizeX, int sizeY, bool isTarget = false) : base(name, sizeX, sizeY, isTarget) { }

        /// <summary>
        /// The friendly name of the font of this glyph.
        /// </summary>
        internal string? Font { get; set; }

        /// <summary>
        /// The character this glyph represents.
        /// </summary>
        internal char Character { get; set; }

        /// <summary>
        /// The foreground colour of this glyph.
        /// </summary>
        internal Color ForegroundColor { get; set; }

        /// <summary>
        /// Determines if this glyph was used this frame.
        /// </summary>
        internal bool UsedThisFrame { get; set; }

        internal FT_GlyphSlotRec* GlyphRec { get; set; }

        internal FontSmoothingType SmoothingType { get; set; }

    }
}
