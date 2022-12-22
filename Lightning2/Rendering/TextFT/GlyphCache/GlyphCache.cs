
namespace LightningGL
{
    /// <summary>
    /// GlyphCache
    /// 
    /// Caches glyphs for each font so that drawing is faster.
    /// </summary>
    internal class GlyphCache : Renderable
    {
        internal GlyphCache(string name) : base(name)
        {
            Glyphs = new();
        }

        internal List<Glyph> Glyphs { get; set; }

        /// <summary>
        /// Caches a character for this font.
        /// </summary>
        /// <param name="character">The character to cache.</param>
        internal unsafe void CacheCharacter(char character, Color foregroundColor, Color backgroundColor = default, 
            FontSmoothingType smoothingType = FontSmoothingType.Default)
        {
            // always a child of a font

            if (Parent is not FTFont)
            {
                NCError.ShowErrorBox("A glyph cache's parent is not a font. This is an engine bug! THIS IS MY BUSTED ASS CODE, NOT YOURS! REPORT THIS ERROR!", 
                    251, "A GlyphCache's parent property was not of type FTFont", NCErrorSeverity.FatalError);
                return;
            }

            FTFont theFont = (FTFont)Parent;

            Debug.Assert(theFont.Handle != null);

            NCLogging.Log($"Trying to cache character {character} for font {Parent.Name}...");

            if (theFont.Handle.GetCharIfDefined(character) == null)
            {
                NCError.ShowErrorBox($"Tried to cache character {character} not defined in font! The character will not be drawn!",
                    252, "Font Glyphcache: Tried to cache a character that the font does not define", NCErrorSeverity.Warning, null, true);
                return;
            }

            uint glyphIndex = theFont.Handle.GetCharIndex(character);

            // load it and weep
            FT_Error error = FT_Load_Glyph(theFont.Handle.Face, glyphIndex, FT_LOAD_DEFAULT);

            if (error != FT_Error.FT_Err_Ok)
            {
                NCError.ShowErrorBox($"Error loading character {character} - FreeType failed to load the glyph",
                    253, "Font Glyphcache: Tried to cache a character that the font does not define", NCErrorSeverity.FatalError);
                return;
            }

            switch (smoothingType)
            {
                case FontSmoothingType.Solid:
                case FontSmoothingType.Shaded:
                case FontSmoothingType.Blended:
                    error = FT_Render_Glyph(theFont.Handle.Face, FT_Render_Mode.FT_RENDER_MODE_NORMAL);
                    break;
                case FontSmoothingType.LCD:
                    error = FT_Render_Glyph(theFont.Handle.Face, FT_Render_Mode.FT_RENDER_MODE_LCD);
                    break;
            }

            if (error != FT_Error.FT_Err_Ok)
            {
                NCError.ShowErrorBox($"Error loading character {character} - FreeType failed to render the glyph",
                    254, "Font Glyphcache: Tried to cache a character that the font does not define", NCErrorSeverity.FatalError);
                return;
            }

            NCLogging.Log("Cache successful! Rendering...");
            // we have to use -> because it's a pointer in c#

            FT_Bitmap bitmap = theFont.Handle.FaceRec->glyph->bitmap;

            nint textureHandle = Lightning.Renderer.CreateTexture(theFont.FontSize, theFont.FontSize);

            Glyph? glyph = new("Glyph", theFont.FontSize, theFont.FontSize)
            {
                GlyphRec = theFont.Handle.FaceRec->glyph,
                Handle = textureHandle,
                ForegroundColor = foregroundColor,
                BackgroundColor = backgroundColor
            };

            glyph = (Glyph?)Lightning.Renderer.TextureFromFreeTypeBitmap(bitmap, glyph, foregroundColor);
            Debug.Assert(glyph != null);

            Glyphs.Add(glyph);
        }
    }
}
