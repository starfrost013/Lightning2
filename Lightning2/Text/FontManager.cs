namespace LightningGL
{
    /// <summary>
    /// FontManager
    /// 
    /// Provides functions for managing fonts and drawing text.
    /// </summary>
    public class FontAssetManager : AssetManager<Font>
    {

        /// <summary>
        /// The font cache, used to increase speed.
        /// See <see cref="FontCache"/>.
        /// </summary>
        internal static FontCache Cache { get; private set; }

        /// <summary>
        /// Constructor for the Font Manager.
        /// </summary>
        static FontAssetManager()
        {
            Cache = new FontCache();
        }

        public override Font AddAsset(Renderer cRenderer, Font asset)
        {
            LoadFont(asset.Name, asset.FontSize, asset.FriendlyName, asset.Path, asset.Index);
            return asset;
        }

        public override void RemoveAsset(Renderer cRenderer, Font asset) => asset.Unload();

        /// <summary>
        /// Acquires a Font if it is loaded, given its <see cref="Font.FriendlyName"/>.
        /// </summary>
        /// <param name="friendlyName">The <see cref="Font.FriendlyName"/></param>
        /// <returns></returns>
        public  Font GetFont(string friendlyName)
        {
            foreach (Font font in Assets)
            {
                if (font.FriendlyName == friendlyName)
                {
                    return font;
                }
            }

            return null;
        }

        /// <summary>
        /// Loads a font.
        /// </summary>
        /// <param name="name">The name of the font to load.</param>
        /// <param name="size">The size of the font to load.</param>
        /// <param name="friendlyName">The friendly name of the font to load.</param>
        /// <param name="path">The path to this font. If it is null, it will be loaded from the system font directory.</param>
        /// <param name="index">Index of the font in the font file to load. Will default to 0.</param>
        public void LoadFont(string name, int size, string friendlyName, string path = null, int index = 0)
        {
            try
            {
                Font font = Font.Load(name, size, friendlyName, path, index);
                Assets.Add(font);
            }
            catch (Exception) // NC Exception
            {
                return;
            }
        }

        /// <summary>
        /// Unloads a font.
        /// </summary>
        /// <param name="font">The font to unload.</param>
        public void UnloadFont(Font font)
        {
            try
            {
                font.Unload();
                Assets.Remove(font);
            }
            catch (Exception) { };
        }

        public void UnloadFont(string friendlyName)
        {
            Font fontToUnload = GetFont(friendlyName);

            if (fontToUnload == null) _ = new NCException($"Attempted to unload invalid font FriendlyName {friendlyName}!", 71, "nonexistent friendlyName passed to TextManager::UnloadFont(string)!", NCExceptionSeverity.FatalError); // possibly not fatal?

            UnloadFont(fontToUnload);
        }

        /// <summary>
        /// <para>Gets the text size for the text <paramref name="text"/> using <paramref name="font"/>. </para>
        /// <para>If the text has multiple lines, it will use the largest line X size as the size and the size of <br/>all lines as the Y size.</para>
        /// </summary>
        /// <param name="font">The font used for <paramref name="text"/></param>
        /// <param name="text">The text to get the font size for</param>
        /// <returns>A <see cref="Vector2"/> containing the size of <paramref name="text"/> in pixels.</returns>
        public Vector2 GetTextSize(Font font, string text)
        {
            // check the string is not empty
            if (string.IsNullOrWhiteSpace(text)) return default(Vector2);

            // call the multiline text function
            if (text.Contains('\n')) return GetLargestTextSize(font, text);

            if (font == null
                || !Assets.Contains(font)) _ = new NCException($"Please load font (Name={font.Name}, Size={font.FontSize}) before trying to use it!", 81, "TextManager::GetTextSize - Font parameter null or font not in font list!", NCExceptionSeverity.FatalError);

            int fontSizeX,
                fontSizeY;

            if (TTF_SizeUTF8(font.Handle, text, out fontSizeX, out fontSizeY) < 0) _ = new NCException($"An error occurred sizing text. {SDL_GetError()}", 80, "TTF_SizeUTF8 call from TextManager::GetTextSize failed", NCExceptionSeverity.FatalError);

            return new Vector2(fontSizeX, fontSizeY);
        }

        /// <summary>
        /// <para>Gets the text size for the text <paramref name="text"/> using the font with the <see cref="Font.FriendlyName"/> <paramref name="font"/></para>
        /// <para>If the text has multiple lines, it will use the largest line X size as the size and the size of <br/>all lines as the Y size.</para>
        /// </summary>
        /// <param name="font">The font used for <paramref name="text"/></param>
        /// <param name="text">The text to get the font size for</param>
        /// <returns>A <see cref="Vector2"/> containing the size of <paramref name="text"/> in pixels.</returns>
        public Vector2 GetTextSize(string font, string text)
        {
            Font curFont = GetFont(font);

            return GetTextSize(curFont, text);
        }

        /// <summary>
        /// <para> Internal: Gets the font size for the largest line for the text <paramref name="text"/> using the font with the <see cref="Font.FriendlyName"/> <paramref name="font"/></para>
        /// <para>If the text has multiple lines, it will use the largest line size as the size.</para>
        /// </summary>
        /// <param name="font">The font used for <paramref name="text"/></param>
        /// <param name="text">The text to get the font size for</param>
        /// <returns>A <see cref="Vector2"/> containing the size of <paramref name="text"/> in pixels.</returns>
        internal Vector2 GetLargestTextSize(Font font, string text)
        {
            // check the string is not empty
            if (string.IsNullOrWhiteSpace(text)) return default(Vector2);

            // check it's a real font
            if (font == null
            || !Assets.Contains(font)) _ = new NCException($"Please load font (Name={font.Name}, Size={font.FontSize}) before trying to use it!", 82, "TextManager::GetTextSize - Font parameter null or font not in font list!", NCExceptionSeverity.FatalError);

            string[] lines = text.Split('\n');

            Vector2 largestLineSize = default(Vector2); // 0,0

            foreach (string line in lines)
            {
                Vector2 curLineSize = GetTextSize(font, line);
                if (curLineSize.X > largestLineSize.X
                    && curLineSize.Y > largestLineSize.Y) largestLineSize = curLineSize;

            }

            return largestLineSize;
        }

        /// <summary>
        /// <para> Internal: Gets the font size for the largest line for the text <paramref name="text"/> using the font with the <see cref="Font.FriendlyName"/> <paramref name="font"/></para>
        /// <para>If the text has multiple lines, it will use the largest line X size as the size and the size of <br/>all lines as the Y size.</para>
        /// </summary>
        /// <param name="font">The font used for <paramref name="text"/></param>
        /// <param name="text">The text to get the font size for</param>
        /// <returns>A <see cref="Vector2"/> containing the size of <paramref name="text"/> in pixels.</returns>
        internal Vector2 GetLargestTextSize(string font, string text)
        {
            Font curFont = GetFont(font);

            return GetLargestTextSize(curFont, text);
        }

        /// <summary>
        /// Draws text to the screen.
        /// </summary>
        /// <param name="cRenderer">The Window to draw this text to.</param>
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
        public void DrawText(Renderer cRenderer, string text, string font, Vector2 position, Color foreground, Color background = default(Color),
            TTF_FontStyle style = TTF_FontStyle.Normal, int outlineSize = -1, int lineLength = -1, FontSmoothingType smoothingType = FontSmoothingType.Default,
            bool snapToScreen = false)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cRenderer.Settings.Camera;

            if (currentCamera != null
                && !snapToScreen)
            {
                position.X -= currentCamera.Position.X;
                position.Y -= currentCamera.Position.Y;
            }

            // Localise the string using Localisation Manager.
            text = LocalisationManager.ProcessString(text);

            // Get the font and throw an error if it's invalid
            Font curFont = GetFont(font);

            if (curFont == null) _ = new NCException($"Attempted to acquire invalid font with name {font}", 39, "TextManager::DrawText font parameter is not a loaded font!", NCExceptionSeverity.FatalError);

            // Set the foreground color
            SDL_Color fgColor = new(foreground.R, foreground.G, foreground.B, foreground.A);

            // split the text into lines
            // add the length of each line to the text length
            string[] textLines = text.Split("\n");

            // default to entirely transparent background (if the user has specified shasded for some reason, we still need a BG color...)
            SDL_Color bgColor = default;

            int fontSizeX = -1;
            int fontSizeY = -1;

            // Draw the background
            if (background != default(Color)
                && smoothingType != FontSmoothingType.Shaded) // draw the background (which requires sizing the entire text)
            {
                // Set the background color
                bgColor = new SDL_Color(background.R, background.G, background.B, background.A);

                Vector2 fontSize = GetTextSize(curFont, text);

                // get the number of lines
                int numberOfLines = textLines.Length;

                fontSizeX = (int)fontSize.X;
                fontSizeY = (int)fontSize.Y;

                int totalSizeX = fontSizeX;
                int totalSizeY = fontSizeY;

                for (int lineId = 0; lineId < numberOfLines - 1; lineId++) // -1 to prevent double-counting the first line
                {
                    if (lineLength < 0)
                    {
                        totalSizeY += fontSizeY;
                    }
                    else
                    {
                        totalSizeY += lineLength;
                    }
                }

                // get the size of the largest line
                if (numberOfLines > 1) totalSizeX = (int)GetLargestTextSize(curFont, text).X;

                // camera-aware is false for this as we have already "pushed" the position, so we don't need to do it again.
                PrimitiveRenderer.DrawRectangle(cRenderer, position, new(totalSizeX, totalSizeY), Color.FromArgb(bgColor.a, bgColor.r, bgColor.g, bgColor.b), true, default, default, true);
            }

            // use the cached entry if it exists
            FontCacheEntry cacheEntry = Cache.GetEntry(font, text, fgColor, style, smoothingType, outlineSize, bgColor);

            // if it doesn't exist add it
            if (cacheEntry == null) cacheEntry = Cache.AddEntry(cRenderer, font, text, fgColor, style, smoothingType, outlineSize, bgColor);

            cacheEntry.UsedThisFrame = true;

            SDL_Rect fontSrcRect = new SDL_Rect(0, 0, fontSizeX, fontSizeY);
            SDL_FRect fontDstRect = new SDL_FRect(position.X, position.Y, fontSizeX, fontSizeY);

            foreach (FontCacheEntryLine line in cacheEntry.Lines)
            {
                fontSrcRect.w = (int)line.Size.X;
                fontSrcRect.h = (int)line.Size.Y;

                fontDstRect.w = fontSrcRect.w;
                fontDstRect.h = fontSrcRect.h;

                SDL_RenderCopyF(cRenderer.Settings.RendererHandle, line.Handle, ref fontSrcRect, ref fontDstRect);

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

        internal override void Update(Renderer cRenderer)
        {
            Cache.PurgeUnusedEntries();
            foreach (FontCacheEntry entry in Cache.Entries) entry.UsedThisFrame = false;
        }

        /// <summary>
        /// Internal: Shuts down the Font Manager.
        /// </summary>
        internal void Shutdown()
        {
            NCLogging.Log("Uncaching all cached text - shutdown requested");
            Cache.Unload();

            for (int curFontId = 0; curFontId < Assets.Count; curFontId++)
            {
                Font curFont = Assets[curFontId];
                NCLogging.Log($"Unloading font {curFont.FriendlyName}...");
                UnloadFont(curFont);
            }
        }
    }
}