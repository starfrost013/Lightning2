
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
            FontStyle style = FontStyle.Default, FontSmoothingType smoothingType = FontSmoothingType.Default)
        {
            // always a child of a font

            Font? font = (Font?)Lightning.Renderer.GetRenderableByName(fontName);

            if (font == null)
            {
                NCLogging.LogError("Passed an unloaded font to GlyphCache::CacheCharacter!",
                    256, NCLoggingSeverity.FatalError);
                return;
            }

            if (font.Handle == default)
            {
                NCLogging.LogError("Font failed to load while trying to cache a character. This is an engine bug! THIS IS MY BUSTED ASS CODE, NOT YOURS! REPORT THIS ERROR!",
                    251, NCLoggingSeverity.FatalError);
                return;
            }

            Debug.Assert(font.Handle != null);

            NCLogging.Log($"Trying to cache character {character} for font {font.Name}...");

            if (font.Handle.GetCharIfDefined(character) == null)
            {
                // hack warning
                NCLogging.LogError($"Tried to cache character {character} not defined in font! The character will not be cached!",
                    252, NCLoggingSeverity.Warning, null, true);

                // create an empty glyph such that we don't try and cache this character again
                Glyph emptyGlyph = new("UndefinedCharGlyph", 1, 1)
                {
                    IsEmpty = true,
                };

                Glyphs.Add(emptyGlyph);

                return;
            }

            uint glyphIndex = font.Handle.GetCharIndex(character);

            // load it and weep
            FT_Error error = FT_Load_Glyph(font.Handle.Face, glyphIndex, FT_LOAD_RENDER);

            if (error != FT_Error.FT_Err_Ok)
            {
                NCLogging.LogError($"Error loading character {character} - FreeType failed to load and render the glyph",
                    253, NCLoggingSeverity.FatalError);
                return;
            }

            NCLogging.Log("Cache successful! Rendering to texture...");

            // we have to use -> because it's a pointer in c#
            FT_Bitmap bitmap = font.Handle.FaceRec->glyph->bitmap;

            bool isEmpty = (bitmap.width == 0
                || bitmap.rows == 0);

            // create a blank bitmap for a space or similar
            // FreeType uses 16.16 fixed point multipliers so we have to shift right by 6 bits in order to get the actual value
            Vector2 advance = new(font.Handle.FaceRec->glyph->advance.x >> 6,
                font.Handle.FaceRec->glyph->advance.y >> 6);

            // todo: get rid of this code, it's dumb.
            if (bitmap.width == 0) bitmap.width = 1;
            if (bitmap.rows == 0) bitmap.rows = 1; // create a square for now

            Glyph? glyph = new("Glyph", (int)bitmap.width, (int)bitmap.rows)
            {
                GlyphRec = font.Handle.FaceRec->glyph,
                ForegroundColor = foregroundColor,
                SmoothingType = smoothingType,
                Font = font.Name,
                Character = character,
                IsEmpty = isEmpty,
                Offset = new(font.Handle.FaceRec->glyph->linearHoriAdvance >> 16,
                font.Handle.FaceRec->glyph->bitmap_top),
                Advance = advance,
                Style = style,
            };

            if (!glyph.IsEmpty)
            {
                glyph = (Glyph?)Lightning.Renderer.TextureFromFreetypeBitmap(bitmap, glyph, foregroundColor);
            }

            Debug.Assert(glyph != null);
            Glyphs.Add(glyph);
        }

        internal static Glyph? QueryCache(string font, char character, Color foregroundColor, FontStyle style,
            FontSmoothingType smoothingType = FontSmoothingType.Default, bool failNow = false)
        {
            // because utf16
            int hexChar = Convert.ToInt32(character);

            foreach (Glyph glyph in Glyphs)
            {
                if (glyph.Font == font
                    && glyph.Character == character
                    && glyph.ForegroundColor == foregroundColor 
                    && glyph.Style == style
                    && glyph.SmoothingType == smoothingType)
                {
                    return glyph;
                }
            }

            NCLogging.Log($"Glyph cache miss (font: {font}), character {character} (0x{hexChar:X}), style {style}, smoothing type {smoothingType}). Caching for next time...");

            // prevent a stack overflow it will only try and cache it once
            if (!failNow)
            {
                CacheCharacter(font, character, foregroundColor, style, smoothingType);

                // try and query the cache again
                return QueryCache(font, character, foregroundColor, style, smoothingType, true);
            }

            // don't try and cache a third time if we already tried once
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

            NCLogging.LogError($"Tried to remove non-cached {character} for font {font}, color {foregroundColor} smoothing type {smoothingType}", 261, NCLoggingSeverity.Error);
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
