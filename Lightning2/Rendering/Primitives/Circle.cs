namespace LightningGL
{
    public class Circle : Primitive
    {
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
            else
            {
                RenderPosition = Position;
            }

            if (BorderSize.X > 0
                && BorderSize.Y > 0)
            {
                filledEllipseRGBA(cRenderer.Settings.RendererHandle, (int)RenderPosition.X - (int)BorderSize.X, (int)RenderPosition.Y - (int)BorderSize.X,
                    (int)Size.X + (int)BorderSize.X, (int)Size.Y + (int)BorderSize.Y, BorderColor.R, BorderColor.G, BorderColor.B, BorderColor.A);
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
