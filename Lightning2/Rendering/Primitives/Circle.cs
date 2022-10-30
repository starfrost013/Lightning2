namespace LightningGL
{
    internal class Circle : Renderable
    {
        internal bool Filled { get; set; }

        internal bool Antialiased { get; set; }

        internal override void Draw(Renderer cRenderer)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cRenderer.Settings.Camera;

            if (currentCamera != null
                && !SnapToScreen)
            {
                RenderPosition = new(Position.X - currentCamera.Position.X,
                    Position.Y - currentCamera.Position.Y);
            }

            if (Filled)
            {
                filledEllipseRGBA(cRenderer.Settings.RendererHandle, (int)RenderPosition.X, (int)RenderPosition.Y, (int)Size.X, (int)Size.Y, Color.R, Color.G, Color.B, Color.A);
            }
            else
            {
                if (!Antialiased)
                {
                    ellipseRGBA(cRenderer.Settings.RendererHandle, (int)RenderPosition.X, (int)RenderPosition.Y, (int)Size.X, (int)Size.Y, Color.R, Color.G, Color.B, Color.A);
                }
                else
                {
                    aaellipseRGBA(cRenderer.Settings.RendererHandle, (int)RenderPosition.X, (int)RenderPosition.Y, (int)Size.X, (int)Size.Y, Color.R, Color.G, Color.B, Color.A);
                }
            }
        }
    }
}
