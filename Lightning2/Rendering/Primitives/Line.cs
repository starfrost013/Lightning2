namespace LightningGL
{
    public class Line : Primitive
    {

        public Line(string name, Vector2 start, Vector2 end, Color color, bool snapToScreen = false) : base(name)
        {
            Color = color;
            Position = start; // set the position to the start of the line
            Size = end - start; // size to the start
            SnapToScreen = snapToScreen;
        }

        public override void Draw()
        {
            // call into the renderer to draw a line
            // this is backend-independent

            if (BorderSize.X > 0
                && BorderSize.Y > 0)
            {
                Lightning.Renderer.DrawLine((int)RenderPosition.X - (int)BorderSize.X, (int)RenderPosition.Y - (int)BorderSize.Y, 
                    (int)RenderPosition.X + (int)Size.X + (int)BorderSize.X, (int)RenderPosition.Y + (int)Size.Y + (int)BorderSize.Y, 
                    BorderColor.R, BorderColor.G, BorderColor.B, BorderColor.A);
            }

            Lightning.Renderer.DrawLine((int)RenderPosition.X, (int)RenderPosition.Y, (int)RenderPosition.X + (int)Size.X, (int)RenderPosition.Y + (int)Size.Y, 
                Color.R, Color.G, Color.B, Color.A);
        }
    }
}
