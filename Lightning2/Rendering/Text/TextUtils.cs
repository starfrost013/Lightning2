namespace LightningGL
{
    /// <summary>
    /// FontManager
    /// 
    /// Provides functions for managing fonts and drawing text.
    /// </summary>
    public static class TextUtils
    {
        /// <summary>
        /// <para>Gets the text size for the text <paramref name="text"/> using <paramref name="font"/>. </para>
        /// <para>If the text has multiple lines, it will use the largest line X size as the size and the size of <br/>all lines as the Y size.</para>
        /// </summary>
        /// <param name="font">The font used for <paramref name="text"/></param>
        /// <param name="text">The text to get the font size for</param>
        /// <returns>A <see cref="Vector2"/> containing the size of <paramref name="text"/> in pixels.</returns>
        public static Vector2 GetTextSize(Font font, string text, Color foregroundColor)
        {
            // check the string is not empty
            if (string.IsNullOrWhiteSpace(text)) return default;

            if (font == null)
            {
                Logger.LogError($"Passed a null font parameter to FontManager::GetTextSize!", 81, LoggerSeverity.FatalError);
                return default;
            }

            // call the multiline text function
            if (text.Contains('\n', StringComparison.InvariantCultureIgnoreCase)) return GetLargestTextSize(font, text, foregroundColor);

            if (!Lightning.Renderer.ContainsRenderable(font.Name)) Logger.LogError($"Please load font (Name={font.Name}, Size={font.FontSize}) before trying to use it!", 
                81, LoggerSeverity.FatalError);

            Vector2 fontSize = new();

            foreach (char textChar in text)
            {
                Glyph? glyph = null;

                // this automatically caches it now
                glyph = GlyphCache.QueryCache(font.Name, textChar, foregroundColor); // todo: set the smoothing type

                // is it still null? then ignore it (there should already be an error here) 
                if (glyph != null)
                {
                    fontSize.X += glyph.Size.X;
                    if (glyph.Size.Y > fontSize.Y) fontSize.Y += glyph.Size.Y;
                }
            }

            return fontSize;
        }

        /// <summary>
        /// <para>Gets the text size for the text <paramref name="text"/> using the font with the <see cref="Font.FriendlyName"/> <paramref name="font"/></para>
        /// <para>If the text has multiple lines, it will use the largest line X size as the size and the size of <br/>all lines as the Y size.</para>
        /// </summary>
        /// <param name="font">The font used for <paramref name="text"/></param>
        /// <param name="text">The text to get the font size for</param>
        /// <returns>A <see cref="Vector2"/> containing the size of <paramref name="text"/> in pixels.</returns>
        public static Vector2 GetTextSize(string font, string text, Color foregroundColor)
        {
            Font? curFont = (Font?)Lightning.Renderer.GetRenderableByName(font);

            if (curFont == null)
            {
                Logger.LogError($"GetTextSize was provided an invalid font, " +
                    $"so (0,0) will be returned! The text will not be drawn!", 190, LoggerSeverity.Warning, null, true);
                return default;
            }

            return GetTextSize(curFont, text, foregroundColor);
        }

        /// <summary>
        /// <para> Internal: Gets the font size for the largest line for the text <paramref name="text"/> using the font with the <see cref="Font.FriendlyName"/> <paramref name="font"/></para>
        /// <para>If the text has multiple lines, it will use the largest line size as the size.</para>
        /// </summary>
        /// <param name="font">The font used for <paramref name="text"/></param>
        /// <param name="text">The text to get the font size for</param>
        /// <returns>A <see cref="Vector2"/> containing the size of <paramref name="text"/> in pixels.</returns>
        internal static Vector2 GetLargestTextSize(Font font, string text, Color foregroundColor)
        {
            // check the string is not empty
            if (string.IsNullOrWhiteSpace(text)) return default;

            // check it's a real font
            if (font == null)
            {
                Logger.LogError($"Tried to pass null to FontManager::GetLargestTextSize!", 183, LoggerSeverity.FatalError);
                return default; 
            }

            if (!Lightning.Renderer.ContainsRenderable(font.Name)) Logger.LogError($"Please load font (Name={font.Name}, " +
                $"Size={font.FontSize}) before trying to use it!", 82, LoggerSeverity.FatalError);

            string[] lines = text.Split('\n');

            Vector2 largestLineSize = default; // 0,0

            foreach (string line in lines)
            {
                Vector2 curLineSize = GetTextSize(font, line, foregroundColor);
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
        internal static Vector2 GetLargestTextSize(string font, string text, Color foregroundColor)
        {
            Font? curFont = (Font?)Lightning.Renderer.GetRenderableByName(font);

            if (curFont != null)
            {
                return GetLargestTextSize(curFont, text, foregroundColor);
            }
            else
            {
                Logger.LogError($"GetLargestTextSize was provided an invalid font, so (0,0) will be returned", 189, LoggerSeverity.Warning, null, true);
                return default;
            }
        }
    }
}