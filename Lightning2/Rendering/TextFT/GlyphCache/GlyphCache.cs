﻿
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

            switch (smoothingType)
            {
                case FontSmoothingType.Solid:
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

            nint textureHandle = Lightning.Renderer.CreateTexture((int)bitmap.width, (int)bitmap.rows);

            Glyph? glyph = new("Glyph", font.FontSize, font.FontSize)
            {
                GlyphRec = font.Handle.FaceRec->glyph,
                Handle = textureHandle,
                ForegroundColor = foregroundColor, 
                SmoothingType = smoothingType,
                Font = font.Name,
                Size = new(bitmap.width, bitmap.rows),
            };

            glyph = (Glyph?)Lightning.Renderer.TextureFromFreetypeBitmap(bitmap, glyph);
            Debug.Assert(glyph != null);

            // it is now time...to colour in the glyph
            // it might be good to move this recolouring part to the cache...but cache misses are easier then
            for (int y = 0; y < glyph.Size.Y; y++)
            {
                for (int x = 0; x < glyph.Size.X; x++)
                {
                    Color oldColor = glyph.GetPixel(x, y);

                    // don't recolour stuff that is already the same colour!!!
                    // still a bit slow due to multiple pieces of text of different colours but hopefully should mitigate some of it
                    if (oldColor.R != foregroundColor.R
                        && oldColor.G != foregroundColor.G
                        && oldColor.B != foregroundColor.B)
                    {
                        // keep alpha
                        // sdl_settexturecolormod faster?
                        glyph.SetPixel(x, y, Color.FromArgb(oldColor.A, foregroundColor.R, foregroundColor.G, foregroundColor.B));
                    }
                }
            }

            Glyphs.Add(glyph);
        }

        internal static Glyph? QueryCache(string font, char character, Color foregroundColor, FontSmoothingType smoothingType = FontSmoothingType.Default)
        {
            foreach (Glyph glyph in Glyphs)
            {
                if (glyph.Font == font
                    && glyph.Character == character
                    && glyph.ForegroundColor == foregroundColor 
                    && glyph.SmoothingType == smoothingType)
                {
                    NCLogging.Log($"Glyph cache hit (font: {glyph.Font} {glyph.Character} (0x{glyph.Character:X}), smoothing type {glyph.SmoothingType})");
                    return glyph;
                }
            }

            NCLogging.Log($"Glyph cache miss (font: {font} {character} (0x{character:X}), smoothing type {smoothingType}). Caching for next time...");

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
                    Glyphs.Remove(glyph);
                    glyphId--;
                }
            }
        }
    }
}
