using NuCore.SDL2;
using NuCore.Utilities;
using System.Drawing;
using System.Numerics;

namespace LightningGL
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
        public static void DrawPixel(Window cWindow, Vector2 position, Color colour, bool snapToScreen = true)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera cur_cam = cWindow.Settings.Camera;

            if (cur_cam != null
                && snapToScreen)
            {
                position.X -= cur_cam.Position.X;
                position.Y -= cur_cam.Position.Y;
            }

            SDL_gfx.pixelRGBA(cWindow.Settings.RendererHandle, (int)position.X, (int)position.Y, colour.R, colour.G, colour.B, colour.A);
        }

        public static void DrawLine(Window cWindow, Vector2 Start, Vector2 End, short thickness, Color colour, bool antiAliased, bool snapToScreen = true)
        {
            // lineRGBA(); just calls SDL.SDL_RenderDrawLine
            // thickLine does other stuff. 
            // therefore call lineRGBA if thickness = 1

            // 2022-02-25: Changed SDL2_gfx in C++
            // to support 16-bit thickness instead of 8-bit

            // nobody will ever need a line more than 32,767 pixels wide
            // (he says, regretting this in the future). If we do we can just change to sint32 in c++.

            if (thickness < 1) throw new NCException($"Cannot draw a line with a thickness property below 1 pixel! (thickness = {thickness})", 18, "PrimitiveRenderer.DrawLine!", NCExceptionSeverity.FatalError);

            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera cur_cam = cWindow.Settings.Camera;

            if (cur_cam != null
                && snapToScreen)
            {
                Start.X -= cur_cam.Position.X;
                Start.Y -= cur_cam.Position.Y;
                End.X -= cur_cam.Position.X;
                End.Y -= cur_cam.Position.Y;
            }

            if (thickness == 1)
            {
                if (antiAliased)
                {
                    SDL_gfx.aalineRGBA(cWindow.Settings.RendererHandle, (int)Start.X, (int)Start.Y, (int)End.X, (int)End.Y, colour.R, colour.G, colour.B, colour.A);
                }
                else
                {
                    SDL_gfx.lineRGBA(cWindow.Settings.RendererHandle, (int)Start.X, (int)Start.Y, (int)End.X, (int)End.Y, colour.R, colour.G, colour.B, colour.A);
                }
            }
            else // sdl2_gfx limitaitons, can't be bothered to rebuild SDL2-gfx 
            {
                SDL_gfx.thickLineRGBA(cWindow.Settings.RendererHandle, (int)Start.X, (int)Start.Y, (int)End.X, (int)End.Y, thickness, colour.R, colour.G, colour.B, colour.A);
            }
        }

        public static void DrawRectangle(Window cWindow, Vector2 position, Vector2 Size, Color colour, bool Filled = false, bool snapToScreen = true, Color borderColor = default(Color), Vector2 borderSize = default(Vector2))
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera cur_cam = cWindow.Settings.Camera;

            if (cur_cam != null
                && snapToScreen)
            {
                position.X -= cur_cam.Position.X;
                position.Y -= cur_cam.Position.Y;
            }

            if (Filled)
            {
                SDL_gfx.boxRGBA(cWindow.Settings.RendererHandle, (int)position.X, (int)position.Y,
                    (int)position.X + (int)Size.X, (int)position.Y + (int)Size.Y, colour.R, colour.G, colour.B, colour.A);
            }
            else
            {
                SDL_gfx.rectangleRGBA(cWindow.Settings.RendererHandle, (int)position.X, (int)position.Y,
                    (int)position.X + (int)Size.X, (int)position.Y + (int)Size.Y, colour.R, colour.G, colour.B, colour.A);
            }

            if (borderColor != default(Color))
            {
                SDL_gfx.rectangleRGBA(cWindow.Settings.RendererHandle, (int)position.X - (int)borderSize.X, (int)position.Y - (int)borderSize.Y,
                    (int)position.X + (int)Size.X + ((int)borderSize.X * 2), (int)position.Y + (int)Size.Y + ((int)borderSize.Y * 2), colour.R, colour.G, colour.B, colour.A);
            }

        }

        public static void DrawRoundedRectangle(Window cWindow, Vector2 position, Vector2 Size, Color colour, int CornerRadius, bool Filled, bool snapToScreen = true)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera cur_cam = cWindow.Settings.Camera;

            if (cur_cam != null
                && snapToScreen)
            {
                position.X -= cur_cam.Position.X;
                position.Y -= cur_cam.Position.Y;
            }

            if (Filled)
            {
                SDL_gfx.roundedBoxRGBA(cWindow.Settings.RendererHandle, (int)position.X, (int)position.Y, (int)position.X + (int)Size.X,
                    (int)position.Y + (int)Size.Y, CornerRadius, colour.R, colour.G, colour.B, colour.A);
            }
            else
            {
                SDL_gfx.rectangleRGBA(cWindow.Settings.RendererHandle, (int)position.X, (int)position.Y,
                    (int)position.X + (int)Size.X, (int)position.Y + (int)Size.Y, colour.R, colour.G, colour.B, colour.A);
            }

        }

        /// <summary>
        /// Draws a triangle using SDL2_gfx
        /// </summary>
        /// <param name="cWindow">The Window to draw the triangle to.</param>
        /// <param name="point1">The first point of the triangle.</param>
        /// <param name="point2">The second point of the triangle.</param>
        /// <param name="point3">The third point of the triangle.</param>
        /// <param name="colour">The colour of the triangle - see <see cref="Color"/></param>
        /// <param name="filled">Determines if the triangle will be filled.</param>
        /// <param name="snapToScreen">Determines if the triangle will obey the current camera. If it is set to false, the camera will always have screen- instead of world-relative coordinates.</param>
        public static void DrawTriangle(Window cWindow, Vector2 point1, Vector2 point2, Vector2 point3, Color colour, bool filled, bool snapToScreen = true)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera cur_cam = cWindow.Settings.Camera;

            if (cur_cam != null
                && snapToScreen)
            {
                point1.X -= cur_cam.Position.X;
                point2.X -= cur_cam.Position.X;
                point3.X -= cur_cam.Position.X;

                point1.Y -= cur_cam.Position.Y;
                point2.Y -= cur_cam.Position.Y;
                point3.Y -= cur_cam.Position.Y;
            }

            if (filled)
            {
                SDL_gfx.filledTrigonRGBA(cWindow.Settings.RendererHandle, (int)point1.X, (int)point1.Y, (int)point2.X, (int)point2.Y, (int)point3.X, (int)point3.Y, colour.R, colour.G, colour.B, colour.A);
            }
            else
            {
                SDL_gfx.trigonRGBA(cWindow.Settings.RendererHandle, (int)point1.X, (int)point1.Y, (int)point2.X, (int)point2.Y, (int)point3.X, (int)point3.Y, colour.R, colour.G, colour.B, colour.A);
            }
        }

        public static void DrawCircle(Window cWindow, Vector2 position, Vector2 Size, Color colour, bool Filled, bool Antialiased = false, bool snapToScreen = true)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera cur_cam = cWindow.Settings.Camera;

            if (cur_cam != null
                && snapToScreen)
            {
                position.X -= cur_cam.Position.X;
                position.Y -= cur_cam.Position.Y;
            }

            if (Filled)
            {
                DrawCircle_DrawFilledCircle(cWindow, position, Size, colour);
            }
            else
            {
                DrawCircle_DrawUnfilledCircle(cWindow, position, Size, colour, Antialiased);
            }
        }

        private static void DrawCircle_DrawUnfilledCircle(Window cWindow, Vector2 position, Vector2 Size, Color colour, bool Antialiased = false)
        {
            if (!Antialiased)
            {
                SDL_gfx.ellipseRGBA(cWindow.Settings.RendererHandle, (int)position.X, (int)position.Y, (int)Size.X, (int)Size.Y, colour.R, colour.G, colour.B, colour.A);
            }
            else
            {
                SDL_gfx.aaellipseRGBA(cWindow.Settings.RendererHandle, (int)position.X, (int)position.Y, (int)Size.X, (int)Size.Y, colour.R, colour.G, colour.B, colour.A);
            }

        }

        private static void DrawCircle_DrawFilledCircle(Window cWindow, Vector2 position, Vector2 Size, Color colour)
        {
            SDL_gfx.filledEllipseRGBA(cWindow.Settings.RendererHandle, (int)position.X, (int)position.Y, (int)Size.X, (int)Size.Y, colour.R, colour.G, colour.B, colour.A);
        }

        /// <summary>
        /// Draws simple text using SDL2_gfx.
        /// </summary>
        /// <param name="cWindow">The Window to draw the text to.</param>
        /// <param name="Text">The text to draw.</param>
        /// <param name="position">The position to draw the text to. </param>
        /// <param name="colour">The colour to draw the text as.</param>
        /// <param name="Localise">If true, the text will be localised with <see cref="LocalisationManager"/> before being drawn.</param>
        public static void DrawText(Window cWindow, string Text, Vector2 position, Color colour, bool snapToScreen = true, bool Localise = true)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera cur_cam = cWindow.Settings.Camera;

            if (cur_cam != null
                && !snapToScreen)
            {
                position.X -= cur_cam.Position.X;
                position.Y -= cur_cam.Position.Y;
            }

            if (Localise) Text = LocalisationManager.ProcessString(Text);

            // todo: in c++: recompile sdl2_gfx to use sint32, not sint16, and modify pinvoke accordingly
            SDL_gfx.stringRGBA(cWindow.Settings.RendererHandle, (short)position.X, (short)position.Y, Text, colour.R, colour.G, colour.B, colour.A);
        }
    }
}