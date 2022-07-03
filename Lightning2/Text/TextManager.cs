using static NuCore.SDL2.SDL;
using static NuCore.SDL2.SDL_ttf;
using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace LightningGL
{
    public static class FontManager
    {
        public static List<Font> Fonts { get; set; }

        static FontManager()
        {
            Fonts = new List<Font>();
        }

        public static Font GetFont(string friendlyName)
        {
            foreach (Font f in Fonts)
            {
                if (f.FriendlyName == friendlyName)
                {
                    return f;
                }
            }

            return null;
        }

        public static void LoadFont(string name, int size, string path = null, string friendlyName = null, int index = -1)
        {
            try
            {
                Font temp_font = Font.Load(name, size, path, friendlyName, index);
                Fonts.Add(temp_font);
            }
            catch (Exception) // NC Exception
            {
                return;
            }
        }

        public static void UnloadFont(Font nFont)
        {
            try
            {
                nFont.Unload();
                Fonts.Remove(nFont);
            }
            catch (Exception) // NC Exception
            {
                return;
            }
        }

        public static void UnloadFont(string friendlyName)
        {
            Font fontToUnload = GetFont(friendlyName);

            if (fontToUnload == null) new NCException($"Attempted to unload invalid font FriendlyName {friendlyName}!", 71, "nonexistent friendlyName passed to TextManager::UnloadFont(string)!", NCExceptionSeverity.FatalError); // possibly not fatal?

            fontToUnload.Unload();
            Fonts.Remove(fontToUnload);
        }


        public static Vector2 GetTextSize(Font font, string text)
        {
            if (font == null  
                || !Fonts.Contains(font)) new NCException($"Please load font (Name={font.Name}, Size={font.Size}) before loading it!", 81, "TextManager::GetTextSize - Font parameter null or font not in font list!", NCExceptionSeverity.FatalError);

            int fontSizeX,
                fontSizeY;

            if (TTF_SizeUTF8(font.Handle, text, out fontSizeX, out fontSizeY) < 0) new NCException($"An error occurred sizing text. {SDL_GetError()}", 80, "TTF_SizeUTF8 call from TextManager::GetTextSize failed", NCExceptionSeverity.FatalError);

            return new Vector2(fontSizeX, fontSizeY);
        }

        public static Vector2 GetLargestTextSize(Font font, string text)
        {
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

        public static void DrawText(Window cWindow, string text, string font, Vector2 position, Color foreground, Color background = default(Color), TTF_FontStyle style = TTF_FontStyle.Normal, 
            int resizeFont = -1, int outlinePixels = -1, int lineLength = -1, FontSmoothingType smoothing = FontSmoothingType.Default)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cWindow.Settings.Camera;

            if (currentCamera != null)
            {
                position.X -= currentCamera.Position.X;
                position.Y -= currentCamera.Position.Y;
            }

            // Localise the string using Localisation Manager.
            text = LocalisationManager.ProcessString(text);

            // Get the font and throw an error if it's invalid
            Font curFont = GetFont(font);

            if (curFont == null) new NCException($"Attempted to acquire invalid font with name {font}", 39, "TextManager.DrawText", NCExceptionSeverity.FatalError);

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
            if (outlinePixels > 0 || outlinePixels <= curFont.Size) TTF_SetFontOutline(curFont.Handle, outlinePixels);

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