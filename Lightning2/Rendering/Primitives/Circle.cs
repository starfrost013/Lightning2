namespace LightningGL
{
    public class Circle : Primitive
    {
        public Circle(string name) : base(name)
        {

        }

        internal override void Draw()
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = Lightning.Renderer.Settings.Camera;

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
                filledEllipseRGBA(Lightning.Renderer.Settings.RendererHandle, (int)RenderPosition.X - (int)BorderSize.X, (int)RenderPosition.Y - (int)BorderSize.X,
                    (int)Size.X + (int)BorderSize.X, (int)Size.Y + (int)BorderSize.Y, BorderColor.R, BorderColor.G, BorderColor.B, BorderColor.A);
            }

            if (Filled)
            {
                filledEllipseRGBA(Lightning.Renderer.Settings.RendererHandle, (int)RenderPosition.X, (int)RenderPosition.Y, (int)Size.X, (int)Size.Y, Color.R, Color.G, Color.B, Color.A);
            }
            else
            {
                if (!Antialiased)
                {
                    ellipseRGBA(Lightning.Renderer.Settings.RendererHandle, (int)RenderPosition.X, (int)RenderPosition.Y, (int)Size.X, (int)Size.Y, Color.R, Color.G, Color.B, Color.A);
                }
                else
                {
                    aaellipseRGBA(Lightning.Renderer.Settings.RendererHandle, (int)RenderPosition.X, (int)RenderPosition.Y, (int)Size.X, (int)Size.Y, Color.R, Color.G, Color.B, Color.A);
                }
            }
        }
    }
}
