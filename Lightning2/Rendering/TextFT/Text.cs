namespace LightningGL
{
    /// <summary>
    /// FTTextAssetManager
    /// 
    /// Text asset manager (FreeType Version)
    /// </summary>
    public class Text : Renderable
    {

        public Text(string name) : base(name) { }

        /// <summary>
        /// The text to draw.
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// The font of the text to draw.
        /// </summary>
        public string? Font { get; set; }

        /// <summary>
        /// The foreground colour of the text to draw.
        /// </summary>
        public Color ForegroundColor { get; set; }

        /// <summary>
        /// The background colour of the text to draw.
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// The style of the text to draw - see <see cref="FontStyle"/>
        /// </summary>
        public FontStyle Style { get; set; }

        /// <summary>
        /// The size of the text's outline.
        /// </summary>
        public int OutlineSize { get; set; }

        /// <summary>
        /// The smoothing type of this font - see <see cref="FontSmoothingType"/>,
        /// </summary>
        public FontSmoothingType SmoothingType { get; set; }

        /// <summary>
        /// Determines if this text will be localised.
        /// </summary>
        public bool Localise { get; set; }

        public override void Draw()
        {
            if (string.IsNullOrWhiteSpace(Font))
            {
                NCError.ShowErrorBox($"Tried to draw a text with no font!", 256,
                    "Text::Draw - Font property is null!", NCErrorSeverity.FatalError);
                return;
            }

            // variable to store localised text
            string? text = Content;

            // just ignore it if there is still text to draw
            if (string.IsNullOrWhiteSpace(text)) return;

            if (Font == null)
            {
                NCError.ShowErrorBox($"Tried to draw a text with no font!", 256,
                    "Text::Draw - Font property is null!", NCErrorSeverity.FatalError);
                return;
            }

            // Localise the string using Localisation Manager.
            if (Localise) text = LocalisationManager.ProcessString(text);

            // Get the font and throw an error if it's invalid
            FTFont? curFont = FontManager.GetFont(Font);

            if (curFont == null
                || curFont.Handle == default)
            {
                NCError.ShowErrorBox($"Attempted to acquire invalid font with name {Font}", 39, 
                    "Text::Draw font parameter is not a loaded font!", NCErrorSeverity.FatalError);
                return;
            }

            // cache everything
            foreach (char character in text)
            {
                Glyph? glyph = GlyphCache.QueryCache(Font, character, SmoothingType);

                if (glyph == null) GlyphCache.CacheCharacter(Font, character, SmoothingType);

                glyph = GlyphCache.QueryCache(Font, character, SmoothingType);

                if (glyph != null)
                {
                    
                }
                else
                {
                    NCError.ShowErrorBox($"Failed to cache character {character} in text, it will not be displayed!", 260, "Call to GlyphCache::QueryCache in Text::Draw failed", NCErrorSeverity.Error, null, true);
                }
            }

            /*
            // Localise the string using Localisation Manager.
            if (localise) text = LocalisationManager.ProcessString(text);

            // Get the font and throw an error if it's invalid
            Font? curFont = FontManager.GetFont(font);

            if (curFont == null)
            {
                NCError.ShowErrorBox($"Attempted to acquire invalid font with name {font}", 39, "TextManager::DrawText font parameter is not a loaded font!", NCErrorSeverity.FatalError);
                return;
            }

            // Set the foreground color
            SDL_Color fgColor = new(foreground.R, foreground.G, foreground.B, foreground.A);

            // default to entirely transparent background (if the user has specified shasded for some reason, we still need a BG color...)
            SDL_Color bgColor = default;

            int fontSizeX = -1;
            int fontSizeY = -1;

            // use the cached entry if it exists
            TextCacheEntry? cacheEntry = GetEntry(font, text, fgColor, style, smoothingType, outlineSize, bgColor);

            // if it doesn't exist add it
            if (cacheEntry == null) cacheEntry = AddEntry(font, text, fgColor, style, smoothingType, outlineSize, bgColor);

            // if we failed to create it bail out
            if (cacheEntry == null) return;

            cacheEntry.SnapToScreen = snapToScreen;
            cacheEntry.UsedThisFrame = true;

            // it's null if there is no background so draw it
            if (cacheEntry.Rectangle != null) cacheEntry.Rectangle.Position = position;

            SDL_Rect fontSrcRect = new(0, 0, fontSizeX, fontSizeY);
            SDL_FRect fontDstRect = new(position.X, position.Y, fontSizeX, fontSizeY);

            foreach (TextCacheEntryLine line in cacheEntry.Lines)
            {
                fontSrcRect.w = (int)line.Size.X;
                fontSrcRect.h = (int)line.Size.Y;

                fontDstRect.w = fontSrcRect.w;
                fontDstRect.h = fontSrcRect.h;

                // TEMPORARY HACK UNTIL FREETYPE (THIS CODE IS OBSOLETE ANYWAY)

                if (Lightning.Renderer is SdlRenderer)
                {
                    SdlRenderer sdlRenderer = (SdlRenderer)Lightning.Renderer;
                    SDL_RenderCopyF(sdlRenderer.Settings.RendererHandle, line.Handle, ref fontSrcRect, ref fontDstRect);
                }
                

                // increment by the line length
                if (lineLength < 0)
                {
                    fontDstRect.y += fontSizeY;
                }
                else
                {
                    fontDstRect.y += lineLength;
                }
            }

            // weird hack. if we don't do this weird stuff happens
            if (outlineSize > -1) TTF_SetFontOutline(curFont.Handle, -1);
            */
        }

        internal TextCacheEntry? AddEntry(string font, string text,
            SDL_Color color, TTF_FontStyle style, FontSmoothingType type = FontSmoothingType.Default, int outlineSize = -1, SDL_Color bgColor = default)
        {
            TextCacheEntry? entry = TextCacheEntry.Render(font, text, color, style, type, outlineSize, bgColor);
            
            if (entry != null) Lightning.Renderer.AddRenderable(entry, this);

            return entry;
        }

        internal void PurgeUnusedEntries()
        {
            // memory leaks are bad
            // TODO: when we extend to multithreading in the future this is a very good and easy TOCTOU / race condition issue

            for (int entryId = 0; entryId < Children.Count; entryId++)
            {
                TextCacheEntry entry = (TextCacheEntry)Children[entryId];

                if (!entry.UsedThisFrame)
                {
                    NCLogging.Log($"Removing unused cached text (font={entry.Font}, text={entry.Text}, style={entry.Style}, " +
                        $"smoothing type={entry.SmoothingType}, bgcolor={entry.BackgroundColor}, outline size={entry.OutlineSize})");
                    DeleteEntry(entry);
                }
            }
        }

        #region Cache code

        internal TextCacheEntry? GetEntry(string font, string text,
            SDL_Color color, TTF_FontStyle style, FontSmoothingType type = FontSmoothingType.Default, int outlineSize = -1, SDL_Color bgColor = default)
        {
            foreach (TextCacheEntry entry in Children)
            {
                if (entry.Font == font
                    && entry.Text == text
                    && entry.Color.Equals(color)
                    && entry.Style == style
                    && entry.SmoothingType == type
                    && entry.OutlineSize == outlineSize
                    && entry.BackgroundColor.Equals(bgColor))
                {
                    return entry;
                }
            }

            return null;
        }

        internal void DeleteEntry(string font, string text,
            SDL_Color color, TTF_FontStyle style, FontSmoothingType type = FontSmoothingType.Default, int outlineSize = -1, SDL_Color bgColor = default)
        {
            TextCacheEntry? fontEntry = GetEntry(font, text, color, style, type, outlineSize, bgColor);

            DeleteEntry(fontEntry);
        }

        internal void DeleteEntry(TextCacheEntry? fontEntry)
        {
            if (fontEntry != null)
            {
                fontEntry.Unload();
                Lightning.Renderer.RemoveRenderable(fontEntry);
            }
            else
            {
                NCError.ShowErrorBox($"Attempted to delete null TextCacheEntry! I twill not be deleted.", 191,
                    "TextManager::DeleteEntry called with fontEntry parameter set to NULL", NCErrorSeverity.Warning, null, true);
            }
        }

        internal void UnloadAll()
        {
            foreach (TextCacheEntry entry in Children)
            {
                entry.Unload();
            }
        }

        public override void Update()
        {
            PurgeUnusedEntries();
            foreach (TextCacheEntry entry in Children) entry.UsedThisFrame = false;
        }

        /// <summary>
        /// Internal: Shuts down the Font Manager.
        /// </summary>
        internal void Shutdown()
        {
            NCLogging.Log("Uncaching all cached text - shutdown requested");
            UnloadAll();
        }
        
        #endregion
    }
}
