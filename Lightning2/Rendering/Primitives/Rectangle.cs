namespace LightningGL
{ 
    public class Rectangle : Primitive
    {
        public Rectangle(string name) : base(name)
        {

        }

        internal override void Draw()
        {
            if (BorderColor != default)
            {
                Lightning.Renderer.DrawRectangle(new((int)RenderPosition.X - (int)BorderSize.X, (int)RenderPosition.Y - (int)BorderSize.Y),
                new((int)Size.X + ((int)BorderSize.X * 2), (int)Size.Y + ((int)BorderSize.Y * 2)), Color.R, Color.G, Color.B, Color.A);
            }

            Lightning.Renderer.DrawRectangle(new((int)RenderPosition.X, (int)RenderPosition.Y),
                new((int)(Size.X), (int)Size.Y), Color.R, Color.G, Color.B, Color.A, Filled);

        }
    }
}
