namespace LightningGL
{
    public class Pixel : Primitive
    {
        public Pixel(string name) : base(name)
        {

        }

        internal override void Draw()
        {
            pixelRGBA(Lightning.Renderer.Settings.RendererHandle, (int)RenderPosition.X, (int)RenderPosition.Y, Color.R, Color.G, Color.B, Color.A);
        }
    }
}
