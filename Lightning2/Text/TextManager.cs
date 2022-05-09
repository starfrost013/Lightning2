using NuCore.SDL2;
using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace Lightning2
{
    public static class TextManager
    {
        public static List<Font> Fonts { get; set; }

        static TextManager()
        {
            Fonts = new List<Font>();
        }

        public static Font GetFont(string friendlyName)
        {
            foreach (Font F in Fonts)
            {
                if (F.FriendlyName == friendlyName)
                {
                    return F;
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

            if (fontToUnload == null) throw new NCException($"Attempted to unload invalid font FriendlyName {friendlyName}!", 71, "nonexistent friendlyName passed to TextManager::UnloadFont(string)!", NCExceptionSeverity.FatalError); // possibly not fatal?

            fontToUnload.Unload();
        }

        public static void DrawTextTTF(Window cWindow, string text, string font, Vector2 position, Color foreground, Color background = default(Color), SDL_ttf.TTF_FontStyle style = SDL_ttf.TTF_FontStyle.Normal, int resizeFont = -1, int outlinePixels = -1, int lineLength = -1, bool snapToScreen = true, FontSmoothingType smoothing = FontSmoothingType.Default)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera cur_cam = cWindow.Settings.Camera;

            if (cur_cam != null && snapToScreen)
            {
                position.X -= cur_cam.Position.X;
                position.Y -= cur_cam.Position.Y;
            }

            // Localise the string using Localisation Manager.
            text = LocalisationManager.ProcessString(text);

            // Get the font and throw an error if it's invalid
            Font temp_font = GetFont(font);

            if (temp_font == null) throw new NCException($"Attempted to acquire invalid font with name {font}", 39, "TextManager.DrawText", NCExceptionSeverity.FatalError);

            // Set the foreground colour
            SDL.SDL_Color font_colour = new SDL.SDL_Color(foreground.R, foreground.G, foreground.B, foreground.A);

            // Resize font if specified
            if (resizeFont > 0)
            {
                SDL_ttf.TTF_SetFontSize(temp_font.Handle, resizeFont);
                temp_font.Size = resizeFont;
            }

            // Draw a background for the user if they have not specified shaded and they want a background
            // To do this we need to size the text.
            int font_size_x = -1;
            int font_size_y = -1;

            // split the text into lines
            // add the length of each line to the text length
            string[] text_lines = text.Split("\n");

            // default to entirely transparent background (if the user has specified shasded for some reason, we still need a BG colour...)
            SDL.SDL_Color bg_colour = new SDL.SDL_Color(0, 0, 0, 0);

            if (background != default(Color)) // draw the background (which requires sizing the entire text)
            {
                // Set the background colour
                bg_colour = new SDL.SDL_Color(background.R, background.G, background.B, background.A);

                SDL_ttf.TTF_SizeUTF8(temp_font.Handle, text, out font_size_x, out font_size_y);

                if (font_size_x == -1 || font_size_y == -1) throw new NCException($"Error sizing font: {SDL_ttf.TTF_GetError()}", 40, "TextManager.DrawText", NCExceptionSeverity.FatalError);

                // get the number of lines
                int no_of_lines = text_lines.Length;

                int total_size_x = font_size_x;
                int total_size_y = font_size_y;

                for (int i = 0; i < no_of_lines - 1; i++) // -1 to prevent double-counting the first line
                {
                    if (lineLength < 0)
                    {
                        total_size_y += font_size_y;
                    }
                    else
                    {
                        total_size_y += lineLength;
                    }
                }

                // rough approximation of x size
                if (no_of_lines > 1) total_size_x /= (no_of_lines - 1);

                // camera-aware is false for this as we have already "pushed" the position, so we don't need to do it again.
                PrimitiveRenderer.DrawRectangle(cWindow, position, new Vector2(total_size_x, total_size_y), Color.FromArgb(bg_colour.a, bg_colour.r, bg_colour.g, bg_colour.b), true, false);
            }

            // Set the font outline, size and style
            // Too much larger than the font size in pt causes C++ exceptions in SDL2 (probably larger than the surface it's being drawn to...) so we just use that as a limit
            if (outlinePixels > 0 || outlinePixels <= temp_font.Size) SDL_ttf.TTF_SetFontOutline(temp_font.Handle, outlinePixels);

            SDL_ttf.TTF_SetFontStyle(temp_font.Handle, style);
            IntPtr text_ptr = IntPtr.Zero;
            IntPtr text_texture_ptr = IntPtr.Zero;

            SDL.SDL_Rect font_src_rect = new SDL.SDL_Rect(0, 0, font_size_x, font_size_y);
            SDL.SDL_Rect font_dst_rect = new SDL.SDL_Rect((int)position.X, (int)position.Y, font_size_x, font_size_y);

            foreach (string line in text_lines)
            {
                int line_size_x = -1;
                int line_size_y = -1;

                SDL_ttf.TTF_SizeUTF8(temp_font.Handle, line, out line_size_x, out line_size_y);

                font_src_rect.w = line_size_x;
                font_src_rect.h = line_size_y;

                font_dst_rect.w = line_size_x;
                font_dst_rect.h = line_size_y;

                switch (smoothing)
                {
                    case FontSmoothingType.Default: // Antialiased
                        text_ptr = SDL_ttf.TTF_RenderUTF8_Blended(temp_font.Handle, line, font_colour);
                        text_texture_ptr = SDL.SDL_CreateTextureFromSurface(cWindow.Settings.RendererHandle, text_ptr);

                        SDL.SDL_RenderCopy(cWindow.Settings.RendererHandle, text_texture_ptr, ref font_src_rect, ref font_dst_rect);

                        SDL.SDL_FreeSurface(text_ptr);
                        SDL.SDL_DestroyTexture(text_texture_ptr);

                        // increment by the line length
                        if (lineLength < 0)
                        {
                            font_dst_rect.y += font_size_y;
                        }
                        else
                        {
                            font_dst_rect.y += lineLength;
                        }

                        continue;
                    case FontSmoothingType.Shaded: // Only shaded
                        text_ptr = SDL_ttf.TTF_RenderUTF8_Shaded(temp_font.Handle, line, font_colour, bg_colour);
                        text_texture_ptr = SDL.SDL_CreateTextureFromSurface(cWindow.Settings.RendererHandle, text_ptr);

                        SDL.SDL_RenderCopy(cWindow.Settings.RendererHandle, text_texture_ptr, ref font_src_rect, ref font_dst_rect);

                        SDL.SDL_FreeSurface(text_ptr);
                        SDL.SDL_DestroyTexture(text_texture_ptr);

                        // increment by the line length
                        if (lineLength < 0)
                        {
                            font_dst_rect.y += font_size_y;
                        }
                        else
                        {
                            font_dst_rect.y += lineLength;
                        }

                        continue;
                    case FontSmoothingType.Solid: // No processing done
                        text_ptr = SDL_ttf.TTF_RenderUTF8_Solid(temp_font.Handle, line, font_colour);
                        text_texture_ptr = SDL.SDL_CreateTextureFromSurface(cWindow.Settings.RendererHandle, text_ptr);

                        SDL.SDL_RenderCopy(cWindow.Settings.RendererHandle, text_texture_ptr, ref font_src_rect, ref font_dst_rect);

                        SDL.SDL_FreeSurface(text_ptr);
                        SDL.SDL_DestroyTexture(text_texture_ptr);

                        // increment by the line length
                        if (lineLength < 0)
                        {
                            font_dst_rect.y += font_size_y;
                        }
                        else
                        {
                            font_dst_rect.y += lineLength;
                        }

                        continue;
                }

                continue;

            }
        }
    }
}