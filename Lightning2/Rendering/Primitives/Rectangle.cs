namespace LightningGL
{ 
    internal class Rectangle : Primitive
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

            if (BorderColor != default(Color))
            {
                rectangleRGBA(cRenderer.Settings.RendererHandle, (int)RenderPosition.X - (int)BorderSize.X, (int)RenderPosition.Y - (int)BorderSize.Y,
                (int)RenderPosition.X + (int)Size.X + ((int)BorderSize.X * 2), (int)RenderPosition.Y + (int)Size.Y + ((int)BorderSize.Y * 2), Color.R, Color.G, Color.B, Color.A);
            }
            if (Filled)
            {
                boxRGBA(cRenderer.Settings.RendererHandle, (int)RenderPosition.X, (int)RenderPosition.Y,
                (int)RenderPosition.X + (int)Size.X, (int)RenderPosition.Y + (int)Size.Y, Color.R, Color.G, Color.B, Color.A);
            }
            else
            {
                rectangleRGBA(cRenderer.Settings.RendererHandle, (int)RenderPosition.X, (int)RenderPosition.Y,
                    (int)RenderPosition.X + (int)Size.X, (int)RenderPosition.Y + (int)Size.Y, Color.R, Color.G, Color.B, Color.A);
            }
        }
    }
}
