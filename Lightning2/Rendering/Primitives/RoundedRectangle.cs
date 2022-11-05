namespace LightningGL
{
    internal class RoundedRectangle : Primitive
    {
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
            else
            {
                RenderPosition = Position;
            }

            // draw the border
            if (BorderSize.X > 0
                && BorderSize.Y > 0)
            {
                roundedRectangleRGBA(cRenderer.Settings.RendererHandle, (int)RenderPosition.X - (int)BorderSize.X, (int)RenderPosition.Y - (int)BorderSize.Y, 
                    (int)RenderPosition.X + (int)Size.X + (int)BorderSize.X, (int)RenderPosition.Y + (int)Size.Y + (int)BorderSize.Y, CornerRadius, 
                    BorderColor.R, BorderColor.G, BorderColor.B, BorderColor.A);
            }

            if (Filled)
            {
                roundedBoxRGBA(cRenderer.Settings.RendererHandle, (int)RenderPosition.X, (int)RenderPosition.Y, (int)RenderPosition.X + (int)Size.X,
                (int)RenderPosition.Y + (int)Size.Y, CornerRadius, Color.R, Color.G, Color.B, Color.A);
            }
            else
            {
                roundedRectangleRGBA(cRenderer.Settings.RendererHandle, (int)RenderPosition.X, (int)RenderPosition.Y,
                (int)RenderPosition.X + (int)Size.X, (int)RenderPosition.Y + (int)Size.Y, CornerRadius, Color.R, Color.G, Color.B, Color.A);
            }
        }
    }
}
