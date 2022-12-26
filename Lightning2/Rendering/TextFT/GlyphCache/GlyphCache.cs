
namespace LightningGL
{
    /// <summary>
    /// GlyphCache
    /// 
    /// Caches glyphs for each font so that drawing is faster.
    /// </summary>
    internal static class GlyphCache
    {
        static GlyphCache(string name)
        {
            Glyphs = new();
        }

        /// <summary>
        /// The list of glyphs
        /// 
        /// These are not in the object hierarchy...because they are strictly internal to a static class.
        /// </summary>
        internal static List<Glyph> Glyphs { get; set; }

        /// <summary>
        /// Caches a character for this font.
        /// </summary>
        /// <param name="character">The character to cache.</param>
        internal static unsafe void CacheCharacter(string fontName, char character, 
            FontSmoothingType smoothingType = FontSmoothingType.Default)
        {
            // always a child of a font

            FTFont? font = FontManager.GetFont(fontName);

            if (font == null)
            {
                NCError.ShowErrorBox("Passed an unloaded font to GlyphCache::CacheCharacter!",
                    256, "GlyphCache::CacheCharacter's font parameter was NULL", NCErrorSeverity.FatalError);
                return;
            }

            if (font.Handle == default)
            {
                NCError.ShowErrorBox("Font failed to load while trying to cache a character. This is an engine bug! THIS IS MY BUSTED ASS CODE, NOT YOURS! REPORT THIS ERROR!", 
                    251, "Called GlyphCache::CacheCharacter with a font that was not loaded!", NCErrorSeverity.FatalError);
                return;
            }

            Debug.Assert(font.Handle != null);

            NCLogging.Log($"Trying to cache character {character} for font {font.Name}...");

            if (font.Handle.GetCharIfDefined(character) == null)
            {
                NCError.ShowErrorBox($"Tried to cache character {character} not defined in font! The character will not be drawn!",
                    252, "Font Glyphcache: Tried to cache a character that the font does not define", NCErrorSeverity.Warning, null, true);
                return;
            }

            uint glyphIndex = font.Handle.GetCharIndex(character);

            // load it and weep
            FT_Error error = FT_Load_Glyph(font.Handle.Face, glyphIndex, FT_LOAD_DEFAULT);

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
                    error = FT_Render_Glyph(font.Handle.Face, FT_Render_Mode.FT_RENDER_MODE_NORMAL);
                    break;
                case FontSmoothingType.LCD:
                    error = FT_Render_Glyph(font.Handle.Face, FT_Render_Mode.FT_RENDER_MODE_LCD);
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

            FT_Bitmap bitmap = font.Handle.FaceRec->glyph->bitmap;

            nint textureHandle = Lightning.Renderer.CreateTexture(font.FontSize, font.FontSize);

            Glyph? glyph = new("Glyph", font.FontSize, font.FontSize)
            {
                GlyphRec = font.Handle.FaceRec->glyph,
                Handle = textureHandle,
                SmoothingType = smoothingType,
                Font = font.Name,
            };

            glyph = (Glyph?)Lightning.Renderer.TextureFromFreetypeBitmap(bitmap, glyph);
            Debug.Assert(glyph != null);

            Glyphs.Add(glyph);
        }

        internal static Glyph? QueryCache(string font, char character, FontSmoothingType smoothingType = FontSmoothingType.Default)
        {
            foreach (Glyph glyph in Glyphs)
            {
                if (glyph.Font == font
                    && glyph.Character == character
                    && glyph.SmoothingType == smoothingType)
                {
                    NCLogging.Log($"Glyph cache hit (font: {glyph.Font} {glyph.Character} (0x{glyph.Character:X}), smoothing type {glyph.SmoothingType})");
                    return glyph;
                }
            }

            NCLogging.Log($"Glyph cache miss (font: {font} {character} (0x{character:X}), smoothing type {smoothingType}). Caching for next time...");

            CacheCharacter(font, character, smoothingType);
            return null;
        }
    }
}
