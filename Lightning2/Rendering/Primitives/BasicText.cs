namespace LightningGL
{
    public class BasicText : Primitive
    {
        public BasicText(string name) : base(name)
        {

        }

        public bool Localise { get; set; }

        public string? Text { get; set; }

        internal override void Draw()
        {
            if (Text == null) Text = $"Localisation failure or null string";

            if (Localise) Text = LocalisationManager.ProcessString(Text);

            // todo: in c++: recompile sdl2_gfx to use sint32, not sint16, and modify pinvoke accordingly
            stringRGBA(Lightning.Renderer.Settings.RendererHandle, (short)RenderPosition.X, (short)RenderPosition.Y, Text, Color.R, Color.G, Color.B, Color.A);
        }
    }
}
