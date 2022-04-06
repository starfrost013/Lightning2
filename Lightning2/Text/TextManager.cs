using NuCore.SDL2;
using NuCore.Utilities; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Lightning2
{
    public static class TextManager
    {
        public static List<Font> Fonts { get; set; }

        static TextManager()
        {
            Fonts = new List<Font>();
        }

        public static Font GetFont(string FriendlyName)
        {
            foreach (Font F in Fonts)
            {
                if (F.FriendlyName == FriendlyName)
                {
                    return F;
                }
            }

            return null;    
        }

        public static void LoadFont(string Name, int Size, string Path = null, string FriendlyName = null, int Index = -1)
        {
            try
            {
                Font temp_font = Font.Load(Name, Size, Path, FriendlyName, Index);
                Fonts.Add(temp_font);
            }
            catch (Exception) // NC Exception
            {
                return; 
            }
        }

        public static void UnloadFont(Font NFont)
        {
            try
            {
                NCLogging.Log($"Unloading font {NFont.Name}, size {NFont.Size}...");
                NFont.Unload();
                Fonts.Remove(NFont);
            }
            catch (Exception) // NC Exception
            {
                return;
            }
        }

        public static void DrawTextTTF(Window Win, string Text, string Font, Vector2 Position, Color4 Foreground, Color4 Background = null, SDL_ttf.TTF_FontStyle Style = SDL_ttf.TTF_FontStyle.Normal, int ResizeFont = -1, int OutlinePixels = -1, int LineLength = -1, bool SnapToScreen = true, FontSmoothingType Smoothing = FontSmoothingType.Default)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera cur_cam = Win.Settings.Camera;

            if (cur_cam != null && SnapToScreen)
            {
                Position.X -= cur_cam.Position.X;
                Position.Y -= cur_cam.Position.Y;
            }

            // Localise the string using Localisation Manager.
            Text = LocalisationManager.ProcessString(Text);

            // Get the font and throw an error if it's invalid
            Font temp_font = GetFont(Font);

            if (temp_font == null) throw new NCException($"Attempted to acquire invalid font with name {Font}", 39, "TextManager.DrawText", NCExceptionSeverity.FatalError);

            // Set the foreground colour
            SDL.SDL_Color font_colour = new SDL.SDL_Color(Foreground.R, Foreground.G, Foreground.B, Foreground.A);

            // Resize font if specified
            if (ResizeFont > 0)
            {
                SDL_ttf.TTF_SetFontSize(temp_font.Handle, ResizeFont);
                temp_font.Size = ResizeFont; 
            }

            // Draw a background for the user if they have not specified shaded and they want a background
            // To do this we need to size the text.
            int font_size_x = -1;
            int font_size_y = -1;

            // split the text into lines
            // add the length of each line to the text length
            string[] text_lines = Text.Split("\n");

            // default to entirely transparent background (if the user has specified shasded for some reason, we still need a BG colour...)
            SDL.SDL_Color bg_colour = new SDL.SDL_Color(0, 0, 0, 0);

            if (Background != null) // draw the background (which requires sizing the entire text)
            {
                // Set the background colour
                bg_colour = new SDL.SDL_Color(Background.R, Background.G, Background.B, Background.A);

                SDL_ttf.TTF_SizeUTF8(temp_font.Handle, Text, out font_size_x, out font_size_y);

                if (font_size_x == -1 || font_size_y == -1) throw new NCException($"Error sizing font: {SDL_ttf.TTF_GetError()}", 40, "TextManager.DrawText", NCExceptionSeverity.FatalError);

                // get the number of lines
                int no_of_lines = text_lines.Length;

                int total_size_x = font_size_x;
                int total_size_y = font_size_y;

                for (int i = 0; i < no_of_lines - 1; i++) // -1 to prevent double-counting the first line
                {
                    if (LineLength < 0)
                    {
                        total_size_y += font_size_y;
                    }
                    else
                    {
                        total_size_y += LineLength;
                    }
                }

                // rough approximation of x size
                if (no_of_lines > 1) total_size_x /= (no_of_lines - 1);

                // camera-aware is false for this as we have already "pushed" the position, so we don't need to do it again.
                PrimitiveRenderer.DrawRectangle(Win, Position, new Vector2(total_size_x, total_size_y), (Color4)bg_colour, true, false);
            }

            // Set the font outline, size and style
            // Too much larger than the font size in pt causes C++ exceptions in SDL2 (probably larger than the surface it's being drawn to...) so we just use that as a limit
            if (OutlinePixels > 0 || OutlinePixels <= temp_font.Size) SDL_ttf.TTF_SetFontOutline(temp_font.Handle, OutlinePixels);

            SDL_ttf.TTF_SetFontStyle(temp_font.Handle, Style);
            IntPtr text_ptr = IntPtr.Zero;
            IntPtr text_texture_ptr = IntPtr.Zero;

            SDL.SDL_Rect font_src_rect = new SDL.SDL_Rect(0, 0, font_size_x, font_size_y);
            SDL.SDL_Rect font_dst_rect = new SDL.SDL_Rect((int)Position.X, (int)Position.Y, font_size_x, font_size_y);

            foreach (string line in text_lines)
            {
                int line_size_x = -1;
                int line_size_y = -1;

                SDL_ttf.TTF_SizeUTF8(temp_font.Handle, line, out line_size_x, out line_size_y);

                font_src_rect.w = line_size_x;
                font_src_rect.h = line_size_y;

                font_dst_rect.w = line_size_x;
                font_dst_rect.h = line_size_y;

                switch (Smoothing)
                {
                    case FontSmoothingType.Default: // Antialiased
                        text_ptr = SDL_ttf.TTF_RenderUTF8_Blended(temp_font.Handle, line, font_colour);
                        text_texture_ptr = SDL.SDL_CreateTextureFromSurface(Win.Settings.RendererHandle, text_ptr);

                        SDL.SDL_RenderCopy(Win.Settings.RendererHandle, text_texture_ptr, ref font_src_rect, ref font_dst_rect);

                        SDL.SDL_FreeSurface(text_ptr);
                        SDL.SDL_DestroyTexture(text_texture_ptr);

                        // increment by the line length
                        if (LineLength < 0)
                        {
                            font_dst_rect.y += font_size_y;
                        }
                        else
                        {
                            font_dst_rect.y += LineLength;
                        }

                        continue; 
                    case FontSmoothingType.Shaded: // Only shaded
                        text_ptr = SDL_ttf.TTF_RenderUTF8_Shaded(temp_font.Handle, line, font_colour, bg_colour);
                        text_texture_ptr = SDL.SDL_CreateTextureFromSurface(Win.Settings.RendererHandle, text_ptr);

                        SDL.SDL_RenderCopy(Win.Settings.RendererHandle, text_texture_ptr, ref font_src_rect, ref font_dst_rect);

                        SDL.SDL_FreeSurface(text_ptr);
                        SDL.SDL_DestroyTexture(text_texture_ptr);

                        // increment by the line length
                        if (LineLength < 0)
                        {
                            font_dst_rect.y += font_size_y;
                        }
                        else
                        {
                            font_dst_rect.y += LineLength;
                        }

                        // increment by the line length
                        if (LineLength < 0)
                        {
                            font_dst_rect.y += font_size_y;
                        }
                        else
                        {
                            font_dst_rect.y += LineLength;
                        }

                        continue;
                    case FontSmoothingType.Solid: // No processing done
                        text_ptr = SDL_ttf.TTF_RenderUTF8_Solid(temp_font.Handle, line, font_colour);
                        text_texture_ptr = SDL.SDL_CreateTextureFromSurface(Win.Settings.RendererHandle, text_ptr);

                        SDL.SDL_RenderCopy(Win.Settings.RendererHandle, text_texture_ptr, ref font_src_rect, ref font_dst_rect);

                        SDL.SDL_FreeSurface(text_ptr);
                        SDL.SDL_DestroyTexture(text_texture_ptr);


                        continue;
                }

                continue;

            }
        }
    }
}