namespace LightningGL
{
    /// <summary>
    /// FontManager
    /// 
    /// Provides functions for managing fonts and drawing text.
    /// </summary>
    public class FontAssetManager : AssetManager<Font>
    {
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

        internal void Shutdown()
        {
            for (int curFontId = 0; curFontId < Assets.Count; curFontId++)
            {
                Font curFont = FontManager.Assets[curFontId];
                NCLogging.Log($"Unloading font {curFont.FriendlyName}...");
                FontManager.UnloadFont(curFont);
            }
        }
    }
}