namespace LightningGL
{
    internal class RoundedRectangle : Renderable
    {
        internal bool Filled { get; set; }

        internal int CornerRadius { get; set; }

        internal override void Draw(Renderer cRenderer)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cRenderer.Settings.Camera;

            if (currentCamera != null
                && !SnapToScreen)
            {
                RenderPosition = new
                    (
                        Position.X - currentCamera.Position.X,
                        Position.Y - currentCamera.Position.Y
                    );
            }

            if (Filled)
            {
                roundedBoxRGBA(cRenderer.Settings.RendererHandle, (int)RenderPosition.X, (int)RenderPosition.Y, (int)RenderPosition.X + (int)Size.X,
                (int)Position.Y + (int)Size.Y, CornerRadius, Color.R, Color.G, Color.B, Color.A);
            }
            else
            {
                roundedRectangleRGBA(cRenderer.Settings.RendererHandle, (int)RenderPosition.X, (int)RenderPosition.Y,
                (int)Position.X + (int)Size.X, (int)Position.Y + (int)Size.Y, CornerRadius, Color.R, Color.G, Color.B, Color.A);
            }
        }
    }
}
