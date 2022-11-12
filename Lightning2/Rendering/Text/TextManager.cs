namespace LightningGL
{
    public class TextAssetManager : AssetManager<TextCacheEntry>
    {
        public override TextCacheEntry AddAsset(TextCacheEntry asset)
        {
            // temp code
            if (asset.Font != null)
            {
                DrawText(asset.Text, asset.Font, asset.Position, Color.FromArgb(asset.Color.a, asset.Color.r, asset.Color.g, asset.Color.b),
                    Color.FromArgb(asset.BackgroundColor.a, asset.BackgroundColor.r, asset.BackgroundColor.g, asset.BackgroundColor.b), asset.Style, asset.OutlineSize, -1,
                    asset.SmoothingType, false);
            }

            return asset;
        }


        /// <summary>
        /// Draws text to the screen.
        /// </summary>
        /// <param name="Lightning.Renderer">The Window to draw this text to.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The <see cref="Font.FriendlyName"/> of the <see cref="Font"/> to draw this text in.</param>
        /// <param name="position">The position to draw the text.</param>
        /// <param name="foreground">The foreground color of the text.</param>
        /// <param name="background">Optional: The background color of the text.</param>
        /// <param name="style">Optional: The <see cref="TTF_FontStyle"/> of the text.</param>
        /// <param name="outlineSize">Optional: Size of the font outline . Range is 1 to the font size.</param>
        /// <param name="lineLength">Optional: Maximum line length in pixels. Ignores newlines.</param>
        /// <param name="smoothingType">Optional: The <see cref="FontSmoothingType"/> of the text.</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public void DrawText(string text, string font, Vector2 position, Color foreground, Color background = default,
            TTF_FontStyle style = TTF_FontStyle.Normal, int outlineSize = -1, int lineLength = -1, FontSmoothingType smoothingType = FontSmoothingType.Default,
            bool snapToScreen = false, bool localise = true)
        {

            // Localise the string using Localisation Manager.
            if (localise) text = LocalisationManager.ProcessString(text);

            // Get the font and throw an error if it's invalid
            Font? curFont = FontManager.GetFont(font);

            if (curFont == null)
            {
                _ = new NCException($"Attempted to acquire invalid font with name {font}", 39, "TextManager::DrawText font parameter is not a loaded font!", NCExceptionSeverity.FatalError);
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

            // if it's still null bail out
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

                SDL_RenderCopyF(Lightning.Renderer.Settings.RendererHandle, line.Handle, ref fontSrcRect, ref fontDstRect);

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
        }

        internal TextCacheEntry? AddEntry(string font, string text,
            SDL_Color color, TTF_FontStyle style, FontSmoothingType type = FontSmoothingType.Default, int outlineSize = -1, SDL_Color bgColor = default)
        {
            TextCacheEntry? entry = TextCacheEntry.Render(font, text, color, style, type, outlineSize, bgColor);
            
            if (entry != null) Lightning.Renderer.AddRenderable(entry);

            return entry;

        }

        internal void PurgeUnusedEntries()
        {
            // memory leaks are bad
            // TODO: when we extend to multithreading in the future this is a very good and easy TOCTOU / race condition issue
            List<TextCacheEntry> textCacheEntries = GetAllTextCacheEntries();

            for (int entryId = 0; entryId < textCacheEntries.Count; entryId++)
            {
                TextCacheEntry entry = textCacheEntries[entryId];

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
            foreach (TextCacheEntry entry in GetAllTextCacheEntries())
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
                _ = new NCException($"Attempted to delete null TextCacheEntry! I twill not be deleted.", 191,
                    "TextManager::DeleteEntry called with fontEntry parameter set to NULL", NCExceptionSeverity.Warning, null, true);
            }
        }

        internal void UnloadAll()
        {
            foreach (TextCacheEntry entry in GetAllTextCacheEntries())
            {
                entry.Unload();
            }
        }

        internal override void Update()
        {
            PurgeUnusedEntries();
            foreach (TextCacheEntry entry in GetAllTextCacheEntries()) entry.UsedThisFrame = false;
        }

        /// <summary>
        /// Internal: Shuts down the Font Manager.
        /// </summary>
        internal void Shutdown()
        {
            NCLogging.Log("Uncaching all cached text - shutdown requested");
            UnloadAll();
        }

        // bit of a hack for the new 2.0 model so that textcacheentries can still be Renderables
        private List<TextCacheEntry> GetAllTextCacheEntries()
        {
            List<TextCacheEntry> textCacheEntries = new();

            foreach (Renderable renderable in Lightning.Renderer.Renderables)
            {
                if (renderable is TextCacheEntry entry)
                {
                    textCacheEntries.Add(entry);
                }
            }

            return textCacheEntries;

        }
        
        #endregion
    }
}
