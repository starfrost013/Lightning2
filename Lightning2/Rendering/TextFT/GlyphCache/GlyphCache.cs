
namespace LightningGL
{
    /// <summary>
    /// GlyphCache
    /// 
    /// Caches glyphs for each font so that drawing is faster.
    /// </summary>
    internal static class GlyphCache
    {
        static GlyphCache()
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
        internal static unsafe void CacheCharacter(string fontName, char character, Color foregroundColor,
            FontSmoothingType smoothingType = FontSmoothingType.Default)
        {
            // always a child of a font

            Font? font = FontManager.GetFont(fontName);

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

            nint glyphPtr = (nint)font.Handle.FaceRec->glyph;

            switch (smoothingType)
            {
                case FontSmoothingType.Solid:
                    error = FT_Render_Glyph((nint)font.Handle.FaceRec->glyph, FT_Render_Mode.FT_RENDER_MODE_NORMAL);
                    break;
                case FontSmoothingType.LCD:
                    error = FT_Render_Glyph((nint)font.Handle.FaceRec->glyph, FT_Render_Mode.FT_RENDER_MODE_LCD);
                    break;
            }

            if (error != FT_Error.FT_Err_Ok)
            {
                NCError.ShowErrorBox($"Error loading character {character} - FreeType failed to render the glyph: {error}",
                    254, "Font Glyphcache: Internal FreeType error trying to render the glyph", NCErrorSeverity.FatalError);
                return;
            }

            NCLogging.Log("Cache successful! Rendering to texture...");
            // we have to use -> because it's a pointer in c#

            FT_Bitmap bitmap = font.Handle.FaceRec->glyph->bitmap;

            // it could be 0 size in case that it's, for example, a space character.
            // so don't try and draw those
            if (bitmap.width != 0
                && bitmap.rows != 0)
            {
                nint textureHandle = Lightning.Renderer.CreateTexture((int)bitmap.width, (int)bitmap.rows);

                Glyph? glyph = new("Glyph", font.FontSize, font.FontSize)
                {
                    GlyphRec = font.Handle.FaceRec->glyph,
                    Handle = textureHandle,
                    ForegroundColor = foregroundColor,
                    SmoothingType = smoothingType,
                    Font = font.Name,
                    Size = new(bitmap.width, bitmap.rows),
                    Character = character,
                };

                glyph = (Glyph?)Lightning.Renderer.TextureFromFreetypeBitmap(bitmap, glyph, foregroundColor);
                Debug.Assert(glyph != null);

                Glyphs.Add(glyph);
            }
        }

        internal static Glyph? QueryCache(string font, char character, Color foregroundColor, FontSmoothingType smoothingType = FontSmoothingType.Default)
        {
            // because utf16
            int hexVersion = Convert.ToInt32(character);

            foreach (Glyph glyph in Glyphs)
            {
                if (glyph.Font == font
                    && glyph.Character == character
                    && glyph.ForegroundColor == foregroundColor 
                    && glyph.SmoothingType == smoothingType)
                {
                    NCLogging.Log($"Glyph cache hit (font: {glyph.Font}), character {glyph.Character} (0x{hexVersion:X}), smoothing type {glyph.SmoothingType})");
                    return glyph;
                }
            }

            NCLogging.Log($"Glyph cache miss (font: {font}), character {character} (0x{hexVersion:X}), smoothing type {smoothingType}). Caching for next time...");

            CacheCharacter(font, character, foregroundColor, smoothingType);
            return null;
        }

        internal static void DeleteEntry(string font, char character, Color foregroundColor, FontSmoothingType smoothingType = FontSmoothingType.Default)
        {
            foreach (Glyph glyph in Glyphs)
            {
                if (glyph.Font == font
                    && glyph.Character == character
                    && glyph.ForegroundColor == foregroundColor
                    && glyph.SmoothingType == smoothingType)
                {
                    Glyphs.Remove(glyph);
                    return;
                }
            }

            NCError.ShowErrorBox($"Tried to remove non-cached {character} for font {font}, color {foregroundColor} smoothing type {smoothingType}", 261, 
                "GlyphCache::DeleteEntry called with glyph not in the glyph cache", NCErrorSeverity.Error);
        }

        internal static void PurgeUnusedEntries()
        {
            for (int glyphId = 0; glyphId < Glyphs.Count; glyphId++)
            {
                Glyph glyph = Glyphs[glyphId];

                if (!glyph.UsedThisFrame)
                {
                    RemoveGlyph(glyph);
                    glyphId--;
                }
            }
        }

        private static unsafe void RemoveGlyph(Glyph glyph)
        {
            SDL_DestroyTexture(glyph.Handle);
            
            Glyphs.Remove(glyph); // i trust that you already made sure that it's in glyphs
        }

        internal static void Shutdown()
        {
            // Clear all glyphs
            for (int glyphId = 0; glyphId < Glyphs.Count; glyphId++)
            {
                Glyph glyph = Glyphs[glyphId];
                RemoveGlyph(glyph);
                glyphId--;
            }
        }
    }
}
