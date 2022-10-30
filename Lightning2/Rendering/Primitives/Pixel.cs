namespace LightningGL
{
    internal class Pixel : Primitive
    {
        internal override void Draw(Renderer cRenderer)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cRenderer.Settings.Camera;

            if (currentCamera != null
                && !SnapToScreen)
            {
                RenderPosition = new(Position.X - currentCamera.Position.X, Position.Y - currentCamera.Position.Y);
            }
            else
            {
                RenderPosition = Position;
            }

            pixelRGBA(cRenderer.Settings.RendererHandle, (int)RenderPosition.X, (int)RenderPosition.Y, Color.R, Color.G, Color.B, Color.A);
        }
    }
}
