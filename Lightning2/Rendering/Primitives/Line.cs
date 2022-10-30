
using System.Drawing;

namespace LightningGL
{
    internal class Line : Renderable
    {
        internal Vector2 Start { get; set; }

        internal Vector2 End { get; set; }

        internal short Thickness { get; set; }

        internal bool Antialiased { get; set; }

        internal override void Draw(Renderer cRenderer)
        {
            // lineRGBA(); just calls SDL.SDL_RenderDrawLine
            // thickLine does other stuff. 
            // therefore call lineRGBA if thickness = 1

            // 2022-02-25: Changed SDL2_gfx in C++
            // to support 16-bit thickness instead of 8-bit

            // nobody will ever need a line more than 32,767 pixels wide
            // (he says, regretting this in the future). If we do we can just change to sint32 in c++.

            if (Thickness < 1) _ = new NCException($"Cannot draw a line with a thickness property below 1 pixel! (thickness = {Thickness})", 18, "PrimitiveRenderer::DrawLine called with thickness property < 1", NCExceptionSeverity.FatalError);

            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cRenderer.Settings.Camera;

            if (currentCamera != null
                && !SnapToScreen)
            {
                Start = new(Start.X - currentCamera.Position.X, Start.Y - currentCamera.Position.Y);
                End = new(End.X - currentCamera.Position.X, End.Y - currentCamera.Position.Y);
            }

            if (Thickness == 1)
            {
                if (Antialiased)
                {
                    aalineRGBA(cRenderer.Settings.RendererHandle, (int)Start.X, (int)Start.Y, (int)End.X, (int)End.Y, Color.R, Color.G, Color.B, Color.A);
                }
                else
                {
                    lineRGBA(cRenderer.Settings.RendererHandle, (int)Start.X, (int)Start.Y, (int)End.X, (int)End.Y, Color.R, Color.G, Color.B, Color.A);
                }
            }
            else
            {
                thickLineRGBA(cRenderer.Settings.RendererHandle, (int)Start.X, (int)Start.Y, (int)End.X, (int)End.Y, Thickness, Color.R, Color.G, Color.B,Color.A);
            }
        }
    }
}
