namespace LightningGL
{
    public class Ellipse : Primitive
    {
        public Ellipse(string name) : base(name) { }

        public override void Draw()
        {
            if (BorderSize.X > 0
                && BorderSize.Y > 0)
            {
                Lightning.Renderer.DrawEllipse((int)RenderPosition.X - (int)BorderSize.X, (int)RenderPosition.Y - (int)BorderSize.X,
                    (int)Size.X + (int)BorderSize.X, (int)Size.Y + (int)BorderSize.Y, BorderColor.R, BorderColor.G, BorderColor.B, BorderColor.A, true);
            }

            Lightning.Renderer.DrawEllipse((int)RenderPosition.X, (int)RenderPosition.Y, (int)Size.X, (int)Size.Y, Color.R, Color.G, Color.B, Color.A, Filled);
        }
    }
}
