﻿namespace LightningGL
{
    /// <summary>
    /// FontManager
    /// 
    /// Provides functions for managing fonts and drawing text.
    /// </summary>
    public class FTFontAssetManager : AssetManager<Font>
    {
        public override Font? AddAsset(Font asset)
        {
            if (asset != null)
            {
                try
                {
                    Lightning.Renderer.AddRenderable(asset);
                }
                catch (Exception) // NC Exception
                {
                    return null;
                }
            }
            else
            {
                NCError.ShowErrorBox("Passed null font to FontAssetManager::AddAsset!", 184, "FontAssetManager::AddAsset asset parameter was null!", NCErrorSeverity.FatalError);
            }

            return asset;
        }

        public override void RemoveAsset(Font asset) => asset.Unload();

        /// <summary>
        /// Acquires a Font if it is loaded, given its <see cref="Font.FriendlyName"/>.
        /// </summary>
        /// <param name="friendlyName">The <see cref="Font.FriendlyName"/></param>
        /// <returns></returns>
        internal Font? GetFont(string friendlyName)
        {
            try
            {
                return (Font?)Lightning.Renderer.GetRenderableByName(friendlyName);
            }
            catch
            {
                NCError.ShowErrorBox("Attempted to acquire invalid font!", 195, "FontManager::GetFont returned an invalid font", NCErrorSeverity.FatalError);
                return null;
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
                Lightning.Renderer.RemoveRenderable(font);
            }
            catch (Exception) { };
        }

        public void UnloadFont(string friendlyName)
        {
            Font? fontToUnload = GetFont(friendlyName);

            if (fontToUnload == null)
            {
                NCError.ShowErrorBox($"Attempted to unload invalid font FriendlyName {friendlyName}!", 71, "nonexistent friendlyName passed to TextManager::UnloadFont(string)!", NCErrorSeverity.FatalError); // possibly not fatal?
                return;
            }

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
            if (string.IsNullOrWhiteSpace(text)) return default;

            if (font == null)
            {
                NCError.ShowErrorBox($"Passed a null font parameter to FontManager::GetTextSize!", 81, "TextManager::GetTextSize - Font parameter null!", 
                    NCErrorSeverity.FatalError);
                return default;
            }

            // call the multiline text function
            if (text.Contains('\n', StringComparison.InvariantCultureIgnoreCase)) return GetLargestTextSize(font, text);

            if (!Lightning.Renderer.ContainsRenderable(font.Name)) NCError.ShowErrorBox($"Please load font (Name={font.Name}, Size={font.FontSize}) before trying to use it!", 81, "TextManager::GetTextSize - Font parameter null or font not in font list!", NCErrorSeverity.FatalError);

            int fontSizeX,
                fontSizeY;

            if (TTF_SizeUTF8(font.Handle, text, out fontSizeX, out fontSizeY) < 0) NCError.ShowErrorBox($"An error occurred sizing text. {SDL_GetError()}", 80, "TTF_SizeUTF8 call from TextManager::GetTextSize failed", NCErrorSeverity.FatalError);

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
            Font? curFont = GetFont(font);

            if (curFont == null)
            {
                NCError.ShowErrorBox($"GetTextSize was provided an invalid font, so (0,0) will be returned", 190,
                    "The Font parameter to FontManager::GetTextSize did not correspond to a loaded font", NCErrorSeverity.Warning, null, true);
                return default;
            }

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
            if (string.IsNullOrWhiteSpace(text)) return default;

            // check it's a real font
            if (font == null)
            {
                NCError.ShowErrorBox($"Tried to pass null to FontManager::GetLargestTextSize!", 183,
                "TextManager::GetTextSize - Font parameter null!", NCErrorSeverity.FatalError);
                return default; 
            }

            if (!Lightning.Renderer.ContainsRenderable(font.Name)) NCError.ShowErrorBox($"Please load font (Name={font.Name}, Size={font.FontSize}) before trying to use it!", 82, 
                "TextManager::GetTextSize - Font not in FontManager::Assets!", NCErrorSeverity.FatalError);

            string[] lines = text.Split('\n');

            Vector2 largestLineSize = default; // 0,0

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
            Font? curFont = GetFont(font);

            if (curFont != null)
            {
                return GetLargestTextSize(curFont, text);
            }
            else
            {
                NCError.ShowErrorBox($"GetLargestTextSize was provided an invalid font, so (0,0) will be returned", 189,
                    "The Font parameter to FontManager::GetLargestTextSize did not correspond to a loaded font", NCErrorSeverity.Warning, null, true);
                return default;
            }
        }
    }
}