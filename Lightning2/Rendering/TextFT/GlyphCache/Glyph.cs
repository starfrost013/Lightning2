
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

        internal FT_GlyphSlotRec* GlyphRec { get; set; }

        internal FontSmoothingType SmoothingType { get; set; }
    }
}
