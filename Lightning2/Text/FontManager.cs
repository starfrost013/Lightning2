using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using static NuCore.SDL2.SDL;
using static NuCore.SDL2.SDL_ttf;

namespace LightningGL
{
    /// <summary>
    /// FontManager
    /// 
    /// Provides functions for managing fonts and drawing text.
    /// </summary>
    public static class FontManager
    {
        /// <summary>
        /// The list of loaded fonts.
        /// </summary>
        public static List<Font> Fonts { get; private set; }

        /// <summary>
        /// Constructor for the Font Manager.
        /// </summary>
        static FontManager()
        {
            Fonts = new List<Font>();
        }

        /// <summary>
        /// Acquires a Font if it is loaded, given its <see cref="Font.FriendlyName"/>.
        /// </summary>
        /// <param name="friendlyName">The <see cref="Font.FriendlyName"/></param>
        /// <returns></returns>
        public static Font GetFont(string friendlyName)
        {
            foreach (Font font in Fonts)
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
        public static void LoadFont(string name, int size, string friendlyName, string path = null, int index = 0)
        {
            try
            {
                Font font = Font.Load(name, size, friendlyName, path, index);
                Fonts.Add(font);
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
        public static void UnloadFont(Font font)
        {
            try
            {
                font.Unload();
                Fonts.Remove(font);
            }
            catch (Exception) { };
        }

        public static void UnloadFont(string friendlyName)
        {
            Font fontToUnload = GetFont(friendlyName);

            if (fontToUnload == null) _ = new NCException($"Attempted to unload invalid font FriendlyName {friendlyName}!", 71, "nonexistent friendlyName passed to TextManager::UnloadFont(string)!", NCExceptionSeverity.FatalError); // possibly not fatal?

            fontToUnload.Unload();
            Fonts.Remove(fontToUnload);
        }

        /// <summary>
        /// <para>Gets the text size for the text <paramref name="text"/> using <paramref name="font"/>. </para>
        /// <para>If the text has multiple lines, it will use the largest line X size as the size and the size of <br/>all lines as the Y size.</para>
        /// </summary>
        /// <param name="font">The font used for <paramref name="text"/></param>
        /// <param name="text">The text to get the font size for</param>
        /// <returns>A <see cref="Vector2"/> containing the size of <paramref name="text"/> in pixels.</returns>
        public static Vector2 GetTextSize(Font font, string text)
        {
            // check the string is not empty
            if (string.IsNullOrWhiteSpace(text)) return default(Vector2);

            // call the multiline text function
            if (text.Contains('\n')) return GetLargestTextSize(font, text);

            if (font == null  
                || !Fonts.Contains(font)) _ = new NCException($"Please load font (Name={font.Name}, Size={font.Size}) before trying to use it!", 81, "TextManager::GetTextSize - Font parameter null or font not in font list!", NCExceptionSeverity.FatalError);

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
        public static Vector2 GetTextSize(string font, string text)
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
        internal static Vector2 GetLargestTextSize(Font font, string text)
        {
            // check the string is not empty
            if (string.IsNullOrWhiteSpace(text)) return default(Vector2);

            // check it's a real font
            if (font == null
            || !Fonts.Contains(font)) _ = new NCException($"Please load font (Name={font.Name}, Size={font.Size}) before trying to use it!", 82, "TextManager::GetTextSize - Font parameter null or font not in font list!", NCExceptionSeverity.FatalError);

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
        public static Vector2 GetLargestTextSize(string font, string text)
        {
            Font curFont = GetFont(font);

            return GetLargestTextSize(curFont, text);
        }

        /// <summary>
        /// Draws text to the screen.
        /// </summary>
        /// <param name="cWindow">The Window to draw this text to.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The <see cref="Font.FriendlyName"/> of the <see cref="Font"/> to draw this text in.</param>
        /// <param name="position">The position to draw the text.</param>
        /// <param name="foreground">The foreground colour of the text.</param>
        /// <param name="background">Optional: The background colour of the text.</param>
        /// <param name="style">Optional: The <see cref="TTF_FontStyle"/> of the text.</param>
        /// <param name="resizeFont">Optional: Font size to resize the font to.</param>
        /// <param name="outlineSize">Optional: Size of the font outline in pixels. Range is 1 to 15.</param>
        /// <param name="lineLength">Optional: Maximum line length in pixels. Ignores newlines.</param>
        /// <param name="smoothing">Optional: The <see cref="FontSmoothingType"/> of the text.</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space (false) or screen-relative space (true).</param>
        public static void DrawText(Window cWindow, string text, string font, Vector2 position, Color foreground, Color background = default(Color), 
            TTF_FontStyle style = TTF_FontStyle.Normal, int resizeFont = -1, int outlineSize = -1, int lineLength = -1, FontSmoothingType smoothing = FontSmoothingType.Default, 
            bool snapToScreen = false)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cWindow.Settings.Camera;

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

            // Set the foreground colour
            SDL_Color fontColour = new SDL_Color(foreground.R, foreground.G, foreground.B, foreground.A);

            // Resize font if specified
            if (resizeFont > 0)
            {
                TTF_SetFontSize(curFont.Handle, resizeFont);
                curFont.Size = resizeFont;
            }

            // split the text into lines
            // add the length of each line to the text length
            string[] textLines = text.Split("\n");

            // default to entirely transparent background (if the user has specified shasded for some reason, we still need a BG colour...)
            SDL_Color bgColour = new SDL_Color(0, 0, 0, 0);

            int fontSizeX = -1;
            int fontSizeY = -1;

            if (background != default(Color)) // draw the background (which requires sizing the entire text)
            {
                // Set the background colour
                bgColour = new SDL_Color(background.R, background.G, background.B, background.A);

                Vector2 fontSize = GetTextSize(curFont, text);

                // get the number of lines
                int numberOfLines = textLines.Length;

                fontSizeX = (int)fontSize.X;
                fontSizeY = (int)fontSize.Y;

                int totalSizeX = fontSizeX;
                int totalSizeY = fontSizeY;

                for (int i = 0; i < numberOfLines - 1; i++) // -1 to prevent double-counting the first line
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
                PrimitiveRenderer.DrawRectangle(cWindow, position, new(totalSizeX, totalSizeY), Color.FromArgb(bgColour.a, bgColour.r, bgColour.g, bgColour.b), true, default, default, true);
            }

            // Set the font outline, size and style
            // Too much larger than the font size in pt causes C++ exceptions in SDL2 (probably larger than the surface it's being drawn to...) so we just use that as a limit
            if (outlineSize > 0 || outlineSize <= curFont.Size) TTF_SetFontOutline(curFont.Handle, outlineSize);

            TTF_SetFontStyle(curFont.Handle, style);

            IntPtr textPtr = IntPtr.Zero;
            IntPtr textTexturePtr = IntPtr.Zero;

            SDL_Rect fontSrcRect = new SDL_Rect(0, 0, fontSizeX, fontSizeY);
            SDL_Rect fontDstRect = new SDL_Rect((int)position.X, (int)position.Y, fontSizeX, fontSizeY);

            foreach (string line in textLines)
            {
                Vector2 lineSize = GetTextSize(curFont, line);

                fontSrcRect.w = (int)lineSize.X;
                fontSrcRect.h = (int)lineSize.Y;

                fontDstRect.w = (int)lineSize.X;
                fontDstRect.h = (int)lineSize.Y;

                switch (smoothing)
                {
                    case FontSmoothingType.Default: // Antialiased
                        textPtr = TTF_RenderUTF8_Blended(curFont.Handle, line, fontColour);
                        textTexturePtr = SDL_CreateTextureFromSurface(cWindow.Settings.RendererHandle, textPtr);

                        SDL_RenderCopy(cWindow.Settings.RendererHandle, textTexturePtr, ref fontSrcRect, ref fontDstRect);

                        SDL_FreeSurface(textPtr);
                        SDL_DestroyTexture(textTexturePtr);

                        // increment by the line length
                        if (lineLength < 0)
                        {
                            fontDstRect.y += fontSizeY;
                        }
                        else
                        {
                            fontDstRect.y += lineLength;
                        }

                        continue;
                    case FontSmoothingType.Shaded: // Only shaded
                        textPtr = TTF_RenderUTF8_Shaded(curFont.Handle, line, fontColour, bgColour);
                        textTexturePtr = SDL_CreateTextureFromSurface(cWindow.Settings.RendererHandle, textPtr);

                        SDL_RenderCopy(cWindow.Settings.RendererHandle, textTexturePtr, ref fontSrcRect, ref fontDstRect);

                        SDL_FreeSurface(textPtr);
                        SDL_DestroyTexture(textTexturePtr);

                        // increment by the line length
                        if (lineLength < 0)
                        {
                            fontDstRect.y += fontSizeY;
                        }
                        else
                        {
                            fontDstRect.y += lineLength;
                        }

                        continue;
                    case FontSmoothingType.Solid: // No processing done
                        textPtr = TTF_RenderUTF8_Solid(curFont.Handle, line, fontColour);
                        textTexturePtr = SDL_CreateTextureFromSurface(cWindow.Settings.RendererHandle, textPtr);

                        SDL_RenderCopy(cWindow.Settings.RendererHandle, textTexturePtr, ref fontSrcRect, ref fontDstRect);

                        SDL_FreeSurface(textPtr);
                        SDL_DestroyTexture(textTexturePtr);

                        // increment by the line length
                        if (lineLength < 0)
                        {
                            fontDstRect.y += fontSizeY;
                        }
                        else
                        {
                            fontDstRect.y += lineLength;
                        }

                        continue;
                }
            }
        }
    }
}