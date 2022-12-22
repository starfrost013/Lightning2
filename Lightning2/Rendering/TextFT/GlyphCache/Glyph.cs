
namespace LightningGL
{
    /// <summary>
    /// Glyph
    /// 
    /// Holds a cached freetype glyph.
    /// DO NOT INSERT INTO THE HIERARCHY!
    /// </summary>
    public unsafe class Glyph : Texture
    {
        public Glyph(string name, int sizeX, int sizeY, bool isTarget = false) : base(name, sizeX, sizeY, isTarget) { }

        public FT_GlyphSlotRec* GlyphRec { get; set; }

        public Color ForegroundColor { get; set; }

        public Color BackgroundColor { get; set; }
    }
}
