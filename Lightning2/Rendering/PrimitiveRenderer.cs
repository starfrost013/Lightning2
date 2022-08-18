using NuCore.SDL2;
using NuCore.Utilities;
using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Draws a pixel on the screen.
        /// </summary>
        /// <param name="cWindow">The window to draw the pixel to.</param>
        /// <param name="position">The position of the pixel to draw.</param>
        /// <param name="colour">The <see cref="Color"/> of the pixel to draw.</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public static void DrawPixel(Window cWindow, Vector2 position, Color colour, bool snapToScreen = false)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cWindow.Settings.Camera;

            if (currentCamera != null 
                && !snapToScreen)
            {
                position.X -= currentCamera.Position.X;
                position.Y -= currentCamera.Position.Y;
            }

            SDL_gfx.pixelRGBA(cWindow.Settings.RendererHandle, (int)position.X, (int)position.Y, colour.R, colour.G, colour.B, colour.A);
        }

        /// <summary>
        /// Draws a line to the screen.
        /// </summary>
        /// <param name="cWindow">The window to draw this line to.</param>
        /// <param name="start">The start point of this line.</param>
        /// <param name="end">The endpoint of this line.</param>
        /// <param name="thickness">The thickness of this line.</param>
        /// <param name="colour">The colour of this line.</param>
        /// <param name="antiAliased">Determines if this line will be anti-aliased or not.</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public static void DrawLine(Window cWindow, Vector2 start, Vector2 end, short thickness, Color colour, bool antiAliased = true, bool snapToScreen = false)
        {
            // lineRGBA(); just calls SDL.SDL_RenderDrawLine
            // thickLine does other stuff. 
            // therefore call lineRGBA if thickness = 1

            // 2022-02-25: Changed SDL2_gfx in C++
            // to support 16-bit thickness instead of 8-bit

            // nobody will ever need a line more than 32,767 pixels wide
            // (he says, regretting this in the future). If we do we can just change to sint32 in c++.

            if (thickness < 1) _ = new NCException($"Cannot draw a line with a thickness property below 1 pixel! (thickness = {thickness})", 18, "PrimitiveRenderer::DrawLine called with thickness property < 1", NCExceptionSeverity.FatalError);

            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cWindow.Settings.Camera;

            if (currentCamera != null
                && !snapToScreen)
            {
                start.X -= currentCamera.Position.X;
                start.Y -= currentCamera.Position.Y;
                end.X -= currentCamera.Position.X;
                end.Y -= currentCamera.Position.Y;
            }

            if (thickness == 1)
            {
                if (antiAliased)
                {
                    SDL_gfx.aalineRGBA(cWindow.Settings.RendererHandle, (int)start.X, (int)start.Y, (int)end.X, (int)end.Y, colour.R, colour.G, colour.B, colour.A);
                }
                else
                {
                    SDL_gfx.lineRGBA(cWindow.Settings.RendererHandle, (int)start.X, (int)start.Y, (int)end.X, (int)end.Y, colour.R, colour.G, colour.B, colour.A);
                }
            }
            else 
            {
                SDL_gfx.thickLineRGBA(cWindow.Settings.RendererHandle, (int)start.X, (int)start.Y, (int)end.X, (int)end.Y, thickness, colour.R, colour.G, colour.B, colour.A);
            }
        }

        /// <summary>
        /// Draws a rectangle to the screen.
        /// </summary>
        /// <param name="cWindow">The window to draw this line to.</param>
        /// <param name="position">The position of the rectangle to draw</param>
        /// <param name="size">The size of the rectangle to draw.</param>
        /// <param name="colour">The colour of the rectangle to draw.</param>
        /// <param name="filled">Determines if this rectangle will be filled or not.</param>
        /// <param name="borderColour">The colour of this rectangle's border.</param>
        /// <param name="borderSize">The size of this rectangle's border.</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space (false) or screen-relative space (true).</param>
        public static void DrawRectangle(Window cWindow, Vector2 position, Vector2 size, Color colour, bool filled = false, Color borderColour = default(Color),
            Vector2 borderSize = default(Vector2), bool snapToScreen = false)
        {

            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cWindow.Settings.Camera;

            if (currentCamera != null
                && !snapToScreen)
            {
                position.X -= currentCamera.Position.X;
                position.Y -= currentCamera.Position.Y;
            }

            if (borderColour != default(Color))
            {
                SDL_gfx.rectangleRGBA(cWindow.Settings.RendererHandle, (int)position.X - (int)borderSize.X, (int)position.Y - (int)borderSize.Y,
                    (int)position.X + (int)size.X + ((int)borderSize.X * 2), (int)position.Y + (int)size.Y + ((int)borderSize.Y * 2), colour.R, colour.G, colour.B, colour.A);
            }

            if (filled)
            {
                SDL_gfx.boxRGBA(cWindow.Settings.RendererHandle, (int)position.X, (int)position.Y,
                    (int)position.X + (int)size.X, (int)position.Y + (int)size.Y, colour.R, colour.G, colour.B, colour.A);
            }
            else
            {
                SDL_gfx.rectangleRGBA(cWindow.Settings.RendererHandle, (int)position.X, (int)position.Y,
                    (int)position.X + (int)size.X, (int)position.Y + (int)size.Y, colour.R, colour.G, colour.B, colour.A);
            }
        }

        /// <summary>
        /// Draws a rounded rectangle to the screen.
        /// </summary>
        /// <param name="cWindow">The window to draw this rectangle to.</param>
        /// <param name="position">The position of the rectangle to draw</param>
        /// <param name="size">The size of the rectangle to draw.</param>
        /// <param name="colour">The colour of the rectangle to draw.</param>
        /// <param name="cornerRadius">The radius, in pixels, of this rectangle's corners.</param>
        /// <param name="filled">Determines if this rectangle will be filled or not.</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public static void DrawRoundedRectangle(Window cWindow, Vector2 position, Vector2 size, Color colour, int cornerRadius, bool filled = false, bool snapToScreen = false)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cWindow.Settings.Camera;

            if (currentCamera != null
                && !snapToScreen)
            {
                position.X -= currentCamera.Position.X;
                position.Y -= currentCamera.Position.Y;
            }

            if (filled)
            {
                SDL_gfx.roundedBoxRGBA(cWindow.Settings.RendererHandle, (int)position.X, (int)position.Y, (int)position.X + (int)size.X,
                    (int)position.Y + (int)size.Y, cornerRadius, colour.R, colour.G, colour.B, colour.A);
            }
            else
            {
                SDL_gfx.roundedRectangleRGBA(cWindow.Settings.RendererHandle, (int)position.X, (int)position.Y,
                    (int)position.X + (int)size.X, (int)position.Y + (int)size.Y, cornerRadius, colour.R, colour.G, colour.B, colour.A);
            }

        }

        /// <summary>
        /// Draws a triangle to the screen
        /// </summary>
        /// <param name="cWindow">The Window to draw the triangle to.</param>
        /// <param name="point1">The first point of the triangle.</param>
        /// <param name="point2">The second point of the triangle.</param>
        /// <param name="point3">The third point of the triangle.</param>
        /// <param name="colour">The colour of the triangle - see <see cref="Color"/></param>
        /// <param name="filled">Determines if the triangle will be filled.</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public static void DrawTriangle(Window cWindow, Vector2 point1, Vector2 point2, Vector2 point3, Color colour, bool filled, bool snapToScreen = false)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cWindow.Settings.Camera;

            if (currentCamera != null
                && !snapToScreen)
            {
                point1.X -= currentCamera.Position.X;
                point2.X -= currentCamera.Position.X;
                point3.X -= currentCamera.Position.X;

                point1.Y -= currentCamera.Position.Y;
                point2.Y -= currentCamera.Position.Y;
                point3.Y -= currentCamera.Position.Y;
            }

            if (filled)
            {
                SDL_gfx.filledTrigonRGBA(cWindow.Settings.RendererHandle, (int)point1.X, (int)point1.Y, (int)point2.X, (int)point2.Y, (int)point3.X, 
                    (int)point3.Y, colour.R, colour.G, colour.B, colour.A);
            }
            else
            {
                SDL_gfx.trigonRGBA(cWindow.Settings.RendererHandle, (int)point1.X, (int)point1.Y, (int)point2.X, (int)point2.Y, (int)point3.X, 
                    (int)point3.Y, colour.R, colour.G, colour.B, colour.A);
            }
        }

        /// <summary>
        /// Draws a polygon to the screen.
        /// </summary>
        /// <param name="cWindow">The window to draw the polygon to.</param>
        /// <param name="points">The points of the polygon.</param>
        /// <param name="colour">The colour of the polygon.</param>
        /// <param name="filled">Determines if the polygon will be filled or not.</param>
        /// <param name="antiAliased">Determines if the polygon will be anti-aliased - UNFILLED POLYGONS ONLY</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public static void DrawPolygon(Window cWindow, List<Vector2> points, Color colour, bool filled, bool antiAliased = false, bool snapToScreen = false)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cWindow.Settings.Camera;

            if (currentCamera != null
                && !snapToScreen)
            {
                for (int curPoint = 0; curPoint < points.Count; curPoint++)
                {
                    Vector2 point = points[curPoint];

                    point.X -= currentCamera.Position.X;
                    point.Y -= currentCamera.Position.Y;

                    // Vector2s cannot be returned by reference so we have to do this terribleness
                    points[curPoint] = point;
                }
            }

            // build a list of points
            // convert to make sdl2-gfx happy
            List<short> finalPointsX = new List<short>();
            List<short> finalPointsY = new List<short>();

            foreach (Vector2 point in points)
            {
                finalPointsX.Add(Convert.ToInt16(point.X));
                finalPointsY.Add(Convert.ToInt16(point.Y));
            }

            // count will always be the same
            if (filled)
            {
                SDL_gfx.filledPolygonRGBA(cWindow.Settings.RendererHandle, finalPointsX.ToArray(), finalPointsY.ToArray(), finalPointsX.Count, colour.R, colour.G, colour.B, colour.A);
            }
            else
            {
                if (antiAliased)
                {
                    SDL_gfx.aapolygonRGBA(cWindow.Settings.RendererHandle, finalPointsX.ToArray(), finalPointsY.ToArray(), finalPointsX.Count, colour.R, colour.G, colour.B, colour.A);
                }
                else
                {
                    SDL_gfx.polygonRGBA(cWindow.Settings.RendererHandle, finalPointsX.ToArray(), finalPointsY.ToArray(), finalPointsX.Count, colour.R, colour.G, colour.B, colour.A);
                }
            }
        }

        /// <summary>
        /// Draws a circle to the screen.
        /// </summary>
        /// <param name="cWindow">The window to draw this circle to.</param>
        /// <param name="position">The position of the circle to draw</param>
        /// <param name="size">The size of the circle to draw.</param>
        /// <param name="colour">The colour of the circle to draw.</param>
        /// <param name="filled">Determines if this circle is filled.</param>
        /// <param name="antiAliased">Determines if this circle is anti-aliased. Only has an effect on unfilled circles for now</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public static void DrawCircle(Window cWindow, Vector2 position, Vector2 size, Color colour, bool filled = false, bool antiAliased = false, bool snapToScreen = false)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cWindow.Settings.Camera;

            if (currentCamera != null
                && !snapToScreen)
            {
                position.X -= currentCamera.Position.X;
                position.Y -= currentCamera.Position.Y;
            }

            if (filled)
            {
                DrawCircle_DrawFilledCircle(cWindow, position, size, colour);
            }
            else
            {
                DrawCircle_DrawUnfilledCircle(cWindow, position, size, colour, antiAliased);
            }
        }

        /// <summary>
        /// Private: Draws an unfilled circle to the screen.
        /// </summary>
        /// <param name="cWindow">The window to draw this circle to.</param>
        /// <param name="position">The position of the circle to draw</param>
        /// <param name="size">The size of the circle to draw.</param>
        /// <param name="colour">The colour of the circle to draw.</param>
        /// <param name="antiAliased">Determines if this circle is anti-aliased.</param>
        private static void DrawCircle_DrawUnfilledCircle(Window cWindow, Vector2 position, Vector2 size, Color colour, bool antiAliased = false)
        {
            if (!antiAliased)
            {
                SDL_gfx.ellipseRGBA(cWindow.Settings.RendererHandle, (int)position.X, (int)position.Y, (int)size.X, (int)size.Y, colour.R, colour.G, colour.B, colour.A);
            }
            else
            {
                SDL_gfx.aaellipseRGBA(cWindow.Settings.RendererHandle, (int)position.X, (int)position.Y, (int)size.X, (int)size.Y, colour.R, colour.G, colour.B, colour.A);
            }
        }

        /// <summary>
        /// Private: Draws a filled circle to the screen.
        /// </summary>
        /// <param name="cWindow">The window to draw this circle to.</param>
        /// <param name="position">The position of the circle to draw</param>
        /// <param name="size">The size of the circle to draw.</param>
        /// <param name="colour">The colour of the circle to draw.</param>
        private static void DrawCircle_DrawFilledCircle(Window cWindow, Vector2 position, Vector2 size, Color colour)
        {
            SDL_gfx.filledEllipseRGBA(cWindow.Settings.RendererHandle, (int)position.X, (int)position.Y, (int)size.X, (int)size.Y, colour.R, colour.G, colour.B, colour.A);
        }

        /// <summary>
        /// Draws simple text to the screen using a debug font.
        /// </summary>
        /// <param name="cWindow">The Window to draw the text to.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The position to draw the text to. </param>
        /// <param name="colour">The colour to draw the text as.</param>
        /// <param name="localise">If true, the text will be localised with <see cref="LocalisationManager"/> before being drawn.</param>
        ///  <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public static void DrawText(Window cWindow, string text, Vector2 position, Color colour, bool localise = true, bool snapToScreen = false)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cWindow.Settings.Camera;

            if (currentCamera != null
                && !snapToScreen)
            {
                position.X -= currentCamera.Position.X;
                position.Y -= currentCamera.Position.Y;
            }

            if (localise) text = LocalisationManager.ProcessString(text);

            // todo: in c++: recompile sdl2_gfx to use sint32, not sint16, and modify pinvoke accordingly
            SDL_gfx.stringRGBA(cWindow.Settings.RendererHandle, (short)position.X, (short)position.Y, text, colour.R, colour.G, colour.B, colour.A);
        }
    }
}