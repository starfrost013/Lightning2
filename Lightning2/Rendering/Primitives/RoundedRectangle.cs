namespace LightningGL
{
    public class RoundedRectangle : Primitive
    {
        public int CornerRadius { get; set; }

        public RoundedRectangle(string name) : base(name)
        {
            
        }

        internal override void Draw()
        {
            // draw the border
            if (BorderSize.X > 0
                && BorderSize.Y > 0)
            {
                roundedRectangleRGBA(Lightning.Renderer.Settings.RendererHandle, (int)RenderPosition.X - (int)BorderSize.X, (int)RenderPosition.Y - (int)BorderSize.Y, 
                    (int)RenderPosition.X + (int)Size.X + (int)BorderSize.X, (int)RenderPosition.Y + (int)Size.Y + (int)BorderSize.Y, CornerRadius, 
                    BorderColor.R, BorderColor.G, BorderColor.B, BorderColor.A);
            }

            if (Filled)
            {
                roundedBoxRGBA(Lightning.Renderer.Settings.RendererHandle, (int)RenderPosition.X, (int)RenderPosition.Y, (int)RenderPosition.X + (int)Size.X,
                (int)RenderPosition.Y + (int)Size.Y, CornerRadius, Color.R, Color.G, Color.B, Color.A);
            }
            else
            {
                roundedRectangleRGBA(Lightning.Renderer.Settings.RendererHandle, (int)RenderPosition.X, (int)RenderPosition.Y,
                (int)RenderPosition.X + (int)Size.X, (int)RenderPosition.Y + (int)Size.Y, CornerRadius, Color.R, Color.G, Color.B, Color.A);
            }
        }
    }
}
