namespace LightningGL
{ 
    public class Rectangle : Primitive
    {
        public Rectangle(string name, Vector2 position, Vector2 size, Color color, bool filled = false, Color borderColor = default,
            Vector2 borderSize = default, bool snapToScreen = false) : base(name)
        {
            Position = position;
            Size = size;
            Color = color;
            Filled = filled;
            BorderColor = borderColor;
            BorderSize = borderSize;
            SnapToScreen = snapToScreen;
        }

        public override void Draw()
        {
            if (BorderColor != default)
            {
                Lightning.Renderer.DrawRectangle(new((int)RenderPosition.X - ((int)BorderSize.X + 1), (int)RenderPosition.Y - ((int)BorderSize.Y) + 1),
                new((int)Size.X + ((int)BorderSize.X * 2), (int)Size.Y + ((int)BorderSize.Y * 2) - 1), Color.R, Color.G, Color.B, Color.A);
            }

            Lightning.Renderer.DrawRectangle(new((int)RenderPosition.X, (int)RenderPosition.Y),
                new((int)(Size.X), (int)Size.Y), Color.R, Color.G, Color.B, Color.A, Filled);

        }
    }
}
