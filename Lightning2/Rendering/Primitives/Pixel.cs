namespace LightningGL
{
    public class Pixel : Primitive
    {
        public Pixel(string name) : base(name)
        {

        }

        internal override void Draw()
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = Lightning.Renderer.Settings.Camera;

            if (currentCamera != null
                && !SnapToScreen)
            {
                RenderPosition = new(Position.X - currentCamera.Position.X, Position.Y - currentCamera.Position.Y);
            }
            else
            {
                RenderPosition = Position;
            }

            pixelRGBA(Lightning.Renderer.Settings.RendererHandle, (int)RenderPosition.X, (int)RenderPosition.Y, Color.R, Color.G, Color.B, Color.A);
        }
    }
}
