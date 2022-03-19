using NuCore.SDL2;
using NuCore.Utilities; 
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static void LoadFont(string Name, int Size, string FriendlyName = null, string Path = null, int Index = -1)
        {
            try
            {
                Font temp_font = Font.Load(Name, Size, FriendlyName, Path, Index);
                Fonts.Add(temp_font);
            }
            catch (Exception) // NC Exception
            {
                return; 
            }
        }

        public static void DrawTextTTF(Window Win, string Text, string Font, Vector2 Position, Color4 Foreground, SDL_ttf.TTF_FontStyle Style = SDL_ttf.TTF_FontStyle.Normal, Color4 Background = null, int ResizeFont = -1, int OutlinePixels = -1, uint LineLength = 0, FontSmoothingType Smoothing = FontSmoothingType.Default)
        {
            Text = LocalisationManager.ProcessString(Text);

            Font temp_font = GetFont(Font);

            if (temp_font == null) throw new NCException($"Attempted to acquire invalid font with name {Font}", 39, "TextManager.DrawText", NCExceptionSeverity.FatalError);
            
            SDL.SDL_Color font_colour = new SDL.SDL_Color(Foreground.R, Foreground.G, Foreground.B, Foreground.A);

            // default to entirely transparent background (if the user has specified shasded for some reason, we still need a BG colour...)
            SDL.SDL_Color bg_colour = new SDL.SDL_Color(0, 0, 0, 0); 

            if (Background != null)
            {
                bg_colour = new SDL.SDL_Color(Background.R, Background.G, Background.B, Background.A);
            }

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

            SDL_ttf.TTF_SizeUTF8(temp_font.Handle, Text, out font_size_x, out font_size_y);

            if (font_size_x == -1 || font_size_y == -1) throw new NCException($"Error sizing font: {SDL_ttf.TTF_GetError()}", 40, "TextManager.DrawText", NCExceptionSeverity.FatalError);

            if (Background != null)
            {
                PrimitiveRenderer.DrawRectangle(Win, Position, new Vector2(font_size_x, font_size_y), (Color4)bg_colour, true);
            }

            // Set the font outline size and style

            // Too much larger than the font size in pt causes C++ exceptions in SDL2 (probably larger than the surface it's being drawn to...) so we just use that as a limit
            if (OutlinePixels > 0 || OutlinePixels <= temp_font.Size) SDL_ttf.TTF_SetFontOutline(temp_font.Handle, OutlinePixels);

            SDL_ttf.TTF_SetFontStyle(temp_font.Handle, Style);

            IntPtr text_ptr = IntPtr.Zero;
            IntPtr text_texture_ptr = IntPtr.Zero;

            SDL.SDL_Rect font_src_rect = new SDL.SDL_Rect(0, 0, font_size_x, font_size_y);
            SDL.SDL_Rect font_dst_rect = new SDL.SDL_Rect(Position.X, Position.Y, font_size_x, font_size_y);

            switch (Smoothing)
            {
                case FontSmoothingType.Default: // Antialiased
                    if (LineLength == 0)
                    {
                        text_ptr = SDL_ttf.TTF_RenderUTF8_Blended(temp_font.Handle, Text, font_colour);
                    }
                    else
                    {
                        text_ptr = SDL_ttf.TTF_RenderUTF8_Blended_Wrapped(temp_font.Handle, Text, font_colour, LineLength);
                    }

                    text_texture_ptr = SDL.SDL_CreateTextureFromSurface(Win.Settings.RendererHandle, text_ptr);

                    SDL.SDL_RenderCopy(Win.Settings.RendererHandle, text_texture_ptr, ref font_src_rect, ref font_dst_rect);

                    SDL.SDL_FreeSurface(text_ptr);
                    SDL.SDL_DestroyTexture(text_texture_ptr);

                    return;
                case FontSmoothingType.Shaded: // Only shaded
                    if (LineLength == 0)
                    {
                        text_ptr = SDL_ttf.TTF_RenderUTF8_Shaded(temp_font.Handle, Text, font_colour, bg_colour);
                    }
                    else
                    {
                        text_ptr = SDL_ttf.TTF_RenderUTF8_Shaded_Wrapped(temp_font.Handle, Text, font_colour, bg_colour, LineLength);
                    }

                    text_texture_ptr = SDL.SDL_CreateTextureFromSurface(Win.Settings.RendererHandle, text_ptr);

                    SDL.SDL_RenderCopy(Win.Settings.RendererHandle, text_texture_ptr, ref font_src_rect, ref font_dst_rect);

                    SDL.SDL_FreeSurface(text_ptr);
                    SDL.SDL_DestroyTexture(text_texture_ptr);

                    return;
                case FontSmoothingType.Solid: // No processing done
                    if (LineLength == 0)
                    {
                        text_ptr = SDL_ttf.TTF_RenderUTF8_Solid(temp_font.Handle, Text, font_colour);
                    }
                    else
                    {
                        text_ptr = SDL_ttf.TTF_RenderUTF8_Solid_Wrapped(temp_font.Handle, Text, font_colour, LineLength);
                    }

                    text_texture_ptr = SDL.SDL_CreateTextureFromSurface(Win.Settings.RendererHandle, text_ptr);

                    SDL.SDL_RenderCopy(Win.Settings.RendererHandle, text_texture_ptr, ref font_src_rect, ref font_dst_rect);

                    SDL.SDL_FreeSurface(text_ptr);
                    SDL.SDL_DestroyTexture(text_texture_ptr);

                    return;
            }



        }
    }
}
