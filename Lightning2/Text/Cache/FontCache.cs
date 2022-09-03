using static NuCore.SDL2.SDL;
using static NuCore.SDL2.SDL_ttf;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace LightningGL
{
    /// <summary>
    /// FontCache
    /// 
    /// September 3, 2022
    /// 
    /// Defines the Lightning font cache.
    /// </summary>
    internal class FontCache
    {
        internal List<FontCacheEntry> Entries { get; private set; }

        internal FontCache()
        {
            Entries = new List<FontCacheEntry>();
        }

        internal FontCacheEntry AddEntry(Window cWindow, string font, string text,
            SDL_Color color, TTF_FontStyle style, FontSmoothingType type = FontSmoothingType.Default, int outlineSize = -1, SDL_Color bgColor = default)
        {
            FontCacheEntry entry = FontCacheEntry.Render(cWindow, font, text, color, style, type, outlineSize, bgColor);
            Entries.Add(entry);
            return entry;
        }

        internal void PurgeUnusedEntries()
        {
            // todo (reuqired to prevent memory leaks
        }

        internal FontCacheEntry GetEntry(string font, string text, 
            SDL_Color color, TTF_FontStyle style, FontSmoothingType type = FontSmoothingType.Default, int outlineSize = -1, SDL_Color bgColor = default)
        {
            foreach (FontCacheEntry entry in Entries)
            {
                if (entry.Font == font
                    && entry.Text == text
                    && entry.Color == color
                    && entry.Style == style
                    && entry.SmoothingType == type
                    && entry.OutlineSize == outlineSize
                    && entry.BackgroundColor == bgColor)
                {
                    return entry;
                }
            }

            return null;
        }

        internal void DeleteEntry(string font, string text,
            SDL_Color color, TTF_FontStyle style, FontSmoothingType type = FontSmoothingType.Default, int outlineSize = -1, SDL_Color bgColor = default)
        {
            FontCacheEntry fontEntry = GetEntry(font, text, color, style, type, outlineSize, bgColor);

            if (fontEntry != null)
            {
                fontEntry.Unload();
                Entries.Remove(fontEntry);
            }
        }

        internal void Unload()
        {
            foreach (FontCacheEntry entry in Entries)
            {
                entry.Unload();
            }
        }
    }
}
