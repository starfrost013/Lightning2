namespace LightningGL
{
    /// <summary>
    /// FontCacheEntry
    /// 
    /// September 3, 2022
    /// 
    /// Defines an entry into the font cache.
    /// </summary>
    public class TextCacheEntry : Renderable
    {
        /// <summary>
        /// The font of this font cache entry.
        /// </summary>
        internal string? Font { get; private set; }

        internal string Text
        {
            get
            {
                string final = "";

                for (int curLineId = 0; curLineId < Lines.Count; curLineId++)
                {
                    string? curLine = Lines[curLineId].Text;

                    if (curLine != null)
                    {
                        final = $"{final}{curLine}";

                        if (Lines.Count - curLineId > 1) final = $"{final}\n";
                    }
                }

                return final;
            }
        }

        /// <summary>
        /// The set of lines in this font cache entry.
        /// </summary>
        internal List<TextCacheEntryLine> Lines { get; private set; }

        /// <summary>
        /// The colour of this font.
        /// </summary>
        internal SDL_Color Color { get; private set; }

        /// <summary>
        /// The background colour of this font.
        /// Ignored if the value of <see cref="SmoothingType"/> is not <see cref="FontSmoothingType.Blended"/>
        /// </summary>
        internal SDL_Color BackgroundColor { get; private set; }

        /// <summary>
        /// The font style of this font.
        /// </summary>
        internal TTF_FontStyle Style { get; private set; }

        /// <summary>
        /// The smoothing type of this font.
        /// </summary>
        internal FontSmoothingType SmoothingType { get; set; }

        /// <summary>
        /// The outline size of this font.
        /// </summary>
        internal int OutlineSize { get; set; }

        /// <summary>
        /// Determines if this cache entry was used this frame.
        /// If not, it will be removed.
        /// </summary>
        internal bool UsedThisFrame { get; set; }

        internal TextCacheEntry()
        {
            Lines = new List<TextCacheEntryLine>();
        }

        internal static TextCacheEntry? Render(Renderer cRenderer, string font, string text,
            SDL_Color fgColor, TTF_FontStyle style, FontSmoothingType smoothingType = FontSmoothingType.Default,
            int outlineSize = -1, SDL_Color bgColor = default)
        {
            TextCacheEntry entry = new TextCacheEntry();

            entry.Color = fgColor;
            entry.Font = font;
            entry.Style = style;
            entry.SmoothingType = smoothingType;
            entry.BackgroundColor = bgColor;
            entry.OutlineSize = outlineSize;

            // render the font
            Font? fontForRender = FontManager.GetFont(font);

            if (fontForRender == null
                || text == null)
            {
                _ = new NCException("Cannot render a non-existent font or text into the font cache!", 136,
                    "FontCache::Render - font parameter is not a font, or text parameter is purely null!", NCExceptionSeverity.FatalError);
                return null;
            }

            NCLogging.Log($"Precaching font bitmap (font={font}, text={text}, color={fgColor}, smoothing type={smoothingType}, bgcolor={bgColor}, outline size={outlineSize})");

            // split the text into lines
            // add the length of each line to the text length
            string[] textLines = text.Split("\n");

            TTF_SetFontStyle(fontForRender.Handle, style);
            if (outlineSize > 0) TTF_SetFontOutline(fontForRender.Handle, outlineSize);

            foreach (string line in textLines)
            {
                TextCacheEntryLine cachedLine = new TextCacheEntryLine
                {
                    Text = line,
                };

                TTF_SizeUTF8(fontForRender.Handle, line, out var sizeX, out var sizeY);

                cachedLine.Size = new Vector2(
                    sizeX,
                    sizeY);

                switch (smoothingType)
                {
                    case FontSmoothingType.Default: // Antialiased
                        IntPtr surfaceBlended = TTF_RenderUTF8_Blended(fontForRender.Handle, cachedLine.Text, fgColor);
                        cachedLine.Handle = SDL_CreateTextureFromSurface(cRenderer.Settings.RendererHandle, surfaceBlended);

                        SDL_FreeSurface(surfaceBlended);
                        break;
                    case FontSmoothingType.Shaded: // Only shaded
                        IntPtr surfaceShaded = TTF_RenderUTF8_Shaded(fontForRender.Handle, cachedLine.Text, fgColor, bgColor);
                        cachedLine.Handle = SDL_CreateTextureFromSurface(cRenderer.Settings.RendererHandle, surfaceShaded);

                        SDL_FreeSurface(surfaceShaded);
                        break;
                    case FontSmoothingType.Solid: // No processing done
                        IntPtr surfaceSolid = TTF_RenderUTF8_Solid(fontForRender.Handle, cachedLine.Text, fgColor);
                        cachedLine.Handle = SDL_CreateTextureFromSurface(cRenderer.Settings.RendererHandle, surfaceSolid);

                        SDL_FreeSurface(surfaceSolid);
                        break;
                }

                entry.Lines.Add(cachedLine);
            }

            return entry;
        }

        internal void Unload()
        {
            for (int lineId = 0; lineId < Lines.Count; lineId++)
            {
                TextCacheEntryLine line = Lines[lineId];
                SDL_DestroyTexture(line.Handle);
                Lines.Remove(line);
            }
        }
    }
}