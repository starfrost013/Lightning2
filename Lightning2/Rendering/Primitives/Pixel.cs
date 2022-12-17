namespace LightningGL
{
    public class Pixel : Primitive
    {
        public Pixel(string name) : base(name) { }

        public override void Draw() => Lightning.Renderer.DrawPixel((int)RenderPosition.X, (int)RenderPosition.Y, Color.R, Color.G, Color.B, Color.A);
    }
}
