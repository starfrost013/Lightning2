using NuCore.SDL2;
using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning2
{
    /// <summary>
    /// PrimitiveRenderer
    /// 
    /// February 7, 2022
    /// 
    /// Defines a static primitive renderer class that takes a Window and renders a primitive to it. 
    /// </summary>
    public static class PrimitiveRenderer
    {
        /// <summary>
        /// Circle Precision of the primitive renderer
        /// 
        /// Lower values are more precise but slower, higher values are less precise but faster
        /// </summary>
        private const double PRIMITIVE_RENDERER_CIRCLE_PRECISION = 3; 

        public static void DrawLine(Window Win, Vector2 Start, Vector2 End, int Thickness, Color4 Colour, bool AntiAliased)
        {
            // lineRGBA(); just calls SDL.SDL_RenderDrawLine
            // thickLine does other stuff. 
            // therefore call lineRGBA if thickness = 1

            // 2022-02-25: Changed SDL2_gfx in C++
            // to support 16-bit thickness instead of 8-bit

            // nobody will ever need more than 65,536 pixels wide
            // (he says, regretting this in the future). If we do we can just change to sint32 in c++.
            
            if (Thickness < 1
                || Thickness > short.MaxValue) throw new NCException($"Cannot draw a line with a thickness between 1 and {short.MaxValue}! (thickness = {Thickness})", 18, "PrimitiveRenderer.DrawLine Thickness paraemeter <1!", NCExceptionSeverity.FatalError);

            if (Thickness == 1)
            {
                if (AntiAliased)
                {
                    SDL_gfx.aalineRGBA(Win.Settings.RendererHandle, Start.X, Start.Y, End.X, End.Y, Colour.R, Colour.G, Colour.B, Colour.A);
                }
                else
                {
                    SDL_gfx.lineRGBA(Win.Settings.RendererHandle, Start.X, Start.Y, End.X, End.Y, Colour.R, Colour.G, Colour.B, Colour.A);
                }
            }
            else // sdl2_gfx limitaitons, can't be bothered to rebuild SDL2-gfx 
            {
                SDL_gfx.thickLineRGBA(Win.Settings.RendererHandle, Start.X, Start.Y, End.X, End.Y, (short)Thickness, Colour.R, Colour.G, Colour.B, Colour.A);
                SDL.SDL_SetRenderDrawColor(Win.Settings.RendererHandle, 0, 0, 0, 255);
            }
            
        }

        public static void DrawRectangle(Window Win, Vector2 Position, Vector2 Size, Color4 Colour, bool Filled)
        {
            if (Filled)
            {
                SDL_gfx.boxRGBA(Win.Settings.RendererHandle, Position.X, Position.Y, Position.X + Size.X, Position.Y + Size.Y, Colour.R, Colour.G, Colour.B, Colour.A);
            }
            else
            {
                SDL_gfx.rectangleRGBA(Win.Settings.RendererHandle, Position.X, Position.Y, Position.X + Size.X, Position.Y + Size.Y, Colour.R, Colour.G, Colour.B, Colour.A);
            }

            SDL.SDL_SetRenderDrawColor(Win.Settings.RendererHandle, 0, 0, 0, 255);
        }

        public static void DrawRoundedRectangle(Window Win, Vector2 Position, Vector2 Size, Color4 Colour, int CornerRadius, bool Filled)
        {
            if (Filled)
            {
                SDL_gfx.roundedBoxRGBA(Win.Settings.RendererHandle, Position.X, Position.Y, Position.X + Size.X, Position.Y + Size.Y, CornerRadius, Colour.R, Colour.G, Colour.B, Colour.A);
            }
            else
            {
                SDL_gfx.rectangleRGBA(Win.Settings.RendererHandle, Position.X, Position.Y, Position.X + Size.X, Position.Y + Size.Y, Colour.R, Colour.G, Colour.B, Colour.A);
            }

            SDL.SDL_SetRenderDrawColor(Win.Settings.RendererHandle, 0, 0, 0, 255);
        }

        public static void DrawTriangle(Window Win, Vector2 Point1, Vector2 Point2, Vector2 Point3, Color4 Colour, bool Filled)
        {
            if (Filled)
            {
                SDL_gfx.filledTrigonRGBA(Win.Settings.RendererHandle, Point1.X, Point1.Y, Point2.X, Point2.Y, Point3.X, Point3.Y, Colour.R, Colour.G, Colour.B, Colour.A);
            }
            else
            {
                SDL_gfx.trigonRGBA(Win.Settings.RendererHandle, Point1.X, Point1.Y, Point2.X, Point2.Y, Point3.X, Point3.Y, Colour.R, Colour.G, Colour.B, Colour.A);
            }

            SDL.SDL_SetRenderDrawColor(Win.Settings.RendererHandle, 0, 0, 0, 255);
        }

        public static void DrawCircle(Window Win, Vector2 Position, Vector2 Size, Color4 Colour, bool Filled, bool Antialiased = false)
        {
            if (Filled)
            {
                DrawCircle_DrawFilledCircle(Win, Position, Size, Colour);
            }
            else
            {
                DrawCircle_DrawUnfilledCircle(Win, Position, Size, Colour, Antialiased);
            }

            SDL.SDL_SetRenderDrawColor(Win.Settings.RendererHandle, 0, 0, 0, 255);
        }

        private static void DrawCircle_DrawUnfilledCircle(Window Win, Vector2 Position, Vector2 Size, Color4 Colour, bool Antialiased = false)
        {
            if (!Antialiased)
            {
                SDL_gfx.ellipseRGBA(Win.Settings.RendererHandle, Position.X, Position.Y, Size.X, Size.Y, Colour.R, Colour.G, Colour.B, Colour.A);
            }
            else
            {
                SDL_gfx.aaellipseRGBA(Win.Settings.RendererHandle, Position.X, Position.Y, Size.X, Size.Y, Colour.R, Colour.G, Colour.B, Colour.A);
            }

            /*
            SDL.SDL_SetRenderDrawColor(Win.Settings.RendererHandle, Colour.R, Colour.G, Colour.B, Colour.A);

            // Test code to see if i can do this properly
            int max = (int)(360 / PRIMITIVE_RENDERER_CIRCLE_PRECISION);

            Vector2 initial_pos = new Vector2(Position.X, Position.Y);


            for (int i = 0; i < max; i ++)
            {
                Vector2 next_pos = new Vector2(Position.X + (int)(Size.X * Math.Sin(i)), Position.Y + (int)(Size.Y * Math.Cos(i)));

                SDL.SDL_RenderDrawLine(Win.Settings.RendererHandle, initial_pos.X, initial_pos.Y, next_pos.X, next_pos.Y);

                initial_pos = next_pos;

            }
            */
        }

        private static void DrawCircle_DrawFilledCircle(Window Win, Vector2 Position, Vector2 Size, Color4 Colour)
        {
            SDL_gfx.filledEllipseRGBA(Win.Settings.RendererHandle, Position.X, Position.Y, Size.X, Size.Y, Colour.R, Colour.G, Colour.B, Colour.A);
        }
        
    }
}
