namespace LightningGL
{
    public class Line : Primitive
    {
        public Vector2 Start { get; set; }

        public Vector2 End { get; set; }

        public short Thickness { get; set; }

        public Line(string name) : base(name)
        {

        }

        internal override void Draw()
        {
            // lineRGBA(); just calls SDL.SDL_RenderDrawLine
            // thickLine does other stuff. 
            // therefore call lineRGBA if thickness = 1

            // 2022-02-25: Changed SDL2_gfx in C++
            // to support 16-bit thickness instead of 8-bit

            // nobody will ever need a line more than 32,767 pixels wide
            // (he says, regretting this in the future). If we do we can just change to sint32 in c++.

            if (Thickness < 1)
            {
                NCError.ShowErrorBox($"Cannot draw a line with a thickness property below 1 pixel! (thickness = {Thickness})", 18,
                "PrimitiveRenderer::DrawLine called with thickness property < 1", NCErrorSeverity.Warning, null, false);
                return;
            }

            // Check for a set camera and move relative to the position of that camera if it is set.
            // we have to do this here for now as we don't use renderposition (todo: fix this...)
            Camera currentCamera = Lightning.Renderer.Settings.Camera;

            Vector2 renderStart = Start;
            Vector2 renderEnd = End;

            if (currentCamera != null
                && !SnapToScreen)
            {
                renderStart = new(Start.X - currentCamera.Position.X, Start.Y - currentCamera.Position.Y);
                renderEnd = new(End.X - currentCamera.Position.X, End.Y - currentCamera.Position.Y);
            }

            // before we manually called lineRGBA. this is now done in c++, so we don't need to.

            if (BorderSize.X > 0
                && BorderSize.Y > 0)
            {
                thickLineRGBA(Lightning.Renderer.Settings.RendererHandle, (int)renderStart.X - (int)BorderSize.X, (int)renderStart.Y - (int)BorderSize.Y, 
                    (int)renderEnd.X + (int)BorderSize.X, (int)renderEnd.Y + (int)BorderSize.Y, Thickness, BorderColor.R, BorderColor.G, BorderColor.B, BorderColor.A, Antialiased);
            }

            thickLineRGBA(Lightning.Renderer.Settings.RendererHandle, (int)renderStart.X, (int)renderStart.Y, (int)renderEnd.X, (int)renderEnd.Y, Thickness, 
                Color.R, Color.G, Color.B, Color.A, Antialiased);
        }
    }
}
