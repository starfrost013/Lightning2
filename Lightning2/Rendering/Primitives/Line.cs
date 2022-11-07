﻿namespace LightningGL
{
    public class Line : Primitive
    {
        public Vector2 Start { get; set; }

        public Vector2 End { get; set; }

        public short Thickness { get; set; }

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

            // before we manually called lineRGBA. this is now done in c++, so we don't need to.

            if (BorderSize.X > 0
                && BorderSize.Y > 0)
            {
                thickLineRGBA(cRenderer.Settings.RendererHandle, (int)Start.X - (int)BorderSize.X, (int)Start.Y - (int)BorderSize.Y, 
                    (int)End.X + (int)BorderSize.X, (int)End.Y + (int)BorderSize.Y, Thickness, BorderColor.R, BorderColor.G, BorderColor.B, BorderColor.A, Antialiased);
            }

            thickLineRGBA(cRenderer.Settings.RendererHandle, (int)Start.X, (int)Start.Y, (int)End.X, (int)End.Y, Thickness, 
                Color.R, Color.G, Color.B, Color.A, Antialiased);
        }
    }
}
