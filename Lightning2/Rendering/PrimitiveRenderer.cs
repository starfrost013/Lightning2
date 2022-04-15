using NuCore.SDL2;
using NuCore.Utilities;
using System.Drawing;
using System.Numerics;

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

        public static void DrawPixel(Window Win, Vector2 Position, Color Colour, bool SnapToScreen = true)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera cur_cam = Win.Settings.Camera;

            if (cur_cam != null
                && SnapToScreen)
            {
                Position.X -= cur_cam.Position.X;
                Position.Y -= cur_cam.Position.Y;
            }

            SDL_gfx.pixelRGBA(Win.Settings.RendererHandle, (int)Position.X, (int)Position.Y, Colour.R, Colour.G, Colour.B, Colour.A);
        }

        public static void DrawLine(Window Win, Vector2 Start, Vector2 End, short Thickness, Color Colour, bool AntiAliased, bool SnapToScreen = true)
        {
            // lineRGBA(); just calls SDL.SDL_RenderDrawLine
            // thickLine does other stuff. 
            // therefore call lineRGBA if thickness = 1

            // 2022-02-25: Changed SDL2_gfx in C++
            // to support 16-bit thickness instead of 8-bit

            // nobody will ever need a line more than 32,767 pixels wide
            // (he says, regretting this in the future). If we do we can just change to sint32 in c++.

            if (Thickness < 1) throw new NCException($"Cannot draw a line with a Thickness property below 1 pixel! (thickness = {Thickness})", 18, "PrimitiveRenderer.DrawLine!", NCExceptionSeverity.FatalError);

            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera cur_cam = Win.Settings.Camera;

            if (cur_cam != null
                && SnapToScreen)
            {
                Start.X -= cur_cam.Position.X;
                Start.Y -= cur_cam.Position.Y;
                End.X -= cur_cam.Position.X;
                End.Y -= cur_cam.Position.Y;
            }

            if (Thickness == 1)
            {
                if (AntiAliased)
                {
                    SDL_gfx.aalineRGBA(Win.Settings.RendererHandle, (int)Start.X, (int)Start.Y, (int)End.X, (int)End.Y, Colour.R, Colour.G, Colour.B, Colour.A);
                }
                else
                {
                    SDL_gfx.lineRGBA(Win.Settings.RendererHandle, (int)Start.X, (int)Start.Y, (int)End.X, (int)End.Y, Colour.R, Colour.G, Colour.B, Colour.A);
                }
            }
            else // sdl2_gfx limitaitons, can't be bothered to rebuild SDL2-gfx 
            {
                SDL_gfx.thickLineRGBA(Win.Settings.RendererHandle, (int)Start.X, (int)Start.Y, (int)End.X, (int)End.Y, Thickness, Colour.R, Colour.G, Colour.B, Colour.A);
            }
        }

        public static void DrawRectangle(Window Win, Vector2 Position, Vector2 Size, Color Colour, bool Filled, bool SnapToScreen = true)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera cur_cam = Win.Settings.Camera;

            if (cur_cam != null
                && SnapToScreen)
            {
                Position.X -= cur_cam.Position.X;
                Position.Y -= cur_cam.Position.Y;
            }

            if (Filled)
            {
                SDL_gfx.boxRGBA(Win.Settings.RendererHandle, (int)Position.X, (int)Position.Y,
                    (int)Position.X + (int)Size.X, (int)Position.Y + (int)Size.Y, Colour.R, Colour.G, Colour.B, Colour.A);
            }
            else
            {
                SDL_gfx.rectangleRGBA(Win.Settings.RendererHandle, (int)Position.X, (int)Position.Y,
                    (int)Position.X + (int)Size.X, (int)Position.Y + (int)Size.Y, Colour.R, Colour.G, Colour.B, Colour.A);
            }

            //SDL.SDL_SetRenderDrawColor(Win.Settings.RendererHandle, Win.Settings.Background.R, Win.Settings.Background.G, Win.Settings.Background.B, Win.Settings.Background.A);
        }

        public static void DrawRoundedRectangle(Window Win, Vector2 Position, Vector2 Size, Color Colour, int CornerRadius, bool Filled, bool SnapToScreen = true)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera cur_cam = Win.Settings.Camera;

            if (cur_cam != null
                && SnapToScreen)
            {
                Position.X -= cur_cam.Position.X;
                Position.Y -= cur_cam.Position.Y;
            }

            if (Filled)
            {
                SDL_gfx.roundedBoxRGBA(Win.Settings.RendererHandle, (int)Position.X, (int)Position.Y, (int)Position.X + (int)Size.X,
                    (int)Position.Y + (int)Size.Y, CornerRadius, Colour.R, Colour.G, Colour.B, Colour.A);
            }
            else
            {
                SDL_gfx.rectangleRGBA(Win.Settings.RendererHandle, (int)Position.X, (int)Position.Y,
                    (int)Position.X + (int)Size.X, (int)Position.Y + (int)Size.Y, Colour.R, Colour.G, Colour.B, Colour.A);
            }

            //SDL.SDL_SetRenderDrawColor(Win.Settings.RendererHandle, Win.Settings.Background.R, Win.Settings.Background.G, Win.Settings.Background.B, Win.Settings.Background.A);
        }

        public static void DrawTriangle(Window Win, Vector2 Point1, Vector2 Point2, Vector2 Point3, Color Colour, bool Filled, bool SnapToScreen = true)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera cur_cam = Win.Settings.Camera;

            if (cur_cam != null
                && SnapToScreen)
            {
                Point1.X -= cur_cam.Position.X;
                Point2.X -= cur_cam.Position.X;
                Point3.X -= cur_cam.Position.X;

                Point1.Y -= cur_cam.Position.Y;
                Point2.Y -= cur_cam.Position.Y;
                Point3.Y -= cur_cam.Position.Y;
            }

            if (Filled)
            {
                SDL_gfx.filledTrigonRGBA(Win.Settings.RendererHandle, (int)Point1.X, (int)Point1.Y, (int)Point2.X, (int)Point2.Y, (int)Point3.X, (int)Point3.Y, Colour.R, Colour.G, Colour.B, Colour.A);
            }
            else
            {
                SDL_gfx.trigonRGBA(Win.Settings.RendererHandle, (int)Point1.X, (int)Point1.Y, (int)Point2.X, (int)Point2.Y, (int)Point3.X, (int)Point3.Y, Colour.R, Colour.G, Colour.B, Colour.A);
            }
        }

        public static void DrawCircle(Window Win, Vector2 Position, Vector2 Size, Color Colour, bool Filled, bool Antialiased = false, bool SnapToScreen = true)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera cur_cam = Win.Settings.Camera;

            if (cur_cam != null
                && SnapToScreen)
            {
                Position.X -= cur_cam.Position.X;
                Position.Y -= cur_cam.Position.Y;
            }

            if (Filled)
            {
                DrawCircle_DrawFilledCircle(Win, Position, Size, Colour);
            }
            else
            {
                DrawCircle_DrawUnfilledCircle(Win, Position, Size, Colour, Antialiased);
            }
        }

        private static void DrawCircle_DrawUnfilledCircle(Window Win, Vector2 Position, Vector2 Size, Color Colour, bool Antialiased = false)
        {
            if (!Antialiased)
            {
                SDL_gfx.ellipseRGBA(Win.Settings.RendererHandle, (int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y, Colour.R, Colour.G, Colour.B, Colour.A);
            }
            else
            {
                SDL_gfx.aaellipseRGBA(Win.Settings.RendererHandle, (int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y, Colour.R, Colour.G, Colour.B, Colour.A);
            }

        }

        private static void DrawCircle_DrawFilledCircle(Window Win, Vector2 Position, Vector2 Size, Color Colour)
        {
            SDL_gfx.filledEllipseRGBA(Win.Settings.RendererHandle, (int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y, Colour.R, Colour.G, Colour.B, Colour.A);
        }

        /// <summary>
        /// Draws simple text using SDL2_gfx.
        /// </summary>
        /// <param name="Win">The window to draw the text to.</param>
        /// <param name="Text">The text to draw.</param>
        /// <param name="Position">The position to draw the text to. </param>
        /// <param name="Colour">The colour to draw the text as.</param>
        /// <param name="Localise">If true, the text will be localised with <see cref="LocalisationManager"/> before being drawn.</param>
        public static void DrawText(Window Win, string Text, Vector2 Position, Color Colour, bool SnapToScreen = true, bool Localise = true)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera cur_cam = Win.Settings.Camera;

            if (cur_cam != null
                && !SnapToScreen)
            {
                Position.X -= cur_cam.Position.X;
                Position.Y -= cur_cam.Position.Y;
            }

            if (Localise) Text = LocalisationManager.ProcessString(Text);

            // todo: in c++: recompile sdl2_gfx to use sint32, not sint16, and modify pinvoke accordingly
            SDL_gfx.stringRGBA(Win.Settings.RendererHandle, (short)Position.X, (short)Position.Y, Text, Colour.R, Colour.G, Colour.B, Colour.A);
        }
    }
}