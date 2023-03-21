namespace LightningGL
{
    public class Ellipse : Primitive
    {
        public Ellipse(string name, Vector2 position, Vector2 size, Color color, bool filled = false, bool snapToScreen = false,
            Vector2 borderSize = default, Color borderColor = default) : base(name)
        {
            Position = position;
            Size = size;
            Color = color;
            Filled = filled;
            SnapToScreen = snapToScreen;
            BorderSize = borderSize;
            BorderColor = borderColor;
        }

        public override void Draw()
        {
            if (BorderSize.X > 0
                && BorderSize.Y > 0)
            {
                Lightning.Renderer.DrawEllipse((int)RenderPosition.X - ((int)BorderSize.X + 1), (int)RenderPosition.Y - ((int)BorderSize.X + 1),
                    (int)Size.X + ((int)BorderSize.X - 1), (int)Size.Y + ((int)BorderSize.Y - 1), BorderColor.R, BorderColor.G, BorderColor.B, BorderColor.A, true);
            }

            Lightning.Renderer.DrawEllipse((int)RenderPosition.X, (int)RenderPosition.Y, Size.X, (int)Size.Y, Color.R, Color.G, Color.B, Color.A, Filled);
        }
    }
}
