namespace LightningGL
{
    /// <summary>
    /// Pixel
    /// 
    /// Defines a pixel primitive.
    /// </summary>
    public class Pixel : Primitive
    {
        public Pixel(string name, Vector2 position, Color color, bool snapToScreen = false) : base(name)
        {
            Position = position;
            Color = color;
            SnapToScreen = snapToScreen;
        }

        public override void Draw() => Lightning.Renderer.DrawPixel((int)RenderPosition.X, (int)RenderPosition.Y, Color.R, Color.G, Color.B, Color.A);
    }
}
