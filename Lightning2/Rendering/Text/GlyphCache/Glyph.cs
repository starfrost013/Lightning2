
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

        /// <summary>
        /// Determines if this glyph is empty or not.
        /// </summary>
        internal bool IsEmpty { get; set; }

        /// <summary>
        /// A pointer to the FreeType glyphslot take up by this glyph.
        /// </summary>
        internal FT_GlyphSlotRec* GlyphRec { get; set; }

        /// <summary>
        /// The smoothing type of this font.
        /// 
        /// See <see cref="FontSmoothingType"/>.
        /// </summary>
        internal FontSmoothingType SmoothingType { get; set; }

        /// <summary>
        /// The style of this font.
        /// </summary>
        internal FontStyle Style { get; set; }

        /// <summary>
        /// The advance amount of this glyph.
        /// </summary>
        internal Vector2 Advance { get; set; }

        /// <summary>
        /// The offset of this glyph.
        /// </summary>
        internal Vector2 Offset { get; set; }
    }
}
